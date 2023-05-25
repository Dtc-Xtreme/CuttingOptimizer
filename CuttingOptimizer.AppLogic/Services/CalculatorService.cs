using CuttingOptimizer.AppLogic.Exceptions;
using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

[assembly: InternalsVisibleTo("CuttingOptimizer.AppLogic.Tests")]
namespace CuttingOptimizer.AppLogic.Services
{
    public class CalculatorService : ICalculatorService
    {
        private int svgPrioCounter = 1;
        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products)
        {
            // Creeer svgs and sorting
            List<Svg> svgs = Init(plates);

            // Keep looping throug products till it's empty
            // Remove when added or reduce the quantity of places groups/rectangles
            while (products.Count > 0)
            {
                products = products.OrderByDescending(c => c.Quantity * c.Area).ThenByDescending(c => c.Length).ToList();

                ChooseCalculation(ref svgs, saw, products);
                RemoveProductsWithQuantityZero(products);
            }

            CalculateScale(ref svgs);

            return svgs.Where(c => c.Groups.Any(c=>c.ID == 1)).OrderByDescending(c => c.Priority).ThenByDescending(c => c.Area).ToList();
        }

        private void CalculateScale(ref List<Svg> svgs)
        {
            double scale = 0;
            if (svgs.Count > 0)
            {
                int biggestLength = svgs.Where(c => c.Groups.Any(c => c.ID == 1)).Max(c => c.ViewBox.Length);
                int biggestWidth = svgs.Where(c => c.Groups.Any(c => c.ID == 1)).Max(c => c.ViewBox.Width);

                scale = biggestLength < biggestWidth ? biggestLength / (double)100 : biggestLength / (double)100;
                bool lengthBiggest = biggestLength < biggestWidth ? true : false;

                foreach (Svg s in svgs)
                {
                    s.Scale = biggestLength / s.ViewBox.Length;
                }
            }
        }

        private List<Svg> Init(List<Plate> plates)
        {
            List<Svg> svgs = new List<Svg>();

            foreach (Plate plate in plates)
            {
                for (int x = 0; x < plate.Quantity; x++)
                {
                    if (plate.Priority != 0)
                    {
                        Svg svg = new Svg
                        {
                            ID = plate.ID,
                            ViewBox = new ViewBox(0, 0, plate.LengthWithTrim, plate.WidthWithTrim),
                            Priority = plate.Priority,
                            Plate = new Plate(1, plate)
                        };
                        svgs.Add(svg);
                    }
                }
            }

            foreach (Svg svg in svgs)
            {
                Group newGroup = new Group
                {
                    ID = 0,
                    X = 0,
                    Y = 0,
                    Length = svg.ViewBox.Length,
                    Width = svg.ViewBox.Width,
                    Svg = svg
                };
                svg.AddGroup(newGroup);
            }
            return svgs;
        }

        private List<Svg> AddSvg(List<Svg> svgs)
        {
            Svg baseSvg = svgs.First(c => c.Plate.Base == true);
            Svg newSvg = new Svg("Extra", new ViewBox(0, 0, baseSvg.ViewBox.Length, baseSvg.ViewBox.Width), svgPrioCounter, new Plate(1, baseSvg.Plate));
            svgPrioCounter++;
            newSvg.AddGroup(
                new Group
                {
                    ID = 0,
                    X = 0,
                    Y = 0,
                    Length = baseSvg.ViewBox.Length,
                    Width = baseSvg.ViewBox.Width,
                });
                
            if(svgs.Any(c=>c.Hash == newSvg.Hash))
            {
                throw new CreateSvgLoopException("Standaard plaat is te klein om te hergebruiken!");
            }

            svgs.Add(newSvg);
            return svgs;
        }
        private void RemoveProductsWithQuantityZero(List<Product> products)
        {
            products.RemoveAll(c => c.Quantity <= 0);
        }

        private void ChooseCalculation(ref List<Svg> svgs, Saw saw, List<Product> products)
        {

            List<Group> groups = new List<Group>();
            foreach (Svg svg in svgs)
            {
                groups.AddRange(svg.Groups.Where(c => c.ID == 0));
            }

            
            List<RestResult> results = CalculateDiffrentPossibilitiesForGroups(groups, products, saw)
                .Where(c => (c.MaxHorizontalQuantity > 0 && c.HorizontalScaleVsVertical > 0))
                .Where(c => c.MaxVerticalQuantity > 0 && c.VerticalScaleVsHorizontal > 0)
                .Where(c=>c.Group.Width > 0 && c.Group.Length > 0).OrderByDescending(c=>c.Group.Svg.Priority).OrderByDescending(c=>c.VerticalAlignment)
                //.ThenByDescending(c=>c.CompareBestCandidate())
                //.ThenByDescending(c=>c.Rotated)
                .ToList();


            RestResult? selectedResult = results.FirstOrDefault();
            Product? selectedProduct = products.FirstOrDefault();

            if (selectedResult.Rotated) RotateProduct(selectedResult.Product);
            int hor = selectedResult.HorizontalUsed < selectedResult.VerticalUsed ? selectedResult.MaxHorizontalQuantity : selectedResult.VerticalScaleVsHorizontal;
            int vert = selectedResult.HorizontalUsed < selectedResult.VerticalUsed ? selectedResult.HorizontalScaleVsVertical : selectedResult.MaxVerticalQuantity;
            if (vert > selectedResult.Product.Quantity) vert = selectedResult.Product.Quantity;
            if (hor > selectedResult.Product.Quantity) hor = selectedResult.Product.Quantity;
            CalculateGroupsVertical(selectedProduct, saw, selectedResult.Group, vert);
            if (selectedResult.Rotated) RotateProduct(selectedResult.Product);

            //if (selectedResult != null)
            //{
            //    // Check and replace if vert or hor is bigger then product.qty
            //    if (selectedResult.Rotated) RotateProduct(selectedResult.Product);
            //    int hor = selectedResult.HorizontalUsed < selectedResult.VerticalUsed ? selectedResult.MaxHorizontalQuantity : selectedResult.VerticalScaleVsHorizontal;
            //    int vert = selectedResult.HorizontalUsed < selectedResult.VerticalUsed ? selectedResult.HorizontalScaleVsVertical : selectedResult.MaxVerticalQuantity;
            //    if (vert > selectedResult.Product.Quantity) vert = selectedResult.Product.Quantity;
            //    if (hor > selectedResult.Product.Quantity) hor = selectedResult.Product.Quantity;

            //    if (selectedResult.Product.Quantity == 1)
            //    {
            //        CalculateGroupsVertical(selectedResult.Product, saw, selectedResult.Group, vert);
            //    }
            //    else
            //    {
            //        CalculateGroupBlock(selectedResult.Product, saw, selectedResult.Group, hor, vert);
            //    }

            //    if (selectedResult.Rotated) RotateProduct(selectedResult.Product);
            //    return;
            //}

            //AddSvg(svgs);

        }
        private bool VerticalTest(Group group, Saw saw, Product product, int quantity)
        {
            int length = product.Length * quantity + (saw.Thickness * (quantity - 1));
            bool lengthCheck = group.Length >= length;
            bool widthCheck = group.Width >= product.Width;
            return lengthCheck && widthCheck;

        }

        private List<RestResult> CalculateDiffrentPossibilitiesForGroups(List<Group> groups, List<Product> products, Saw saw)
        {
            List<RestResult> results = new List<RestResult>();

            foreach (Product product in products)
            {
                int runs = 0;
                bool rotate = false;
                do
                {
                    if (!groups.Any(c => c.Svg.Plate.Veneer))
                    {
                        if (rotate) RotateProduct(product);
                    }

                    foreach (Group group in groups)
                    {
                        int maxHorizontal = CalculateQuantityHorizontal(saw, group, product);
                        int maxVertical = CalculateQuantityVertical(saw, group, product);
                        int rest = maxHorizontal > 0 ? product.Quantity % maxHorizontal : 0;
                        bool fit = maxHorizontal * maxVertical >= product.Quantity;

                        int hor;
                        int horvsvert;
                        if (maxHorizontal == 0)
                        {
                            horvsvert = 0;
                            hor = 0;
                        }
                        else if (product.Quantity >= maxHorizontal)
                        {
                            horvsvert = product.Quantity / maxHorizontal;
                            hor = maxHorizontal * horvsvert;
                            if (maxVertical != 0 && horvsvert > maxVertical) horvsvert = maxVertical; //?
                        }
                        else
                        {
                            horvsvert = 1;
                            hor = product.Quantity;
                        }


                        int ver;
                        int vertvshor;
                        if (maxVertical == 0)
                        {
                            vertvshor = 0;
                            ver = 0;
                        }
                        else if (product.Quantity >= maxVertical)
                        {
                            vertvshor = product.Quantity / maxVertical;
                            ver = maxVertical * vertvshor;
                            if (maxHorizontal != 0 && vertvshor > maxHorizontal) vertvshor = maxHorizontal; //?
                        }
                        else
                        {
                            vertvshor = 1;
                            ver = product.Quantity;
                        }

                        RestResult result = new RestResult
                        {
                            MaxHorizontalQuantity = maxHorizontal,
                            HorizontalScaleVsVertical = horvsvert,
                            HorizontalQuantity = hor,
                            MaxVerticalQuantity = maxVertical,
                            VerticalScaleVsHorizontal = vertvshor,
                            VerticalQuantity = ver,
                            Rest = rest,
                            Rotated = rotate,
                            Group = group,
                            Product = product,
                        };

                        results.Add(result);

                    }
                    if (!groups.Any(c => c.Svg.Plate.Veneer))
                    {
                        if (rotate) RotateProduct(product);
                    }
                    else
                    {
                        runs = 1;
                    }
                    runs++;
                    rotate = true;
                } while (runs < 2);
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
                if (amount == 0)
                {
                    rest -= product.Length;
                }
                else
                {
                    rest -= (saw.Thickness + product.Length + 1);

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
                if (amount == 0)
                {
                    rest -= product.Width;
                }
                else
                {
                    rest -= (saw.Thickness + product.Width + 1);
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

        private void RotateProduct(Product product)
        {
            int t = product.Length;
            product.Length = product.Width;
            product.Width = t;
        }

        internal Group? CalculateGroupHorizontal(Product selectedProduct, Saw saw, Group group, int quantity, bool checkCount = true)
        {
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

                lastCreated = new Group
                {
                    ID = 1,
                    X = x,
                    Y = y,
                    Length = length,
                    Width = width,
                    Product = selectedProduct,
                    Svg = group.Svg,
                    Rectangle = new Rectangle(1, x, y, length, width, new Label(selectedProduct.ID))
                };

                newGroups.Add(lastCreated);
                selectedProduct.Quantity--;
            }

            Group? right = CalculateGroupRight(saw, group, lastCreated, newGroups, checkCount, true);
            Group? under = CalculateGroupUnder(saw, group, lastCreated, newGroups);

            if (right != null) newGroups.Add(right);
            if (under != null) newGroups.Add(under);

            group.Svg.Groups.AddRange(newGroups);
            group.Svg.Groups.Remove(group);

            return right;
        }
        internal Group? CalculateGroupsVertical(Product selectedProduct, Saw saw, Group group, int quantity, bool checkCount = true)
        {
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

                lastCreated = new Group
                {
                    ID = 1,
                    X = x,
                    Y = y,
                    Length = length,
                    Width = width,
                    Product = selectedProduct,
                    Svg = group.Svg,
                    Rectangle = new Rectangle(1, x, y, length, width, new Label(selectedProduct.ID))
                };

                newGroups.Add(lastCreated);
                selectedProduct.Quantity--;
            }

            Group? right = CalculateGroupRight(saw, group, lastCreated, newGroups, checkCount, false);
            Group? under = CalculateGroupUnder(saw, group, lastCreated, newGroups, false, false);

            if (right != null) newGroups.Add(right);
            if (under != null) newGroups.Add(under);

            group.Svg.Groups.AddRange(newGroups);
            group.Svg.Groups.Remove(group);

            return right;
        }
        internal Group? CalculateGroupRight(Saw saw, Group group, Group lastCreated, List<Group> newGroups, bool checkCount = true, bool horizontal = true)
        {
            bool run = checkCount ? (lastCreated.Length * newGroups.Count) + ((saw.Thickness + 1) * newGroups.Count) < group.Length : lastCreated.Length < group.Length;

            if (run)
            {
                int length = 0;
                int width = 0;
                int x = 0;
                int y = 0;

                if (horizontal)
                {
                    length = group.Length - lastCreated.Length - saw.Thickness - 1;
                    width = (lastCreated.Width * newGroups.Count) + (saw.Thickness * (newGroups.Count - 1)) + (1 * (newGroups.Count - 1));
                    x = lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
                    y = group.Y;
                }
                else
                {
                    length = group.Length - lastCreated.Length - saw.Thickness - 1;
                    width = group.Width;
                    x = lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
                    y = group.Y;
                }


                Group rightGroup = new Group
                {
                    ID = 0,
                    X = x,
                    Y = y,
                    Length = length,
                    Width = width,
                    Rectangle = new Rectangle(0, x, y, length, width, new Label("0")),
                    Svg = group.Svg
                };

                return rightGroup;
            }
            return null;
        }
        internal Group? CalculateGroupUnder(Saw saw, Group group, Group lastCreated, List<Group> newGroups, bool checkCount = true, bool horizontal = true)
        {
            var a = (lastCreated.Width * newGroups.Count) + ((saw.Thickness + 1) * newGroups.Count);

            bool run = checkCount ? (lastCreated.Width * newGroups.Count) + ((saw.Thickness + 1) * newGroups.Count) < group.Width : lastCreated.Width < group.Width;

            if (run)
            {
                int length = 0;
                int width = 0;
                int x = 0;
                int y = 0;

                if (horizontal)
                {
                    length = group.Length;
                    width = group.Width - (lastCreated.Width * newGroups.Count) - (saw.Thickness * newGroups.Count) - (newGroups.Count * 1);
                    x = group.X;
                    y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;
                }
                else
                {
                    length = lastCreated.Length;
                    width = group.Width - (lastCreated.Width * newGroups.Count) - (saw.Thickness * newGroups.Count) - (newGroups.Count * 1);
                    x = group.X;
                    y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;
                }

                Group underGroup = new Group
                {
                    ID = 0,
                    X = x,
                    Y = y,
                    Length = length,
                    Width = width,
                    Rectangle = new Rectangle(0, x, y, length, width, new Label("0")),
                    Svg = group.Svg
                };

                return underGroup;
            }
            return null;
        }
        internal void CalculateGroupBlock(Product selectedProduct, Saw saw, Group group, int hor, int vert)
        {
            Group selectedGroup = group;

            for (int i = 0; i < hor && selectedProduct.Quantity >= vert; i++)
            {
                if (selectedGroup == null) break;
                selectedGroup = CalculateGroupsVertical(selectedProduct, saw, selectedGroup, vert, false);
            }
        }

        internal void CalculateGroupsNewVertical(Product selectedProduct, Saw saw, Group group, int quantity)
        {
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

                lastCreated = new Group
                {
                    ID = 1,
                    X = x,
                    Y = y,
                    Length = length,
                    Width = width,
                    Product = selectedProduct,
                    Svg = group.Svg,
                    Rectangle = new Rectangle(1, x, y, length, width, new Label(selectedProduct.ID))
                };

                group.Svg.AddGroup(lastCreated);
                selectedProduct.Quantity--;
            }

            group.Svg.Groups.AddRange(newGroups);
            //group.Svg.Groups.Remove(group);
        }

        internal void CalculateEmptyGroups(List<Svg> svgs)
        {
            // y axis calc
            var xGroup = svgs[0].Groups.Where(c => c.ID == 1).MaxBy(c => c.X);
            var yGroup = svgs[0].Groups.Where(c => c.ID == 1).MaxBy(c => c.Y);

            ViewBox vb = svgs[0].ViewBox;

            int x = xGroup.X;
            int y = xGroup.Y;
            int length = 0;
            int width = 0;



        }

        public int CalculateCutLines(Svg svg)
        {
            int x = svg.Groups.GroupBy(c => c.X).Count();
            int y = svg.Groups.GroupBy(c => c.Y).Count();

            return x + y - 2;
        }

        public List<Product> CombineProductsWithSameDimentions(List<Product> products)
        {

            List<Product> newProductList = new List<Product>();

            var productsGrouped = products.GroupBy(x => (x.Length, x.Width));

            foreach (var group in productsGrouped)
            {
                var ids = group.Select(c => c.ID).Distinct();
                string id = String.Join(", ", ids);
                newProductList.Add(new Product(group.Sum(c => c.Quantity), id, group.First().Length, group.First().Width, group.First().Height));
            }

            return newProductList;
        }
    }
}