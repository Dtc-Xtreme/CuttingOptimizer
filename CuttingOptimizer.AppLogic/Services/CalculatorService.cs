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
        private void ChooseCalculation(ref List<Svg> svgs, Saw saw, List<Product> products)
        {
            List<Group> groups = new List<Group>();
            foreach (Svg svg in svgs)
            {
                groups.AddRange(svg.Groups.Where(c => c.ID == 0));
            }

            List<RestResult> results = CalculateDiffrentPossibilitiesForGroups(groups, products, saw);

            if (svgs.Any(c => c.Plate.Veneer))
            {
                results = results
                .Where(c => c.Group.Width > 0 && c.Group.Length > 0)
                .Where(c => c.Quantity > 0)
                .OrderByDescending(c => c.Group.Svg.Priority)
                .OrderBy(c => c.Group.Area)
                .ThenBy(c => c.RestArea)
                .ThenByDescending(c => c.Area)
                .ToList();
            }
            else
            {
                results = results
                .Where(c => c.Group.Width > 0 && c.Group.Length > 0)
                .Where(c => c.Quantity > 0)
                .OrderByDescending(c => c.Group.Svg.Priority)
                .OrderBy(c => c.Group.Area)
                .ThenByDescending(c => c.Size())
                .ThenBy(c => c.RestArea)
                //.ThenBy(c => c.OrderOnRest())
                .ThenByDescending(c => c.Area)
                .ToList();
            }

            RestResult? selectedResult = results.FirstOrDefault();

            if (selectedResult != null)
            {
                // If veneer does not work properly set the HorizontalAlignment always on true this could make it better.
                CalculateGroups(selectedResult.Product, saw, selectedResult.Group, selectedResult.Columns, selectedResult.Rows, selectedResult.HorizontalAlignment, selectedResult.Rotated);
            }
            else
            {
                AddSvg(svgs);
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

            if (svgs.Any(c => c.Hash == newSvg.Hash))
            {
                throw new CreateSvgLoopException("Standaard plaat is te klein om te hergebruiken!");
            }

            svgs.Add(newSvg);
            return svgs;
        }
        private void RotateProduct(Product product)
        {
            int t = product.Length;
            product.Length = product.Width;
            product.Width = t;
        }
        private void RemoveProductsWithQuantityZero(List<Product> products)
        {
            products.RemoveAll(c => c.Quantity <= 0);
        }
        private List<RestResult> CalculateDiffrentPossibilitiesForGroups(List<Group> groups, List<Product> products, Saw saw)
        {
            List<RestResult> results = new List<RestResult>();

            foreach (Product product in products)
            {
                foreach (Group group in groups)
                {
                    //// Horizontal
                    //NewRestResult result1 = new NewRestResult(group, product, saw, RestResultType.Horizontal);
                    //results.Add(result1);

                    //// Vertical
                    //NewRestResult result3 = new NewRestResult(group, product, saw, RestResultType.Vertical);
                    //results.Add(result3);

                    #region Block
                    RestResult result5 = new RestResult(group, product, saw, RestResultType.BlockHorizontal);
                    results.Add(result5);

                    RestResult result7 = new RestResult(group, product, saw, RestResultType.BlockVertical);
                    results.Add(result7);
                    #endregion

                    // Rotated
                    if (!group.Svg.Plate.Veneer)
                    {
                        RotateProduct(product);

                        // Horizontal
                        //    NewRestResult result2 = new NewRestResult(group, product, saw, RestResultType.HorizontalRotated);
                        //    results.Add(result2);

                        //    // Vertical
                        //    NewRestResult result4 = new NewRestResult(group, product, saw, RestResultType.VerticalRotated);
                        //    results.Add(result4);

                        #region Block
                        RestResult result6 = new RestResult(group, product, saw, RestResultType.BlockHorizontalRotated);
                        results.Add(result6);

                        RestResult result8 = new RestResult(group, product, saw, RestResultType.BlockVerticalRotated);
                        results.Add(result8);
                        #endregion

                        RotateProduct(product);
                    }
                }
            }
            return results;
        }
        internal Group? CalculateGroups(Product selectedProduct, Saw saw, Group group, int horizontalQuantity, int verticalQuantity, bool horizontal=true, bool rotated=false)
        {
            List<Group> newGroups = new List<Group>();
            Group lastCreated = new Group();

            int length = 0;
            int width = 0;
            int x = 0;
            int y = 0;
            int xOffset = 0;
            int yOffset = 0;

            if (rotated) RotateProduct(selectedProduct);

            for(int vert = 0; vert < verticalQuantity; vert++)
            {
                for (int hor = 0; hor < horizontalQuantity; hor++)
                {
                    if (selectedProduct.Quantity != 0)
                    {
                        length = selectedProduct.Length;
                        width = selectedProduct.Width;

                        x = group.X + xOffset;
                        y = group.Y + yOffset;

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

                        xOffset += selectedProduct.Length + saw.Thickness + 1;
                    }
                    else
                    {
                        Group? rest = CalculateGroupRight(saw, group, lastCreated, newGroups, true, true);
                        if (rest != null) newGroups.Add(rest);
                    }
                    
                }
                x = 0;
                xOffset = 0;
                yOffset += selectedProduct.Width + 1 + saw.Thickness;
            }

            Group? right = CalculateGroupRight(saw, group, lastCreated, newGroups, horizontal);
            Group? under = CalculateGroupUnder(saw, group, lastCreated, newGroups, horizontal);

            if (right != null && right.Length > 0 && right.Width > 0) newGroups.Add(right);
            if (under != null && under.Length > 0 && under.Width > 0) newGroups.Add(under);

            group.Svg.Groups.AddRange(newGroups);
            group.Svg.Groups.Remove(group);

            if (rotated) RotateProduct(selectedProduct);

            return null;
        }
        internal Group? CalculateGroupRight(Saw saw, Group group, Group lastCreated, List<Group> newGroups, bool horizontal = true, bool rest=false)
        {
            int length = 0;
            int width = 0;
            int x = 0;
            int y = 0;
            Group? maxX = newGroups.MaxBy(c => c.X);
            Group? maxY = newGroups.MaxBy(c => c.Y);
            int minColumnCount = newGroups.GroupBy(c => c.Y).OrderBy(c => c.Count()).First().Count();
            int maxColumnCount = newGroups.GroupBy(c => c.Y).OrderBy(c => c.Count()).Last().Count();
            int maxRowCount = newGroups.GroupBy(c => c.X).OrderBy(c => c.Count()).First().Count();

            if (horizontal)
            {
                if (rest)
                {
                    x = group.X + (maxX.Length * minColumnCount) + ((saw.Thickness + 1) * minColumnCount);
                    y = maxY.Y;
                    length = maxX.Length;
                    width = maxX.Width;
                }
                else
                {
                    x = maxX.X + maxX.Length + saw.Thickness + 1;
                    y = maxX.Y;
                    length = group.Length - ((maxX.Length * maxColumnCount) + ((saw.Thickness + 1) * maxColumnCount));
                    width = (maxY.Width * maxRowCount) + ((saw.Thickness + 1) * (maxRowCount-1));
                }
            }
            else
            {
                x = maxX.X + maxX.Length + saw.Thickness + 1;
                y = group.Y;
                length = group.Length - ((maxX.Length * maxColumnCount) + ((saw.Thickness + 1) * maxColumnCount));
                width = group.Width;
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
        internal Group? CalculateGroupUnder(Saw saw, Group group, Group lastCreated, List<Group> newGroups, bool horizontal = true)
        {
            int length = 0;
            int width = 0;
            int x = 0;
            int y = 0;
            Group? maxX = newGroups.MaxBy(c => c.X);
            Group? maxY = newGroups.MaxBy(c => c.Y);
            int maxColumnCount = newGroups.GroupBy(c => c.Y).OrderBy(c => c.Count()).Last().Count();
            int maxRowCount = newGroups.GroupBy(c => c.X).OrderBy(c => c.Count()).First().Count();

            if (horizontal)
            {
                x = group.X;
                y = maxY.Y + maxY.Width + saw.Thickness + 1;
                length = group.Length;
                width = group.Width - ((maxY.Width * maxRowCount) + ((saw.Thickness + 1) * maxRowCount));
            }
            else
            {
                x = group.X;
                y = group.Y + (maxY.Width * maxRowCount) + ((saw.Thickness + 1) * maxRowCount);
                length = (maxX.Length * maxColumnCount) + ((saw.Thickness + 1) * (maxColumnCount - 1));
                width = group.Width - ((maxY.Width * maxRowCount) + ((saw.Thickness + 1) * maxRowCount));
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
        public int CalculateCutLines(Svg svg)
        {
            int x = svg.Groups.GroupBy(c => c.X).Count();
            int y = svg.Groups.GroupBy(c => c.Y).Count();

            return x + y - 2;
        }
    }
}