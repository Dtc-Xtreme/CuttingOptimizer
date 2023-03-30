using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class PlateQuantity
    {
        public PlateQuantity()
        {
            
        }
        public PlateQuantity(int quantity, Plate plate)
        {
            Quantity = quantity;
            Plate = plate;
        }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public Plate Plate { get; set; }
    }
}
