using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingOptimizer.Domain.Models
{
    public class Plate
    {
        public Plate()
        {

        }
        public Plate(Plate plate)
        {
            ID = plate.ID;
            Length = plate.Length;
            Width = plate.Width;
            Height = plate.Height;
            Base = plate.Base;
            Quantity = plate.Quantity;
            Priority = plate.Priority;
            Base = plate.Base;
            Veneer = plate.Veneer;
            Trim = plate.Trim;
        }
        public Plate(bool basePlate)
        {
            Quantity = 1;
            Base = basePlate;
        }
        public Plate(string id, int length, int width, int height)
        {
            ID = id;
            Length = length;
            Width = width;
            Height = height;
        }
        public Plate(int quantity, Plate plate)
        {
            Quantity = quantity;
            ID = plate.ID;
            Length = plate.Length;
            Width = plate.Width;
            Height = plate.Height;
            Veneer = plate.Veneer;
            Trim = plate.Trim;
            Base = plate.Base;
        }
        public Plate(int quantity, string id, int length, int width, int height, int trim)
        {
            Quantity = quantity;
            ID = id;
            Width = width;
            Length = length;
            Height = height;
            Trim = trim;
        }
        public Plate(int quantity, string id, int length, int width, int height, int trim, int priority)
        {
            Quantity = quantity;
            ID = id;
            Width = width;
            Length = length;
            Height = height;
            Trim = trim;
            Priority = priority;
        }
        public Plate(int quantity, string id, int length, int width, int height, int trim, int priority, bool basePlate)
        {
            Quantity = quantity;
            ID = id;
            Width = width;
            Length = length;
            Height = height;
            Trim = trim;
            Priority = priority;
            Base = basePlate;
        }

        [Required]
        public string ID { get; set; }

        [Required]
        [Range(10, 100000)]
        public int Length { get; set; }

        [Required]
        [Range(10, 100000)]
        public int Width { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Height { get; set; }

        [NotMapped]
        public bool Veneer { get; set; }

        [NotMapped]
        public bool Base { get; set; }

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

        public int Area
        {
            get { return Length * Width; }
        }

        public int AreaWithTrim
        {
            get { return LengthWithTrim * WidthWithTrim; }
        }

        [NotMapped]
        [Required]
        [Range(0, 1000)]
        public int Trim { get; set; }

        [NotMapped]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [NotMapped]
        [Required]
        [Range(0, 10)]
        public int Priority { get; set; }       // Higher the Priority the more important it is
        
        public override string ToString()
        {
            return "Quantity: " + Quantity + " | ID: " + ID + " | L: " + Length + " | W: " + Width + " | H: " + Height + " | Trim: " + Trim + " | Vineer: " + Veneer + " | Priority: " + Priority;
        }
    }
}