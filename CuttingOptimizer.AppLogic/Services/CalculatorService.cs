using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int Area(int width, int length)
        {
            return width * length;
        }

        public bool CheckIfFits(Plate plate, Product product)
        {
            bool wr = false;
            bool lr = false;

            if(plate.Products == null || plate.Products.Count == 0)
            {
                wr = plate.WidthWithTrim > product.Width;
                lr = plate.LengthWithTrim > product.Length;
                if(wr && lr)
                {
                    plate.Products = new List<Product>();
                    plate.Products.Add(product);
                }
            }

            return wr && lr;
        }
    }
}
