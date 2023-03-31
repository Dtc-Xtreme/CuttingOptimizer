using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                svgs.Add(new Svg(0, new ViewBox(0,0, plate.LengthWithTrim, plate.WidthWithTrim)));
            }

            // Creeer Eerste group per svg
            // Eerste is altijd 1 group met totale oppvlakte
            foreach(Svg svg in svgs)
            {
                svg.AddGroup(new Group(0, 0, 0, svg.ViewBox.Length, svg.ViewBox.Width));
            }

            foreach (Product product in products)
            {
                //for (int i = 0; i < product.Quantity; i++)
                //{
                    SearchFit(svgs, product, saw);
                //}
            }

            return svgs;
        }

        private void SearchFit(List<Svg> svgs,Product product, Saw saw)
        {
            // Welke past
            List<Group> fitGroups = new List<Group>();

            foreach(Svg svg in svgs)
            {
                Group? group = svg.Groups.FirstOrDefault(c=>c.Length >= product.Length && c.Width >= product.Width && c.Id == 0);

                if(group != null) fitGroups.Add(group);
            }

            fitGroups = fitGroups.OrderByDescending(c=>c.Width).ToList();

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
            // Group with product
            Group groupWithProduct = new Group(1, new Rectangle(0, group.X, group.Y, product.Length, product.Width, new Label(product.Info)), group.X, group.Y, product.Length, product.Width);
            groupWithProduct.Svg = group.Svg;
            newGroups.Add(groupWithProduct);

            // Group right of product
            int groupRightX = groupWithProduct.X + groupWithProduct.Length + 1 + saw.Thickness;
            int groupRightY = groupWithProduct.Y;
            int groupRightLength = svg.ViewBox.Length - groupWithProduct.X - groupWithProduct.Length - saw.Thickness;
            int groupRightWidth = groupWithProduct.Width;
            Group groupRight = new Group(0, new Rectangle(0, groupRightX, groupRightY, groupRightLength, groupRightWidth, new Label("0")), groupRightX, groupRightY, groupRightLength , groupRightWidth);
            groupRight.Svg = group.Svg;
            newGroups.Add(groupRight);

            if (groupWithProduct.Width != group.Width)
            {
                // Group under product
                int groupUnderX = groupWithProduct.X;
                int groupUnderY = groupWithProduct.Y + groupWithProduct.Width + 1 + saw.Thickness;
                int groupUnderLength = group.Length;
                int groupUnderWidth = group.Width - groupRightY - groupWithProduct.Width - saw.Thickness;
                //if (groupUnderY + groupUnderWidth < svg.ViewBox.Width) groupUnderWidth -= saw.Thickness;
                Group groupUnder = new Group(0, new Rectangle(0, groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth, new Label("0")), groupUnderX, groupUnderY, groupUnderLength, groupUnderWidth);
                groupUnder.Svg = group.Svg;
                newGroups.Add(groupUnder);
            }

            return newGroups;
        }
        public bool PlaceNextInBundle(Saw saw, List<Plate> plates, Product product)
        {
            throw new NotImplementedException();
        }

        public bool PlaceNext(Saw saw, List<Plate> plates, List<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
