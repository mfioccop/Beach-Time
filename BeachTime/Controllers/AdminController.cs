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
	/// <summary>
	/// 
	/// </summary>
	[AuthorizeBeachUser(Roles = "Admin")]
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
		/// <value>
		/// The user manager.
		/// </value>
		public BeachUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BeachUserManager>(); }

			private set { _userManager = value; }

		}

		#endregion

		#region Index

		// GET: Admin
		/// <summary>
		/// GET: Admin index page (dashboard)
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			try
			{
				// Get all users and initialize the view model
				var users = UserManager.FindAll();

				var store = new UserStore();
				var user = UserManager.FindById(User.Identity.GetUserId());
				var indexViewmodel = new AdminIndexViewModel
				{
					UserViewModels = new List<AdminUserViewModel>(),
					RequestViewModels = new List<AdminRoleRequestViewModel>(),
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
						Id = user.UserId,
						Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project"
					}
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
						UserId = beachUser.UserId,

					};
					indexViewmodel.UserViewModels.Add(userViewModel);
				}

				// TODO: FindAll requests from database
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
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}

			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region UpdateUser

		// GET: Admin/UpdateUser/5
		/// <summary>
		/// GET: Update user page.
		/// </summary>
		/// <param name="id">The id of the user to update.</param>
		/// <returns></returns>
		public ActionResult UpdateUser(int id)
		{
			try
			{
				// Find the user in the database and retrieve basic account information
				var user = UserManager.FindById(id.ToString());

				// URL id doesn't match a user in the database, 404
				if (user == null)
				{
					HttpContext.AddError(new HttpException(404, "Internal server error."));
				}

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
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}

			return RedirectToAction("Index", "Home");
		}

		// POST: Admin/UpdateUser/5
		/// <summary>
		/// POST: Updates a user's information.
		/// </summary>
		/// <param name="model">The model containing user information to update.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateUser(AdminUserViewModel model)
		{
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
				HttpContext.AddError(new HttpException(404, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region UpdateRole

		// GET: Admin/UpdateRole/5
		/// <summary>
		/// GET: View a role request details.
		/// </summary>
		/// <param name="id">The id of the request.</param>
		/// <returns></returns>
		public ActionResult UpdateRole(int id)
		{

			try
			{
				var store = new UserStore();
				var request = store.GetRoleChangeRequestById(id.ToString()).First();

				// URL id doesn't match a user in the database, 404
				if (request == null)
				{
					return RedirectToAction("PageNotFound", "Home");
				}

				// Find the user in the database and retrieve basic account information
				var user = UserManager.FindById(request.UserId.ToString());

				// URL id doesn't match a user in the database, 404
				if (user == null)
				{
					return RedirectToAction("PageNotFound", "Home");
				}

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
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(404, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		// POST: Admin/Accept/5
		/// <summary>
		/// POST: Accepts a role request.
		/// </summary>
		/// <param name="accept">The model containing request to accept.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[MultipleButton(Name = "action", Argument = "AcceptRole")]
		public ActionResult AcceptRole(AdminUpdateRoleViewModel accept)
		{
			try
			{
				// Add user to role if they aren't already in it
				if (!UserManager.IsInRole(accept.UserId.ToString(), accept.RoleName))
				{
					UserManager.AddToRole(accept.UserId.ToString(), accept.RoleName);
				}

				// Remove role request from the database
				var store = new UserStore();
				store.RemoveRoleRequestAsync(accept.RequestId.ToString());

				return RedirectToAction("Index");
			}
			catch
			{
				return PartialView("_UpdateRole");
			}
		}

		// POST: Admin/Accept/5
		/// <summary>
		/// POST: Denies a role request.
		/// </summary>
		/// <param name="reject">The model containing request to reject.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[MultipleButton(Name = "action", Argument = "DenyRole")]
		public ActionResult DenyRole(AdminUpdateRoleViewModel reject)
		{
			try
			{
				// Remove role request from the database
				var store = new UserStore();
				store.RemoveRoleRequestAsync(reject.RequestId.ToString());

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
