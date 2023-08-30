using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Areas.Customer.Controllers;
using Todo.DataAccess.data;
using Todo.Models;
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

        [HttpGet]
        public IActionResult Settings()
        {
            return PartialView("_ProjectSettingsPartial");
        }

        [HttpGet]
        public IActionResult Add() 
        { 
            return PartialView("_AddProjectPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(UserProject project)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (project == null)
            {
                return BadRequest("Wrong Project");
            }

            project.ApplicationUserId = currentUserId;
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { Area = "Customer" });
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
            var todos = await _context.todos.Where(t => t.ApplicationUserId == currentUserId && t.ProjectId == id && t.IDeleted == false && t.IChecked == false).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId && project.IsDeleted == false).ToListAsync();
            var liked_projects = await _context.projects.Where(likedProject => likedProject.ApplicationUserId == currentUserId && likedProject.IsDeleted == false && likedProject.IsLiked == true).ToListAsync();

            projectDetailPartial.Project = project;
            projectDetailPartial.TodoEntries = todos;
            projectDetailPartial.Projects = projects;
            projectDetailPartial.LikedProjects = liked_projects;

            return View("_ProjectDetailPartial", projectDetailPartial);
        }
    }
}
