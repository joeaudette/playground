namespace FSharp.WebLib

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.JsonPatch
open FSharp.Models

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
            let! data = queries.GetAll()
            return JsonResult(data) :> _ } 

    [<HttpGet("{id}", Name = "GetFsToDo")>]
    member this.Get(id) =
        ActionResult.ofAsync <| async {
            let! res = queries.Find id
            match res with 
            | None -> return this.NotFound() :> _
            | Some data -> return ObjectResult(data) :> _ } 

    // RB: TOCONSIDER client should supply id so HTTP request can be idempotently retried
    // JA: NOTES: http://restcookbook.com/HTTP%20Methods/put-vs-post/
    // create
    [<HttpPost>]
    member this.Post([<FromBody>] item:ToDoItem) =
        ActionResult.ofAsync <| async {
            if not this.ModelState.IsValid then
                return this.BadRequest() :> _
            else  
                let item = { item with Id = Guid.NewGuid() |> string }
                do! commands.Add item
                let rv = RouteValueDictionary()
                rv.Add("id",item.Id)
                return this.CreatedAtRoute("GetFsToDo", rv, item) :> _ } 

    [<HttpPut("{id}")>]
    member this.Put(id:String, [<FromBody>] item:ToDoItem) =
        ActionResult.ofAsync <| async {
            if (not this.ModelState.IsValid) || String.IsNullOrEmpty item.Id then
                return this.BadRequest() :> _
            else
                let! res = queries.Find id
                match res with
                | None -> return this.NotFound() :> _
                | Some toDo ->
                    do! commands.Update item
                    return NoContentResult() :> _ } 

    // http://restful-api-design.readthedocs.io/en/latest/methods.html#patch-vs-put
    // http://benfoster.io/blog/aspnet-core-json-patch-partial-api-updates
    // this method is not currently used from the client
    [<HttpPatch("{id}")>]
    member this.Patch(id:String, [<FromBody>] patch:JsonPatchDocument<ToDoItem>) =
        ActionResult.ofAsync <| async {
            if isNull patch || String.IsNullOrEmpty id then
                return this.BadRequest() :> _
            else
                let! res = queries.Find id
                match res with
                | None -> return this.NotFound() :> _
                | Some item ->
                    let patched = item
                    patch.ApplyTo(patched, this.ModelState) // NB ApplyTo mutates the ModelState
                    if not this.ModelState.IsValid then
                        return BadRequestObjectResult(this.ModelState) :> _
                    else
                        let model = { Original = item; Patched = patched }
                        return this.Ok(model) :> _ }
 
    [<HttpDelete("{id}")>]
    member this.Delete(id: string) =
        ActionResult.ofAsync <| async {
            let! res = queries.Find id
            match res with
            | None -> return this.NotFound() :> _
            | Some item ->
                do! commands.Remove item
                return NoContentResult() :> _ }
