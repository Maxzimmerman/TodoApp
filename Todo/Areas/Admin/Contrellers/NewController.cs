using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Admin.Contrellers
{
    public class NewController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
