using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Services
{
    public class CalculatorService : ICalculatorService
    {
        public int Area(int width, int length)
        {
            return width * length;
        }

        //public bool CheckIfFits(Plate plate, Product product)
        //{
        //    bool wr = false;
        //    bool lr = false;

        //    if(plate.Products == null || plate.Products.Count == 0)
        //    {
        //        wr = plate.WidthWithTrim > product.Width;
        //        lr = plate.LengthWithTrim > product.Length;
        //        if(wr && lr)
        //        {
        //            plate.Products = new List<Product>();
        //            plate.Products.Add(product);
        //        }
        //    }

        //    return wr && lr;
        //}

        private bool DoesProductFitOnPlate(Saw saw, Plate plate, Product product)
        {
            return (plate.Length - saw.Thickness - product.Length) > 0 && (plate.Width - saw.Thickness - product.Width) > 0;
        }

        public bool PlaceNext(Saw saw, List<Plate> plates, Product product)
        {
            // Check if fits on plate
            bool horizontalFit = DoesProductFitOnPlate(saw, plates[0], product);



            // Find Product that is placed most to the right
            var lastItemHorizontal = plates[0].Products.MaxBy(x => x.X);

            if(lastItemHorizontal != null)
            {
                // Check if total width is smaller then plate width with trim
                bool check = (lastItemHorizontal.X + saw.Thickness + product.Length) < plates[0].LengthWithTrim;
                if(check)
                {
                    product.X = lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness;
                    product.Y = 0;
                    plates[0].Products.Add(product);
                }
            }
            else
            {
                // Er zijn geen items
                product.X = 0;
                product.Y = 0;
                plates[0].Products.Add(product);

                // Nog checken of het op de Y as kan
                return true;
            }
            // checken of de x y nummer in een van de plate.products zit van d
            return false;
        }

        private int CalculateQuantityHorizontal(Saw saw, Plate plate, Product product)
        {
            bool test = true;
            int amount = 0;
            int rest = plate.LengthWithTrim;
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
        private int CalculateQuantityVertical(Saw saw, Plate plate, Product product)
        {
            bool test = true;
            int amount = 0;
            int rest = plate.WidthWithTrim;

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

        private double CalculatetMaxQuantityRestHorizontal(Saw saw, Plate plate, Product product, int quantity)
        {
            int length = (product.Length * quantity) + (quantity - 1 + saw.Thickness);
            int rest = (plate.LengthWithTrim - length) * product.Width;
            return (double)rest / plate.AreaWithTrim;
        }
        private double CalculateMaxQuantityRestVertical(Saw saw, Plate plate, Product product, int quantity)
        {
            int width = (product.Width * quantity) + (quantity - 1 + saw.Thickness);
            int rest = (plate.WidthWithTrim - width) * product.Width;
            return (double)rest / plate.AreaWithTrim;
        }

        private double CalculateRestHorizontal(Plate plate)
        {
            Product? lastProd = plate.Products.MaxBy(c => c.X);
            int lenght = plate.LengthWithTrim - lastProd.Length;
            return (double)(lenght * lastProd.Width) / plate.AreaWithTrim;
        }

        public bool PlaceNextInBundle(Saw saw, List<Plate> plates, Product product)
        {
            int totalLength = (product.Length * product.Quantity) + (saw.Thickness * (product.Quantity - 1));
            int totalWidth = (product.Width * product.Quantity) + (saw.Thickness * (product.Quantity - 1));

            // past het langs elkaar?
            bool horizontal = plates[0].LengthWithTrim > totalLength;
            bool vertical = plates[0].WidthWithTrim > totalWidth;

            // als er overschot is dan moet je de zaagblad breedte bereken voor elke snede
            int lCount= plates[0].LengthWithTrim % product.Length;
            int wCount= plates[0].WidthWithTrim % product.Width;

            int maxHorizontal = CalculateQuantityHorizontal(saw, plates[0], product);
            int maxVertical = CalculateQuantityVertical(saw, plates[0], product);

            double restHorPercentage = CalculatetMaxQuantityRestHorizontal(saw, plates[0], product, maxHorizontal);
            double restVertPercentage = CalculateMaxQuantityRestVertical(saw, plates[0], product, maxVertical);

            for (int i = 0; i < product.Quantity; i++)
            {
                Product newProd = new Product(1, "", product.Length, product.Width, product.Height, product.Info);

                
            }

            //for (int i = 0; i < product.Quantity; i++)
            //{
            //    Product prod = new Product(1, "", product.Length, product.Width, product.Height, product.Info);
            //    if (plates[0].Products.Count == 0)
            //    {
            //        prod.X = 0;
            //        prod.Y = 0;
            //    }
            //    else
            //    {
            //        Product? lastItemVertical = plates[0].Products.MaxBy(x => x.Y);
            //        if ((lastItemVertical.Y + lastItemVertical.Width + saw.Thickness + prod.Width) < plates[0].WidthWithTrim)
            //        {
            //            // kan er nog een onder?
            //            prod.X = 0;
            //            prod.Y = lastItemVertical.Y + lastItemVertical.Width + saw.Thickness;
            //        }
            //        else
            //        {
            //            // kan er nog een langs
            //            Product? lastItemHorizontal = plates[0].Products.MaxBy(x => x.X);
            //            if ((lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness + prod.Length) < plates[0].LengthWithTrim)
            //            {
            //                if (!plates[0].Products.Any(c => c.X == lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness))
            //                {
            //                    prod.X = lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness;
            //                    prod.Y = 0;
            //                }
            //                else
            //                {
            //                    prod.X = lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness;
            //                    prod.Y = lastItemHorizontal.Y + lastItemHorizontal.Width;
            //                }

            //            }
            //        }

            //    }

            //    plates[0].Products.Add(prod);

            //}
            return false;
        }

        public void Step1()
        {
            //// Vertical
            //for (int i = 0; i < maxVertical; i++)
            //{
            //    Product prod = new Product(1, "", product.Length, product.Width, product.Height, product.Info);
            //    if (i == 0)
            //    {
            //        prod.X = 0;
            //        prod.Y = 0;
            //    }
            //    else
            //    {
            //        Product? lastItemVertical = plates[0].Products.MaxBy(x => x.Y);

            //        prod.X = 0;
            //        prod.Y = lastItemVertical.Y + lastItemVertical.Width + saw.Thickness;
            //    }
            //    plates[0].Products.Add(prod);
            //}

            //// Horizontal
            //for(int i = 0; i < maxHorizontal; i++)
            //{
            //    Product prod = new Product(1, "", product.Length, product.Width, product.Height, product.Info);
            //    if(i == 0)
            //    {
            //        prod.X = 0;
            //        prod.Y = 0;
            //    }
            //    else
            //    {
            //        Product? lastItemHorizontal = plates[0].Products.MaxBy(x => x.X);

            //        prod.X = lastItemHorizontal.X + lastItemHorizontal.Length + saw.Thickness;
            //        prod.Y = 0;
            //    }
            //    plates[0].Products.Add(prod);
            //}
        }

        public bool PlaceNext(Saw saw, List<Plate> plates, List<Product> products)
        {
            throw new NotImplementedException();
        }

        public List<Svg> Place(Saw saw, List<Plate> plates, List<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
