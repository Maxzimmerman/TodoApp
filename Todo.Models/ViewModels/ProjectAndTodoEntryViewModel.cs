using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.ViewModels
{
    public class ProjectAndTodoEntryViewModel
    {
        public List<TodoEntry> TodoEntries { get; set; }
        public List<Project> Projects { get; set; }
    }
}
