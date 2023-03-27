
using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class CalculatorModel
    {
        public Saw Saw { get; set; }
        public Plate Plate { get; set; }
        public Product Product { get; set; }
    }
}
