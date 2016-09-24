namespace FSharp.Storage

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open FSharp.Models
open NoDb

type IProjectIdResolver = 
    abstract member GetProjectId : Task<String>

type DefaultProjectIdResolver() =
    interface IProjectIdResolver with
        member this.GetProjectId = 
            Task.FromResult("default");

type ToDoCommands(c, q, r) =
    member this.Commands:IBasicCommands<ToDoItem> = c
    member this.Queries:IBasicQueries<ToDoItem> = q
    member this.ProjectIdResolver : IProjectIdResolver = r

//    interface IToDoCommands with
//        member this.Add: toDo:ToDoItem * ?cancellationToken:CancellationToken =
//          
//
//        member this.Remove: toDo:ToDoItem * ?cancellationToken:CancellationToken -> Task<unit>
//
//        member this.Update: toDo:ToDoItem * ?cancellationToken:CancellationToken -> Task<unit>

type ToDoQueries(q, r) = 
    member this.Queries:IBasicQueries<ToDoItem> = q
    member this.ProjectIdResolver : IProjectIdResolver = r

//    interface IToDoQueries with
//        member GetAll: ?cancellationToken:CancellationToken -> Task<List<ToDoItem>>
//        member Find: key:String * ?cancellationToken:CancellationToken -> Task<ToDoItem>
