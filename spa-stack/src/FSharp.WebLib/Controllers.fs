namespace FSharp.WebLib

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing
open CSharp.Models


[<Route("api/[controller]")>]
type FSToDoController(c, q) =
    inherit Controller()
    member __.Commands:IToDoCommands = c
    member __.Queries:IToDoQueries = q

// this works but isn't async 
//    [<HttpGet>]
//    member __.Get() =
//           __.Queries.GetAll() // this should be awaited

// this works 
    [<HttpGet>]
    member __.Get() =
        async {
               let! data = __.Queries.GetAll() |> asyncReturn // I think data is a tuple of Result, AsyncState
               return data.Result  }
            |> Async.StartAsTask

// this works but is not async
//    [<HttpGet("{id}", Name = "GetFSTodo")>]
//    member __.GetToDoItem(id) = 
//        let data = __.Queries.Find(id) // this should be awaited
//        if isNull data 
//            then  __.NotFound() :> IActionResult
//            else
//            new ObjectResult(data) :> IActionResult 

// this works!
    [<HttpGet("{id}", Name = "GetFSTodo")>]
    member __.GetToDoItem(id) = 
        async {
            let! data = __.Queries.Find(id) |> asyncReturn
            if isNull data.Result 
                then return  __.NotFound() :> IActionResult
                else
                    return new ObjectResult(data.Result) :> IActionResult  } 
            |> Async.StartAsTask

// this works but is not async
//    [<HttpPost>]
//    member __.Create([<FromBody>] item:ToDoItem) = 
//            item.Id <- Guid.NewGuid().ToString()
//            (__.Commands.Add(item)) |> ignore // this should be awaited
//            let rv = new RouteValueDictionary()
//            rv.Add("id",item.Id)
//            __.CreatedAtRoute("GetTodo", rv, item) :> IActionResult        

    [<HttpPost>]
    member __.Create([<FromBody>] item:ToDoItem) = 
        async {
            if isNull item 
                then return __.BadRequest() :> IActionResult
                else
                    item.Id <- Guid.NewGuid().ToString()
                    (__.Commands.Add(item)) |> asyncReturn |> ignore 
                    let rv = new RouteValueDictionary()
                    rv.Add("id",item.Id)
                    return __.CreatedAtRoute("GetTodo", rv, item) :> IActionResult  } 
            |> Async.StartAsTask

// this works but isn't async
//    [<HttpPut("{id}")>]
//    member __.Update(id:String, [<FromBody>] item:ToDoItem) = 
//            if isNull item || String.IsNullOrEmpty item.Id
//                then __.BadRequest() :> IActionResult
//                else
//                    let data = __.Queries.Find(id)
//                    if isNull data 
//                        then __.NotFound() :> IActionResult
//                    else
//                        (__.Commands.Update(item)) |> ignore // this should be awaited
//                        new NoContentResult() :> IActionResult

    [<HttpPut("{id}")>]
    member __.Update(id:String, [<FromBody>] item:ToDoItem) = 
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
             
//    // TODO fix duplicate code, can/should it just return invoke the above method?
//    // note that the method signature params had to be reversed to get it to compile
//    // should these methods have different names?
//    // could HttpPatch attribute be added to the above method or would that go against put patch semantics?
//    // http://restful-api-design.readthedocs.io/en/latest/methods.html#patch-vs-put
//    // seems like to do patch right we need the method to accept a dictionary instead of model bind to a ToDoItem
//    // so that we know specifically which properties have changed and then we would only update the changed properties
//    // I conclude that this method is not correctly implemented currently. How to implement it correctly?
//    // same problem in the C# version

//    [<HttpPatch("{id}")>]
//    member __.Update([<FromBody>] item:ToDoItem, id:String) = 
//            if isNull item || String.IsNullOrEmpty item.Id
//                then __.BadRequest() :> IActionResult
//                else
//                    let data = __.Queries.Find(id)
//                    if isNull data 
//                        then __.NotFound() :> IActionResult
//                    else
//                        (__.Commands.Update(item)) |> ignore // this should be awaited
//                        new NoContentResult() :> IActionResult

    [<HttpPatch("{id}")>]
    member __.Update([<FromBody>] item:ToDoItem, id:String) = 
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

// this works but isn't async
//    [<HttpDelete("{id}")>]
//    member __.Delete(id:String) = 
//            let toDo = Async.AwaitTask (__.Queries.Find(id)) |> Async.RunSynchronously
//            if isNull toDo
//                then __.NotFound() :> IActionResult
//                else
//                    (__.Commands.Remove(toDo)) |> ignore // this should be awaited
//                    new NoContentResult() :> IActionResult

    [<HttpDelete("{id}")>]
    member __.Delete(id:String) = 
        async {
            let! toDo =  __.Queries.Find(id) |> asyncReturn
            if isNull toDo.Result
                then return __.NotFound() :> IActionResult
                else
                    (__.Commands.Remove(toDo.Result)) |> asyncReturn |> ignore 
                    return new NoContentResult() :> IActionResult } 
            |> Async.StartAsTask


//    [<HttpDelete("{id}")>]
//    member this.Delete(id:String) = 
//        async {
//            let! toDo = Async.AwaitTask (this.Queries.Find(id))
//            if isNull toDo 
//                then return this.NotFound() :> IActionResult
//                else
//                    Async.AwaitTask (this.Commands.Remove(toDo)) |> ignore
//                    return new NoContentResult() :> IActionResult
//        } //|> Async.StartAsTask



