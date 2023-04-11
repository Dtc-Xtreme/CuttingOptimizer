﻿using CuttingOptimizer.AppLogic.Models;
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

                ChooseCalculation(svgs, saw, products);
                RemoveProductsWithQuantityZero(products);
            }

            return svgs;
        }

        private List<Svg> Init(List<Plate> plates)
        {
            List<Svg> svgs = new List<Svg>();

            foreach (Plate plate in plates)
            {
                svgs.Add(new Svg(plate.ID, new ViewBox(0, 0, plate.LengthWithTrim, plate.WidthWithTrim), plate.Priority));
            }

            foreach (Svg svg in svgs)
            {
                svg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));
            }

            return svgs;
        }
        private Svg AddSvg(Svg svg)
        {
            Svg newSvg = new Svg("Extra", new ViewBox(0, 0, svg.ViewBox.Length, svg.ViewBox.Width), svg.Priority);
            newSvg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));

            return svg;
        }
        private void RemoveProductsWithQuantityZero(List<Product> products)
        {
            products.RemoveAll(c => c.Quantity <= 0);
        }

        private void ChooseCalculation(List<Svg> svgs, Saw saw, List<Product> products)
        {
            List<Group> groups = SearchFits(svgs, products[0], saw);
            Group selectedGroup;
            Product selectedProduct = products[0];

            if (selectedProduct.Quantity == 1)
            {
                // Sort smalles to biggest and select first
                selectedGroup = groups.OrderBy(c => c.Length).ThenBy(c => c.Width).First();
                CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
            }
            else if (selectedProduct.Quantity > 1)
            {
                selectedGroup = groups.OrderBy(c => c.Length).ThenBy(c => c.Width).First();
                // Max Horizontal/Vertical
                int maxHorizontal = CalculateQuantityHorizontal(saw, selectedGroup, selectedProduct);
                int maxVertical = CalculateQuantityVertical(saw, selectedGroup, selectedProduct);

                int vert = selectedProduct.Quantity / maxHorizontal;
                int rest = selectedProduct.Quantity % maxHorizontal;
                //int runs = selectedProduct.Quantity / vert;
                //if (vert == 1) runs = vert;

                // Search groups
                var a = groups.FindAll(c => MaxHorizontalFit(c, selectedProduct, saw));
                var b = groups.FindAll(c => MaxVerticalFit(c, selectedProduct, saw));

                //selectedGroup = a.First();
                //CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
                //CalculateGroupsVertical(selectedProduct, saw, selectedGroup, maxVertical);

                if (rest == 0)
                {
                    // als het past
                    CalculateGroupBlock(selectedProduct, saw, selectedGroup, (selectedProduct.Quantity/maxHorizontal), maxHorizontal);
                    // als het groter is dan 1 plate moet de rest opgesplits worden
                }
                else if (selectedProduct.Quantity == maxHorizontal)
                {
                    CalculateGroupHorizontal(selectedProduct, saw, selectedGroup, maxHorizontal);
                }
                else
                {
                    for(int i = 0; i < selectedProduct.Quantity; i++)
                    {
                        groups = SearchFits(svgs, products[0], saw);
                        selectedGroup = groups.OrderBy(c => c.Length).ThenBy(c => c.Width).First();
                        CalculateGroupsVertical(selectedProduct, saw, selectedGroup, 1);
                    }
                }
            }
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

        private List<Group> SearchFits(List<Svg> svgs, Product product, Saw saw)
        {
            List<Group> fitGroups = new List<Group>();

            while (fitGroups.Count == 0)
            {
                foreach (Svg svg in svgs)
                {
                    fitGroups.AddRange(
                        svg.Groups.Where(c => c.Length >= product.Length && c.Width >= product.Width && c.Id == 0));
                }

                // Add new Svg when there's no space left.
                if (fitGroups.Count == 0)
                {
                    svgs.Add(svgs.MaxBy(c => c.Priority));
                }
            }

            return fitGroups.OrderBy(c => c.Area).ToList();
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

                lastCreated = new Group(1, new Rectangle(1, x, y, length, width, new Label(selectedProduct.ID)), x, y, length, width);
                lastCreated.Svg = group.Svg;

                newGroups.Add(lastCreated);
                selectedProduct.Quantity--;
            }

            Group? right = CalculateGroupRight(saw, group, lastCreated, newGroups);
            Group? under = CalculateGroupUnder(saw, group, right, lastCreated, selectedProduct);

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
            if(lastCreated.X + lastCreated.Length != group.Width)
            {
                int length = group.Length - lastCreated.Length - saw.Thickness - 1;

                int width = (lastCreated.Width * newGroups.Count) + (saw.Thickness * (newGroups.Count - 1)) + (1 * (newGroups.Count - 1));
                int x = lastCreated.X + lastCreated.Length + 1 + saw.Thickness;
                int y = group.Y;

                Group rightGroup = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
                rightGroup.Svg = group.Svg;
                return rightGroup;
            }
            return null;
        }
        private Group? CalculateGroupUnder(Saw saw, Group group, Group right, Group lastCreated, Product product)
        {
            if(lastCreated.Y + lastCreated.Width != group.Width)
            {
                int length = group.Length;
                int width = group.Width - right.Width - saw.Thickness;
                int x = group.X;
                int y = lastCreated.Y + lastCreated.Width + saw.Thickness + 1;

                Group underGroup = new Group(0, new Rectangle(0, x, y, length, width, new Label("0")), x, y, length, width);
                underGroup.Svg = group.Svg;
                //newGroups.Add(underGroup);
                return underGroup;
            }
            return null;
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