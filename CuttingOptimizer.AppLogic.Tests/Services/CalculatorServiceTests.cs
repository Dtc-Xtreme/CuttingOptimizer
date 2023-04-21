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

        [SetUp]
        public void Setup()
        {
            calculatorService = new CalculatorService();
        }

        [TestCase(1,200,30, 1000, 300, 5)]
        [TestCase(2, 200, 30, 200, 66, 5)]
        public void CalculateGroupsVertical(int prodQty, int prodLength, int prodWidth, int groupLength, int groupWidth, int sawThickness)
        {
            //Arange
            Product sp = new Product(prodQty, "X1", prodLength, prodWidth, 0);
            Saw sw = new Saw("SX1", sawThickness);
            Svg svg = new Svg();
            Group sg = new Group(0, 0, 0, groupLength, groupWidth);
            sg.Svg = svg;
            int qty = 2;

            //Act
            Group? result = calculatorService.CalculateGroupsVertical(sp, sw, sg, qty);
        }

        [TestCase(200, 50, 200, 50, false)]
        [TestCase(200, 50, 100, 50, true)]
        public void CalculateGroupRight(int groupLength, int groupWidth, int lGroupLength, int lGroupWidth, bool hasUnderGroup)
        {
            List<Group> newGroups = new List<Group>();
            Saw sw = new Saw("SX1", 2);
            Group g = new Group(0, 0, 0, groupLength, groupWidth);
            Group lg = new Group(0,0,0, lGroupLength, lGroupWidth);    

            Group? result = calculatorService.CalculateGroupRight(sw, g, lg, newGroups);

            if (hasUnderGroup)
            {
                Assert.NotNull(result);
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestCase(200, 50, 200, 50, false)]
        [TestCase(200, 50, 200, 20, true)]
        public void CalculateGroupUnder(int groupLength, int groupWidth, int lGroupLength, int lGroupWidth, bool hasUnderGroup)
        {
            List<Group> newGroups = new List<Group>();
            Saw sw = new Saw("SX1", 2);
            Group g = new Group(0, 0, 0, groupLength, groupWidth);
            Group lg = new Group(0, 0, 0, lGroupLength, lGroupWidth);

            Group? result = calculatorService.CalculateGroupUnder(sw, g, lg, newGroups);

            if (hasUnderGroup)
            {
                Assert.NotNull(result);
            }
            else
            {
                Assert.IsNull(result);
            }
        }
    }
}
