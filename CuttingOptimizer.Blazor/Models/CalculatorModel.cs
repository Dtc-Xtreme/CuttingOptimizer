
using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class CalculatorModel
    {
        public Saw Saw { get; set; }

        public PlateQuantity Plate { get; set; }

        public PlateQuantity Product { get; set; }
    }
}
