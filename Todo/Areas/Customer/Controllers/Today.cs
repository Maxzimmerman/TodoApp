using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    public class Today : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
