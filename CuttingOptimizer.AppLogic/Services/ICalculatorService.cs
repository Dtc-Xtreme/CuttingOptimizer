using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public interface ICalculatorService
    {
        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products);

        public List<Product> CombineProductsWithSameDimentions(List<Product> products);
    }
}
