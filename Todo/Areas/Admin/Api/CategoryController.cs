using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.ModelsIn;

namespace Todo.Areas.Admin.Api
{
    [ApiController]
    [Route("apicategorycontroller")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context, 
            ILogger<CategoryController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        // https://localhost:7208/apicategorycontroller/all
        [HttpGet("All", Name = "All")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> All()
        {
            _logger.LogInformation("Get all Categories");
            return Ok(await _context.categories.ToListAsync());
        }

        // https://localhost:7208/apicategorycontroller/detail
        
        [HttpGet("Detail", Name = "Detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Id can't be 0");
                return BadRequest("Id can't be 0");
            }

            var category = await _context.categories.FirstOrDefaultAsync(c => c.Id == id);

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

        // https://localhost:7208/apicategorycontroller/create
        // geht nicht
        [HttpPost("Create", Name = "Create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Create([FromBody]Category category)
        {
            if(category == null)
            {
                _logger.LogInformation("No matching category");
                return BadRequest("NO VALID BODY");
            }
            else
            {
                await _context.categories.AddAsync(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Added {category.Name}");
                return Ok("Added" + category.Name);
            }
        }

        // https://localhost:7208/apicategorycontroller/edit

        [HttpPut("Edit", Name ="Edit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Edit([FromBody]Category category)
        { 
            if(category.Id == 0)
            {
                _logger.LogInformation("No matching Category");
                return BadRequest("NO VALID VALUE");
            }
            else
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Edited" +  category.Name);
                return Ok("Updated" + category.Name);
            }
        }

        // https://localhost:7208/apicategorycontroller/delete

        [HttpDelete("Delete", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Delete(int id)
        {
            List<Category> categories = await _context.categories.ToListAsync();

            if(id  == 0 || id > categories.Count)
            {
                _logger.LogInformation("No matching Category");
                return NotFound("NO VALID VALUE");
            }
            else
            {
                var entry = await _context.categories.FirstOrDefaultAsync(c => c.Id == id);
                _context.categories.Remove(entry);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{entry.Name} deleted");
                return Ok(entry);
            }
        }
    }
}
