using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
					UserId = beachUser.UserId
				};
				indexViewmodel.UserViewModels.Add(userViewModel);
			}

			// TODO: FindAll requests from database
			var store = new UserStore();
			var requests = store.GetAllRoleChangeRequests();

			// Populate requests for new roles
			foreach (var request in requests)
			{
				// TODO: fill in info from each request + add to RequestViewModels
				var requestViewModel = new AdminRoleRequestViewModel()
				{
					RequestId = request.RequestId,
					UserId = request.UserId,
					RoleName = request.RoleName,
					RequestDate = request.RequestDate
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
				UserId = user.UserId
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
				var user = UserManager.FindById(model.UserId.ToString());
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

		// GET: Admin/UpdateRole/5
		public ActionResult UpdateRole(int id)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			// TODO: Find the request in the database
			var store = new UserStore();
			var request = store.GetRoleChangeRequestById(id.ToString()).First();

			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(request.UserId.ToString());

			var updateRoleViewModel = new AdminUpdateRoleViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
				RequestId = request.RequestId,
				UserId = user.UserId,
				RoleName = request.RoleName,
				RequestDate = request.RequestDate
			};

			return PartialView("_UpdateRole", updateRoleViewModel);
		}

		// POST: Admin/Accept/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[MultipleButton(Name = "action", Argument = "AcceptRole")]
		public ActionResult AcceptRole(AdminUpdateRoleViewModel accept)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			try
			{

				// TODO: update UserRoles table with new entry

				// Add user to role if they aren't already in it
				if (!UserManager.IsInRole(accept.UserId.ToString(), accept.RoleName))
				{
					UserManager.AddToRole(accept.UserId.ToString(), accept.RoleName);
				}

				// TODO: get RoleRequest store
				// delete request from table


				return RedirectToAction("Index");
			}
			catch
			{
				return PartialView("_UpdateRole");
			}
		}

		// POST: Admin/Accept/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[MultipleButton(Name = "action", Argument = "DenyRole")]
		public ActionResult DenyRole(AdminUpdateRoleViewModel model)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
				return RedirectToAction("Login", "Account");

			try
			{
				// TODO: get RoleRequest store
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
