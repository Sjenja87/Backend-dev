#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelManagement.Data;
using ModelManagement.Models;

namespace ModelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ModelDb _context;

        public ExpensesController(ModelDb context)
        {
            _context = context;
        }


        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            
            var model = await _context.Models.FindAsync(expense.ModelId);
            if (model == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(expense.JobId);
            if (job == null)
            {
                return NotFound();
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();


            return _context.Expenses.LastOrDefault();
        }
    }
}
