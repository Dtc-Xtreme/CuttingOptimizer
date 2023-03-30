using CuttingOptimizer.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class ProductQuantity
    {
        public ProductQuantity()
        {
            
        }
        public ProductQuantity(int quantity, Product product)
        {
            Quantity = quantity;
            Product = product;
        }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
