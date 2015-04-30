using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	/// Controls all Administrator functionality.  This includes user and role management.
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
				IEnumerable<BeachUser> users = UserManager.FindAll();

				UserStore store = new UserStore();
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				if (user == null)
				{
					HttpContext.AddError(new HttpException(403, "Not authorized."));
					return RedirectToAction("Index", "Home");
				}

				AdminIndexViewModel indexViewmodel = new AdminIndexViewModel
				{
					UserViewModels = new List<AdminUserViewModel>(),
					RequestViewModels = new List<AdminRoleRequestViewModel>(),
					NewUserViewModel = new RegisterViewModel()
					{
						FirstName = "",
						LastName = "",
						UserName = "",
						Email = "",
						Password = "",
						ConfirmPassword = ""
					},
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
				foreach (BeachUser beachUser in users)
				{
					AdminUserViewModel userViewModel = new AdminUserViewModel()
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
				IEnumerable<RoleChangeRequest> requests = store.GetAllRoleChangeRequests();

				// Populate requests for new roles
				foreach (RoleChangeRequest request in requests)
				{
					// TODO: fill in info from each request + add to RequestViewModels
					AdminRoleRequestViewModel requestViewModel = new AdminRoleRequestViewModel()
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
				BeachUser user = UserManager.FindById(id.ToString());

				// URL id doesn't match a user in the database, 404
				if (user == null)
				{
					HttpContext.AddError(new HttpException(404, "Page not found."));
					return RedirectToAction("Index");
				}

				AdminUserViewModel userViewModel = new AdminUserViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					UserName = user.UserName,
					UserId = user.UserId,
					DeleteUser = false
				};

				return PartialView("_UpdateUser", userViewModel);
			}
			catch (InvalidOperationException ioe)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));				
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}

			return RedirectToAction("Index");
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
				UserStore store = new UserStore();
				BeachUser user = UserManager.FindById(model.UserId.ToString());


				// Delete user from the database if requested. WARNING: Permanent change!
				if (model.DeleteUser)
				{
					store.DeleteAsync(user);
					return RedirectToAction("Index");
				}


				// Find the user in the database and populate it with updated data
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.Email = model.Email;
				user.UserName = model.UserName;

				// Update the user in the database
				store.UpdateAsync(user);

				return RedirectToAction("Index");
			}
			catch
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index");
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
				UserStore store = new UserStore();
				RoleChangeRequest request = store.GetRoleChangeRequestById(id.ToString()).First();	// Throws InvalidOperationException if no results for that id

				// URL id doesn't match a user in the database, 404
				if (request == null)
				{
					HttpContext.AddError(new HttpException(404, "Page not found."));
					return RedirectToAction("PageNotFound", "Home");
				}

				// Find the user in the database and retrieve basic account information
				BeachUser user = UserManager.FindById(request.UserId.ToString());

				// URL id doesn't match a user in the database, 404
				if (user == null)
				{
					HttpContext.AddError(new HttpException(404, "Page not found."));
					return RedirectToAction("PageNotFound", "Home");
				}

				AdminUpdateRoleViewModel updateRoleViewModel = new AdminUpdateRoleViewModel()
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
			catch (InvalidOperationException ioe)
			{
				HttpContext.AddError(new HttpException(404, "Page not found."));
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}

			return RedirectToAction("Index");
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

				try
				{
					// Remove role request from the database
					UserStore store = new UserStore();
					store.RemoveRoleRequestAsync(accept.RequestId.ToString());
				}
				catch (ArgumentNullException e)
				{
					Console.WriteLine(e);
					HttpContext.AddError(new HttpException(500, "Internal server error."));
					return RedirectToAction("Error500", "Error");
				}

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
				return PartialView("_UpdateRole");
			}
		}

		// POST: Admin/Deny/5
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
				UserStore store = new UserStore();
				store.RemoveRoleRequestAsync(reject.RequestId.ToString());

				return RedirectToAction("Index");
			}
			catch (ArgumentNullException ane)
			{
				HttpContext.AddError(new HttpException(404, "Page not found."));
				return RedirectToAction("Index");

			}
			catch (InvalidOperationException ioe)
			{
				HttpContext.AddError(new HttpException(404, "Page not found."));
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
				return PartialView("_UpdateRole");
			}
		}

		#endregion



		/// <summary>
		/// Returns a view for adding a new user.
		/// </summary>
		/// <returns></returns>
		public ActionResult AddNewUser()
		{
			return PartialView("AddNewUser");
		}


		/// <summary>
		/// Adds a new user to the database.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewUser(RegisterViewModel model)
		{
			if (model == null)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error"));
				return RedirectToAction("Index");
			}

			try
			{
				if (ModelState.IsValid)
				{
					var user = new BeachUser()
					{
						UserName = model.UserName,
						Email = model.Email,
						FirstName = model.FirstName,
						LastName = model.LastName
					};
					IdentityResult result = UserManager.CreateAsync(user, model.Password).Result;

				}
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error"));
				return RedirectToAction("Index");				
			}

			// If we got this far, something failed, redisplay form
			return RedirectToAction("Index");
		}
	}
}
