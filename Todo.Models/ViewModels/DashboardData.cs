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
        public List<TodoEntry> SixAMChecked { get; set; }
        public List<TodoEntry> NineAMChecked { get; set; }
        public List<TodoEntry> TwelveAMChecked { get; set; }
        public List<TodoEntry> FifteenAMChecked { get; set; }
        public List<TodoEntry> EightenAMChecked { get; set; }
        public List<TodoEntry> TwentyOneAMChecked { get; set; }
        public List<TodoEntry> JanuaryChecked { get; set; }
        public List<TodoEntry> FebruaryChecked { get; set; }
        public List<TodoEntry> MarchChecked { get; set; }
        public List<TodoEntry> AptrilChecked { get; set; }
        public List<TodoEntry> MayChecked { get; set; }
        public List<TodoEntry> JuneChecked { get; set; }
        public List<TodoEntry> JulyChecked { get; set; }
        public List<TodoEntry> AugustChecked { get; set; }
        public List<TodoEntry> SeptemberChecked { get; set; }
        public List<TodoEntry> OktoberChecked { get; set; }
        public List<TodoEntry> NovemberChecked { get; set; }
        public List<TodoEntry> DecemberChecked { get; set; }
        public List<TodoEntry> UncheckedToday { get; set; }
        public List<TodoEntry> CheckedThisWeek { get; set; }
        public List<TodoEntry> CheckedThisYear { get; set; }    
        public List<Dictionary<DateTime, DateTime>> keyValuePairs { get; set; }
    }
}