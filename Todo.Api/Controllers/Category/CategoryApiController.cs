using Microsoft.AspNetCore.Mvc;

namespace Todo.Api.Controllers.Category
{
    public class CategoryApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
