using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class CalculatorFormModel
    {
        [ValidateComplexType]
        public Saw Saw { get; set; }

        [ValidateComplexType]
        public List<Plate> Plates { get; set; }
        [ValidateComplexType]
        public List<Product> Products { get; set; }

        public bool Veneer { get; set; }

        public string UserID { get;set; }

        public CalculatorFormModel() {
            Saw = new Saw();
            Plates = new List<Plate>();
            Products = new List<Product>();
        }
    }
}
