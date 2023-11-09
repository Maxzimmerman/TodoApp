using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models
{
    public class TodoEntryList
    {
        public List<TodoEntry> TodoEntries { get; set; }
        public TodoEntry this[int index]
        {
            get { return TodoEntries[index]; }
            set { TodoEntries[index] = value;}
        }
    }
}
