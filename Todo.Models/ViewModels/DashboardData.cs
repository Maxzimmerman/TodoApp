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
        public List<TodoEntry> TodaysCheckedEntries { get; set; }
        public List<TodoEntry> ThisWeekCheckedEntries { get; set; }
        public List<TodoEntry> NotCheckedEntrie { get; set; }
        public List<TodoEntry> MondayChecked { get; set; }
        public List<TodoEntry> ThuesdayChecked { get; set; }
        public List<TodoEntry> WednesDayChecked { get; set; }
        public List<TodoEntry> ThursdayChecked { get; set; }
        public List<TodoEntry> FridayChecked { get; set; }
        public List<TodoEntry> SaturdayChecked { get; set; }
        public List<TodoEntry> SondayChecked { get; set; }
    }
}
