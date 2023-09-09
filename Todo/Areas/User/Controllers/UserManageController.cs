using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Todo.DataAccess.data;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Areas.User.Controllers
{
    [Area("User")]
    public class UserManageController : Controller
    {
        private readonly ILogger<UserManageController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserManageController(ILogger<UserManageController> logger, 
            ApplicationDbContext context, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = registerViewModel.Email, 
                    Email = registerViewModel.Email, 
                    ApplicationUserName = registerViewModel.Name
                };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home", new {Area="Customer"});
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel user = new LoginViewModel();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                if(loginViewModel.RememberMe == true)
                {
                    
                }
                var result = await _signInManager.PasswordSignInAsync
                    (
                        loginViewModel.Email, 
                        loginViewModel.Password, 
                        loginViewModel.RememberMe, 
                        lockoutOnFailure: false
                    );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }

            // If ModelState is not valid or user is not found, return to the login view
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            //var user = await _userManager.FindByIdAsync(Convert.ToString(id));

            //Console.WriteLine(user.UserName);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "Customer" });
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var currentUser = (ClaimsIdentity)User.Identity;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userManager.FindByIdAsync(currentUserId);
            return View(user);
        }
    }
}
