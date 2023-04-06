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
        public int Area(int width, int length);
        //public bool CheckIfFits(Plate plate, Product product);
        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products);
    }
}
