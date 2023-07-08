using Microsoft.AspNetCore.Mvc;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Admin.Api
{
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult All()
        {
            List<TodoEntry> todoEntries = _context.todos.ToList();

            return Ok(todoEntries);
        }
    }
}
