using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class Rectangle
    {
        public Rectangle()
        {
            
        }
        public Rectangle(int id, int x, int y, int length, int width, Label label)
        {
            Id = id;
            X = x;
            Y = y;
            Length = length;
            Width = width;
            Label = label;
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public Label Label { get; set; }
    }
}
