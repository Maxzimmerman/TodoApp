using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Todo.Areas.Customer.Controllers;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Admin.Contrellers
{
    [Area("Admin")]
    public class TodayController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public TodayController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<TodoEntry> entries;
            try
            {
                var currentUser = (ClaimsIdentity)User.Identity;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                entries = _context.todos.Where(e => e.ApplicationUserId == currentUserId).ToList();

                if (entries.Count == 0)
                {
                    return View(entries);
                }
                _logger.LogInformation("All");
                return View(entries);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
                return View();
            }
        }

        public IActionResult SearchResults(string input)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<TodoEntry> todoList = new List<TodoEntry>();

            if (string.IsNullOrEmpty(input))
            {
                _logger.LogInformation($"not entreis found");
                todoList = _context.todos.Where(x => x.ApplicationUserId == currentUserId).ToList();
            }
            else
            {
                _logger.LogInformation($"{todoList.Count}");

                todoList = _context.todos.
                    Where(x => x.ApplicationUserId == currentUserId && x.Title.ToLower().Contains(input.ToLower())).ToList();
            }
            return PartialView("_SearchResults", todoList);
        }

        // Checked Partial

        public IActionResult Checked()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var entries = _context.todos.Where(e => e.IChecked == true && e.ApplicationUserId == currentUserId).ToList();

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
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (todoEntry == null)
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
                return RedirectToAction("Admin/Today");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult CheckTodo(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            var entry = _context.todos.FirstOrDefault(t => t.Id == id);

            if (entry == null)
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
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var entry = _context.todos.FirstOrDefault(e => e.Id == id);

            if (entry == null)
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

            if (priorities == null || categories == null)
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
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (addTodo == null)
            {
                _logger.LogInformation($"{addTodo.Title} not in correct in shape");
                return BadRequest($"{addTodo.Title} in invalidem Zustand");
            }

            var entry = _context.todos.FirstOrDefault(e => e.Id == addTodo.Id);

            entry.Title = addTodo.Title;
            entry.Description = addTodo.Description;
            entry.StartDate = addTodo.StartDate;
            entry.EndDate = addTodo.EndDate;
            entry.IChecked = addTodo.IChecked;
            entry.IDeleted = addTodo.IDeleted;
            entry.CategoryId = addTodo.CategoryId;
            entry.PriorityId = addTodo.PriorityId;
            entry.ApplicationUserId = currentUserId;

            _context.SaveChanges();

            _logger.LogInformation($"{addTodo.Title} adjusted");
            return RedirectToAction("index");
        }
    }
}
