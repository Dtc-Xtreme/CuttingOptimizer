using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CuttingOptimizer.Infrastructure
{
    public class CuttingOptimizerDbContext : DbContext
    {
        public CuttingOptimizerDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database=CuttingOptimizer; Trusted_Connection = True; TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer("Server=GB-SQL;Database=CuttingOptimizer;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Saw> Saws { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<Quotation> Quotes { get; set; }
    }
}