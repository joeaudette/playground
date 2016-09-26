namespace FSharp.Storage.NoDb

open FSharp.Models
open FSharp.Control
open NoDb

module Option =
    let inline ofObj' (x:^T when ^T : not struct) =
        if obj.ReferenceEquals (x, null) then None else Some x

type ToDoQueries(queries: IBasicQueries<ToDoItem>, pidResolver: IProjectIdResolver) =
    let mutable cachedProjectId = None

    // RB: Equivalent to C# but room for tidying (though I'd actually push this out of this tpye and force the id to be passed in
    // JA: the IProjectIdResolver is passed in by DI, a default implementation is provided
    // the only purpose of projectid is to configure the project folder name for NoDb storage
    // a custom implementation would be needed for things like multi-tenancy or per user data sotrage location
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
