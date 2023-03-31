using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class ViewBox
    {
        public ViewBox() { }

        public ViewBox(int x, int y, int length, int width)
        {
            X = x;
            Y = y;
            Length = length;
            Width = width;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
    }
}
