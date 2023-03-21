using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Web.Models
{
    public class ProductWithQuantity
    {
        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
