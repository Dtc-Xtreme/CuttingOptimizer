using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public interface ISawRepository : ICuttingOptimizerRepository<Saw>
    {
        public IQueryable<Saw> Saws { get; }
    }
}
