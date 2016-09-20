namespace WebLib

open System.Threading.Tasks

[<AutoOpen>]
module Interop =
    let inline isNull value = System.Object.ReferenceEquals(value, null)
    let inline nil<'T> = Unchecked.defaultof<'T>
    let inline safeUnbox value = if isNull value then nil else unbox value
    let (|Null|_|) value = if isNull value then Some() else None
    


//[<AutoOpen>]
//module Async =
//    let inline awaitPlainTask (task: Task) = 
//        // rethrow exception from preceding task if it fauled
//        let continuation (t : Task) : unit =
//            match t.IsFaulted with
//            | true -> raise t.Exception
//            | arg -> ()
//        task.ContinueWith continuation |> Async.AwaitTask
//
//    let inline startAsPlainTask (work : Async<unit>) = Task.Factory.StartNew(fun () -> work |> Async.RunSynchronously)
