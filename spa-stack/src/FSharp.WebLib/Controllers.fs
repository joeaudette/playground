namespace FSharp.WebLib

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing
open CSharp.Models


[<Route("api/[controller]")>]
type ToDoController(c, q) =
    inherit Controller()
    member this.Commands:IToDoCommands = c
    member this.Queries:IToDoQueries = q

    member this.GetAll() =
        async {
            let! data = Async.AwaitTask (this.Queries.GetAll())
            return data
        } //|> Async.StartAsTask

    [<HttpGet("{id}", Name = "GetTodo")>]
    member this.GetById(id) = 
        async {
            let! data = Async.AwaitTask (this.Queries.Find(id))
            if isNull data 
                then return this.NotFound() :> IActionResult
                else
                return new ObjectResult(data) :> IActionResult
        } //|> Async.StartAsTask

    [<HttpPost>]
    member this.Create([<FromBody>] item:ToDoItem) = 
        async {
            item.Id <- Guid.NewGuid().ToString()
            Async.AwaitTask (this.Commands.Add(item)) |> ignore
            let rv = new RouteValueDictionary()
            rv.Add("id",item.Id)
            return this.CreatedAtRoute("GetTodo", rv, item) :> IActionResult        
        } //|> Async.StartAsTask




   // [<HttpGet>]
  //  member __.Get() =
   //     "Hello World"


   // [<HttpGet>]
   // member this.Index () =
    //    this.View() :> ActionResult
