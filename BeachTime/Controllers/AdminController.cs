using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeachTime.Data;
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

		#region Index

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

		#endregion

		// GET: Admin/UpdateUser/5
		public ActionResult UpdateUser(int id)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(id.ToString());

			var userViewModel = new AdminUserViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
				Id = user.UserId
			};

			return PartialView("_UpdateUser", userViewModel);
		}

		// POST: Admin/UpdateUser/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateUser(AdminUserViewModel model)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			try
			{
				// Find the user in the database and populate it with updated data
				var user = UserManager.FindById(model.Id.ToString());
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.Email = model.Email;
				user.UserName = model.UserName;

				// Update the user in the database
				var store = new UserStore();
				store.UpdateAsync(user);

				return RedirectToAction("Index");
			}
			catch
			{
				return PartialView("_UpdateUser");
			}



		}



	}
}
