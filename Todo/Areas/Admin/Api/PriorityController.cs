using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.ModelsIn;

namespace Todo.Areas.Admin.Api
{
    [ApiController]
    [Route("apipriority")]
    public class PriorityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PriorityController> _logger;

        public PriorityController(ApplicationDbContext context, ILogger<PriorityController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // https://localhost:7208/apipriority/AllPriorities
        [HttpGet("AllPriorities", Name = "AllPriorities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Priority>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async  Task<IActionResult> All()
        {
            List<Priority> priorities = await _context.priorities.ToListAsync();

            if (priorities == null)
            {
                _logger.LogInformation("No priorities");
                return NotFound("No priorities");
            }
            else if (priorities.Count == 0)
            {
                _logger.LogInformation("0 priorities");
                return NotFound("0 priorities");
            }
            else
            {
                _logger.LogInformation($"{priorities.Count} priorities");
                return Ok(priorities);
            }
        }

        // https://localhost:7208/apipriority/DetailPriorities?id=1

        [HttpGet("DetailPriorities", Name = "DetailPriorities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Priority))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("id can't be 0");
                return NotFound("id can't be 0");
            }

            Priority priority = await _context.priorities.FirstOrDefaultAsync(p => p.Id == id);

            if (priority == null)
            {
                _logger.LogInformation("id can't be 0");
                return BadRequest("id can't be 0");
            }
            else
            {
                return Ok(priority);
            }
        }

        // https://localhost:7208/apipriority/CreatePriorities

        [HttpPost("CreatePriorities", Name = "CreatePriorities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Priority))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Create([FromBody] Priority priority)
        {
            if (priority == null)
            {
                _logger.LogInformation($"{nameof(Priority)} is null");
                return BadRequest("Entry is null");
            }
            else
            {
                await _context.AddAsync(priority);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"priority {priority.Name} created");
                return Ok(priority);
            }
        }

        [HttpPost("UpdatePriorities", Name = "UpdatePriorities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Priority))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Update(int id, [FromBody] Priority priority)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} Not found");
                return BadRequest($"{id} Not found");
            }
            else if (priority == null)
            {
                _logger.LogInformation($"{priority.Name} not in correct shape");
                return BadRequest($"{priority.Name} not in correct shape");
            }
            else
            {
                var entry = await _context.priorities.FirstOrDefaultAsync(p => p.Id == id);
                entry.Id = priority.Id;
                entry.Name = priority.Name;
                entry.Color = priority.Color;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"{priority.Name} updated");
                return Ok(priority);
            }
        }

        [HttpDelete("DeletePriorities", Name = "DeletePriorities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Priority))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"{id} is 0");
                return NotFound($"{id} is 0");
            }
            var entry = await _context.priorities.FirstOrDefaultAsync(e => e.Id == id);

            if (entry == null)
            {
                _logger.LogInformation($"{entry.Name} is not in correct shape");
                return BadRequest($"{entry.Name} is not in correct shape");
            }
            else
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{entry.Name} is removed");
                return Ok(entry.Name.ToString());
            }
        }
    }
}
