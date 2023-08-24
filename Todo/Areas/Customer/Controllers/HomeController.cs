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
            List<TodoEntry> entries;
            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    var currentUser = (ClaimsIdentity)User.Identity;
                    var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                    ProjectLikedAndTodoEntryViewModel projectAndTodoEntryViewModel = new ProjectLikedAndTodoEntryViewModel();

                    var projects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsDeleted == false).ToListAsync();
                    var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();

                    entries = await _context.todos.Where(e => e.ApplicationUserId == currentUserId && e.IDeleted == false && e.IChecked == false).ToListAsync();

                    projectAndTodoEntryViewModel.TodoEntries = entries;
                    projectAndTodoEntryViewModel.Projects = projects;
                    projectAndTodoEntryViewModel.LikdedProjects = likedProjects;

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AddModal(TodoEntry todoEntry)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(todoEntry == null)
            {
                _logger.LogInformation($"{todoEntry.Title} not in correct shape");
                return BadRequest($"{todoEntry.Title} Ist nicht valide");
            }
            else
            {
                _logger.LogInformation($"{todoEntry.Title} added");

                todoEntry.ApplicationUserId = currentUserId;
                await _context.todos.AddAsync(todoEntry);
                await _context.SaveChangesAsync();

                TempData["addedtodo"] = $"{todoEntry.Title} Hinzugefügt";

                return RedirectToAction("Index");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CheckTodo(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            var entry = await _context.todos.FirstOrDefaultAsync(t => t.Id == id);

            if(entry == null)
            {
                _logger.LogInformation($"{entry.Title} not is correct shape");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            entry.IChecked = true;
            await _context.SaveChangesAsync();

            TempData["checkedtodo"] = $"{entry.Title} Angepasst";

            _logger.LogInformation($"{entry.Title} is checked");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> AddDetail(TodoEntry addTodo)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (addTodo == null)
            {
                _logger.LogInformation($"{addTodo.Title} not in correct in shape");
                return BadRequest($"{addTodo.Title} in invalidem Zustand");
            }

            _context.Update(addTodo);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"{addTodo.Title} adjusted");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DeleteButton(int id)
        {
            if (id == 0)
            {
                return BadRequest($"{id} can't be 0");
            }
            else
            {
                var todo = await _context.todos.FirstOrDefaultAsync(x => x.Id == id);

                if (todo == null)
                {
                    return NotFound($"Could not found any entry with id: {id}!");
                }
                else
                {
                    todo.IDeleted = true;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
        }
    }
}