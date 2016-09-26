namespace FSharp.Storage.NoDb

open FSharp.Models
open NoDb

type ToDoCommands(commands: IBasicCommands<ToDoItem>, pidResolver: IProjectIdResolver) =
    let mutable cachedProjectId = None

    // Equivalent to C# but room for tidying (though I'd actually push this out of this type and force the id to be passed in from the caller)
    // The IProjectIdResolver is passed in by DI, a default implementation is provided
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

    interface IToDoCommands with
        member this.Add (item: ToDoItem) = async {
            let! projectId = getProjectId()
            return! commands.CreateAsync(projectId, item.Id, item) |> Async.AwaitTask }
        
        member this.Remove (item: ToDoItem) = async {
            if isNull item.Id then invalidArg "item.Id" "Cannot be null"
            let! projectId = getProjectId()
            return! commands.DeleteAsync(projectId, item.Id) |> Async.AwaitTask }

        member this.Update (item: ToDoItem) = async {
            let! projectId = getProjectId()
            return! commands.UpdateAsync(projectId, item.Id, item) |> Async.AwaitTask }