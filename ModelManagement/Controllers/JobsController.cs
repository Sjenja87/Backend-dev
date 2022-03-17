#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelManagement.Data;
using ModelManagement.Models;

namespace ModelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ModelDb _context;

        #region Controller Methods
        
        public JobsController(ModelDb context)
        {
            _context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs()
        {
            return await _context.Jobs.ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJobWithExpenses(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            List<Expense> expenses = await _context.Entry(job)
                        .Collection(j => j.Expenses)
                        .Query()
                        .ToListAsync();

            foreach (Expense ex in expenses)
            {
                job.Expenses.Add(ex);
            }

            return job;
        }

        // PUT: api/Jobs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, [Bind("StartDate", "Days", "Location", "Comments")] Job job)
        {
            job.JobId = id;
            
            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllJobs", new { id = job.JobId }, job);
        }

        [HttpPut]
        public async Task<ActionResult<Job>> AddModelToJob(long jobId, long modelId)
        {
          JobConnector jobConnector = new JobConnector(_context);

          if (JobExists(jobId) && ModelExists(modelId))
          {
            jobConnector.AddElementToNavigator(jobId, modelId);
            return NoContent();
          }

          return BadRequest();
        }

        [HttpPut("RemoveModel")]
        public async Task<ActionResult<Job>> DeleteModelWithIdFromJobWithId(long jobId, long modelId)
        {
          JobConnector jobConnector = new JobConnector(_context);
          
          if (JobExists(jobId) && ModelExists(modelId))
          {
            jobConnector.RemoveElementFromNavigator(jobId, modelId);
            return NoContent();
          }
          
          return BadRequest();
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Helper Methods

        private bool JobExists(long id)
          => _context.Jobs.Any(e => e.JobId == id);

        private bool ModelExists(long id) 
          => _context.Models.Any(e => e.ModelId == id);

        #endregion
    }
}
