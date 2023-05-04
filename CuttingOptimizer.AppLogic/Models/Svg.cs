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

        public void AddGroup(Group group)
        {
            if (group.Product != null) Hash += group.Product.GetHashCode();
            Groups.Add(group);
        }

        public void AddGroups(List<Group> groups)
        {
            foreach (Group group in groups)
            {
                if (group.Product != null) Hash += group.Product.GetHashCode();
            }
            Groups.AddRange(groups);
        }

        public string Hash { get; set; }
    }
}
