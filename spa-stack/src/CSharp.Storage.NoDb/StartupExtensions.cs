using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoDb;
using CSharp.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CSharp.Storage.NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddNoDbStorageForToDoItems(this IServiceCollection services)
        {
            services.AddNoDb<ToDoItem>();
            services.TryAddScoped<IProjectIdResolver, DefaultProjectIdResolver>();
            services.TryAddScoped<IToDoQueries, ToDoQueries>();
            services.TryAddScoped<IToDoCommands, ToDoCommands>();

            return services;
        }

    }
}
