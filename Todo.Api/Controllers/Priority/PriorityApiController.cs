using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers.Priority
{
    public class PriorityApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
