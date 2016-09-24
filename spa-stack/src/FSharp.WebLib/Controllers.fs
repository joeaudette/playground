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
               let! data = Async.AwaitTask(__.Queries.GetAll()) 
               return new JsonResult(data) :> IActionResult }
            |> Async.StartAsTask

//    [<HttpGet("{id}", Name = "GetFsToDo")>]
//    member __.Get(id)  = 
//        async {
//            let! data = Async.AwaitTask(__.Queries.Find(id))
//            if isNull data 
//                then return  __.NotFound() :> IActionResult
//                else
//                    return new ObjectResult(data) :> IActionResult  } 
//            |> Async.StartAsTask

    [<HttpGet("{id}", Name = "GetFsToDo")>]
    member __.Get(id) = 
       async {
            let! dataOrDefault = __.Queries.Find(id) |> Async.AwaitTask
            match Option.ofObj dataOrDefault with 
            | None -> return __.NotFound() :> IActionResult
            | Some data -> return ObjectResult(data) :> IActionResult
       } |> Async.StartAsTask

    // create
    [<HttpPost>]
    member __.Post([<FromBody>] item:ToDoItem) = 
        async {
            if isNull item 
                then return __.BadRequest() :> IActionResult
                else
                    item.Id <- Guid.NewGuid().ToString()
                    do! Async.AwaitTask(__.Commands.Add(item))
                    let rv = new RouteValueDictionary()
                    rv.Add("id",item.Id)
                    return __.CreatedAtRoute("GetFsToDo", rv, item) :> IActionResult  } 
            |> Async.StartAsTask

    //update
    [<HttpPut("{id}")>]
    member __.Put(id:String, [<FromBody>] item:ToDoItem) = 
        async {
            if isNull item || String.IsNullOrEmpty item.Id
                then return __.BadRequest() :> IActionResult
                else
                    let! data = Async.AwaitTask(__.Queries.Find(id))  
                    if isNull data
                        then return __.NotFound() :> IActionResult
                    else
                        do! Async.AwaitTask(__.Commands.Update(item)) 
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
                    let! toDo = Async.AwaitTask(__.Queries.Find(id)) 
                    if isNull toDo
                        then return __.NotFound() :> IActionResult
                    else
                        let patched = toDo.Copy()
                        patch.ApplyTo(patched, __.ModelState);
                        if __.ModelState.IsValid then
                            do! Async.AwaitTask(__.Commands.Update(patched))
                            let model = { Original = toDo; Patched = patched; }
                            return __.Ok(model) :> IActionResult 
                        else
                            return new BadRequestObjectResult(__.ModelState) :> IActionResult } 
            |> Async.StartAsTask


    [<HttpDelete("{id}")>]
    member __.Delete(id:string) = 
        async {
            let! toDo =  Async.AwaitTask(__.Queries.Find(id)) 
            if isNull toDo
                then return __.NotFound() :> IActionResult
                else
                    do! Async.AwaitTask(__.Commands.Remove(toDo)) 
                    return new NoContentResult() :> IActionResult } 
            |> Async.StartAsTask

