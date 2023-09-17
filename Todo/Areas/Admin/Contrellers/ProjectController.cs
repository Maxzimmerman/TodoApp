using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> DeleteProject(int id)
        {
            if(id == 0)
            {
                return BadRequest($"Id was {id}");
            }
            else
            {
                var project = await _context.projects.FirstOrDefaultAsync(e => e.Id == id);

                if(project == null)
                {
                    return NotFound($"Project: {project.Title} was not found");
                }
                else
                {
                    project.IsDeleted = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home", new { Area = "Customer" });
                }
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> ProjectDetail(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return BadRequest($"{id} ist 0");
            }

            var entry = await _context.projects.FirstOrDefaultAsync(e => e.Id == id);

            if (entry == null)
            {
                _logger.LogInformation($"{entry.Title} not Found");
                return NotFound($"{entry.Title} not Found");
            }
            else
            {
                _logger.LogInformation($"return detail {entry.Title}");

                return PartialView("_ProjectAdjustDetail", entry);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> DetailProject(UserProject project)
        {
            if (project.Id == 0)
            {
                return BadRequest($"Id was {project.Id}");
            }
            else
            {
                _context.Update(project);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new { Area = "Customer" });
            }
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

        public async Task<IActionResult> AddModalProject()
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

            return PartialView("_AddModalProject");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AddModalProject(TodoEntry todoEntry)
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            string urlReferrer = Request.Headers["Referer"]!.ToString();

            if (todoEntry == null)
            {
                _logger.LogInformation($"{todoEntry.Title} not in correct shape");
                return BadRequest($"{todoEntry.Title} Ist nicht valide");
            }
            else
            {
                _logger.LogInformation($"{todoEntry.Title} added");

                todoEntry.ApplicationUserId = currentUserId;
                todoEntry.ProjectId = Convert.ToInt32(urlReferrer[^2..]);
                todoEntry.DateOfCreation = DateTime.Today;

               
                await _context.todos.AddAsync(todoEntry);
                await _context.SaveChangesAsync();

                TempData["addedtodo"] = $"{todoEntry.Title} Hinzugefügt";

                return RedirectToAction("Index", "Home", new { Area = "Customer" });
            }
        }
    }
}
