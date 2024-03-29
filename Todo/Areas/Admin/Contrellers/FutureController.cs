﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.Data;
using Todo.ModelsIn.ViewModels;

namespace Todo.Areas.Admin.Contrellers
{
    [Area("Admin")]
    public class FutureController : Controller
    {
        private readonly ILogger<FutureController> _logger;
        private readonly ApplicationDbContext _context;

        public FutureController(ILogger<FutureController> logger, ApplicationDbContext context)
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
            var likedProjects = await _context.projects.Where(p => p.ApplicationUserId == currentUserId && p.IsLiked == true && p.IsDeleted == false).ToListAsync();
            var projects = await _context.projects.Where(project => project.ApplicationUserId == currentUserId && project.IsDeleted == false).ToListAsync();
            var user = await _context.users.Where(u => u.Id == currentUserId).FirstOrDefaultAsync();

            navBarViewModel.TodoEntries = todos;
            navBarViewModel.Projects = projects;
            navBarViewModel.LikedProjects = likedProjects;
            navBarViewModel.User = user;

            return View(navBarViewModel);
        }
    }
}
