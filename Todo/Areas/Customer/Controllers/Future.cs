using Microsoft.AspNetCore.Mvc;

namespace Todo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class Future : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
