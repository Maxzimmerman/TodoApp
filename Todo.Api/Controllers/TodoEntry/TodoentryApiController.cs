using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers.TodoEntry
{
    public class TodoentryApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
