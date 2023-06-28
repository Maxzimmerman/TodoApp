using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Todo.data;
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

        public IActionResult AddModal()
        {
            return PartialView("_AddModal");
        }
    }
}