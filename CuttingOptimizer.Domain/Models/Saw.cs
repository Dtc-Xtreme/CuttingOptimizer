using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain
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

        [Range(0,10)]
        public int Thickness { get; set; }
    }
}
