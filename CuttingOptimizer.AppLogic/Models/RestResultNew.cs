using CuttingOptimizer.Domain.Models;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace CuttingOptimizer.AppLogic.Models
{
    public enum RestResultType
    {
        Horizontal,
        HorizontalRotated,
        Vertical,
        VerticalRotated,
        BlockHorizontal,
        BlockHorizontalRotated,
        BlockVertical,
        BlockVerticalRotated,
    }

    public class NewRestResult
    {
        public NewRestResult(Group group, Product product, Saw saw, RestResultType type)
        {
            Group = group;
            Product = product;
            Saw = saw;
            Type = type;

            switch (Type)
            {
                case RestResultType.Horizontal:
                    CalculateHorizontal();
                    break;
                case RestResultType.HorizontalRotated:
                    CalculateHorizontalRotated();
                    break;
                case RestResultType.Vertical:
                    CalculateVertical();
                    break;
                case RestResultType.VerticalRotated:
                    CalculateVerticalRotated();
                    break;
                case RestResultType.BlockHorizontal:
                    CalculateBlockHorizontal();
                    break;
                case RestResultType.BlockHorizontalRotated:
                    CalculateBlockHorizontalRotated();
                    break;
                case RestResultType.BlockVertical:
                    CalculateBlockVertical();
                    break;
                case RestResultType.BlockVerticalRotated:
                    CalculateBlockVerticalRotated();
                    break;
            }
        }

        public RestResultType Type { get; set; }
        public int MaxQuantity { get; set; }
        public int MaxHorizontalQuantity { get; set; }
        public int MaxVerticalQuantity { get; set; }
        public int Quantity { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }

        public int HorizontalRestLine { get; set; }
        public int VerticalRestLine { get; set; }

        public bool HorizontalAlignment
        {
            get
            {
                return VerticalRestLine > HorizontalRestLine ? true : false;
            }
        }
        public bool Rotated { get; set; }

        public Group Group { get; set; }
        public Product Product { get; set; }
        public Saw Saw { get; set; }

        #region Calculations
        private void CalculateHorizontal()
        {
            MaxQuantity = CalculateMaxQuantityHorizontal();
            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Columns = Quantity;
            Rows = Quantity > 0 ? 1 : 0;
            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns));
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows));
        }
        private void CalculateHorizontalRotated()
        {
            MaxQuantity = CalculateMaxQuantityVertical();
            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Columns = Quantity > 0 ? 1 : 0;
            Rows = Quantity;
            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns));
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows));
            Rotated = true;
        }
        private void CalculateVertical()
        {
            MaxQuantity = CalculateMaxQuantityVertical();
            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Columns = Quantity > 0 ? 1 : 0;
            Rows = Quantity;
            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns));
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows));
        }
        private void CalculateVerticalRotated()
        {
            MaxQuantity = CalculateMaxQuantityHorizontal();
            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Columns = Quantity;
            Rows = Quantity > 0 ? 1 : 0;
            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns));
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows));
            Rotated = true;
        }

        private void CalculateBlockHorizontal()
        {
            MaxHorizontalQuantity = CalculateMaxQuantityHorizontal();
            MaxVerticalQuantity = CalculateMaxQuantityVertical();
            MaxQuantity = MaxHorizontalQuantity * MaxVerticalQuantity;

            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Columns = Quantity < MaxHorizontalQuantity ? Quantity : MaxHorizontalQuantity;
            Rows = Quantity != 0 && Columns != 0 ? Quantity / Columns : 0;

            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns)); // bereken opp ipv lengte of breedte
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows)); // bereken opp ipv lengte of breedte
            Quantity = Columns * Rows;
        }
        private void CalculateBlockHorizontalRotated()
        {
            MaxHorizontalQuantity = CalculateMaxQuantityHorizontal();
            MaxVerticalQuantity = CalculateMaxQuantityVertical();
            MaxQuantity = MaxHorizontalQuantity * MaxVerticalQuantity;

            Quantity = Product.Quantity < MaxQuantity ? Product.Quantity : MaxQuantity;
            Rows = Quantity < MaxVerticalQuantity ? Quantity : MaxVerticalQuantity;
            Columns = Quantity != 0 && Rows !=0 ? Quantity / Rows : 0;
            
            HorizontalRestLine = Group.Length - ((Product.Length * Columns) + ((Saw.Thickness + 1) * Columns)); // bereken opp ipv lengte of breedte
            VerticalRestLine = Group.Width - ((Product.Width * Rows) + ((Saw.Thickness + 1) * Rows)); // bereken opp ipv lengte of breedte
            Quantity = Columns * Rows;
            Rotated = true;
        }
        private void CalculateBlockVertical()
        {

        }
        private void CalculateBlockVerticalRotated()
        {

        }

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
        #endregion

        #region Sorting
        public int Area
        {
            get
            {
                return Quantity * Product.Area;
            }
        }
        public int Size()
        {
            return Product.Width > Product.Length ? Product.Width : Product.Length;
        }
        public int OrderOnRest()
        {
            return HorizontalRestLine < VerticalRestLine ? HorizontalRestLine : VerticalRestLine;
        }
        #endregion
    }
}
