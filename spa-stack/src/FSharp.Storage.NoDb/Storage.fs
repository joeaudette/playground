namespace FSharp.Storage.NoDb

open FSharp.Models

// projectid is a NoDb implementation detail, the projectid is used as a folder name to group sub folders where serialized types are stored on disk
// here we are just defining a hard coded one named "default", so files will go under
// nodb_storage/projects/default/[type]/key.json
// this implementation is wired up from Startup.cs in the main web app services.AddNoDbStorageForToDoItems();
// allowing a different implementation to be plugged in if needed ie for multi tenant or per user storage isolation

type IProjectIdResolver = 
    abstract member GetProjectId : unit -> Async<string>

type DefaultProjectIdResolver() =
    interface IProjectIdResolver with
        member this.GetProjectId () = async {
            return "default"
        }


