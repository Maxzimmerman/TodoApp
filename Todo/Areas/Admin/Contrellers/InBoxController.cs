using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Admin.Contrellers
{
    [Area("Admin")]
    public class InBoxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
