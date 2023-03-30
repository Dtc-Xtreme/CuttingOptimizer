using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    internal class VerticalResult
    {
        public VerticalResult()
        {
            
        }

        public VerticalResult(int quantity, int rest)
        {
            Quantity = quantity;
            Rest = rest;
        }

        public int Quantity { get; set; }
        public int Rest { get; set; }
    }
}
