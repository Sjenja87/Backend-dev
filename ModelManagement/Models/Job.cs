using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ModelManagement.Models
{

    public class JobBase
    {
        public JobBase() { }

        public JobBase(JobBase job)
        {
            JobId = job.JobId;
            Customer = job.Customer;
            StartDate = job.StartDate;
            Days = job.Days;
            Location = job.Location;
            Comments = job.Comments;
        }

        public long JobId { get; set; }
        [MaxLength(64)]
        public string? Customer { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int Days { get; set; }
        [MaxLength(128)]
        public string? Location { get; set; }
        [MaxLength(2000)]
        public string? Comments { get; set; }
    }
    public class Job : JobBase
    {
        public ICollection<Model>? Models { get; set; } = new List<Model>();
        public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
    }

    public class ModelNameJob : JobBase
    {
        public ModelNameJob(JobBase job) : base(job) { }
        
        public List<string> modelNames { get; set; } = new List<string>();
    }


}
