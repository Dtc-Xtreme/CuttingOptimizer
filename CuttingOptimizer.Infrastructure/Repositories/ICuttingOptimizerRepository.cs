using CuttingOptimizer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Infrastructure.Repositories
{
    public interface ICuttingOptimizerRepository<T>
    {
        public Task<bool> Create(T item);
        public Task<T?> FindById(string id);
        public Task<bool> Remove(string id);
    }
}
