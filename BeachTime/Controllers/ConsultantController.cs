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

			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

	        var consultant = new ConsultantEditViewModel()
	        {
				Projects = new List<ProjectViewModel>() { new ProjectViewModel() { ProjectName = "BeachTime", IsCompleted = false } },
				// TODO Projects = UserManager.GetUserProjects(user).ToList(),

				SkillList = UserManager.GetUserSkills(user).ToList(),
				
				Status = "On a project"
				// TODO Status = UserManager.GetUserBeachStatus(user)
	        };

            return View(consultant);
        }

        // POST: Consultant/Edit
        [HttpPost]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

            try
            {
				// Find the user in the database and retrieve basic account information
				var user = UserManager.FindById(User.Identity.GetUserId());

				// TODO: Projects
				//UserManager.SetUserProjects(user, Request.Form["Projects"]);

				// TODO: Skill tags
				List<string> skillsList = new List<string>();
				UserManager.SetUserSkills(user, skillsList);
				
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
