using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class RestResult
    {
        public int HorizontalQuantity { get; set; }
        public int Rest { get; set; }
        public int VerticalQuantity { get; set; }
        public int Vert { get; set; }
        public bool Fit { get; set; }

        public override string ToString()
        {
            return "HorQ: " + HorizontalQuantity + " | VertQ: " + VerticalQuantity + " | Rest: " + Rest + " | Vert: " + Vert + " | Fit: " + Fit; 
        }
    }
}
