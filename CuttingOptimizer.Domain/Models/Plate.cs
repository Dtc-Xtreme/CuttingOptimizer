using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingOptimizer.Domain.Models
{
    public class Plate
    {
        public Plate()
        {

        }
        public Plate(string id, int length, int width, int height)
        {
            ID = id;
            Width = width;
            Length = length;
            Height = height;
        }

        [Required]
        public string ID { get; set; }

        [Range(0, 100000)]
        public int Length { get; set; }

        [Range(10, 100000)]
        public int Width { get; set; }

        [Range(0, 1000)]
        public int Height { get; set; }

        public bool Veneer { get; set; }

        [NotMapped]
        [Range(0, 1000)]
        public int Trim { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; }

        public int WidthWithTrim { 
            get { 
                return Width - Trim; 
            } 
        }

        public int LengthWithTrim
        {
            get
            {
                return Length - Trim;
            }
        }

        public override string ToString()
        {
            return "ID: " + ID + " | L: " + Length + " | W: " + Width + " | H: " + Height + " | Trim: " + Trim + " | Vineer: " + Veneer;
        }
    }
}