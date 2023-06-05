using CuttingOptimizer.AppLogic.Models;
using CuttingOptimizer.AppLogic.Services;
using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.AppLogic.Tests.Services
{
    public class CalculatorServiceTests
    {
        private CalculatorService calculatorService;
        private List<Group> groups;
        private List<Product> products;
        private Saw saw;

        [SetUp]
        public void Setup()
        {
            calculatorService = new CalculatorService();
            groups = new List<Group>
            {
                new Group
                {
                    ID = 0,
                    Length = 2050,
                    Width = 1050,
                    X = 0,
                    Y = 0,
                }
            };
        }


        [Test]
        public void CalculatePossibilities()
        {

        }

        [TestCase(1, 200, 30, 1000, 300, 5, 1, 1, true, false, 3)]
        [TestCase(1, 200, 30, 1000, 300, 5, 1, 1, false, false, 3)]
        [TestCase(1, 200, 30, 1000, 300, 5, 1, 1, true, true, 3)]
        [TestCase(1, 200, 30, 1000, 300, 5, 1, 1, false, true, 3)]

        [TestCase(4, 200, 30, 1000, 300, 5, 1, 4, true, false, 6)]
        [TestCase(4, 200, 30, 1000, 300, 5, 1, 4, false, false, 6)]
        [TestCase(4, 200, 30, 1000, 300, 5, 1, 4, true, true, 5)]
        [TestCase(4, 200, 30, 1000, 300, 5, 1, 4, false, true, 5)]
        public void CalculateGroupsVertical(int prodQty, int prodLength, int prodWidth, int groupLength, int groupWidth, int sawThickness, int horQty, int vertQty, bool horizontal, bool rotated, int resultGroupCount)
        {
            //Arange
            Product sp = new Product(prodQty, "X1", prodLength, prodWidth, 0);
            Saw sw = new Saw("SX1", sawThickness);
            Svg svg = new Svg();
            Group sg = new Group
            {
                ID = 0,
                X = 0,
                Y = 0,
                Length = groupLength,
                Width = groupWidth,
                Svg = svg
            };
            int qty = 2;

            //Act
            bool result = calculatorService.CalculateGroups(sp, sw, sg, horQty, vertQty, horizontal, rotated);

            //Assert
            Assert.IsTrue(result);
            Assert.That(svg.Groups.Count, Is.EqualTo(resultGroupCount));
        }

        //[TestCase(200, 50, 200, 50, false)]
        //[TestCase(200, 50, 100, 50, true)]
        //public void CalculateGroupRight(int groupLength, int groupWidth, int lGroupLength, int lGroupWidth, bool hasUnderGroup)
        //{
        //    List<Group> newGroups = new List<Group>();
        //    Saw sw = new Saw("SX1", 2);
        //    Svg svg = new Svg { Priority = 1 };
        //    Group g = new Group
        //    {
        //        ID = 0,
        //        X = 0,
        //        Y = 0,
        //        Length = groupLength,
        //        Width = groupWidth,
        //        Svg = svg
        //    };
        //    Group lg = new Group
        //    {
        //        ID = 1,
        //        X = 0,
        //        Y = 0,
        //        Length = lGroupLength,
        //        Width = lGroupWidth,
        //        Product = new Product { ID = "X1", Quantity = 1, Length = lGroupLength, Width = lGroupWidth },
        //        Svg = svg
        //    };

        //    Group? result = calculatorService.CalculateGroupRight(sw, g, lg, newGroups);

        //    if (hasUnderGroup)
        //    {
        //        Assert.NotNull(result);
        //    }
        //    else
        //    {
        //        Assert.IsNull(result);
        //    }
        //}

        //[TestCase(200, 50, 200, 50, false)]
        //[TestCase(200, 50, 200, 20, true)]
        //public void CalculateGroupUnder(int groupLength, int groupWidth, int lGroupLength, int lGroupWidth, bool hasUnderGroup)
        //{
        //    List<Group> newGroups = new List<Group>();
        //    Saw sw = new Saw("SX1", 2);
        //    Group g = new Group
        //    {
        //        ID = 0,
        //        X = 0,
        //        Y = 0,
        //        Length = groupLength,
        //        Width = groupWidth
        //    };
        //    Group lg = new Group
        //    {
        //        ID = 0,
        //        X = 0,
        //        Y = 0,
        //        Length = lGroupLength,
        //        Width = lGroupWidth
        //    };

        //    Group? result = calculatorService.CalculateGroupUnder(sw, g, lg, newGroups);

        //    if (hasUnderGroup)
        //    {
        //        Assert.NotNull(result);
        //    }
        //    else
        //    {
        //        Assert.IsNull(result);
        //    }
        //}
    }
}
