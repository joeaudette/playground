using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoDb;
using CSharp.Models;
using System.Threading;

namespace CSharp.Storage.NoDb
{
    public class ToDoCommands : IToDoCommands
    {
        public ToDoCommands(
            IBasicCommands<ToDoItem> basicCommands,
            IBasicQueries<ToDoItem> basicQueries,
            IProjectIdResolver projectResolver
            )
        {
            commands = basicCommands;
            queries = basicQueries;
            this.projectResolver = projectResolver;
        }

        private IBasicCommands<ToDoItem> commands;
        private IBasicQueries<ToDoItem> queries;
        private IProjectIdResolver projectResolver;
        private string projectId = null;

        private async Task EnsureProjectId()
        {
            if (projectId != null) { return; }
            projectId = await projectResolver.GetProjectId().ConfigureAwait(false);
        }


        public async Task Add(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null) throw new ArgumentNullException("ToDoItem must be provided");
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            await commands.CreateAsync(projectId, item.Id, item).ConfigureAwait(false);

        }

        public async Task Remove(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null) throw new ArgumentNullException("ToDoItem must be provided");
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            await commands.DeleteAsync(projectId, item.Id).ConfigureAwait(false);

        }

        public async Task Update(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null) throw new ArgumentNullException("ToDoItem must be provided");
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureProjectId().ConfigureAwait(false);

            await commands.UpdateAsync(projectId, item.Id, item).ConfigureAwait(false);


        }
    }
}
