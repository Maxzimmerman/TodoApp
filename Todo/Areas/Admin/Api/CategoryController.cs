using Microsoft.AspNetCore.Mvc;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Admin.Api
{
    [ApiController]
    [Route("api/categorycontroller")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // https://localhost:7208/api/categorycontroller/all

        [HttpGet("All", Name = "All")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult All()
        {
            List<Category> categories = _context.categories.ToList();
            _logger.LogInformation("Get all Categories");
            return Ok(categories);
        }

        // https://localhost:7208/api/categorycontroller/detail

        [HttpGet("Detail", Name = "Detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Detail(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Id can't be 0");
                return BadRequest("Id can't be 0");
            }

            Category category = _context.categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                _logger.LogInformation("Not matching category");
                return NotFound("No matching category");
            }
            else
            {
                _logger.LogInformation("Got " + id);
                return Ok(category);
            }
        }

        // https://localhost:7208/api/categorycontroller/create
        // TODO Should work
        [HttpPost("Create", Name = "Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Create([FromBody]Category category)
        {
            if(category == null)
            {
                _logger.LogInformation("No matching category");
                return BadRequest("NO VALID BODY");
            }
            else
            {
                _context.categories.Add(category);
                _context.SaveChanges();
                _logger.LogInformation($"Added {category.Name}");
                return Ok("Added" + category.Name);
            }
        }

        // https://localhost:7208/api/categorycontroller/edit

        [HttpPut("Edit", Name ="Edit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Edit([FromBody]Category category)
        { 
            if(category.Id == 0)
            {
                _logger.LogInformation("No matching Category");
                return BadRequest("NO VALID VALUE");
            }
            else
            {
                _context.Update(category);
                _context.SaveChanges();
                _logger.LogInformation("Edited" +  category.Name);
                return Ok("Updated" + category.Name);
            }
        }

        // https://localhost:7208/api/categorycontroller/delete
        // TODO Test
        [HttpDelete("Delete", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            List<Category> categories = _context.categories.ToList();

            if(id  == 0 || id > categories.Count)
            {
                _logger.LogInformation("No matching Category");
                return NotFound("NO VALID VALUE");
            }
            else
            {
                var entry = _context.categories.FirstOrDefault(c => c.Id == id);
                _context.categories.Remove(entry);
                _context.SaveChanges();
                _logger.LogInformation($"{entry.Name} deleted");
                return Ok(entry);
            }
        }
    }
}
