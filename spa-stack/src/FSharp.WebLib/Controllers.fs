namespace FSharp.WebLib

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing
open CSharp.Models


[<Route("api/[controller]")>]
type FSToDoController(c, q) =
    inherit Controller()
    member __.Commands:IToDoCommands = c
    member __.Queries:IToDoQueries = q

// this works but isn't async I think
    [<HttpGet>]
    member __.Get() =
           __.Queries.GetAll() // this should be awaited

// thought it should be like this but it doesn't return any result in the browser
//    [<HttpGet>]
//    member __.Get() =
//        async {
//            let! data = __.Queries.GetAll()
//        } |> Async.StartAsTask

    
    [<HttpGet("{id}", Name = "GetFSTodo")>]
    member __.GetToDoItem(id) = 
            let data = __.Queries.Find(id) // this should be awaited
            if isNull data 
                then  __.NotFound() :> IActionResult
                else
                new ObjectResult(data) :> IActionResult 

//    [<HttpGet("{id}", Name = "GetFSTodo")>]
//    member this.GetToDoItem(id) = 
//        async {
//            let data = Async.AwaitTask (this.Queries.Find(id))
//            if isNull data 
//                then return this.NotFound() :> IActionResult
//                else
//                return new ObjectResult(data) :> IActionResult
//        } //|> Async.StartAsTask


    [<HttpPost>]
    member __.Create([<FromBody>] item:ToDoItem) = 
            item.Id <- Guid.NewGuid().ToString()
            (__.Commands.Add(item)) |> ignore // this should be awaited
            let rv = new RouteValueDictionary()
            rv.Add("id",item.Id)
            __.CreatedAtRoute("GetTodo", rv, item) :> IActionResult        

//    [<HttpPost>]
//    member this.Create([<FromBody>] item:ToDoItem) = 
//        async {
//            item.Id <- Guid.NewGuid().ToString()
//            Async.AwaitTask (this.Commands.Add(item)) |> ignore
//            let rv = new RouteValueDictionary()
//            rv.Add("id",item.Id)
//            return this.CreatedAtRoute("GetTodo", rv, item) :> IActionResult        
//        } //|> Async.StartAsTask

    [<HttpPut("{id}")>]
    member __.Update(id:String, [<FromBody>] item:ToDoItem) = 
            if isNull item || String.IsNullOrEmpty item.Id
                then __.BadRequest() :> IActionResult
                else
                    let data = __.Queries.Find(id)
                    if isNull data 
                        then __.NotFound() :> IActionResult
                    else
                        (__.Commands.Update(item)) |> ignore // this should be awaited
                        new NoContentResult() :> IActionResult
             


//    [<HttpPut("{id}")>]
//    member this.Update(id:String, [<FromBody>] item:ToDoItem) = 
//        async {
//            if isNull item || String.IsNullOrEmpty item.Id
//                then return this.BadRequest() :> IActionResult
//                else
//                    let! data = Async.AwaitTask (this.Queries.Find(id))
//                    if isNull data 
//                        then return this.NotFound() :> IActionResult
//                    else
//                        Async.AwaitTask (this.Commands.Update(item)) |> ignore
//                        return new NoContentResult() :> IActionResult
//             
//        } //|> Async.StartAsTask
//
//    // TODO fix duplicate code, can/should it just return invoke the above method?
//    // note that the method signature params had to be reversed to get it to compile
//    // should these methods have different names?
//    // could HttpPatch attribute be added to the above method or would that go against put patch semantics?
//    // http://restful-api-design.readthedocs.io/en/latest/methods.html#patch-vs-put
//    // seems like to do patch right we need the method to accept a dictionary instead of model bind to a ToDoItem
//    // so that we know specifically which properties have changed and then we would only update the changed properties
//    // I conclude that this method is not correctly implemented currently. How to implement it correctly?
//    // same problem in the C# version

    [<HttpPatch("{id}")>]
    member __.Update([<FromBody>] item:ToDoItem, id:String) = 
            if isNull item || String.IsNullOrEmpty item.Id
                then __.BadRequest() :> IActionResult
                else
                    let data = __.Queries.Find(id)
                    if isNull data 
                        then __.NotFound() :> IActionResult
                    else
                        (__.Commands.Update(item)) |> ignore // this should be awaited
                        new NoContentResult() :> IActionResult

//    [<HttpPatch("{id}")>]
//    member this.Update([<FromBody>] item:ToDoItem, id:String) = 
//        async {
//            if isNull item || String.IsNullOrEmpty item.Id
//                then return this.BadRequest() :> IActionResult
//                else
//                    let! data = Async.AwaitTask (this.Queries.Find(id))
//                    if isNull data 
//                        then return this.NotFound() :> IActionResult
//                    else
//                        Async.AwaitTask (this.Commands.Update(item)) |> ignore
//                        return new NoContentResult() :> IActionResult
//             
//        } //|> Async.StartAsTask

    [<HttpDelete("{id}")>]
    member __.Delete(id:String) = 
            let toDo = Async.AwaitTask (__.Queries.Find(id)) |> Async.RunSynchronously
            if isNull toDo
                then __.NotFound() :> IActionResult
                else
                    (__.Commands.Remove(toDo)) |> ignore // this should be awaited
                    new NoContentResult() :> IActionResult


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

