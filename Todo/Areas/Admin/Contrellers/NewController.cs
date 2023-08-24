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
            var todos = await _context.todos.Where(t => t.ApplicationUserId == currentUserId && t.IDeleted == false && t.IChecked == false && t.ProjectId == null).ToListAsync();
            var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId && project.IsDeleted == false).ToListAsync();

            navBarViewModel.TodoEntries = todos;
            navBarViewModel.Projects = projects;
            navBarViewModel.LikedProjects = likedProjects;

            return View(navBarViewModel);
        }
    }
}
