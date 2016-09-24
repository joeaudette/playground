namespace FSharp.WebLib

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.JsonPatch
open CSharp.Models

type PatchedToDoModel = { Original: ToDoItem; Patched: ToDoItem }

module ActionResult =
    let ofAsync (res: Async<IActionResult>) =
        res |> Async.StartAsTask

[<Route("api/[controller]")>]
type FSToDoController(commands: IToDoCommands, queries: IToDoQueries) =
    inherit Controller()
  
    [<HttpGet>]
    member this.Get() = 
        ActionResult.ofAsync <| async {
            let! data = queries.GetAll() |> Async.AwaitTask 
            return JsonResult(data) :> _ } 

    [<HttpGet("{id}", Name = "GetFsToDo")>]
    member this.Get(id) =
        ActionResult.ofAsync <| async {
            let! dataOrDefault = queries.Find(id) |> Async.AwaitTask
            match Option.ofObj dataOrDefault with 
            | None -> return this.NotFound() :> _
            | Some data -> return ObjectResult(data) :> _ } 

    // create
    [<HttpPost>]
    member this.Post([<FromBody>] item:ToDoItem) =
        ActionResult.ofAsync <| async {
            if isNull item then return this.BadRequest() :> _
            else
                item.Id <- Guid.NewGuid() |> string
                do! commands.Add(item) |> Async.AwaitTask
                let rv = RouteValueDictionary()
                rv.Add("id",item.Id)
                return this.CreatedAtRoute("GetFsToDo", rv, item) :> _ } 

    //update
    [<HttpPut("{id}")>]
    member this.Put(id:String, [<FromBody>] item:ToDoItem) =
        ActionResult.ofAsync <| async {
            if isNull item || String.IsNullOrEmpty item.Id then
                return this.BadRequest() :> _
            else
                let! dataOrDefault = queries.Find(id) |> Async.AwaitTask
                match Option.ofObj dataOrDefault with
                | None -> return this.NotFound() :> _
                | Some toDo ->
                    do! commands.Update(item) |> Async.AwaitTask 
                    return NoContentResult() :> _ } 

    // http://restful-api-design.readthedocs.io/en/latest/methods.html#patch-vs-put
    // http://benfoster.io/blog/aspnet-core-json-patch-partial-api-updates
    // this method is not currently used from the client
    [<HttpPatch("{id}")>]
    member this.Patch(id:String, [<FromBody>] patch:JsonPatchDocument<ToDoItem>) =
        ActionResult.ofAsync <| async {
            if isNull patch || String.IsNullOrEmpty id then
                return this.BadRequest() :> _
            elif this.ModelState.IsValid then
                return BadRequestObjectResult(this.ModelState) :> _
            else
                let! dataOrDefault = queries.Find(id) |> Async.AwaitTask
                match Option.ofObj dataOrDefault with
                | None -> return this.NotFound() :> _
                | Some toDo ->
                    let patched = toDo.Copy()
                    patch.ApplyTo(patched, this.ModelState)
                    do! commands.Update(patched) |> Async.AwaitTask
                    let model = { Original = toDo; Patched = patched }
                    return this.Ok(model) :> _ }
 
    [<HttpDelete("{id}")>]
    member this.Delete(id:string) =
        ActionResult.ofAsync <| async {
            let! dataOrDefault =  queries.Find(id) |> Async.AwaitTask
            match Option.ofObj dataOrDefault with
            | None -> return this.NotFound() :> _
            | Some toDo ->
                do! commands.Remove(toDo) |> Async.AwaitTask 
                return NoContentResult() :> _ }