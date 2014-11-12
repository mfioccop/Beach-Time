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
    public class ConsultantController : Controller
    {

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

	    // GET: Consultant
        public ActionResult Index()
        {
	        if (!UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
		        return RedirectToAction("Login", "Account");
			
			// Find the user in the database and retrieve basic account information
	        var user = UserManager.FindById(User.Identity.GetUserId());

			// TODO: Implement actual UserStore call
	        var cons = new ConsultantIndexViewModel()
	        {
				FirstName = user.Email,
				LastName = "Doe",
				CurrentProject = "On the beach",
				SkillList = UserManager.GetSkills();
			};

            return View(cons);
        }


        // GET: Consultant/Edit/5
        public ActionResult Edit(int id)
        {
			if (!UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			// Current Project
			// Skill tags
			// New email
			// Confirm new email
			// New password
			// Confirm new password

			// OLD PASSWORD???

            return View();
        }

        // POST: Consultant/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
			if (!UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

            try
            {
                // TODO: Add update logic here

				// Current Project
				// Skill tags
				// New email
				// Confirm new email
				// New password
				// Confirm new password



				// OLD PASSWORD???

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


		// GET: Consultant/Upload
	    public ActionResult Upload()
	    {
			if (!UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

		    return PartialView("_Upload");
	    }

    }
}
