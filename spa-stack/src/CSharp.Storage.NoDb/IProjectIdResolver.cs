using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Storage.NoDb
{
    public interface IProjectIdResolver
    {
        Task<string> GetProjectId();
    }
}
