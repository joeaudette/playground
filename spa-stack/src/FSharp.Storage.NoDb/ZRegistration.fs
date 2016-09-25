namespace FSharp.Storage.NoDb

open System.Runtime.CompilerServices
open Microsoft.Extensions.DependencyInjection

[<Extension>]
type StartupExtensionsFSharp =
    [<Extension>]
    static member AddNoDbStorageForToDoItemsFSharp(services: IServiceCollection) =
        services.AddNoDb<ToDoItem>()
        services.TryAddScoped<IProjectIdResolver, DefaultProjectIdResolver>()
        services.TryAddScoped<IToDoQueries, ToDoQueries>()
        services.TryAddScoped<IToDoCommands, ToDoCommands>()
        services