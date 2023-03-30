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
        public bool PlaceNext(Saw saw, List<Plate> plates, Product product);
        public bool PlaceNextInBundle(Saw saw, List<Plate> plates, Product product);
    }
}
