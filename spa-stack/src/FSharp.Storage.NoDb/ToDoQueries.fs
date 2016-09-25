namespace FSharp.Storage.NoDb

open FSharp.Models
open FSharp.Control
open NoDb

module Option =
    let inline ofObj' (x:^T when ^T : not struct) =
        if obj.ReferenceEquals (x, null) then None else Some x

type ToDoQueries(queries: IBasicQueries<ToDoItem>, pidResolver: IProjectIdResolver) =
    let mutable cachedProjectId = None

    // Equivalent to C# but room for tidying (though I'd actually push this out of this tpye and force the id to be passed in
    let getProjectId() = async {
        match cachedProjectId with
        | Some pid -> return pid
        | None ->
            let! pid = pidResolver.GetProjectId()
            cachedProjectId <- Some pid
            return pid
    }
    interface IToDoQueries with
        member this.GetAll () = async {
            let! projectId = getProjectId()
            let! result = queries.GetAllAsync(projectId) |> Async.AwaitTask
            return result |> List.ofSeq |> List.sortBy (fun x-> x.DateAdded) }
        
        member this.Find key  = async {
            let! projectId = getProjectId()
            let! res = queries.FetchAsync(projectId, key) |> Async.AwaitTask
            return res |> Option.ofObj' }