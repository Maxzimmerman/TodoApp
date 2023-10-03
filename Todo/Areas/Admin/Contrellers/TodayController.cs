using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Areas.Customer.Controllers;
using Todo.DataAccess.data;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Areas.Admin.Contrellers
{
    // These endpints are in fact splited into these and the Customer/HomeController
    // Here are all endpoints which are returning partial views
    // because i can't access them in the Customer/HomeController
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

        public async Task<IActionResult> SearchResults(string input)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<TodoEntry> todoList = new List<TodoEntry>();

            if (string.IsNullOrEmpty(input))
            {
                _logger.LogInformation($"not entreis found");
                var user = _context.users.Where(e => e.Id == currentUserId).FirstOrDefault();

                todoList = await _context.todos.Where(x => x.ApplicationUserId == currentUserId && x.IDeleted == false && x.IChecked == false).ToListAsync();
            }
            else
            {
                _logger.LogInformation($"{todoList.Count}");

                todoList = await _context.todos.
                    Where(x => x.ApplicationUserId == currentUserId 
                    && x.IDeleted == false 
                    && x.IChecked == false 
                    && x.Title.ToLower().Trim().Contains(input.ToLower().Trim())).ToListAsync();
            }
            return PartialView("_SearchResults", todoList);
        }

        // Checked Partial

        public async Task<IActionResult> Checked()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var entries = await _context.todos.Where(e => e.IChecked == true && e.ApplicationUserId == currentUserId).ToListAsync();

            return PartialView("_Checked", entries);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CheckTodo(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            var entry = await _context.todos.FirstOrDefaultAsync(t => t.Id == id);

            if (entry == null)
            {
                _logger.LogInformation($"{entry.Title} not is correct shape");
                return NotFound("Entschuldige bitte es ist etwas schief gelaufen");
            }

            entry.IChecked = true;
            entry.ChecktedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["checkedtodo"] = $"{entry.Title} Angepasst";

            _logger.LogInformation($"{entry.Title} is checked");

            return RedirectToAction("Index", "Home", new {Area="Customer"});
        }

        public async Task<IActionResult> AddModal()
        {
            IEnumerable<SelectListItem> categories =
                await _context.categories.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }).ToListAsync();

            IEnumerable<SelectListItem> priorities =
                await _context.priorities.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToListAsync();

            IEnumerable<SelectListItem> projects =
                await _context.projects.Select(p => new SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString(),
                }).ToListAsync();
                

            ViewBag.Categories = categories;
            ViewBag.Priorities = priorities;
            ViewBag.Projects = projects;

            return PartialView("_AddModal");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AddModal(TodoEntry todoEntry)
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
                todoEntry.ProjectId = null;
                todoEntry.DateOfCreation = DateTime.Today;
                todoEntry.ChecktedDate = null;
                await _context.todos.AddAsync(todoEntry);
                await _context.SaveChangesAsync();

                TempData["addedtodo"] = $"{todoEntry.Title} Hinzugefügt";

                return RedirectToAction("Index", "Home", new {Area="Customer"});
            }
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var entry = await _context.todos.FirstOrDefaultAsync(e => e.Id == id);

            if (entry == null)
            {
                _logger.LogInformation($"{entry.Title} not Found");
                return NotFound($"{entry.Title} not Found");
            }

            IEnumerable<SelectListItem> priorities = _context.priorities.ToList()
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                }).ToList();

            IEnumerable<SelectListItem> categories = _context.categories.ToList()
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                }).ToList();

            IEnumerable<SelectListItem> projects = _context.projects
                .Select(p => new SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString(),
                }).ToList();

            if (priorities == null || categories == null)
            {
                _logger.LogInformation($"{priorities} - {categories} not found");
                return NotFound($"{priorities} - {categories} not found");
            }
            else
            {
                ViewBag.Categories = categories;
                ViewBag.Priorities = priorities;
                ViewBag.Projects = projects;
                _logger.LogInformation($"return detail {entry.Title}");

                return PartialView("_DetailPartial", entry);
            }
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

            return RedirectToAction("Index", "Home", new {Area="Customer"});
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DetailSearchLinkResult(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var entry = await _context.todos.FirstOrDefaultAsync(e => e.Id == id);

            if (entry == null)
            {
                _logger.LogInformation($"{entry.Title} not Found");
                return NotFound($"{entry.Title} not Found");
            }

            IEnumerable<SelectListItem> priorities = _context.priorities.ToList()
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                }).ToList();

            IEnumerable<SelectListItem> categories = _context.categories.ToList()
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                }).ToList();

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

                return PartialView("_DetailSearchLinkPartial", entry);
            }
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

                    return RedirectToAction("Index", "Home", new {Area="Customer"});
                }
            }
        }
    }
}
