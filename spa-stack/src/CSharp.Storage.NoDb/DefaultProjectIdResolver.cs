using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Storage.NoDb
{
    public class DefaultProjectIdResolver : IProjectIdResolver
    {
        public Task<string> GetProjectId()
        {
            return Task.FromResult("default");
        }
    }
}
