using CSharp.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Storage.NoDb
{
    public class ToDoQueries : IToDoQueries
    {
        public ToDoQueries(
            IBasicQueries<ToDoItem> basicQueries,
            IProjectIdResolver projectResolver
            )
        {
            queries = basicQueries;
            this.projectResolver = projectResolver;
        }

        private IBasicQueries<ToDoItem> queries;
        private IProjectIdResolver projectResolver;
        private string projectId = null;

        private async Task EnsureBlogSettings()
        {
            if (projectId != null) { return; }
            projectId = await projectResolver.GetProjectId().ConfigureAwait(false);
        }

        public async Task<List<ToDoItem>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureBlogSettings().ConfigureAwait(false);

            var result =  await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return result.OrderBy(x => x.DateAdded).ToList<ToDoItem>();
        }

        public async Task<ToDoItem> Find(string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await EnsureBlogSettings().ConfigureAwait(false);

            return await queries.FetchAsync(projectId, key).ConfigureAwait(false);

        }


    }
}
