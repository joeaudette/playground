using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Models
{
    public interface IToDoQueries
    {
        Task<List<ToDoItem>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        Task<ToDoItem> Find(string key, CancellationToken cancellationToken = default(CancellationToken));
    }
}
