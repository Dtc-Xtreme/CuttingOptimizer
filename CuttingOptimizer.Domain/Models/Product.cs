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

        public Product(int quantity)
        {
            Quantity = quantity;
        }

        public Product(int quantity, Product product)
        {
            Quantity = quantity;
            ID = product.ID;
            Width   = product.Width;
            Length = product.Length;
            Height  = product.Height;
        }
        public Product(int quantity, string id, int length, int width,  int height/*, string info*/)
        {
            Quantity = quantity;
            ID = id;
            Width = width;
            Length = length;
            Height = height;
        }

        [Required(ErrorMessage = "Item is een verplicht veld.")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Item breedte is een verplicht veld.")]
        [Range(1, int.MaxValue, ErrorMessage = "Item breedte moet groter zijn dan 0.")]
        public int Width { get; set; }

        [Required(ErrorMessage = "Item lengte is een verplicht veld.")]
        [Range(1, int.MaxValue, ErrorMessage = "Item lengte moet groter zijn dan 0.")]
        public int Length { get; set; }

        [Required(ErrorMessage = "Item dikte is een verplicht veld.")]
        [Range(0, int.MaxValue, ErrorMessage = "Item dikte moet groter zijn dan 0.")]
        public int Height { get; set; }

        public double Area
        {
            get
            {
                return (Width * Length) * Quantity;
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
        [Range(1, int.MaxValue, ErrorMessage = "Item dikte moet groter zijn dan 0.")]
        public int Quantity { get; set; }

        public override string ToString()
        {
            return "Quantity: " + Quantity + " | ID: " + ID + " | L : " + Length + " | W : " + Width + " | H: " + Height + " | Area: " +  (Quantity * Area).ToString("0.00 mm²", CultureInfo.InvariantCulture);
        }

        public Product ShallowCopy()
        {
            return (Product)this.MemberwiseClone();
        }
        public void SwitchHeightAndWidth()
        {
            int rest = this.Length;
            this.Length = this.Width;
            this.Width = rest;
        }
    }
}
