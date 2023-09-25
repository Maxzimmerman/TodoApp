using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.ViewModels
{
    public class NavBarViewModel
    {
        public List<TodoEntry> TodoEntries { get; set; }
        public List<UserProject> Projects { get; set; }
        public List<UserProject> LikedProjects { get; set; }
        public ApplicationUser User { get; set; }
    }
}
