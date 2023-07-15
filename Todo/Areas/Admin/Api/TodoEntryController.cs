using Microsoft.AspNetCore.Mvc;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Admin.Api
{
    [ApiController]
    [Route("apitodoentries")]
    public class TodoEntryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TodoEntryController> _logger;

        public TodoEntryController(ApplicationDbContext context, ILogger<TodoEntryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // https://localhost:7208/apitodoentries/AllTodos

        [HttpGet("AllTodos", Name = "AllTodos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TodoEntry>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AllTodos()
        {
            List<TodoEntry> entries = _context.todos.ToList();

            if(entries.Count == 0)
            {
                _logger.LogInformation("No entries");
                return NotFound("No entries");
            }
            else
            {
                _logger.LogInformation($"{entries.Count}");
                return Ok(entries);
            }
        }

        [HttpGet("DetailTodo", Name = "DetailTodo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult DetailTodo(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return BadRequest("id can't be 0");
            }

            var todo = _context.todos.FirstOrDefault(x => x.Id == id);

            if(todo == null)
            {
                _logger.LogInformation($"no entry with id: {id} found");
                return NotFound($"no entry with id: {id} found");
            }
            else
            {
                _logger.LogInformation($"{todo.Title}");
                return Ok(todo);
            }
        }

        [HttpPost("CreateTodo", Name = "CreateTodo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult CreateTodo([FromBody]TodoEntry entry)
        {
            if(entry == null)
            {
                _logger.LogInformation($"{entry.Title} not in correct shape");
                return BadRequest($"{entry.Title} not in correct shape");
            }

            _context.todos.Add(entry);
            _context.SaveChanges();

            return Ok(entry);
        }

        [HttpPut("UpdateTodo", Name = "UpdateTodo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult CreateTodo(int id, [FromBody]TodoEntry entry)
        {
            var todo = _context.todos.FirstOrDefault(x => x.Id == id);
            if(id == 0 || todo == null)
            {
                _logger.LogInformation($"entry with id: {id} not found");
                return BadRequest($"entry with id: {id} not found");
            }
            else
            {
                todo.Title = entry.Title;
                todo.Description = entry.Description;
                todo.StartDate = entry.StartDate;
                todo.EndDate = entry.EndDate;
                todo.IChecked = entry.IChecked;
                todo.IDeleted = entry.IDeleted;
                todo.CategoryId = entry.CategoryId;
                todo.Category = entry.Category;

                _context.SaveChanges();

                return Ok(todo);
            }
        }

        [HttpDelete("DeleteTodo", Name = "DeleteTodo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoEntry))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult DeleteTodo(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation($"entry with id: {id} not found");
                return NotFound($"entry with id: {id} not found");
            }

            var entry = _context.todos.FirstOrDefault(x => x.Id == id);

            if(entry == null)
            {
                _logger.LogInformation($"{entry.Title} not in correct shape");
                return BadRequest($"{entry.Title} not in correct shape");
            }
            else
            {
                _logger.LogInformation($"{entry.Title} deleted");
                _context.Remove(entry);
                _context.SaveChanges();
                return Ok(entry);
            }
        }
    }
}
