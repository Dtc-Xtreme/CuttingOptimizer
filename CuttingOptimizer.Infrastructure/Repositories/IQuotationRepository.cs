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
        public IQueryable<Quotation> Quotes { get; }
        public Task<bool> Create(Quotation item);
        public Task<bool> Update(Quotation quotation);
        public Task<Quotation?> FindById(int id);
        public Task<bool> Remove(int id);
    }
}
