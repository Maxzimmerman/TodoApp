using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    public class Today : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
