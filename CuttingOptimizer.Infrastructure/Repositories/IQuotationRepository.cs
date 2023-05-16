using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public interface IQuotationRepository
    {
        public IQueryable<Blueprint> Quotes { get; }
        public Task<bool> Create(Blueprint item);
        public Task<bool> Update(Blueprint quotation);
        public Task<Blueprint?> FindById(int id);
        public Task<bool> Remove(int id);
    }
}
