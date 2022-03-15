using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ModelManagement.Models;

namespace ModelManagement.Data
{
    public class ModelDb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseInMemoryDatabase("ModelDB");
        
        public DbSet<Model> Models { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Expense> Expenses { get; set; }    

        public ModelDb(DbContextOptions<ModelDb> options) : base(options) {}
        
    }
}
