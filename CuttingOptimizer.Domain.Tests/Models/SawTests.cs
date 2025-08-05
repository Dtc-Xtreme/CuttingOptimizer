using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace CuttingOptimizer.Domain.Tests.Models
{
    public class SawTests
    {

        [Test]
        public void ConstructorWithZeroParams()
        {
            //Act 
            Saw saw = new Saw();

            //Assert
            Assert.That(saw, Is.Not.Null);
            Assert.That(saw.ID, Is.Null);
            Assert.That(saw.Thickness, Is.EqualTo(0));
        }

        [TestCase("SW15", 1, 0)]
        [TestCase("SW15", 50, 0)]
        [TestCase("SW15", 0, 1)]
        [TestCase("SW15", 52, 1)]
        [TestCase(null, 5, 1)]
        [TestCase(null, 52, 2)]
        public void ConstructorWithTwoParams(string? id, int thickness, int errors)
        {
            //Act 
            Saw saw = new Saw(id, thickness);

            //Assert
            Assert.That(saw, Is.Not.Null);
            Assert.That(saw.ID, Is.EqualTo(id));
            Assert.That(saw.Thickness, Is.EqualTo(thickness));
            Assert.That( Validation.ValidateModel(saw).Count, Is.EqualTo(errors));
        }

        [Test]
        public void SawToString()
        {
            //Arrange
            string id = "SW15";
            int thickness = 5;

            //Act 
            Saw saw = new Saw(id, thickness);

            //Assert
            Assert.That(saw.ToString, Is.EqualTo("ID: " + id + " | Thickness: " + thickness));
        }

    }
}
