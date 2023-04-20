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
        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products)
        {
            // Creeer svgs and sorting
            List<Svg> svgs = Init(plates);

            // Keep looping throug products till it's empty
            // Remove when added or reduce the quantity of places groups/rectangles
            while (products.Count > 0)
            {
                products = products.OrderByDescending(c => c.Quantity).ThenByDescending(c => c.Area).ToList();

                ChooseCalculation(ref svgs, saw, products);
                RemoveProductsWithQuantityZero(products);
            }

            return svgs.OrderBy(c => c.Priority).ThenBy(c => c.Area).ToList();
        }

        private List<Svg> Init(List<Plate> plates)
        {
            List<Svg> svgs = new List<Svg>();

            foreach (Plate plate in plates)
            {
                for(int x = 0; x < plate.Quantity; x++)
                {
                    svgs.Add(new Svg(plate.ID, new ViewBox(0, 0, plate.LengthWithTrim, plate.WidthWithTrim), plate.Priority, new Plate(1, plate)));
                }
            }

            foreach (Svg svg in svgs)
            {
                svg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));
            }

            return svgs;
        }
        private List<Svg> AddSvg(List<Svg> svgs, Svg svg)
        {
            Svg newSvg = new Svg("Extra", new ViewBox(0, 0, svg.ViewBox.Length, svg.ViewBox.Width), 2, new Plate(1, svg.Plate));
            newSvg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));

            svgs.Add(newSvg);

            return svgs;
        }
        private void RemoveProductsWithQuantityZero(List<Product> products)
        {
            products.RemoveAll(c => c.Quantity <= 0);
        }

        private void ChooseCalculation(ref List<Svg> svgs, Saw saw, List<Product> products)
        {
            Group selectedGroup;
            Product selectedProduct = products[0];
            List<Group> groups = SearchFits(ref svgs, selectedProduct, saw);

            if (selectedProduct.Quantity == 1)
            {
                // Sort smalles to biggest and select first
                selectedGroup = groups.First();
                CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
            }
            else if (selectedProduct.Quantity > 1)
            {
                var results = CalculateDiffrentPossibilitiesForGroups(groups, selectedProduct, saw);
                // Find the smalles where they fill all
                var result = results.First();
                //var result = results.FirstOrDefault(c => c.Value.Fit == true);
                //if(result.Key == null) result = results.First();

                if (result.Value.Rest == 0)
                {

                    // als het past
                    if (selectedProduct.Quantity / result.Value.HorizontalQuantity < result.Value.VerticalQuantity)
                    {
                        CalculateGroupBlock(selectedProduct, saw, result.Key, (selectedProduct.Quantity / result.Value.HorizontalQuantity), result.Value.HorizontalQuantity);
                    }
                    else
                    {
                        // het neemt de kleinste en kijkt als het er min 1x inpast. hier kan je ook een andere group kiezen waar er een grotere block in kan als er meerdere groups mogelijkzijn.
                        CalculateGroupBlock(selectedProduct, saw, result.Key, result.Value.VerticalQuantity, result.Value.HorizontalQuantity);
                    }
                    // als het groter is dan 1 plate moet de rest opgesplits worden

                }
                else if (result.Value.Rest != 0 && selectedProduct.Quantity > result.Value.HorizontalQuantity)
                {
                    CalculateGroupBlock(selectedProduct, saw, result.Key, result.Value.Vert, result.Value.HorizontalQuantity);
                }
                else if (selectedProduct.Quantity == result.Value.HorizontalQuantity)
                {
                    CalculateGroupHorizontal(selectedProduct, saw, result.Key, result.Value.HorizontalQuantity);
                }
                else
                {
                    while(selectedProduct.Quantity != 0)
                    {
                        groups = SearchFits(ref svgs, selectedProduct, saw);
                        selectedGroup = groups.First();
                        CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
                    }
                }
            }
        }

        private Dictionary<Group, RestResult> CalculateDiffrentPossibilitiesForGroups(List<Group> groups, Product product, Saw saw)
        {
            Dictionary<Group, RestResult> results = new Dictionary<Group, RestResult>();

            foreach(Group group in groups)
            {
                int maxHorizontal = CalculateQuantityHorizontal(saw, group, product);
                int maxVertical = CalculateQuantityVertical(saw, group, product);

                var a = (group.Width / (product.Width + saw.Thickness + 1));
                int vert = a;
                //int vert = product.Quantity / maxHorizontal;
                int rest = product.Quantity % maxHorizontal;
                int restWidth = 0;
                int restHeight = 0;
                bool fit = maxHorizontal * maxVertical >= product.Quantity;

                results.Add(group, new RestResult
                {
                    HorizontalQuantity = maxHorizontal,
                    VerticalQuantity = maxVertical,
                    Rest = rest,
                    Vert = vert,
                    Fit = fit
                });
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

        private List<Group> SearchFits(ref List<Svg> svgs, Product product, Saw saw)
        {
            List<Group> fitGroups = new List<Group>();

            while (fitGroups.Count == 0)
            {
                svgs = svgs.OrderByDescending(c => c.Priority).ThenByDescending(c => c.Area).ToList();

                foreach (Svg svg in svgs)
                {
                    fitGroups.AddRange(
                        svg.Groups.Where(c => c.Length >= product.Length && c.Width >= product.Width && c.ID == 0));
                }

                // Add new Svg when there's no space left.
                if (fitGroups.Count == 0)
                {
                    AddSvg(svgs, svgs.First(c=>c.Plate.Base == true));
                    //AddSvg(svgs, svgs.MinBy(c => c.Priority));
                }
            }

            return fitGroups.OrderBy(c => c.Length).ThenBy(c => c.Width).ToList();
        }
        private bool MaxHorizontalFit(Group group, Product product, Saw saw)
        {
            int length = (product.Length * product.Quantity) + (saw.Thickness * (product.Quantity - 1));
            return group.Length >= length && group.Width >= product.Width;
        }
        private bool MaxVerticalFit(Group group, Product product, Saw saw)
        {
            int width = (product.Width * product.Quantity) + (saw.Thickness * (product.Quantity - 1));
            return group.Width >= width && group.Length >= product.Length;
        }

        private void CalculateGroupHorizontal(Product selectedProduct, Saw saw, Group group, int quantity)
        {
            Group selectedGroup = group;
            for(int i = 0; i < quantity && selectedProduct.Quantity > 0; i++)
            {
                selectedGroup = CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
            }
        }
        private Group CalculateGroupsVertical(Product selectedProduct, Saw saw, Group group, int quantity) {
            List<Group> newGroups = new List<Group>();
            Group lastCreated = new Group();

            int length = 0;
            int width = 0;
            int x = 0;
            int y = 0;

            for (int i = 0; i < quantity && selectedProduct.Quantity > 0; i++)
            {
                length = selectedProduct.Length;
                width = selectedProduct.Width;
                x = group.X;
                y = group.Y + lastCreated.Y + lastCreated.Width;

                if (i > 0)
                {
                    y += saw.Thickness + 1;
                    y -= group.Y;
                }

                lastCreated = new Group(new Rectangle(1, x, y, length, width, new Label(selectedProduct.ID)), x, y, length, width, new Product(1,selectedProduct));
                lastCreated.Svg = group.Svg;

                newGroups.Add(lastCreated);
                selectedProduct.Quantity--;
            }

            Group? right = CalculateGroupRight(saw, group, lastCreated, newGroups);
            Group? under = CalculateGroupUnder(saw, group, lastCreated, newGroups);

            if(right != null) newGroups.Add(right);
            if (under != null) newGroups.Add(under);

            group.Svg.Groups.AddRange(newGroups);
            group.Svg.Groups.Remove(group);

            return right;
        }
        private void CalculateGroupBlock(Product selectedProduct, Saw saw, Group group, int vert, int hor)
        {
            Group selectedGroup = group;
            for(int i = 0; i < hor && selectedProduct.Quantity > 0; i++) {
                selectedGroup = CalculateGroupsVertical(selectedProduct, saw, selectedGroup, vert);
            }
        }
        private Group? CalculateGroupRight(Saw saw, Group group, Group lastCreated, List<Group> newGroups)
        {
            //if (lastCreated.X + lastCreated.Length < group.Length)
            if (lastCreated.Length <= group.Length)
            {
                int length = group.Length - lastCreated.Length - saw.Thickness - 1;

                int width = (lastCreated.Width * newGroups.Count) + (saw.Thickness * (newGroups.Count - 1)) + (1 * (newGroups.Count - 1));
                int x = lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
                int y = group.Y;

                Group rightGroup = new Group(new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
                rightGroup.Svg = group.Svg;
                return rightGroup;
            }
            return null;
        }
        private Group? CalculateGroupUnder(Saw saw, Group group, Group lastCreated, List<Group> newGroups)
        {
            if (lastCreated.Y + lastCreated.Width != group.Width /*&& group.Width != right.Width*/)
            {
                int length = group.Length;
                int width = group.Width - (lastCreated.Width * newGroups.Count) - (saw.Thickness * newGroups.Count) - (newGroups.Count * 1);
                int x = group.X;
                int y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;

                Group underGroup = new Group(new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
                underGroup.Svg = group.Svg;
                return underGroup;
            }
            return null;
        }

        public int CalculateCutLines(Svg svg)
        {
            int x = svg.Groups.GroupBy(c=>c.X).Count();
            int y = svg.Groups.GroupBy(c => c.Y).Count();

            return x + y - 2;
        }

        public List<Product> CombineProductsWithSameDimentions(List<Product> products) {

            List<Product> newProductList = new List<Product>();

            var productsGrouped = products.GroupBy(x => (x.Length, x.Width));

            foreach(var group in productsGrouped) {
                var ids = group.Select(c => c.ID).Distinct();
                string id = String.Join(", ", ids);
                newProductList.Add(new Product(group.Sum(c=>c.Quantity), id, group.First().Length, group.First().Width, group.First().Height));
            }

            return newProductList;
        }

    }
}