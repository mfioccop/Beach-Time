using BeachTime.Controllers;
using BeachTime.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
//using System.Web.HttpContext;
using System.Net.Http;
using BeachTime;
using System.Windows.Forms;

namespace BeachTimeTest.ControllersTest
{
    [TestFixture]
    class AccountControllerTest
    {
        LoginViewModel lvm = new LoginViewModel();
        
        
        [Test]
        public void LoginActionReturnsLoginView()
        {
            LoginViewModel lvm = new LoginViewModel();
            lvm.UserName = "blah1";
            lvm.Password = "password";
            string expected = "";
            AccountController controller = new AccountController();

            var result = controller.Login("/Account/Login") as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }

        [Test]
        public async Task LoginActionReturnsLogin()
        {
            LoginViewModel model = new LoginViewModel();
            var controller = new AccountController();
            var result = controller.Login(model, "/Account/Login");
            Assert.IsNotNull(result);
        }    

        [Test]
        public void RegisterActionReturnsRegisterView()
        {
            string expected = "";
            AccountController controller = new AccountController();

            var result = controller.Register() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }

        [Test]
        public async Task RegisterActionReturnsRegister()
        {
            RegisterViewModel model = new RegisterViewModel();
            var controller = new AccountController();
            var result = controller.Register(model);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ConfirmEmailActionReturnsConfirmEmailView()
        {
            var expected = "Error";
            var controller = new AccountController();
            var result = await controller.ConfirmEmail(null, null) as ViewResult;
            Assert.AreEqual(expected, result.ViewName);

            // TODO test other two methods using generated code and uid combo?
        }

        [Test]
        public void ForgotPasswordActionReturnsForgotPasswordView()
        {
            string expected = "";
            AccountController controller = new AccountController();

            var result = controller.ForgotPassword() as ViewResult;

            Assert.AreEqual(expected, result.ViewName);
        }

        
    }
}
