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
	        if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
		        return RedirectToAction("Login", "Account");
			
			// Find the user in the database and retrieve basic account information
	        var user = UserManager.FindById(User.Identity.GetUserId());

			// TODO: Implement actual data calls
	        var consultant = new ConsultantIndexViewModel()
	        {
				FirstName = user.Email,
				LastName = "Doe",
				Projects = new List<ProjectViewModel>
				{
					new ProjectViewModel{IsCompleted = false, ProjectName = "BeachTime"},
					new ProjectViewModel{IsCompleted = true, ProjectName = "Old project"}
				
				},

				SkillList = UserManager.GetUserSkills(user).ToList()
			};

            return View(consultant);
        }


        // GET: Consultant/Edit
        public ActionResult Edit()
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			// Current Project
			// Skill tags
			// New email
			// Confirm new email
			// New password
			// Confirm new password

			// OLD PASSWORD???

			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

	        var consultant = new ConsultantEditViewModel()
	        {
				Projects = new List<ProjectViewModel>() { new ProjectViewModel() { ProjectName = "BeachTime", IsCompleted = false } },
				SkillList = UserManager.GetUserSkills(user).ToList(),
				NewEmail = String.Empty,
				ConfirmEmail = String.Empty,
				NewPassword = String.Empty,
				ConfirmPassword = String.Empty,
				OldPassword = String.Empty,
				Status = "On a project"
				
	        };

            return View(consultant);
        }

        // POST: Consultant/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
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
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

		    return PartialView("_Upload");
	    }

    }
}
