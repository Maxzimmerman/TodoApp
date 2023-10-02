using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.Models;
using Todo.Models.ViewModels;
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
                var user = new ApplicationUser 
                { 
                    UserName = registerViewModel.Email, 
                    Email = registerViewModel.Email, 
                    ApplicationUserName = registerViewModel.Name
                };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home", new {Area="Customer"});
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
                if(loginViewModel.RememberMe == true)
                {
                    
                }
                var result = await _signInManager.PasswordSignInAsync
                    (
                        loginViewModel.Email, 
                        loginViewModel.Password, 
                        loginViewModel.RememberMe, 
                        lockoutOnFailure: false
                    );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
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
        public async Task<IActionResult> Dashboard()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            DateTime startOfWeek = DateTime.UtcNow.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek).AddDays(1);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            DateTime Tuesdaynday = startOfWeek.AddDays(1);
            DateTime Wednestday = startOfWeek.AddDays(2);
            DateTime Thursday = startOfWeek.AddDays(3);
            DateTime FriDay = startOfWeek.AddDays(4);
            DateTime Saturday = startOfWeek.AddDays(5);

            DashboardData dashboardData = new DashboardData();

            var todos = await _context.todos.Where(t => t.ApplicationUserId == currentUserId).ToListAsync();
            var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId && project.IsDeleted == false).ToListAsync();
            var user = await _context.users.Where(u => u.Id == currentUserId).FirstOrDefaultAsync();
            var notCheckedEntries = await _context.todos.Where(todo => todo.ApplicationUserId == currentUserId && todo.IChecked == false && todo.IDeleted == false).ToListAsync();
            var todaysChecked = await _context.todos.Where(todo => todo.ApplicationUserId == currentUserId && todo.ChecktedDate ==  DateTime.Today && todo.IChecked == true && todo.IDeleted == false).ToListAsync();
            var thisWeekChecked = await _context.todos.Where(todo => todo.ApplicationUserId == currentUserId && todo.ChecktedDate >= startOfWeek && todo.ChecktedDate <= endOfWeek && todo.IChecked == true && todo.IDeleted == false).ToListAsync();

            var MondayChecked = await _context.todos
                    .Where(todo => todo.ApplicationUserId == currentUserId && 
                    todo.ChecktedDate == startOfWeek.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) && 
                    todo.IChecked == true && 
                    todo.IDeleted == false).ToListAsync();

            _logger.LogInformation("''''''''''''''''''''''''''''****************************" + startOfWeek.Date.AddHours(5).AddMinutes(5).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0));

            var TuesdayChecked = await _context.todos
                   .Where(todo => todo.ApplicationUserId == currentUserId &&
                   todo.ChecktedDate == Tuesdaynday.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                   todo.IChecked == true &&
                   todo.IDeleted == false)
                   .ToListAsync();
            
            var WednesdayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate == Wednestday.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                todo.IChecked == true &&
                todo.IDeleted == false).ToListAsync();

            var ThursdayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate == Thursday.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var FriDayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate == FriDay.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var SaturDayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate == Saturday.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            var SondayChecked = await _context.todos
                .Where(todo => todo.ApplicationUserId == currentUserId && 
                todo.ChecktedDate == endOfWeek.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0) &&
                todo.IChecked == true && 
                todo.IDeleted == false).ToListAsync();

            _logger.LogInformation("**********************************" + startOfWeek.Date.AddHours(0).AddMinutes(0).AddSeconds(0).AddMilliseconds(0).AddMicroseconds(0));

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

            return View(dashboardData);
        }
    }
}
