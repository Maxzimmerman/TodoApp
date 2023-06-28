using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    public class New : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
