using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class Group
    {
        public Group()
        {
            
        }
        public Group(int id, int x, int y, int length, int width)
        {
            Id = id;
            X = x;
            Y = y;
            Length = length;
            Width = width;
        }
        public Group(int id, Rectangle rectangle, int x, int y, int length, int width)
        {
            Id = id;
            Rectangle = rectangle;
            X = x;
            Y = y;
            Length = length;
            Width = width;
        }

        public int Id { get; set; }
        public Rectangle Rectangle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public Svg Svg { get; set; }
        public int Area
        {
            get { return Length * Width; }
        }

        public override string ToString()
        {
            return "L: " + Length + " | W: " + Width + " | Area: " + Area + " | ID: " + Id + " | SVG Prio: " + Svg.Priority;
        }
    }
}
