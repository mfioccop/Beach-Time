using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BeachTime.Data;
using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using FileInfo = BeachTime.Data.FileInfo;

namespace BeachTime.Controllers
{
	[AuthorizeBeachUser(Roles = "Consultant")]
	public class ConsultantController : Controller
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

		// GET: Consultant
		public ActionResult Index()
		{
			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

			// Get all projects
			var projectRepo = new ProjectRepository();
			var projects = projectRepo.FindByUserId(user.UserId);
			var projectViewModels = new List<ProjectViewModel>();

			// Create the ProjectViewModels
			foreach (var project in projects)
			{
				var pvm = new ProjectViewModel()
				{
					ProjectName = project.Name,
					IsCompleted = project.Completed,
					ProjectId = project.ProjectId
				};
				projectViewModels.Add(pvm);
			}

			// Get all files
			var fileRepo = new FileRepository();
			var files = fileRepo.FindByUserId(user.UserId);
			var fileViewModels = new List<FileIndexViewModel>();

			// Create the FileIndexViewModels
			foreach (var file in files)
			{
				var fvm = new FileIndexViewModel()
				{
					Title = file.Title,
					Description = file.Description,
					Path = Server.MapPath(file.Path)
				};
				fileViewModels.Add(fvm);
			}

			// Construct view model for the consultant
			var consultant = new ConsultantIndexViewModel()
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Projects = projectViewModels,
				SkillList = UserManager.GetUserSkills(user).ToList(),
				Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
				FileList = fileViewModels,
				Id = user.UserId,
				Navbar = new HomeNavbarViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Id = user.UserId,
					Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project"
				},
				SkillViewModel = new ConsultantSkillViewModel()
			};

			return View(consultant);
		}

		#endregion

		#region Edit

		// GET: Consultant/Edit
		public ActionResult Edit()
		{
			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

			// URL id doesn't match a user in the database, 404
			if (user == null)
			{
				return RedirectToAction("PageNotFound", "Home");
			}

			// Get projects for this user
			var projectRepo = new ProjectRepository();
			var projects = projectRepo.FindByUserId(user.UserId);
			var projectViewModels = new List<ProjectViewModel>();

			foreach (var project in projects)
			{
				var pvm = new ProjectViewModel()
				{
					ProjectName = project.Name,
					IsCompleted = project.Completed,
					ProjectId = project.ProjectId
				};
				projectViewModels.Add(pvm);
			}

			var skills = UserManager.GetUserSkills(user).ToList();

			var consultant = new ConsultantEditViewModel()
			{
				Projects = projectViewModels,

				SkillList = skills,

				SkillsString = string.Join(",", skills.ToArray()),
				Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
				Navbar = new HomeNavbarViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Id = user.UserId,
					Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project"
				}
			};

			return View(consultant);
		}

		// POST: Consultant/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ConsultantEditViewModel model)
		{
			try
			{
				// Find the user in the database and retrieve basic account information
				var user = UserManager.FindById(User.Identity.GetUserId());

				// TODO: Skill tags when Jason updates input method
				var skillsList = model.SkillsString.Split(',').ToList();
				UserManager.SetUserSkills(user, skillsList);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		#endregion

		#region Projects

		// GET: Consultant/CreateProject
		public ActionResult CreateProject()
		{
			return PartialView("_CreateProject");
		}


		// POST: Consultant/CreateProject
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateProject(ProjectCreateViewModel model)
		{
			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var project = new Project()
				{
					Name = model.ProjectName,
					Completed = model.IsCompleted,
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
			var projectRepo = new ProjectRepository();
			Project project = projectRepo.FindByProjectId(id);

			// URL id doesn't match a project in the database, 404
			if (project == null)
			{
				return RedirectToAction("PageNotFound", "Home");
			}

			// Check that the current user owns this project before allowing an update
			if (int.Parse(User.Identity.GetUserId()) != project.UserId)
				return RedirectToAction("Index", "Consultant");

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
		public ActionResult UpdateProject(ProjectViewModel model)
		{
			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var project = new Project()
				{
					Name = model.ProjectName,
					Completed = model.IsCompleted,
					UserId = user.UserId,
					ProjectId = model.ProjectId
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

		#endregion

		#region FileUpload

		// GET: Consultant/UploadFile
		public ActionResult UploadFile()
		{
			return PartialView("_UploadFile", new FileUploadViewModel());
		}


		// POST: Consultant/UploadFile
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UploadFile(FileUploadViewModel model)
		{
			try
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var validFileTypes = new string[]
			    {
				    "application/pdf",		// .pdf
				    "application/msword",	// .doc
					"application/vnd.openxmlformats-officedocument.wordprocessingml.document",	// .docx
				    "application/rtf",		// .rtf
				    "application/x-rtf",	// .rtf
				    "text/richtext"			// .rtf
			    };

				var validFileExtensions = new string[]
				{
					".doc", ".docx", ".pdf", ".rtf"
				};

				if (model.FileUpload == null || model.FileUpload.ContentLength == 0)
				{
					ModelState.AddModelError("FileUpload", "A file is required");
				}
				else if (!validFileTypes.Contains(model.FileUpload.ContentType))
				{
					// FIXES: Firefox issue where .doc files are incorrectly labeled as "application/octet-stream", which is too broad of a filetype to accept
					//	Check the validity of file extension
					if(!validFileExtensions.Contains(Path.GetExtension(model.FileUpload.FileName)))
						ModelState.AddModelError("FileUpload", "Please choose a valid file type (PDF, DOC, DOCX, RTF)");
				}

				if (ModelState.IsValid)
				{
					if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
					{
						var uploadDirectory = ConfigurationManager.AppSettings["FileUploadPath"];
						uploadDirectory += "/" + user.UserId;

						if (!Directory.Exists(Server.MapPath(uploadDirectory)))
						{
							Directory.CreateDirectory(Server.MapPath(uploadDirectory));
						}

						// This is the path used to save the file to the application directory
						var filePath = Path.Combine(Server.MapPath(uploadDirectory), model.FileUpload.FileName);
						model.FileUpload.SaveAs(filePath);

						// This is the tail of the url that will be saved in the database
						var fileUrl = Path.Combine(uploadDirectory, model.FileUpload.FileName);

						// Create the FileInfo instance to add to the database
						var file = new FileInfo()
						{
							Title = model.Title,
							Description = model.Description,
							UserId = user.UserId,
							Path = fileUrl
						};

						var repository = new FileRepository();
						repository.Create(file);
						
						return RedirectToAction("Index");
					}

					ModelState.AddModelError("FileUpload", "Please choose a file to upload");
				}

				return PartialView("_UploadFile");
			}
			catch
			{
				ModelState.AddModelError("", "There was an error processing your file upload, please try again");
				return PartialView("_UploadFile");
			}
		}

		#endregion

		#region Skills

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddSkill(ConsultantIndexViewModel model)
		{
			if (model.SkillViewModel.SkillName == null)
				return RedirectToAction("Index");

			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

			// Get current skills and add the newest addition, then update the database
			var currentSkills = UserManager.GetUserSkills(user);
			currentSkills.Add(model.SkillViewModel.SkillName);
			UserManager.SetUserSkills(user, currentSkills);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteSkill(string name)
		{
			// Find the user in the database and retrieve basic account information
			var user = UserManager.FindById(User.Identity.GetUserId());

			// Get current skills and remove specified skill, then update the database
			var currentSkills = UserManager.GetUserSkills(user);
			currentSkills.Remove(name);
			UserManager.SetUserSkills(user, currentSkills);

			return RedirectToAction("Index");
		}

		#endregion


	}
}
