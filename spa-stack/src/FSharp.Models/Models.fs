namespace FSharp.Models

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks

    type ToDoItem = {
      Id: string
      Title: string
      IsDone: bool 
      DateAdded : DateTime
      }

    type ToDoItem with
        member this.Copy = 
            let result = { Id= this.Id; Title = this.Title; IsDone = this.IsDone; DateAdded = this.DateAdded }
            result

    type IToDoCommands =
        abstract member Add: toDo:ToDoItem * ?cancellationToken:CancellationToken -> Task<unit>
        abstract member Remove: toDo:ToDoItem * ?cancellationToken:CancellationToken -> Task<unit>
        abstract member Update: toDo:ToDoItem * ?cancellationToken:CancellationToken -> Task<unit>

    type IToDoQueries = 
        abstract member GetAll: ?cancellationToken:CancellationToken -> Task<List<ToDoItem>>
        abstract member Find: key:String * ?cancellationToken:CancellationToken -> Task<ToDoItem>

