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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Saw> Saws { get; set; }
        public DbSet<Plate> Plates { get; set; }
        public DbSet<Quotation> Quotes { get; set; }
    }
}