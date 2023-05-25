using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private CuttingOptimizerDbContext context;
        public IQueryable<Blueprint> Quotes => context.Blueprints;

        public QuotationRepository(CuttingOptimizerDbContext ctx)
        {
            context = ctx;
        }

        public async Task<bool> Create(Blueprint quotation)
        {
            context.Blueprints.Add(quotation);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }

        public async Task<bool> Update(Blueprint quotation)
        {
            Blueprint? selected = await context.Blueprints.FirstOrDefaultAsync(c=>c.ID == quotation.ID);
            if(selected != null)
            {
                selected.JsonString = quotation.JsonString;
                return await context.SaveChangesAsync() == 0 ? false : true;
            }
            return false;
        }

        public async Task<Blueprint?> FindById(int id)
        {
            return await Quotes.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<bool> Remove(int id)
        {
            Blueprint? quotation = await Quotes.FirstOrDefaultAsync(c => c.ID == id);
            if(quotation != null) context.Blueprints.Remove(quotation);
            return await context.SaveChangesAsync() == 0 ? false : true;
        }
    }
}
