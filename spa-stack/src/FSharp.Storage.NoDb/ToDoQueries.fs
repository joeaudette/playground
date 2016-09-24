namespace FSharp.Storage.NoDb

open FSharp.Models

type ToDoQueries(queries: IBasicQueries<ToDoItem>, pidResolver: IProjectIdResolver) =
    let cachedProjectId = None

    // Equivalent to C# but room for tidying (though I'd actually push this out of this tpye and force the id to be passed in
    let getProjectId() = async {
        match cachedProjectId with
        | Some pid -> return pid
        | None ->
            let! pid = pidResolver.GetProjectId() |> Async.AwaitTask
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
            return! queries.FetchAsync(projectId,key) |> Async.AwaitTask }