using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain.Models
{
    public class Product
    {
        public Product()
        {

        }

        public Product(int quantity, string iD, int length, int width,  int height, string info)
        {
            Quantity = quantity;
            ID = iD;
            Width = width;
            Length = length;
            Height = height;
            Info = info;
        }

        [Required]
        public string ID { get; set; }

        [Range(10, 100000)]
        public int Width { get; set; }

        [Range(10, 100000)]
        public int Length { get; set; }

        [Range(1, 50)]
        public int Height { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public string Info { get; set; }

        public double Area
        {
            get
            {
                return Width * Length;
            }
        }

        public void Rotate()
        {
            int length = Length;
            int width = Width;

            Length = width;
            Width = length;
           
        }

        [NotMapped]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [NotMapped]
        public bool Used { get; set; }

        public override string ToString()
        {
            return "Quantity: " + Quantity + " | Info: " + Info + " | L : " + Length + " | W : " + Width + " | H: " + Height + " | Area: " +  (Quantity * Area).ToString("0.00 mm²", CultureInfo.InvariantCulture) + " | X: " + X + " | Y: " + Y;
        }
    }
}
