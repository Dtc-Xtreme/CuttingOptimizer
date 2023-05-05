using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public class PlateRepository : IPlateRepository
    {
        private CuttingOptimizerDbContext context;
        public IQueryable<Plate> Plates => context.Plates;

        public PlateRepository(CuttingOptimizerDbContext ctx)
        {
            this.context = ctx;
        }

        public async Task<bool> Create(Plate plate)
        {
            context.Plates.Add(plate);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }

        public async Task<Plate?> FindById(string id)
        {
            return await Plates.FirstOrDefaultAsync(c => c.ID.ToLower().Equals(id.ToLower()));
        }

        public async Task<bool> Remove(string id)
        {
            Plate? plate = await Plates.FirstOrDefaultAsync(c => c.ID.ToLower().Equals(id.ToLower()));
            if (plate != null) context.Plates.Remove(plate);
            context.Plates.Remove(plate);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }
    }
}
