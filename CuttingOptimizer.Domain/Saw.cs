using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain
{
    public class Saw
    {
        public Saw() { }

        public Saw(string id, int width) {
            ID = id;
            Width = width;
        }

        public string ID { get; set; }
        public int Width { get; set; }
    }
}
