using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ModelManagement.Models
{
    public class Job
    {
        public long JobId { get; set; }
        [MaxLength(64)]
        public string? Customer { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int Days { get; set; }
        [MaxLength(128)]
        public string? Location { get; set; }
        [MaxLength(2000)]
        public string? Comments { get; set; }

        public ICollection<Model>? Models { get; set; } = new List<Model>();
        public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
    }
}
