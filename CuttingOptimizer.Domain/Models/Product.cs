using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Product(string iD, int width, int height, string info)
        {
            ID = iD;
            Width = width;
            Height = height;
            Info = info;
        }

        [Required]
        public string ID { get; set; }

        [Range(10, 100000)]
        public int Width { get; set; }

        [Range(10, 100000)]
        public int Length { get; set; }

        [Range(10, 50)]
        public int Height { get; set; }

        public string Info { get; set; }
    }
}
