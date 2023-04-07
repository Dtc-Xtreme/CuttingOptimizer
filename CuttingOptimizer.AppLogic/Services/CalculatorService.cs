using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int Area(int width, int length)
        {
            throw new NotImplementedException();
        }

        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products)
        {
            // Creeer svgs and sorting
            List<Svg> svgs = Init(plates);

            // Keep looping throug products till it's empty
            // Remove when added or reduce the quantity of places groups/rectangles
            while (products.Count > 0)
            {
                List<Group> groups = SearchFits(svgs, products[0], saw);

                //if (products[0].Quantity > 1)
                //{
                    CalculateProductQuantityManyWithRest(groups, saw, products);

                //}else if (products[0].Quantity == 1)
                //{
                //    CalculateProductQuantityOneWithRest(groups, saw, products);
                //}
            }

            return svgs;
        }

        private List<Svg> Init(List<Plate> plates)
        {
            List<Svg> svgs = new List<Svg>();

            foreach (Plate plate in plates)
            {
                svgs.Add(new Svg(0, new ViewBox(0, 0, plate.LengthWithTrim, plate.WidthWithTrim), plate.Priority));
            }

            foreach (Svg svg in svgs)
            {
                svg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));
            }

            return svgs;
        }
        private Svg AddSvg(Svg svg)
        {
            Svg newSvg = new Svg(0, new ViewBox(0, 0, svg.ViewBox.Length, svg.ViewBox.Width), svg.Priority);
            newSvg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));

            return svg;
        }
        private List<Group> SearchFits(List<Svg> svgs, Product product, Saw saw)
        {
            List<Group> fitGroups = new List<Group>();

            while(fitGroups.Count == 0)
            {
                foreach (Svg svg in svgs)
                {
                    fitGroups.AddRange(
                        svg.Groups.Where(c => c.Length >= product.Length && c.Width >= product.Width && c.Id == 0));
                }

                if (fitGroups.Count == 0)
                {
                    svgs.Add(svgs.MaxBy(c => c.Priority));
                }
            }

            return fitGroups;
        }
        private Dictionary<Group, RestResult> CalculateGroupPosibilities(Saw saw, Product product, List<Svg> svgs)
        {
            Dictionary<Group, RestResult> results = new Dictionary<Group, RestResult>();



            foreach (Svg svg in svgs) { 
                foreach(Group group in svg.Groups)
                {
                    //RestResult rest = new RestResult {
                    //    HorizontalQuantity = CalculateQuantityHorizontal(saw, group, product),
                    //    HorizontalRest = 0,
                    //    VerticalQuantity = CalculateQuantityVertical(saw, group, product),
                    //    VerticalRest =  0
                    //};

                    //results.Add(group, rest); 
                }
            }

            return results;
        }
        private int CalculateQuantityHorizontal(Saw saw, Group group, Product product)
        {
            bool test = true;
            int amount = 0;
            int rest = group.Length;
            while (test)
            {
                if(amount == 0)
                {
                    rest -= product.Length;
                }
                else
                {
                    rest -= (saw.Thickness + product.Length);

                }

                if (rest > 0)
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
        private int CalculateQuantityVertical(Saw saw, Group group, Product product)
        {
            bool test = true;
            int amount = 0;
            int rest = group.Width;


            while (test)
            {
                if(amount == 0)
                {
                    rest -= product.Width;
                }
                else
                {
                    rest -= (saw.Thickness + product.Width);
                }

                if (rest > 0)
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
        private void CalculateProductQuantityOneWithRest(List<Group> groups, Saw saw, List<Product> products)
        {
            List<Group> newGroups = new List<Group>();
            Group selectedGroup = groups[0];
            Product selectedProduct = products[0];

            // Group with product
            Group groupWithProduct = new Group(1, new Rectangle(0, selectedGroup.X, selectedGroup.Y, selectedProduct.Length, selectedProduct.Width, new Label(selectedProduct.Info)), selectedGroup.X, selectedGroup.Y, selectedProduct.Length, selectedProduct.Width);
            groupWithProduct.Svg = selectedGroup.Svg;
            newGroups.Add(groupWithProduct);

            //CaclulateGroupRight(saw, selectedGroup, groupWithProduct, newGroups);

            // Group right of product
            int groupRightX = groupWithProduct.X + groupWithProduct.Length + 1 + saw.Thickness;
            int groupRightY = groupWithProduct.Y;
            int groupRightLength = selectedGroup.Svg.ViewBox.Length - groupWithProduct.X - groupWithProduct.Length - saw.Thickness;
            int groupRightWidth = groupWithProduct.Width;
            Group groupRight = new Group(0, new Rectangle(0, groupRightX, groupRightY, groupRightLength, groupRightWidth, new Label("0")), groupRightX, groupRightY, groupRightLength, groupRightWidth);
            groupRight.Svg = selectedGroup.Svg;
            newGroups.Add(groupRight);

            if (groupWithProduct.Width != selectedGroup.Width)
            {
                //CalculateGroupUnder(saw,selectedGroup, groupWithProduct, newGroups, selectedProduct);

                // Group under product
                int groupUnderX = groupWithProduct.X;
                int groupUnderY = groupWithProduct.Y + groupWithProduct.Width + 1 + saw.Thickness;
                int groupUnderLength = selectedGroup.Length;
                int groupUnderWidth = selectedGroup.Width - groupWithProduct.Width - saw.Thickness;
                //if (groupUnderY + groupUnderWidth < svg.ViewBox.Width) groupUnderWidth -= saw.Thickness;
                Group groupUnder = new Group(0, new Rectangle(0, groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth, new Label("0")), groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth);
                groupUnder.Svg = selectedGroup.Svg;
                newGroups.Add(groupUnder);
            }

            selectedGroup.Svg.Groups.AddRange(newGroups);
            selectedGroup.Svg.Groups.Remove(selectedGroup);
            products.Remove(products[0]);

        }
        private void CalculateProductQuantityManyWithRest(List<Group> groups, Saw saw, List<Product> products)
        {
            List<Group> newGroups = new List<Group>();
            Group selectedGroup = groups.First();
            Product selectedProduct = products[0];


            // Groeperingen maken voor products met 2 of meer quantity
            int maxHorizontal = CalculateQuantityHorizontal(saw, selectedGroup, selectedProduct);
            int maxVertical = CalculateQuantityVertical(saw, selectedGroup, selectedProduct);

            int length = 0;
            int width = 0;
            int x = 0;
            int y = 0;

            Group lastCreated = new();

            #region
            //if (maxHorizontal >= selectedProduct.Quantity)
            //{
            //    // alles kan op een lijn.
            //    for (int i = 0; i < selectedProduct.Quantity; i++)
            //    {
            //        length = selectedProduct.Length;
            //        width = selectedProduct.Width;
            //        x = selectedGroup.X + lastCreated.X + lastCreated.Length;
            //        y = selectedGroup.Y;

            //        if (i > 0) x += saw.Thickness + 1;

            //        lastCreated = new Group(1, new Rectangle(1, x, y, length, width, new Label("1")), x, y, length, width);
            //        lastCreated.Svg = selectedGroup.Svg;

            //        newGroups.Add(lastCreated);
            //    }

            //    CaclulateGroupRight(saw, selectedGroup, lastCreated, newGroups);
            //    CalculateGroupUnder(saw, selectedGroup, lastCreated, newGroups, selectedProduct);

            //    //}else if (product.Quantity % maxHorizontal == 0) {
            //    //    // even aantal over meerdere lijnen

            //}
            //else
            #endregion
            if (maxVertical >= selectedProduct.Quantity)
            {
                // passen niet allemaal horizontaal dus verticale proberen

                for (int i = 0; i < selectedProduct.Quantity; i++)
                {
                    length = selectedProduct.Length;
                    width = selectedProduct.Width;
                    x = selectedGroup.X;
                    y = selectedGroup.Y + lastCreated.Y + lastCreated.Width;

                    if (i > 0) y += saw.Thickness + 1;

                    lastCreated = new Group(1, new Rectangle(1, x, y, length, width, new Label("1")), x, y, length, width);
                    lastCreated.Svg = selectedGroup.Svg;

                    newGroups.Add(lastCreated);
                }
                CaclulateGroupRight(saw, selectedGroup, lastCreated, newGroups);
                CalculateGroupUnder(saw, selectedGroup, lastCreated, newGroups, selectedProduct);
            }

            selectedGroup.Svg.Groups.AddRange(newGroups);
            selectedGroup.Svg.Groups.Remove(selectedGroup);
            products.Remove(selectedProduct);
        }
        private void CaclulateGroupRight(Saw saw, Group group, Group lastCreated, List<Group> newGroups)
        {
            bool horizontalRun = newGroups.GroupBy(c=>c.Y).Count() == 1;
            int length = 0;
            if(horizontalRun)
            {
                length = group.Length - (lastCreated.Length * newGroups.Count) - ((saw.Thickness + 1) * newGroups.Count);
            }
            else
            {
                length = group.Length - lastCreated.Length - saw.Thickness - 1;
            }

            int width = lastCreated.Y + lastCreated.Width;
            int x = lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
            int y = group.Y;

            lastCreated = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
            lastCreated.Svg = group.Svg; ;
            newGroups.Add(lastCreated);
        }
        private void CalculateGroupUnder(Saw saw, Group group, Group lastCreated, List<Group> newGroups, Product product)
        {
            if(lastCreated.Y + lastCreated.Width != group.Width)
            {
                int length = group.Length;
                int width = group.Width - (lastCreated.Y + lastCreated.Width + saw.Thickness + 1);
                int x = group.X;
                int y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;

                lastCreated = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
                lastCreated.Svg = group.Svg;
                newGroups.Add(lastCreated);
            }
        }
    }
}
