using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public class SawRepository : ISawRepository
    {
        private CuttingOptimizerDbContext context;
        public IQueryable<Saw> Saws => context.Saws;

        public SawRepository(CuttingOptimizerDbContext ctx)
        {
            context = ctx;
        }

        public async Task<bool> Create(Saw saw)
        {
            context.Saws.Add(saw);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }

        public async Task<Saw?> FindById(string id)
        {
            return await Saws.FirstOrDefaultAsync(c => c.ID.ToLower().Equals(id.ToLower()));
        }

        public async Task<bool> Remove(string id)
        {
            Saw? saw = await Saws.FirstOrDefaultAsync(c => c.ID.ToLower().Equals(id.ToLower()));
            if(saw != null) context.Saws.Remove(saw);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }

        public async Task<IList<Saw>> FindMultipleByIdOrThickness(string search)
        {
            return await Saws.Where(c => c.ID.ToLower().Contains(search.ToLower()) || c.Thickness.ToString().Equals(search)).ToListAsync();
        }
    }
}
