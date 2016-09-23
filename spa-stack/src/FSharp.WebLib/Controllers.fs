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

type PatchedToDoModel = { Original:ToDoItem; Patched:ToDoItem }

[<Route("api/[controller]")>]
type FSToDoController(c, q) =
    inherit Controller()
    member __.Commands:IToDoCommands = c
    member __.Queries:IToDoQueries = q


    [<HttpGet>]
    member __.Get() =
        async {
               let! data = __.Queries.GetAll() |> asyncReturn // I think data is a tuple of Result, AsyncState
               return data.Result  }
            |> Async.StartAsTask

    [<HttpGet("{id}", Name = "GetFsToDo")>]
    member __.Get(id) = 
        async {
            let! data = __.Queries.Find(id) |> asyncReturn
            if isNull data.Result 
                then return  __.NotFound() :> IActionResult
                else
                    return new ObjectResult(data.Result) :> IActionResult  } 
            |> Async.StartAsTask


    [<HttpPost>]
    member __.Post([<FromBody>] item:ToDoItem) = 
        async {
            if isNull item 
                then return __.BadRequest() :> IActionResult
                else
                    item.Id <- Guid.NewGuid().ToString()
                    (__.Commands.Add(item)) |> asyncReturn |> ignore 
                    let rv = new RouteValueDictionary()
                    rv.Add("id",item.Id)
                    return __.CreatedAtRoute("GetFsToDo", rv, item) :> IActionResult  } 
            |> Async.StartAsTask


    [<HttpPut("{id}")>]
    member __.Put(id:String, [<FromBody>] item:ToDoItem) = 
        async {
            if isNull item || String.IsNullOrEmpty item.Id
                then return __.BadRequest() :> IActionResult
                else
                    let! data = __.Queries.Find(id) |> asyncReturn
                    if isNull data.Result
                        then return __.NotFound() :> IActionResult
                    else
                        (__.Commands.Update(item)) |> asyncReturn |> ignore 
                        return new NoContentResult() :> IActionResult } 
            |> Async.StartAsTask
             

    // http://restful-api-design.readthedocs.io/en/latest/methods.html#patch-vs-put
    // http://benfoster.io/blog/aspnet-core-json-patch-partial-api-updates
    // this method is not currently used from the client
    [<HttpPatch("{id}")>]
    member __.Patch(id:String, [<FromBody>] patch:JsonPatchDocument<ToDoItem>) = 
        async {
            if isNull patch || String.IsNullOrEmpty id
                then return __.BadRequest() :> IActionResult
                else
                    let! data = __.Queries.Find(id) |> asyncReturn
                    if isNull data.Result
                        then return __.NotFound() :> IActionResult
                    else
                        let patched = data.Result.Copy()
                        patch.ApplyTo(patched, __.ModelState);
                        if __.ModelState.IsValid then
                            (__.Commands.Update(patched)) |> asyncReturn |> ignore 
                            let model = { Original = data.Result; Patched = patched; }
                            return __.Ok(model) :> IActionResult 
                        else
                            return new BadRequestObjectResult(__.ModelState) :> IActionResult } 
            |> Async.StartAsTask


    [<HttpDelete("{id}")>]
    member __.Delete(id:string) = 
        async {
            let! toDo =  __.Queries.Find(id) |> asyncReturn
            if isNull toDo.Result
                then return __.NotFound() :> IActionResult
                else
                    (__.Commands.Remove(toDo.Result)) |> asyncReturn |> ignore 
                    return new NoContentResult() :> IActionResult } 
            |> Async.StartAsTask

