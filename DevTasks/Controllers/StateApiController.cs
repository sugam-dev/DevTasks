using DevTasks.Data;
using DevTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DevTasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StateApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/State
        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            var states = await _context.States.ToListAsync();
            return Ok(states);
        }

        // GET: api/State/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        // POST: api/State
        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] State state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.States.Add(state);
            await _context.SaveChangesAsync();
            return Ok(state);
        }

        // PUT: api/State/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] State state)
        {
            if (id != state.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingState = await _context.States.FindAsync(id);
            if (existingState == null)
            {
                return NotFound();
            }

            existingState.Name = state.Name;
            await _context.SaveChangesAsync();
            return Ok(existingState);
        }

        // DELETE: api/State/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}