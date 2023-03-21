using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Web.Models
{
    public class PlateWithQuantity
    {
        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }

        public Plate Plate { get; set; }
    }
}
