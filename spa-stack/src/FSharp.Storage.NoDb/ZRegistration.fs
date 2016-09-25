namespace Microsoft.Extensions.DependencyInjection
//namespace FSharp.Storage.NoDb
//namespace Microsoft.Extensions.DependencyInjection

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
        services.TryAddScoped<IProjectIdResolver, DefaultProjectIdResolver>() |> ignore
        services.TryAddScoped<IToDoQueries, ToDoQueries>() |> ignore
        services.TryAddScoped<IToDoCommands, ToDoCommands>() |> ignore
        services

//[<AutoOpen>]
//module StartupExtensionsFS =
//
//    [<Extension>]
//    type IServiceCollection  with
//        [<Extension>]
//        static member AddNoDbStorageForToDoItemsFSharp(services: IServiceCollection) =
//            services.AddNoDb<ToDoItem>() |> ignore
//            services.TryAddScoped<IProjectIdResolver, DefaultProjectIdResolver>() |> ignore
//            services.TryAddScoped<IToDoQueries, ToDoQueries>() |> ignore
//            services.TryAddScoped<IToDoCommands, ToDoCommands>() |> ignore
//            services
