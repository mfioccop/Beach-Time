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

	public class HomeController : Controller
	{
		#region UserManager_HelperMethods

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

		private HomeNavbarViewModel getHomeNavbarViewModel()
		{
			var navbarViewModel = new HomeNavbarViewModel()
			{
				FirstName = String.Empty,
				LastName = String.Empty,
				Email = String.Empty,
				Id = -1
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

			return navbarViewModel;
		}

		#endregion

		public ActionResult Index()
		{
			return View(getHomeNavbarViewModel());
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View(getHomeNavbarViewModel());
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View(getHomeNavbarViewModel());
		}
	}
}