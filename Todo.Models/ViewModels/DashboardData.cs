using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.ViewModels
{
    public class DashboardData
    {
        public List<UserProject> Projects { get; set; }
        public List<UserProject> LikedProjects { get; set; }
        public ApplicationUser User { get; set; }
        public List<TodoEntry> TodoEntries { get; set; }
        public List<TodoEntry> CheckedEntries { get; set; }
        public List<TodoEntry> TodaysCheckedEntries { get; set; }
        public List<TodoEntry> ThisWeekCheckedEntries { get; set; }
        public List<TodoEntry> NotCheckedEntrie { get; set; }
    }
}
