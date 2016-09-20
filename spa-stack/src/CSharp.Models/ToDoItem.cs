using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Models
{
    public class ToDoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public bool IsDone { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
