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
using ModelManagement.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ModelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ModelDb _context;
        private readonly IHubContext<ExpenseMessageHub> _messageHub;

        public ExpensesController(ModelDb context, IHubContext<ExpenseMessageHub> messageHub)
        {
            _context = context;
            _messageHub = messageHub;
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

            Expense lastSaveExpense = _context.Expenses.LastOrDefault();

            await _messageHub.Clients.All.SendAsync("NewExpense", lastSaveExpense.ExpenseId , "2012313");


            return lastSaveExpense;
        }
    }
}
