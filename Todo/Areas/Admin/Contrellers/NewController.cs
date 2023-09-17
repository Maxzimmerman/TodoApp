using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            ProjectLikedAndTodoEntryViewModel projectLikedAndTodoEntryViewModel = new ProjectLikedAndTodoEntryViewModel();
            var todos = await _context.todos.Where(t => t.ApplicationUserId == currentUserId && t.IDeleted == false && t.IChecked == false && t.ProjectId == null && t.DateOfCreation == DateTime.Today).ToListAsync();
            var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId && project.IsDeleted == false).ToListAsync();

            projectLikedAndTodoEntryViewModel.TodoEntries = todos;
            projectLikedAndTodoEntryViewModel.Projects = projects;
            projectLikedAndTodoEntryViewModel.LikdedProjects = likedProjects;

            return View(projectLikedAndTodoEntryViewModel);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DetailNew(int id)
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
    }
}
