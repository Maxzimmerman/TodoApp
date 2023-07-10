using Microsoft.AspNetCore.Mvc;
using Todo.DataAccess.data;
using Todo.Models;

namespace Todo.Areas.Admin.Api
{
    [ApiController]
    [Route("api/categorycontroller")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // https://localhost:7208/api/categorycontroller/all
        [HttpGet("All")]
        public IActionResult All()
        {
            List<Category> categories = _context.categories.ToList();

            return Ok(categories);
        }

        // https://localhost:7208/api/categorycontroller/detail
        [HttpGet("Detail")]
        public IActionResult Detail(int id)
        {
            Category category = _context.categories.FirstOrDefault(c => c.Id == id);

            return Ok(category);
        }

        // https://localhost:7208/api/categorycontroller/create
        // TODO Should work
        [HttpPost("Create")]
        public IActionResult Create(Category category)
        {
            if(category == null)
            {
                return BadRequest("NO VALID BODY");
            }
            else
            {
                _context.categories.Add(category);
                _context.SaveChanges();
                return Ok("Added" + category.Name);
            }
        }

        // https://localhost:7208/api/categorycontroller/edit
        // TODO Should take an object as input
        [HttpPut("Edit")]
        public IActionResult Edit(int id)
        {
            List<Category> categories = _context.categories.ToList();

            if (id == 0 || id > categories.Count)
            {
                return BadRequest("NO VALID VALUE");
            }
            else
            {
                var entry = _context.categories.FirstOrDefault(e => e.Id == id);
                _context.categories.Update(entry);
                _context.SaveChanges();
                return Ok("Updated" + entry.Name);
            }
        }

        // https://localhost:7208/api/categorycontroller/delete
        // TODO Test
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            List<Category> categories = _context.categories.ToList();

            if(id  == 0 || id > categories.Count)
            {
                return NotFound("NO VALID VALUE");
            }
            else
            {
                var entry = _context.categories.FirstOrDefault(c => c.Id == id);
                _context.categories.Remove(entry);
                _context.SaveChanges();
                return Ok();
            }
        }
    }
}
