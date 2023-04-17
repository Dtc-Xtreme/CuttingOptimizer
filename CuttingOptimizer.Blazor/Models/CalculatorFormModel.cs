using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class CalculatorFormModel
    {
        public Saw Saw { get; set; }
        public List<Plate> Plates { get; set; }
        public List<Product> Products { get; set; }

        public CalculatorFormModel() {
            Saw = new Saw();
            Plates = new List<Plate>();
            Products = new List<Product>();
        }
    }
}
