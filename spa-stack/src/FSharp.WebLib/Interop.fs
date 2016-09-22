namespace FSharp.WebLib

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Routing

[<AutoOpen>]
module Interop =
    let inline isNull value = System.Object.ReferenceEquals(value, null)
    let inline nil<'T> = Unchecked.defaultof<'T>
    let inline safeUnbox value = if isNull value then nil else unbox value
    let (|Null|_|) value = if isNull value then Some() else None

    type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

    type AsyncEitherBuilder () =
        // Async<Result<'a,'c>> * ('a -> Async<Result<'b,'c>>)
        // -> Async<Result<'b,'c>>
        member this.Bind(x, f) =
            async {
                let! x' = x
                match x' with
                | Success s -> return! f s
                | Failure f -> return Failure f }
        // 'a -> 'a
        member this.ReturnFrom x = x
 
    let asyncEither = AsyncEitherBuilder ()

    let liftAsync x = async {
        let! x' = x
        return Success x' }
 
    // 'a -> Async<'a>
    let asyncReturn x = async { return x }
   
    


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
