using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public class RestResult
    {
        public int MaxHorizontalQuantity { get; set; }
        public int HorizontalScaleVsVertical { get; set; }
        public int HorizontalQuantity
        {
            get; set;
            //get { 
            //    return MaxHorizontalQuantity * HorizontalScaleVsVertical; 
            //} 
        }
        public int HorizontalArea
        {
            get
            {
                return HorizontalQuantity * Product.Area;
            }
        }
        public double HorizontalUsed
        {
            get
            {
                double a = HorizontalArea / (double)Group.Area;
                return a <= 0 ? 0 : a;
            }
        }

        public int MaxVerticalQuantity { get; set; }
        public int VerticalScaleVsHorizontal { get; set; }
        public int VerticalQuantity { get; set; }
        public int VerticalArea
        {
            get
            {
                return VerticalQuantity * Product.Area;
            }
        }
        public double VerticalUsed
        {
            get
            {
                double a = VerticalArea / (double)Group.Area;
                return a <= 0 ? 0 : a;
            }
        }

        public int Rest { get; set; }

        public bool Rotated { get; set; }
        public Group Group { get; set; }
        public Product Product { get; set; }

        public int CompareHighestArea()
        {
            return this.HorizontalArea > this.VerticalArea ? this.HorizontalArea : this.VerticalArea;
        }

        public double CompareBestCandidate()
        {
            return this.HorizontalUsed > this.VerticalUsed ? this.HorizontalUsed : this.VerticalUsed;
        }

        public int CompareMostPossible()
        {
            return this.HorizontalQuantity > this.VerticalQuantity ? this.HorizontalQuantity : this.VerticalQuantity;
        }

        public override string ToString()
        {
            return "Prio: " + Group.Svg.Priority + " | HorMaxQ: " + MaxHorizontalQuantity + " | : HvsV: " + HorizontalScaleVsVertical + " | HUsed: " + HorizontalUsed + " | HArea: " + HorizontalArea + " | HorQ: " + HorizontalQuantity + " | VertMaxQ: " + MaxVerticalQuantity + " | : VvsH: " + VerticalScaleVsHorizontal + " | VUsed: " + VerticalUsed + " | VArea: " + VerticalArea + " | VertQ: " + VerticalQuantity + " | Rest: " + Rest + " | Rotated: " + Rotated + " | Product: " + Product.ToString();
        }
    }
}
