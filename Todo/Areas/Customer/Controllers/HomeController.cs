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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Todo()
        {
            var entries = _context.todos.Include(entry => entry.Category).Include(entry => entry.Priority).ToList();
            return Ok(entries);
        }
        [HttpGet]
        public IActionResult Category()
        {
            var entries = _context.categories.ToList();
            return Ok(entries);
        }
        [HttpGet]
        public IActionResult Prio()
        {
            var entries = _context.priorities.ToList();
            return Ok(entries);
        }

        public IActionResult SearchResults(string input)
        {
            List<TodoEntry> todoList = new List<TodoEntry>();

            if (string.IsNullOrEmpty(input))
                todoList = _context.todos.ToList();
            else
                todoList = _context.todos.
                    Where(x => x.Title.ToLower().Contains(input.ToLower()))
                    .ToList();

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

        //TODO: make this work
        [HttpPost]
        public IActionResult _AddModal(TodoEntry todoEntry)
        {
            _context.todos.Add(todoEntry);
            _context.SaveChanges();
            return Ok();
        }
    }
}