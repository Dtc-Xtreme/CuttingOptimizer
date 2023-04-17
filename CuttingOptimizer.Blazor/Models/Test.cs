using System.ComponentModel.DataAnnotations;

namespace CuttingOptimizer.Blazor.Models
{
    public class Test
    {
        [Required]
        [Range(5,10)]
        public int A { get; set; }

        public int B { get; set; }

    }
}
