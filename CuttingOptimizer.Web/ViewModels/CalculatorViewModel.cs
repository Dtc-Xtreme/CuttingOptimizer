using CuttingOptimizer.Domain;
using CuttingOptimizer.Domain.Models;
using CuttingOptimizer.Web.Models;

namespace CuttingOptimizer.Web.ViewModels
{
    public class CalculatorViewModel
    {
        public Saw Saw { get; set; }
        public List<PlateWithQuantity> Plates { get; set; }
        public List<ProductWithQuantity> Products { get; set; }
    }
}
