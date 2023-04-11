using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class Svg
    {
        public Svg() { 
            ViewBox = new ViewBox();
        }

        public Svg(string id, ViewBox viewBox, int priority)
        {
            ID = id;
            ViewBox = viewBox;
            Priority = priority;
            Groups = new List<Group>();
        }

        public string ID { get; set; }
        public ViewBox ViewBox { get;set; }
        public List<Group> Groups { get; set; }
        public int Priority { get; set; }
        public int Area
        {
            get { 
                return ViewBox.Length * ViewBox.Width; 
            }
        }

        public void AddGroup(Group group)
        {
            group.Svg = this;
            Groups.Add(group);
        }
    }
}
