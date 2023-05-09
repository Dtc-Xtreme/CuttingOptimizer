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

        public string Hash
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (Group group in Groups)
                {
                    if (group.Product != null) stringBuilder.Append(group.Product.GetHashCode());
                }
                return stringBuilder.ToString();
            }
        }
    }
}
