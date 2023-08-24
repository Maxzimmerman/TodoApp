using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Areas.Customer.Controllers;
using Todo.DataAccess.data;
using Todo.Models.ViewModels;

namespace Todo.Areas.Admin.Contrellers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly ApplicationDbContext _context;

        public ProjectController(ILogger<ProjectController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DetailProjectPartial(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            ProjectDetailPartialViewModel projectDetailPartial = new ProjectDetailPartialViewModel();
            var project = await _context.projects.FirstOrDefaultAsync(p => p.Id == id);
            var todos = await _context.todos.Where(t => t.ProjectId == project.Id).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId).ToListAsync();

            if(projects == null || projects.Count == 0 || project.Id == 0 || project == null || todos.Count == 0 || todos == null)
            {
                _logger.LogInformation($"Some of these were not found: {project}, {project}, {todos}");
                return NotFound($"Some of these were not found: {project}, {project}, {todos}");
            }
            else
            {
                projectDetailPartial.Project = project;
                projectDetailPartial.TodoEntries = todos;
                projectDetailPartial.Projects = projects;

                return View("_ProjectDetailPartial", projectDetailPartial);
            }
        }
    }
}
