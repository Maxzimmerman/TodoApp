using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.DataAccess.Repository.IRepository;
using Todo.Models;
using Todo.Models.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Todo.Areas.User.Controllers
{
    [Area("User")]
    public class UserManageController : Controller
    {
        private readonly ILogger<UserManageController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserManageController(ILogger<UserManageController> logger, 
            ApplicationDbContext context, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName= registerViewModel.Email, Email=registerViewModel.Email, ApplicationUserName = registerViewModel.Name };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new {Area="Customer"});
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel user = new LoginViewModel();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Invalid login attempt.");
                    return View();
                }
            }

            // If ModelState is not valid or user is not found, return to the login view
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            //var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            //Console.WriteLine(user.UserName);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "Customer" });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;


            var user = _context.users.FirstOrDefault(x => x.Id == currentUserId);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser user)
        {

            if (user.Id == null || user.Id == string.Empty)
            {
                _logger.LogInformation($"{user.UserName} not in correct in shape");
                return BadRequest($"{user.UserName} in invalidem Zustand");
            }
            
            var db_user = await _context.users.FirstOrDefaultAsync(e => e.Id == user.Id);

            if(db_user == null)
            {
                return NotFound();
            }

            db_user.ApplicationUserName = user.ApplicationUserName;
            db_user.Email = user.Email;
            db_user.PhoneNumber = user.PhoneNumber;

            await _context.SaveChangesAsync();

            return View(db_user);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Week
            DateTime startOfWeek = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            DateTime Tuesdaynday = startOfWeek.AddDays(1);
            DateTime Wednestday = startOfWeek.AddDays(2);
            DateTime Thursday = startOfWeek.AddDays(3);
            DateTime FriDay = startOfWeek.AddDays(4);
            DateTime Saturday = startOfWeek.AddDays(5);

            // Time
            DateTime SixAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 1);
            DateTime NineAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            DateTime TwelfeAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
            DateTime FifteenAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0);
            DateTime EigthenAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
            DateTime TwentyOneAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 0, 0);
            DateTime NullAM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 1);


            // Months
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            int weeksInMonth = calendar.GetWeekOfYear(firstDayOfMonth, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            // Year
            int CurrentYear = DateTime.Now.Year;
            DateTime CurrentMonth =  new DateTime(CurrentYear, 1, 1);


            DashboardData dashboardData = new DashboardData();

            var todos = await _context.todos
                .Where(t => t.ApplicationUserId == currentUserId).ToListAsync();

            var likedProjects = await _context.projects
                .Where(p => p.ApplicationUserId == currentUserId && 
                p.IsLiked == true && 
                p.IsDeleted == false).ToListAsync();


            var projects = await _context.projects
                .Where(project => project.ApplicationUserId == currentUserId
                && project.IsDeleted == false).ToListAsync();

            var user = await _context.users
                .Where(u => u.Id == currentUserId).FirstOrDefaultAsync();

            var notCheckedEntries = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.IChecked == false && 
                todo.IDeleted == false).ToListAsync();


            var todaysChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date ==  DateTime.Today && 
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var thisWeekChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate >= startOfWeek && 
                todo.ChecktedDate <= endOfWeek && 
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var MondayChecked = await _context.todos
                    .Where(todo => todo.ApplicationUserId == currentUserId &&
                    todo.ChecktedDate.Value.Date == startOfWeek.Date &&
                    todo.IChecked == true &&
                    todo.IDeleted == false).ToListAsync();


            var TuesdayChecked = await _context.todos
                   .Where(todo => todo.ApplicationUserId == currentUserId &&
                   todo.ChecktedDate.Value.Date == Tuesdaynday.Date &&
                   todo.IChecked == true &&
                   todo.IDeleted == false)
                   .ToListAsync();

            var WednesdayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date == Wednestday.Date &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var ThursdayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date == Thursday.Date &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var FriDayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date == FriDay.Date &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var SaturDayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date == Saturday.Date &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var SondayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate.Value.Date == endOfWeek.Date &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();


            var SixAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= SixAM &&
                todo.ChecktedDate < NineAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var NineAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= NineAM &&
                todo.ChecktedDate < TwelfeAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var TwelfeAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= TwelfeAM &&
                todo.ChecktedDate < FifteenAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var FiftenAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= FifteenAM &&
                todo.ChecktedDate < EigthenAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var EigthenAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= EigthenAM &&
                todo.ChecktedDate < TwentyOneAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var TwentyOneAmChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= TwentyOneAM &&
                todo.ChecktedDate < NullAM &&
                todo.ChecktedDate.Value.Date == DateTime.Today &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();


            var JanuaryChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth &&
                todo.ChecktedDate < CurrentMonth.AddMonths(1) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var FebruaryChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(1) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(2) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var MarchChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(2) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(3) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var AprilChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(3) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(4) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var MayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(4) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(5) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var JuneChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(5) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(6) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var JulyChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(6) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(7) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var AugustChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(7) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(8) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var SeptemberChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(8) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(9) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var OktoberChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(9) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(10) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var NovomberChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(10) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(11) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var DecemberChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= CurrentMonth.AddMonths(11) &&
                todo.ChecktedDate < CurrentMonth.AddMonths(12) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();


            var UnCheckedToday = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId &&
                todo.ChecktedDate >= DateTime.Today &&
                todo.ChecktedDate < CurrentMonth.AddMonths(12) &&
                todo.IChecked == false &&
                todo.IDeleted == false).ToListAsync();


            var ThisWeekChecked = await _context.todos
                    .Where(todo => todo.ApplicationUserId == currentUserId &&
                    todo.ChecktedDate.Value.Date >= startOfWeek.Date &&
                    todo.ChecktedDate.Value.Date <= endOfWeek.Date &&
                    todo.IChecked == true &&
                    todo.IDeleted == false).ToListAsync();


            var ThisYearChecked = await _context.todos
                    .Where(todo => todo.ApplicationUserId == currentUserId &&
                    todo.ChecktedDate >= CurrentMonth &&
                    todo.ChecktedDate < CurrentMonth.AddMonths(12) &&
                    todo.IChecked == true &&
                    todo.IDeleted == false).ToListAsync();


            dashboardData.TodoEntries = todos;
            dashboardData.Projects = projects;
            dashboardData.LikedProjects = likedProjects;
            dashboardData.User = user;
            dashboardData.NotCheckedEntrie = notCheckedEntries;
            dashboardData.TodaysCheckedEntries = todaysChecked;
            dashboardData.ThisWeekCheckedEntries = thisWeekChecked;
            dashboardData.MondayChecked = MondayChecked;
            dashboardData.ThuesdayChecked = TuesdayChecked;
            dashboardData.WednesDayChecked = WednesdayChecked;
            dashboardData.ThursdayChecked = ThursdayChecked;
            dashboardData.FridayChecked = FriDayChecked;
            dashboardData.SaturdayChecked = SaturDayChecked;
            dashboardData.SondayChecked = SondayChecked;
            dashboardData.SixAMChecked = SixAmChecked;
            dashboardData.NineAMChecked = NineAmChecked;
            dashboardData.TwelveAMChecked = TwelfeAmChecked;
            dashboardData.FifteenAMChecked = FiftenAmChecked;
            dashboardData.EightenAMChecked = EigthenAmChecked;
            dashboardData.TwentyOneAMChecked = TwentyOneAmChecked;
            dashboardData.JanuaryChecked = JanuaryChecked;
            dashboardData.FebruaryChecked = FebruaryChecked;
            dashboardData.MarchChecked = MarchChecked;
            dashboardData.AptrilChecked = AprilChecked;
            dashboardData.MayChecked = MayChecked;
            dashboardData.JuneChecked = JuneChecked;
            dashboardData.JulyChecked = JulyChecked;
            dashboardData.AugustChecked = AugustChecked;
            dashboardData.SeptemberChecked = SeptemberChecked;
            dashboardData.OktoberChecked = OktoberChecked;
            dashboardData.NovemberChecked = NovomberChecked;
            dashboardData.DecemberChecked = DecemberChecked;
            dashboardData.UncheckedToday = UnCheckedToday;
            dashboardData.CheckedThisWeek = ThisWeekChecked;
            dashboardData.CheckedThisYear = ThisYearChecked;

            _logger.LogInformation("#####################################################" + CurrentMonth + CurrentMonth.AddMonths(11).AddDays(30));

            return View(dashboardData);
        }
    }
}
