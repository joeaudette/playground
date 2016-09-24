namespace FSharp.Storage

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open FSharp.Models
open NoDb

type IProjectIdResolver = 
    abstract member GetProjectId : Async<string>

type DefaultProjectIdResolver() =
    interface IProjectIdResolver with
        member this.GetProjectId = async {
            return "default"
        }

type ToDoCommands(c, q, r) =
    member this.Commands:IBasicCommands<ToDoItem> = c
    member this.Queries:IBasicQueries<ToDoItem> = q
    member this.ProjectIdResolver : IProjectIdResolver = r

type ToDoQueries(q, r) = 
    member this.Queries:IBasicQueries<ToDoItem> = q
    member this.ProjectIdResolver : IProjectIdResolver = r
