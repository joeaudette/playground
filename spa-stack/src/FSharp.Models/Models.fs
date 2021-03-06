namespace FSharp.Models

open System

type ToDoItem = 
    {   Id: string
        Title: string
        IsDone: bool 
        DateAdded : DateTime }

type IToDoCommands =
    abstract member Add: item:ToDoItem -> Async<unit>
    abstract member Remove: item:ToDoItem -> Async<unit>
    abstract member Update: item:ToDoItem -> Async<unit>

type IToDoQueries =
    abstract member GetAll: unit -> Async<ToDoItem list>
    abstract member Find: key:String -> Async<ToDoItem option>
