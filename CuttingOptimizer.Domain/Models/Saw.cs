using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain.Models
{
    public class Saw
    {
        public Saw() { }

        public Saw(string id, int thickness) {
            ID = id;
            Thickness = thickness;
        }

        [Required]
        public string ID { get; set; }

        [Required]
        [Range(1,50)]
        public int Thickness { get; set; }

        public override string ToString()
        {
            return "ID: " + ID + " | Thickness: " + Thickness;
        }
    }
}
