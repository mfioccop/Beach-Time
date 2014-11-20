using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BeachTime.Controllers
{
	public class AdminController : Controller
	{

		#region UserManager

		/// <summary>
		/// Manages user account registration and authentication
		/// </summary>
		private BeachUserManager _userManager;


		/// <summary>
		/// Gets the current UserManager
		/// </summary>
		public BeachUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BeachUserManager>(); }

			private set { _userManager = value; }

		}

		#endregion



		// GET: Admin
		public ActionResult Index()
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			// Get all users and initialize the view model
	        var users = UserManager.FindAll();
	        var indexViewmodel = new AdminIndexViewModel
	        {
		        UserViewModels = new List<AdminUserViewModel>()
	        };

			// Populate the list of users
			foreach (var beachUser in users)
	        {
		        var userViewModel = new AdminUserViewModel()
		        {
			        FirstName = beachUser.FirstName,
					LastName = beachUser.LastName,
					Email = beachUser.Email,
					UserName = beachUser.UserName,
					Id = beachUser.UserId
		        };
				indexViewmodel.UserViewModels.Add(userViewModel);
	        }

            return View(indexViewmodel);
        }




	}
}
