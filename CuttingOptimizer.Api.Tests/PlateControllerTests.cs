using CuttingOptimizer.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Api.Tests
{
    public class PlateControllerTests
    {
        private PlateController controller;

        [SetUp]
        public void SetUp() {
            this.controller = new PlateController();
        }

        [Test]
        public void GetAllOk()
        {

            //Act
            var result = controller.GetAll().Result as OkObjectResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void GetAllNotFound()
        {

            //Act
            var result = controller.FindByName("xxx").Result as NotFoundResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }
    }
}
