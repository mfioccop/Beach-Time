using System;
using NUnit.Framework;
using BeachTime.Controllers;
using BeachTime.Models;
using System.Web.Mvc;

namespace BeachTimeTest.ControllersTest
{
    [TestFixture]
    class AccountControllerTest
    {
        [Test]
        public void LoginActionReturnsLoginView()
        {
            string expected = "";
            AccountController controller = new AccountController();

            var result = controller.Login("/Account/Login") as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }
    }
}
