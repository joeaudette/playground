namespace Microsoft.Extensions.DependencyInjection

open System.Runtime.CompilerServices
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.DependencyInjection.Extensions
open FSharp.Models
open FSharp.Storage.NoDb
open NoDb

[<Extension>]
type StartupExtensionsFSharp  =
    [<Extension>]
    static member AddNoDbStorageForToDoItemsFSharp(services: IServiceCollection) =
        services.AddNoDb<ToDoItem>() |> ignore
        services.TryAddScoped<IProjectIdResolver, DefaultProjectIdResolver>() 
        services.TryAddScoped<IToDoQueries, ToDoQueries>() 
        services.TryAddScoped<IToDoCommands, ToDoCommands>() 
        services

