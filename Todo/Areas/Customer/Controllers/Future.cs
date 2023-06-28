using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    public class Future : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
