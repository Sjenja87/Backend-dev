﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ModelManagement.Models;

namespace ModelManagement.Data
{
    public class ModelDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseInMemoryDatabase("Model Management Database");

        public DbSet<Model> Models { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    
        public ModelDB(DbContextOptions<ModelDB> options) : base(options) {}
        
    }
}