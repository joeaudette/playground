namespace FSharp.Storage.NoDb

open FSharp.Models

type ToDoCommands(commands: IBasicCommands<ToDoItem>, pidResolver: IProjectIdResolver) =
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

    interface ITodoCommands with
        member this.Add (item: ToDoItem) = async {
            let! projectId = getProjectId()
            return! commands.CreateAsync(projectId, item.Id, item) |> Async.AwaitTask }
        
        member this.Remove (item: ToDoItem) = async {
            if isNull id then invalidArg "id"
            let! projectId = getProjectId()
            return! commands.DeleteAsync(projectId, item.Id, item) |> Async.AwaitTask }

        member this.Update (item: ToDoItem) = async {
            let! projectId = getProjectId()
            return! commands.UpdateAsync(projectId, item.Id, item) |> Async.AwaitTask }