using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class Calculator2Service : ICalculatorService
    {
        public int Area(int width, int length)
        {
            throw new NotImplementedException();
        }

        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products)
        {   // plaat een object die alles inneemt
            // plaatst het grootste object eerst.
            // maak nog 2 andere objecten aan met de overschot.

            // Creeer svgs
            List<Svg> svgs = new List<Svg>();

            foreach (Plate plate in plates)
            {
                svgs.Add(new Svg(0, new ViewBox(0,0, plate.LengthWithTrim, plate.WidthWithTrim), plate.Priority));
            }

            // Creeer Eerste group per svg
            // Eerste is altijd 1 group met totale oppvlakte
            foreach(Svg svg in svgs)
            {
                svg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));
            }

            // Sorteer op hoogte en max lengte 
            SortProducts(products, svgs);

            products = products.OrderByDescending(c=>c.Quantity * c.Area + (saw.Thickness * (c.Quantity-1))).ToList();
            //products = products.OrderByDescending(c=>c.Width).ToList();

            foreach (Product product in products)
            {
                //for (int i = 0; i < product.Quantity; i++)
                //{
                    SearchFit(svgs, product, saw);
                //}
            }

            // while loop door products list en verwijder wat gedaan is of reduceer de quantiteit van een product.
            // 

            return svgs;
        }

        private void SortProducts(List<Product> products, List<Svg> svgs)
        {
            List<Product> sortedProducts = new List<Product>();

            var a = products.GroupBy(c => c.Width).ToList();
        }

        private void SearchFit(List<Svg> svgs,Product product, Saw saw)
        {
            List<Group> fitGroups = new List<Group>();

            if (product.Quantity == 1)
            {
                foreach (Svg svg in svgs)
                {
                    fitGroups.AddRange(
                        svg.Groups.Where(c => c.Length >= product.Length && c.Width >= product.Width && c.Id == 0));
                }
        }else if(product.Quantity > 1)
            {
                fitGroups.Add(svgs.First(c=>c.Id == 0).Groups[0]);
            }


            if(fitGroups.Count > 0)
            {
                // maak een group aan met een rectange met de groote van het product.
                // verwijder de oude group
                // maak aangrenzende groupen aan die leeg zijn.

                // Als product quantity hoger is dan 1 zoek naar passende waar ze bijeen kunnen.
                //Group? selectedGroup = fitGroups.FirstOrDefault(c => c.Width == (product.Width * product.Quantity) + ((product.Quantity - 1) * saw.Thickness));

                // Exacte hoogte
                Group? selectedGroup = fitGroups.FirstOrDefault(c=>c.Width == product.Width || c.Length == product.Length);

                if (selectedGroup == null) selectedGroup = fitGroups.First();

                Svg selectedSvg = selectedGroup.Svg;
                List<Group> reworkGroups = CalculateGroupSizes(selectedGroup, product, saw, selectedSvg);

                selectedSvg.Groups.Remove(selectedGroup);
                selectedSvg.Groups.AddRange(reworkGroups);

            }
        }

        private List<Group> CalculateGroupSizes(Group group, Product product, Saw saw, Svg svg)
        {
            List<Group> newGroups = new List<Group>();

            if (product.Quantity == 1)
            {
                // Group with product
                Group groupWithProduct = new Group(1, new Rectangle(0, group.X, group.Y, product.Length, product.Width, new Label(product.Info)), group.X, group.Y, product.Length, product.Width);
                groupWithProduct.Svg = group.Svg;
                newGroups.Add(groupWithProduct);

                // Group right of product
                //CaclulateGroupRight(saw, group, groupWithProduct, svg, newGroups);

                int groupRightX = groupWithProduct.X + groupWithProduct.Length + 1 + saw.Thickness;
                int groupRightY = groupWithProduct.Y;
                int groupRightLength = svg.ViewBox.Length - groupWithProduct.X - groupWithProduct.Length - saw.Thickness;
                int groupRightWidth = groupWithProduct.Width;
                Group groupRight = new Group(0, new Rectangle(0, groupRightX, groupRightY, groupRightLength, groupRightWidth, new Label("0")), groupRightX, groupRightY, groupRightLength, groupRightWidth);
                groupRight.Svg = group.Svg;
                newGroups.Add(groupRight);

                if (groupWithProduct.Width != group.Width)
                {
                    //CalculateGroupUnder(saw, group, groupWithProduct, svg, newGroups);

                    //// Group under product
                    int groupUnderX = groupWithProduct.X;
                    int groupUnderY = groupWithProduct.Y + groupWithProduct.Width + 1 + saw.Thickness;
                    int groupUnderLength = group.Length;
                    int groupUnderWidth = group.Width - groupWithProduct.Width - saw.Thickness;
                    //if (groupUnderY + groupUnderWidth < svg.ViewBox.Width) groupUnderWidth -= saw.Thickness;
                    Group groupUnder = new Group(0, new Rectangle(0, groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth, new Label("0")), groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth);
                    groupUnder.Svg = group.Svg;
                    newGroups.Add(groupUnder);
                }
            }
            else if(product.Quantity > 1)
            {
                // Groeperingen maken voor products met 2 of meer quantity
                int maxHorizontal = CalculateQuantityHorizontal(saw, group, product);
                int maxVertical = CalculateQuantityVertical(saw, group, product);

                int length = 0;
                int width = 0;
                int x = 0;
                int y = 0;

                Group lastCreated = new();

                if (maxHorizontal >= product.Quantity)
                {
                    // alles kan op een lijn.
                    for (int i = 0; i < product.Quantity; i++)
                    {
                        length = product.Length;
                        width = product.Width;
                        x = group.X + lastCreated.X + lastCreated.Length;
                        y = group.Y + lastCreated.Y;

                        if (i > 0) x += saw.Thickness+1;

                        lastCreated = new Group(1, new Rectangle(1, x, y, length, width, new Label("1")), x, y, length, width);
                        lastCreated.Svg = svg;

                        newGroups.Add(lastCreated);
                    }

                    CaclulateGroupRight(saw, group, lastCreated, svg, newGroups);
                    CalculateGroupUnder(saw, group, lastCreated, svg, newGroups, product);

                    //}else if (product.Quantity % maxHorizontal == 0) {
                    //    // even aantal over meerdere lijnen


                }
                else if(maxVertical > product.Quantity) {
                    // passen niet allemaal horizontaal dus verticale proberen

                    for (int i = 0; i < product.Quantity; i++)
                    {
                        length = product.Length;
                        width = product.Width;
                        x = group.X + lastCreated.X;
                        y = group.Y + lastCreated.Y + lastCreated.Width;

                        if (i > 0) y += saw.Thickness;

                        lastCreated = new Group(1, new Rectangle(1, x, y, length, width, new Label("1")), x, y, length, width);
                        lastCreated.Svg = svg;

                        newGroups.Add(lastCreated);
                    }
                    CaclulateGroupRight(saw, group, lastCreated, svg, newGroups);
                    CalculateGroupUnder(saw, group, lastCreated, svg, newGroups, product);

                }
            }

            return newGroups;
        }

        private void CaclulateGroupRight(Saw saw, Group group, Group lastCreated, Svg svg, List<Group> newGroups)
        {

            int length = group.Length - (lastCreated.X + lastCreated.Length) - saw.Thickness;
            int width = lastCreated.Y + lastCreated.Width;
            int x = group.X + lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
            int y = group.Y;

            lastCreated = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
            lastCreated.Svg = svg;
            newGroups.Add(lastCreated);
        }

        private void CalculateGroupUnder(Saw saw, Group group, Group lastCreated, Svg svg, List<Group> newGroups, Product product)
        {
            int length = group.Length;
            int width = group.Width - (lastCreated.Y + lastCreated.Width) - saw.Thickness - 1;
            int x = group.X;
            int y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;

            lastCreated = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
            lastCreated.Svg = svg;
            newGroups.Add(lastCreated);
        }

        //private void CalculateRestGroups(Saw saw, Svg svg)
        //{
        //    svg.Groups.RemoveAll(c => c.Id == 0);
        //    var horizontalGroup = svg.Groups.GroupBy(c => c.X);


        //    foreach( var groupX in horizontalGroup )
        //    {
        //        Group g = groupX.MaxBy(c => c.X);
        //        int X = g.X + g.Length + 1 + saw.Thickness;
        //        int Y = g.Y;
        //        int length = svg.ViewBox.Length - g.Length - 1 - saw.Thickness;
        //        int width = g.Width;

        //        Group newGroup = new Group(0, new Rectangle(0, X, Y, length, width, new Label("0")), X, Y, length , width);
        //        newGroup.Svg = svg;
        //        svg.Groups.Add(newGroup);
        //    }

        //    var verticalGroup = svg.Groups.GroupBy(c => c.Y);

        //    foreach( var groupY in verticalGroup )
        //    {
        //        Group g = groupY.MaxBy(c => c.Y);
        //        int X = 0;
        //        int Y = g.Y + g.Width + 1 + saw.Thickness;
        //        int length = g.Length;
        //        int width = svg.ViewBox.Width - g.Width - saw.Thickness;

        //        Group newGroup = new Group(0, new Rectangle(0, X, Y, length, width, new Label("0")), X, Y, length, width);
        //        newGroup.Svg = svg;
        //        svg.Groups.Add(newGroup);
        //    }
        //}

        private int CalculateQuantityHorizontal(Saw saw, Group group, Product product)
        {
            bool test = true;
            int amount = 0;
            int rest = group.Length;
            while (test)
            {
                rest -= (saw.Thickness + product.Length);
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
                rest -= (saw.Thickness + product.Width);
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
    }
}
