using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class CalculatorFormModel
    {
        [ValidateComplexType]
        public List<Plate> Plates { get; set; }
        [ValidateComplexType]
        public List<Product> Products { get; set; }

        public CalculatorFormModel() {
            Plates = new List<Plate>();
            Products = new List<Product>();
        }
    }
}
