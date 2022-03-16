#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        public JobsController(ModelDb context)
        {
            _context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelNameJob>>> GetAllJobs()
        {
            List<Job> jobList = await _context.Jobs.ToListAsync();

            
            List<ModelNameJob> returnList = new List<ModelNameJob>();

            foreach(Job job in jobList)
            {
                List<Model> models = await _context.Entry(job)
                    .Collection(j => j.Models)
                    .Query()
                    .ToListAsync();

                ModelNameJob returnJob = new ModelNameJob(job);

                foreach(Model m in models)
                {
                    returnJob.modelNames.Add(m.FirstName + " " + m.LastName);
                }
                returnList.Add(returnJob);
            }
            

            return returnList;
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

            //foreach (Expense ex in expenses)
            //{
            //    job.Expenses.Add(ex);
            //}

            return job;
        }

        // GET: api/Jobs/5
        [HttpGet("forModel{id}")]
        public async Task<ActionResult<List<JobBase>>> GetJobForModel(long id)
        {
            var model = await _context.Models.FindAsync(id);
            List<Job> jobList =  await _context.Entry(model)
                        .Collection(j => j.Jobs)
                        .Query()
                        .ToListAsync();

            List<JobBase> returnList = new List<JobBase>();

            foreach(Job job in jobList)
            {
                returnList.Add(new JobBase(job));
            }

            return returnList;
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllJobs", new { id = job.JobId }, job);
        }

        [HttpPut("{JobId},{ModelId}")]
        public async Task<ActionResult<Job>> AddModelToJob(long JobId, long ModelId)
        {
            var existingJob = await _context.Jobs.FindAsync(JobId);
            var existingModel = await _context.Models.FindAsync(ModelId);


            if (existingJob == null || existingModel == null)
            {
                return NotFound();
            }

            existingJob.Models.Add(existingModel);
            _context.Entry(existingJob).State = EntityState.Modified;
           
            
            await _context.SaveChangesAsync();

            return Ok(existingJob);
        }

        [HttpDelete("{JobId},{ModelId}")]
        public async Task<ActionResult<Job>> DeleteModelFromJob(long JobId, long ModelId)
        {
            var existingJob = await _context.Jobs.FindAsync(JobId);
            var existingModel = await _context.Models.FindAsync(ModelId);

            if (existingJob == null || existingModel == null)
            {
                return NotFound();
            }

            List<Model> modelList = await _context.Entry(existingJob)
                .Collection(j => j.Models)
                .Query()
                .ToListAsync();

            foreach (Model m in modelList)
            {
                if (m.ModelId == ModelId)
                {
                    existingJob.Models.Remove(m);
                }
            }
            
            _context.Entry(existingJob).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
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

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
        
    }
}
