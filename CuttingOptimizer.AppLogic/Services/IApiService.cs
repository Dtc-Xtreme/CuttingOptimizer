using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public interface IApiService
    {
        public Task<List<Saw>?> GetAllSaws();
        public Task<List<Saw>?> SearchSaws(string search);

        public Task<List<Plate>?> GetAllPlates();

        public Task<Blueprint?> SaveBlueprint(Blueprint quotation);
        public Task<Blueprint?> GetBlueprintById(int id);
    }
}
