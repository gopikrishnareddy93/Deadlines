using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deadlines.Exceptions;
using Deadlines.Interfaces;

namespace Deadlines.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DeadlinesController : ControllerBase
    {
        private readonly IDeadlineRepository _deadlineRepository;

        public DeadlinesController(IDeadlineRepository deadlineRepository)
        {
            _deadlineRepository = deadlineRepository;
        }

        // GET: api/Deadlines
        [HttpGet]
        public async Task<IEnumerable<Context.Models.Deadlines>> GetDeadlines()
        {
            return await _deadlineRepository.GetDeadlines();
        }

        // GET: api/Deadlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Context.Models.Deadlines>> GetDeadlines(int id)
        {
            return await _deadlineRepository.GetDeadlineById(id);
        }

        // PUT: api/Deadlines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeadlines(int id, Context.Models.Deadlines deadlines)
        {
            if (id != deadlines.Id)
            {
                return BadRequest();
            }

            await _deadlineRepository.UpdateDeadline(id, deadlines);

            return NoContent();
        }

        // POST: api/Deadlines
        [HttpPost]
        public async Task<ActionResult<Context.Models.Deadlines>> PostDeadlines(Context.Models.Deadlines deadlines)
        {
            try
            {
               deadlines = await _deadlineRepository.InsertDeadline(deadlines);

               return CreatedAtAction("GetDeadlines", new { id = deadlines.Id }, deadlines);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Deadlines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Context.Models.Deadlines>> DeleteDeadlines(int id)
        {
            return await _deadlineRepository.DeleteDeadline(id);
        }
    }
}
