using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.Models.ViewModels;

namespace Todo.Areas.Admin.Contrellers
{
    [Area("Admin")]
    public class NewController : Controller
    {
        private readonly ILogger<NewController> _logger;
        private readonly ApplicationDbContext _context;

        public NewController(ILogger<NewController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Index()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            NavBarViewModel navBarViewModel = new NavBarViewModel();
            var todos = await _context.todos.Where(t => t.ApplicationUserId == currentUserId).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId).ToListAsync();

            if (projects == null || projects.Count == 0 || todos.Count == 0 || todos == null)
            {
                _logger.LogInformation($"Some of these were not found: {todos}");
                return NotFound($"Some of these were not found: {projects}, {todos}");
            }
            else
            {
                navBarViewModel.TodoEntries = todos;
                navBarViewModel.Projects = projects;

                return View(navBarViewModel);
            }
        }
    }
}
