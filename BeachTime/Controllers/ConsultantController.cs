﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeachTime.Data;
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

			// Get all projects
			var projectRepo = new ProjectRepository();
			var projects = projectRepo.FindAll();
			var projectViewModels = new List<ProjectViewModel>();

			// Filter projects for this user
			foreach (var project in projects.Where(p => p.UserId == user.UserId))
			{
				var pvm = new ProjectViewModel()
				{
					ProjectName = project.Name,
					IsCompleted = project.Completed
				};
				projectViewModels.Add(pvm);
			}

			// Construct view model for the consultant
	        var consultant = new ConsultantIndexViewModel()
	        {
				FirstName = user.FirstName,
				LastName = user.LastName,
				Projects = projectViewModels,
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
	        
			// Get projects for this user
			var projectRepo = new ProjectRepository();
	        var projects = projectRepo.FindAll();
	        var projectViewModels = new List<ProjectViewModel>();

			foreach (var project in projects.Where(p => p.UserId == user.UserId))
	        {
		        var pvm = new ProjectViewModel()
		        {
			        ProjectName = project.Name,
			        IsCompleted = project.Completed,
					ProjectId = project.ProjectId
		        };
				projectViewModels.Add(pvm);
	        }

			var consultant = new ConsultantEditViewModel()
	        {
				Projects = projectViewModels,

				SkillList = UserManager.GetUserSkills(user).ToList(),
				
				Status = UserManager.UserOnBeach(user).ToString()
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

				// TODO: Skill tags
				var skillsList = new List<string>();
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



		// GET: Consultant/CreateProject
	    public ActionResult CreateProject()
	    {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			return PartialView("_CreateProject");
	    }


		// POST: Consultant/CreateProject
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateProject(FormCollection collection)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var project = new Project()
				{
					Name = collection["ProjectName"],
					Completed = collection["IsCompleted"].Contains("true"),
					UserId = user.UserId
				};
				
				var projectRepo = new ProjectRepository();
				projectRepo.Create(project);

				return RedirectToAction("Index");
			}
			catch
			{
				return View("_CreateProject");
			}
		}

		
		// GET: Consultant/UpdateProject/5
	    public ActionResult UpdateProject(int id)
	    {
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");


			// TODO: CHECK WITH IAN IF THIS IS HOW THIS SHOULD BE DONE
		    var projectRepo = new ProjectRepository();
		    Project project = projectRepo.FindById(id);
			
			var projectViewModel = new ProjectViewModel()
		    {
				ProjectName = project.Name,
				IsCompleted = project.Completed,
				ProjectId = project.ProjectId
		    };

			return PartialView("_UpdateProject", projectViewModel);
	    }


		// POST: Consultant/UpdateProject
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProject(FormCollection collection)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var project = new Project()
				{
					Name = collection["ProjectName"],
					Completed = collection["IsCompleted"].Contains("true"),
					UserId = user.UserId,
					ProjectId = int.Parse(collection["ProjectId"])
				};

				var projectRepo = new ProjectRepository();
				projectRepo.Update(project);

				return RedirectToAction("Index");
			}
			catch
			{
				return View("_UpdateProject");
			}
		}


		// GET: Consultant/UploadFile
		public ActionResult UploadFile()
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			return PartialView("_UploadFile", new FileUploadViewModel());
		}


		// POST: Consultant/UploadFile
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UploadFile(FileUploadViewModel model)
		{
			if (User.Identity.GetUserId() == null || !UserManager.IsInRole(User.Identity.GetUserId(), "Consultant"))
				return RedirectToAction("Login", "Account");

			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var validFileTypes = new string[]
				{
					"application/pdf",
					"application/msword",
					"application/rtf",
					"application/x-rtf",
					"text/richtext"
				};

				if (model.FileUpload == null || model.FileUpload.ContentLength == 0)
				{
					ModelState.AddModelError("FileUpload", "A file is required");
				}
				else if (!validFileTypes.Contains(model.FileUpload.ContentType))
				{
					ModelState.AddModelError("FileUpload", "Please choose a valid file type (PDF, DOC, RTF)");
				}

				if (ModelState.IsValid)
				{
					// TODO: create new file (once Ian finishes table in db)

					if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
					{
						var uploadDirectory = ConfigurationManager.AppSettings["FileUploadPath"];
						uploadDirectory += "/" + user.UserId;

						var filePath = Path.Combine(Server.MapPath(uploadDirectory), model.FileUpload.FileName);

					}
				}


				return RedirectToAction("Index");
			}
			catch
			{
				return View("_UploadFile");
			}
		}

    }
}
