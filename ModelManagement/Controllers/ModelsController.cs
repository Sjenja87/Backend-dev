#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using ModelManagement.Data;
using ModelManagement.Models;
using NuGet.Protocol;

namespace ModelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly ModelDb _context;

        public ModelsController(ModelDb context)
        {
            _context = context;
        }

        // GET: api/Models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelBase>>> GetModels()
        {
            var modelBaseList = await _context
                .Models
                .Cast<ModelBase>()
                .ToListAsync();
            return modelBaseList;
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(long id)
        {
            var model = await _context.Models.FindAsync(id);
            
            if (model == null)
            {
                return NotFound();
            }
            
            List<Job> jobs = await _context.Entry(model)
                        .Collection(j => j.Jobs)
                        .Query()
                        .ToListAsync();
            List<Expense> expenses = await _context.Entry(model)
                        .Collection(j => j.Expenses)
                        .Query()
                        .ToListAsync();

            foreach (Job jb in jobs)
            {
                model.Jobs.Add(jb);
            }
            foreach(Expense expense in expenses)
            {
                model.Expenses.Add(expense);
            }

            return model;
        }

        // PUT: api/Models/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(long id, Model model)
        {
            model.ModelId = id;
               
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
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

        // POST: api/Models
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel([Bind("ModelId, FirstName, LastName, Email, PhoneNo, AddressLine1, AddressLine2, Zip, City, BirthDay, Height, ShoeSize, HairColor, Comments")] Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModel", new { id = model.ModelId }, model);
        }

        // DELETE: api/Models/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(long id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModelExists(long id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
