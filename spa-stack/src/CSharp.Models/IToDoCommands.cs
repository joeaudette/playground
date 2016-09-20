using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Models
{
    public interface IToDoCommands
    {
        Task Add(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task Remove(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(ToDoItem item, CancellationToken cancellationToken = default(CancellationToken));
    }
}
