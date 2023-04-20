using CuttingOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain.Tests.Models
{
    public class PlateTests
    {
        private Plate plate;

        [SetUp]
        public void SetUp() {
            plate = new Plate
            {
                ID = "PL150",
                Length = 1000,
                Width = 100,
                Height = 5,
                Priority = 0,
                Base = true,
                Quantity = 1,
                Trim = 0,
                Veneer = false
            };
        }

        [Test]
        public void ConstructorWithZeroParams()
        {
            //Act 
            plate = new Plate();

            //Assert
            Assert.IsNotNull(plate);
            Assert.IsNull(plate.ID);
            Assert.That(plate.Length, Is.EqualTo(0));
            Assert.That(plate.Width, Is.EqualTo(0));
            Assert.That(plate.Height, Is.EqualTo(0));
            Assert.That(plate.Priority, Is.EqualTo(0));
            Assert.That(plate.Trim, Is.EqualTo(0));
            Assert.That(plate.Quantity, Is.EqualTo(0));
            Assert.That(plate.Veneer, Is.EqualTo(false));
            Assert.That(plate.Base, Is.EqualTo(false));
        }

        [Test]
        public void ConstructorWithPlateParam()
        {
            //Act 
            Plate newPlate = new Plate(plate);

            //Assert
            Assert.IsNotNull(plate);
            Assert.That(newPlate.ID, Is.EqualTo(plate.ID));
            Assert.That(newPlate.Length, Is.EqualTo(plate.Length));
            Assert.That(newPlate.Width, Is.EqualTo(plate.Width));
            Assert.That(newPlate.Height, Is.EqualTo(plate.Height));
            Assert.That(newPlate.Priority, Is.EqualTo(plate.Priority));
            Assert.That(newPlate.Trim, Is.EqualTo(plate.Trim));
            Assert.That(newPlate.Quantity, Is.EqualTo(1));
            Assert.That(newPlate.Veneer, Is.EqualTo(plate.Veneer));
            Assert.That(newPlate.Base, Is.EqualTo(plate.Base));
        }

        [Test]
        public void Area() {
            //Assert
            Assert.That(plate.Area, Is.EqualTo(plate.Length * plate.Width));
        }

        [Test]
        public void LengthWithTrim()
        {
            // Assert
            Assert.That(plate.LengthWithTrim, Is.EqualTo(plate.Length - plate.Trim));
        }

        [Test]
        public void WidthWithTrim()
        {
            // Assert
            Assert.That(plate.WidthWithTrim, Is.EqualTo(plate.Width - plate.Trim));
        }

        [Test]
        public void AreWithTrim()
        {
            //Assert
            Assert.That(plate.AreaWithTrim, Is.EqualTo(plate.LengthWithTrim * plate.WidthWithTrim));
        }
    }
}
