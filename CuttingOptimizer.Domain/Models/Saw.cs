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

        public Saw(Saw saw)
        {
            ID = saw.ID;
            Thickness = saw.Thickness;
        }

        public Saw(string id, int thickness) {
            ID = id;
            Thickness = thickness;
        }

        [Required(ErrorMessage = "Zaagblad is een verplicht veld.")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Zaagsnede is een verplicht veld.")]
        [Range(1,50, ErrorMessage = "Zaagsneden moet tussen 1 en 50 zijn.")]
        public int Thickness { get; set; }

        public override string ToString()
        {
            return "ID: " + ID + " | Thickness: " + Thickness;
        }
    }
}