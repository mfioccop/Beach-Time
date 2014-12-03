using System;
using NUnit.Framework;
using BeachTime.Controllers;
using BeachTime.Models;
using System.Web.Mvc;

namespace BeachTimeTest.ControllerTest
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void IndexActionReturnsIndexView()
        {
            string expected = "";
            HomeController controller = new HomeController();

            var result = controller.Index() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }

        [Test]
        public void AboutActionReturnsAboutView()
        {
            string expected = "";
            HomeController controller = new HomeController();

            var result = controller.About() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }

        [Test]
        public void ContactActionReturnsContactView()
        {
            string expected = "";
            HomeController controller = new HomeController();

            var result = controller.Contact() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }
    }
}
