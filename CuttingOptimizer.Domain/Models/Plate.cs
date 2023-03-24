using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingOptimizer.Domain.Models
{
    public class Plate
    {
        public Plate()
        {

        }

        public Plate(string id, int width, int length, int height)
        {
            ID = id;
            Width = width;
            Length = length;
            Height = height;
        }

        [Required]

        public string ID { get; set; }

        [Range(10, 100000)]
        public int Width { get; set; }

        [Range(0, 100000)]
        public int Length { get; set; }

        [Range(0, 1000)]
        public int Height { get; set; }

        [NotMapped]
        [Range(0, 1000)]
        public int Trim { get; set; }

        public bool Veneer { get; set; }

    }
}