using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeachTime.Data;
using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease;
using WebGrease.Css.Extensions;

namespace BeachTime.Controllers
{
    public class ExecutiveController : Controller
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

        // GET: Executive
        public ActionResult Index()
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Executive"))
				return RedirectToAction("Login", "Account");

            // Find the user in the database and retrieve basic account information
            var user = UserManager.FindById(User.Identity.GetUserId());

            // Get all consultants on projects
            // TODO: waiting for FindAll()

            // Get all consultants on the beach
            UserStore beachedUserStore = new UserStore();
            var beachUsers = beachedUserStore.GetBeachedUsers();
            int numBeach = beachUsers.Count();

            // Get all skills on the beach
            List<string> beachSkillsList = new List<string>();
            foreach (var consultant in beachUsers)
            {
                var consultantManager = UserManager.FindById(consultant.Id);
                beachSkillsList.AddRange(UserManager.GetUserSkills(consultantManager).ToList());
            }

            // Construct view model for the executive
            var executive = new ExecutiveIndexViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BeachConsultants = numBeach,
                SkillList = beachSkillsList
            };
            
            return View(executive);
        }

        // GET: Executive/Beach/5
        public ActionResult Beach()
        {
            if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Executive"))
                return RedirectToAction("Login", "Account");

            // Get all consultants on the beach as ViewModels
            UserStore beachedUserStore = new UserStore();
            var beachUsers = beachedUserStore.GetBeachedUsers().ToList();
            
            // Construct view model for the beach
            var executive = new ExecutiveBeachViewModel()
            {
                //BeachConsultantViewModels = beachUsers
            };

            return View(executive);
        }

        // GET: Executive/Details/5
        public ActionResult Details(int id)
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Executive"))
				return RedirectToAction("Login", "Account");
	      

            return View();
        }



        // GET: Executive/Edit/5
        public ActionResult Edit(int id)
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Executive"))
				return RedirectToAction("Login", "Account");

			return View();
        }

        // POST: Executive/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Executive"))
				return RedirectToAction("Login", "Account");
	      

            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
