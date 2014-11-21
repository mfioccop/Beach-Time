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
				UserViewModels = new List<AdminUserViewModel>(),
				RequestViewModels = new List<AdminRoleRequestViewModel>()
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

			// TODO: FindAll requests from database
			var requests = new List<BeachUser>();

			// Populate requests for new roles
			foreach (var request in requests)
			{
				// TODO: fill in info from each request + add to RequestViewModels
				var requestViewModel = new AdminRoleRequestViewModel()
				{
					UserId = 1,
					RoleId = 1,
					RequestId = 1
				};
				indexViewmodel.RequestViewModels.Add(requestViewModel);
			}
			
			return View(indexViewmodel);
		}

		#endregion

		#region UpdateUser

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

		#endregion

		#region UpdateRole

		// GET: Admin/UpdateUser/5
		public ActionResult UpdateRole(int id)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			// TODO: Find the request in the database
			//var request = RequestStore.FindById()

			var requestViewModel = new AdminRoleRequestViewModel()
			{
				RequestId = 1,
				UserId = 1,
				RoleId = 1
			};

			// Find the user in the database and retrieve basic account information
			// TODO: find user from the request UserId
			//var user = UserManager.FindById(request.UserId);
			var user = new BeachUser();


			var userViewModel = new AdminUserViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
				Id = user.UserId
			};

			var updateRoleViewModel = new AdminUpdateRoleViewModel()
			{
				RequestViewModel = requestViewModel,
				UserViewModel = userViewModel
			};

			return PartialView("_UpdateRole", updateRoleViewModel);
		}

		// POST: Admin/UpdateUser/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateRole(AdminUpdateRoleViewModel model)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			try
			{
				
				// TODO: update UserRoles table with new entry

				var user = UserManager.FindById(model.UserViewModel.Id.ToString());
				
				UserManager.AddToRole(user.Id, "NEW ROLE");


				// get RoleRequest store
				// delete request from table


				return RedirectToAction("Index");
			}
			catch
			{
				return PartialView("_UpdateRole");
			}
		}

		#endregion


	}
}
