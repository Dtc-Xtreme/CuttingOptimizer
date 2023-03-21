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
            optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = CuttingOptimizer; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}