using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Models
{
    public enum RestResultType
    {
        Horizontal,
        HorizontalRotated,
        Vertical,
        VerticalRotated
    }

    public class NewRestResult
    {
        //Max horizontal
        //Max vertical
        //QTY horizontal
        //QTY vertical

        public NewRestResult(Group group, Product product, Saw saw, bool rotated=false)
        {
            Group = group;
            Product = product;
            Saw = saw;
            Rotated = rotated;

            HorizontalMaxQuantity = CalculateMaxQuantityHorizontal();
            HorizontalQuantity = Product.Quantity < HorizontalMaxQuantity ? Product.Quantity : HorizontalMaxQuantity;

            VerticalMaxQuantity = CalculateMaxQuantityVertical();
            VerticalQuantity = Product.Quantity < VerticalMaxQuantity ? Product.Quantity : VerticalMaxQuantity;

            if (HorizontalAlignment)
            {
                RestLineHorizontal = Group.Length - ((Product.Length * HorizontalQuantity) - ((HorizontalQuantity - 1) * Saw.Thickness)) - Saw.Thickness - 1;
                RestLineVertical = Group.Width - Product.Width;
            }
            else
            {
                RestLineHorizontal = Group.Length - Product.Length;
                RestLineVertical = Group.Width - ((Product.Width * VerticalQuantity) - ((VerticalQuantity - 1) * Saw.Thickness)) - Saw.Thickness - 1;
            }
        }
        public RestResultType Type { get; set; }
        public int HorizontalMaxQuantity { get; set; }
        public int HorizontalQuantity { get; set; }

        public int VerticalMaxQuantity { get; set; }
        public int VerticalQuantity { get; set; }

        public int RestLineHorizontal { get; set; }
        public int RestLineVertical { get; set; }

        public bool HorizontalAlignment
        {
            get
            {
                if(Rotated)
                {
                    return Product.Length > Product.Width ? false : true;
                }
                else
                {
                    return Product.Length > Product.Width ? true : false;
                }
            }
        }
        public bool Rotated { get; set; }

        public Group Group { get; set; }
        public Product Product { get; set; }
        public Saw Saw { get; set; }

        private int CalculateMaxQuantityHorizontal()
        {
            bool test = true;
            int amount = 0;
            int rest = Group.Length;
            while (test && Group.Width >= Product.Width)
            {
                if (amount == 0)
                {
                    rest -= Product.Length;
                }
                else
                {
                    rest -= (Saw.Thickness + Product.Length + 1);

                }

                if (rest >= 0)
                {
                    amount++;
                }
                else
                {
                    test = false;
                }
            }
            return amount;
        }
        private int CalculateMaxQuantityVertical()
        {
            bool test = true;
            int amount = 0;
            int rest = Group.Width;


            while (test && Group.Length >= Product.Length)
            {
                if (amount == 0)
                {
                    rest -= Product.Width;
                }
                else
                {
                    rest -= (Saw.Thickness + Product.Width + 1);
                }

                if (rest >= 0)
                {
                    amount++;
                }
                else
                {
                    test = false;
                }
            }
            return amount;
        }

        public bool BestFitHorizontal(NewRestResult result)
        {
            return RestLineHorizontal > result.RestLineHorizontal;
        }
        public bool BestFitVertical(NewRestResult result)
        {
            return RestLineVertical > result.RestLineVertical;
        }
    }
}
