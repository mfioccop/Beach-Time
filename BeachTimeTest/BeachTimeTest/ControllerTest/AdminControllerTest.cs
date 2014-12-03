using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeachTime.Controllers;
using System.Web.Mvc;
using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Moq;
using BeachTime;

namespace BeachTimeTest.ControllersTest
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public void IndexActionReturnsIndexView()
        {
            AdminController controller = new AdminController();          

            var result = controller.Index();

            Assert.IsNotNull(result);
        }
    }
}
