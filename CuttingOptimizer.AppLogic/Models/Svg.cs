using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class Svg
    {
        private int groupNr = 1;
        public Svg()
        {
            Groups = new List<Group>();
            ViewBox = new ViewBox();
        }

        public Svg(string id, ViewBox viewBox, int priority, Plate pl)
        {
            ID = id;
            ViewBox = viewBox;
            Priority = priority;
            Groups = new List<Group>();
            Plate = pl;
        }

        public string ID { get; set; }
        public ViewBox ViewBox { get; set; }
        public List<Group> Groups { get; set; }
        public int Priority { get; set; }
        public int Area
        {
            get
            {
                return ViewBox.Length * ViewBox.Width;
            }
        }
        public Plate Plate { get; set; }
        public double Scale { get; set; }
        public void AddGroup(Group group)
        {
            if (Groups.Count() != 0)
            {
                group.ID = groupNr;
                groupNr++;
            }

            group.Svg = this;
            Groups.Add(group);
        }

        public int CutLines
        {
            get
            {
                var x = Groups.Where(c => c.X != 0).GroupBy(c => c.X).Count();
                var y = Groups.Where(c => c.Y != 0).GroupBy(c => c.Y).Count();
                return x + y; // + 4 voor haaksnijde
            }
        }
        public int CutLineLength {
            get {
                var omtrek = (ViewBox.Length * 2) + (ViewBox.Width * 2);
                var x = Groups.Where(c=>c.X != 0).GroupBy(c => c.X);
                var y = Groups.Where(c=>c.Y != 0).GroupBy(c => c.Y);
                int total = 0;
                foreach (var x2 in x)
                {
                    total += x2.Sum(c => c.Width);
                }
                foreach (var y2 in y)
                {
                    total += y2.Sum(c => c.Length);
                }
                return total; // + omtrek??
            } 
        }
        public double AreaLossPercentage
        {
            get
            {
                double percentage = Groups.Where(c => c.ID == 0).Sum(c => c.Area) / (double)Area;
                return percentage > 0 ? percentage : 0;
            }
        }
        public string Hash
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (Group group in Groups)
                {
                    if (group.Product != null) stringBuilder.Append(group.Product.GetHashCode());
                }
                return stringBuilder.ToString() + this.Plate.ID + this.Plate.Length + this.Plate.Width + this.Plate.Height;
            }
        }
    }
}