using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Customer.Controllers
{
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

        public IActionResult Index()
        {
            List<TodoEntry> entries;
            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    var currentUser = (ClaimsIdentity)User.Identity;
                    var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                    entries = _context.todos.Where(e => e.ApplicationUserId == currentUserId).ToList();

                    if (User.Identity.IsAuthenticated)
                    {
                        return View("Today", entries);
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TodoEntry>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Todo()
        {
            var entries = _context.todos.Include(entry => entry.Category).Include(entry => entry.Priority).ToList();

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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Category()
        {
            var entries = _context.categories.ToList();

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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Priority>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Prio()
        {
            var entries = _context.priorities.ToList();

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
        public IActionResult AddModal(TodoEntry todoEntry)
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
                _context.todos.Add(todoEntry);
                TempData["addedtodo"] = $"{todoEntry.Title} Hinzugefügt";
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult CheckTodo(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            var entry = _context.todos.FirstOrDefault(t => t.Id == id);

            if(entry == null)
            {
                _logger.LogInformation($"{entry.Title} not is correct shape");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            entry.IChecked = true;
            _context.SaveChanges();
            TempData["checkedtodo"] = $"{entry.Title} Angepasst";
            _logger.LogInformation($"{entry.Title} is checked");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AddDetail(TodoEntry addTodo)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (addTodo == null)
            {
                _logger.LogInformation($"{addTodo.Title} not in correct in shape");
                return BadRequest($"{addTodo.Title} in invalidem Zustand");
            }

            _context.Update(addTodo);
            _context.SaveChanges();

            _logger.LogInformation($"{addTodo.Title} adjusted");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult DeleteButton(int id)
        {
            if (id == 0)
            {
                return BadRequest($"{id} can't be 0");
            }
            else
            {
                var todo = _context.todos.FirstOrDefault(x => x.Id == id);

                if (todo == null)
                {
                    return NotFound($"Could not found any entry with id: {id}!");
                }
                else
                {
                    todo.IDeleted = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
        }
    }
}