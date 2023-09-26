using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CuttingOptimizer.Infrastructure
{
    public class CuttingOptimizerDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public CuttingOptimizerDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("db"));
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("OPNR2023").StartsAt(2300001).IncrementsBy(1);
            modelBuilder.HasSequence<int>("OPNR2024").StartsAt(2400001).IncrementsBy(1);
            modelBuilder.HasSequence<int>("OPNR2025").StartsAt(2500001).IncrementsBy(1);
            modelBuilder.Entity<Blueprint>().Property(c => c.ID).HasDefaultValueSql("NEXT VALUE FOR OPNR2023");
        }

        public DbSet<Saw> Saws { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<Blueprint> Blueprints { get; set; }
    }
}