﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingOptimizer.Domain.Models
{
    public class Plate
    {
        public Plate()
        {

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
        }
        public Plate(int quantity, string id, int length, int width, int height, int trim)
        {
            Quantity = quantity;
            ID = id;
            Width = width;
            Length = length;
            Height = height;
            Trim = trim;
            Products = new List<Product>();
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
            Products = new List<Product>();
        }

        [Required]
        public string ID { get; set; }

        [Range(10, 100000)]
        public int Length { get; set; }

        [Range(10, 100000)]
        public int Width { get; set; }

        [Range(1, 1000)]
        public int Height { get; set; }

        public bool Veneer { get; set; }

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
        [Range(0, 1000)]
        public int Trim { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; }

        [NotMapped]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [NotMapped]
        [Range(0, 10)]
        public int Priority { get; set; }       // Higher the Priority the more important it is
        
        public override string ToString()
        {
            return "Quantity: " + Quantity + " | ID: " + ID + " | L: " + Length + " | W: " + Width + " | H: " + Height + " | Trim: " + Trim + " | Vineer: " + Veneer + " | Priority: " + Priority;
        }
    }
}