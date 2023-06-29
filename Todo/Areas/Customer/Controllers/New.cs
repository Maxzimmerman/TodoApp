using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    public class New : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
