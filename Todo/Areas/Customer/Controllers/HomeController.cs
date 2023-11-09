using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Areas.Customer.Controllers
{
    // These endpints are in fact splited into these and the Admin/TodayController
    // Here are all endpoints which are redirecting to the today the homepage after a user i logged in
    // because i can't access it in the Admin/TodayController
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //var todos = _context.todos.ToList();
            //TodoEntryList list = new TodoEntryList();
            //list.TodoEntries = todos;
            //var first = list[0];
            //_logger.LogInformation("'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''" +
            //    "###########################################################################################################################################" +
            //    "########################################################" +  first.Title);


            List<TodoEntry> entries;
            var users = await _context.users.ToListAsync();
            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    var currentUser = (ClaimsIdentity)User.Identity;
                    var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                    ProjectLikedAndTodoEntryViewModel projectAndTodoEntryViewModel = new ProjectLikedAndTodoEntryViewModel();

                    var projects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsDeleted == false).ToListAsync();
                    var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();

                    entries = await _context.todos.Where(e => e.ApplicationUserId == currentUserId && e.IDeleted == false && e.IChecked == false && e.ProjectId == null).ToListAsync();

                    var user = _context.users.Where(u => u.Id == currentUserId).FirstOrDefault();

                    projectAndTodoEntryViewModel.TodoEntries = entries;
                    projectAndTodoEntryViewModel.Projects = projects;
                    projectAndTodoEntryViewModel.LikdedProjects = likedProjects;
                    projectAndTodoEntryViewModel.User = user;
                  

                    if (User.Identity.IsAuthenticated)
                    {
                        return View("Today", projectAndTodoEntryViewModel);
                    }
                    else
                    {
                        return View("StartingPage");
                    }
                }
                else
                {
                    return View("StartingPage");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
                return View("Today");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TodoEntry>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Todo()
        {
            var entries = await _context.todos.Include(entry => entry.Category).Include(entry => entry.Priority).ToListAsync();

            if(entries.Count == 0)
            {
                _logger.LogInformation($"{entries.Count} not entries found");
                return NotFound($"{entries.Count} Einträge gefunden");
            }
            else
            {
                _logger.LogInformation($"{entries.Count} entries");
                return Ok(entries);
            }
        }
    }
}