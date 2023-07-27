using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
            var entries = _context.todos.ToList();
            _logger.LogInformation("All");
            return View("Today", entries);
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

        public IActionResult SearchResults(string input)
        {
            List<TodoEntry> todoList = new List<TodoEntry>();

            if (string.IsNullOrEmpty(input))
            {
                _logger.LogInformation($"not entreis found");
                todoList = _context.todos.ToList();
            } 
            else
            {
                _logger.LogInformation($"{todoList.Count}");

                todoList = _context.todos.
                    Where(x => x.Title.ToLower().Contains(input.ToLower()))
                    .ToList();
            }
            return PartialView("_SearchResults", todoList);
        }

        // Checked Partial

        public IActionResult Checked()
        {
            var entries = _context.todos.Where(e => e.IChecked == true).ToList();

            return PartialView("_Checked", entries);
        }

        public IActionResult AddModal()
        {
            IEnumerable<SelectListItem> categories = 
                _context.categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            IEnumerable<SelectListItem> priorities = 
                _context.priorities.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
            });

            ViewBag.Categories = categories;
            ViewBag.Priorities = priorities;
            
            return PartialView("_AddModal");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult AddModal(TodoEntry todoEntry)
        {
            if(todoEntry == null)
            {
                _logger.LogInformation($"{todoEntry.Title} not in correct shape");
                return BadRequest($"{todoEntry.Title} Ist nicht valide");
            }
            else
            {
                _logger.LogInformation($"{todoEntry.Title} added");
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

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Detail(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var entry = _context.todos.FirstOrDefault(e => e.Id == id);

            if(entry == null)
            {
                _logger.LogInformation($"{entry.Title} not Found");
                return NotFound($"{entry.Title} not Found");
            }

            IEnumerable<SelectListItem> priorities = _context.priorities
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                });

            IEnumerable<SelectListItem> categories = _context.categories
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                });

            if(priorities == null || categories == null)
            {
                _logger.LogInformation($"{priorities} - {categories} not found");
                return NotFound($"{priorities} - {categories} not found");
            } 
            else
            {
                ViewBag.Categories = categories;
                ViewBag.Priorities = priorities;
                _logger.LogInformation($"return detail {entry.Title}");

                return PartialView("_DetailPartial", entry);
            }
        }

        // Todo however the addTodo id is always 0
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AddDetail(TodoEntry addTodo)
        {
            if(addTodo == null)
            {
                _logger.LogInformation($"{addTodo.Title} not in correct in shape");
                return BadRequest($"{addTodo.Title} in invalidem Zustand");
            }

            var entry = _context.todos.FirstOrDefault(e => e.Id == addTodo.Id);
            
            _context.Update(entry);
            _context.SaveChanges();

            _logger.LogInformation($"{addTodo.Title} adjusted");
            return RedirectToAction("index");
        }
    }
}