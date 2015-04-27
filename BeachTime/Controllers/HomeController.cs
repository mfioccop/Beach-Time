using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BeachTime.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	/// <summary>
	/// Controller for most pages common to all users except those directly related to account information.
	/// </summary>
	public class HomeController : Controller
	{
		#region UserManager_HelperMethods

		/// <summary>
		/// Manages user account registration and authentication.
		/// </summary>
		private BeachUserManager _userManager;

		/// <summary>
		/// Gets the current UserManager.
		/// </summary>
		public BeachUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BeachUserManager>(); }

			private set { _userManager = value; }

		}

		/// <summary>
		/// Helper method to generate a HomeNavbarViewModel for the logged in user.
		/// </summary>
		/// <returns>HomeNavbarViewModel populated with the information of the current user.</returns>
		public HomeNavbarViewModel getHomeNavbarViewModel()
		{
			try
			{
				var navbarViewModel = new HomeNavbarViewModel()
				{
					FirstName = String.Empty,
					LastName = String.Empty,
					Email = String.Empty,
					Id = -1,
					Status = String.Empty
				};

				// If no user is logged in, then return with the dummy user
				if (User.Identity.GetUserId() == null) return navbarViewModel;
			
				// Otherwise find the user in the database and retrieve basic account information
				var user = UserManager.FindById(User.Identity.GetUserId());

				// Populate the view model with the proper info
				navbarViewModel.FirstName = user.FirstName;
				navbarViewModel.LastName = user.LastName;
				navbarViewModel.Email = user.Email;
				navbarViewModel.Id = user.UserId;
				navbarViewModel.Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project";

				return navbarViewModel;
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return new HomeNavbarViewModel();
		}

		#endregion

		/// <summary>
		/// GET: Index page for the application.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View(new HomeViewModel{ Navbar = getHomeNavbarViewModel()});
		}

		/// <summary>
		/// GET: About page for the application.
		/// </summary>
		/// <returns></returns>
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View(new HomeViewModel { Navbar = getHomeNavbarViewModel() });
		}

		/// <summary>
		/// GET: Contact page for the application
		/// </summary>
		/// <returns></returns>
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View(new HomeViewModel { Navbar = getHomeNavbarViewModel() });
		}

		/// <summary>
		/// GET: 404 page
		/// </summary>
		/// <returns></returns>
		public ActionResult PageNotFound()
		{
			return View();
		}
	}
}