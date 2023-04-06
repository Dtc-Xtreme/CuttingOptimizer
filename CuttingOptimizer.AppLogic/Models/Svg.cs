using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class Svg
    {
        public Svg() { }

        public Svg(int id, ViewBox viewBox, int priority)
        {
            Id = id;
            ViewBox = viewBox;
            Priority = priority;
            Groups = new List<Group>();
        }

        public int Id { get; set; }
        public ViewBox ViewBox { get;set; }
        public List<Group> Groups { get; set; }
        public int Priority { get; set; }

        public void AddGroup(Group group)
        {
            group.Svg = this;
            Groups.Add(group);
        }
    }
}
