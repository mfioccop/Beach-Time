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
		/// <summary>
		/// GET: Consultant index page (dashboard).
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			try
			{
				// Find the user in the database and retrieve basic account information
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				if (user == null)
				{
					HttpContext.AddError(new HttpException(403, "Not authorized."));
					return RedirectToAction("Index", "Home");
				}

				// Get all projects
				ProjectRepository projectRepo = new ProjectRepository();

				// Get the list of available projects
				List<Project> availableProjects = projectRepo.FindAll().ToList();
				List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
				List<SelectListItem> listItems = new List<SelectListItem>();

				// The "None" project so user's can clear their current project
				listItems.Add(new SelectListItem()
				{
					Disabled = false,
					Text = "None",
					Value = "-1",
					Selected = false
				});

				// Create the ProjectViewModel for the consultant's current project
				Project project = projectRepo.FindByProjectId(user.ProjectId.GetValueOrDefault());

				ProjectViewModel pvm = new ProjectViewModel();

				if (project != null)
				{
					pvm = new ProjectViewModel()
					{
						ProjectId = project.ProjectId,
						Name = project.Name,
						Code = project.Code,
						Description = project.Description,
						StartDate = project.StartDate.GetValueOrDefault(),
						EndDate = project.EndDate.GetValueOrDefault(),
						LastUpdated = project.LastUpdated.GetValueOrDefault()
					};
				}

				// Populate the lists of available projects for the view
				foreach (Project proj in availableProjects)
				{
					projectViewModels.Add(new ProjectViewModel()
					{
						ProjectId = proj.ProjectId,
						Name = proj.Name,
						Code = proj.Code,
						Description = proj.Description,
						StartDate = proj.StartDate.GetValueOrDefault(),
						EndDate = proj.EndDate.GetValueOrDefault(),
						LastUpdated = proj.LastUpdated.GetValueOrDefault()

					});

					SelectListItem item = new SelectListItem()
					{
						Disabled = false,
						Selected = false,
						Text = proj.Name + " (code: " + proj.Code + ")",
						Value = proj.ProjectId.ToString()
					};

					if (project != null && proj.ProjectId == project.ProjectId)
					{
						item.Selected = true;
					}

					listItems.Add(item);
				}

				// Get all files
				FileRepository fileRepo = new FileRepository();
				IEnumerable<FileInfo> files = fileRepo.FindByUserId(user.UserId);
				List<FileIndexViewModel> fileViewModels = new List<FileIndexViewModel>();

				// Create the FileIndexViewModels
				foreach (FileInfo file in files)
				{
					FileIndexViewModel fvm = new FileIndexViewModel()
					{
						Title = file.Title,
						Description = file.Description,
						Path = file.Path
					};
					fileViewModels.Add(fvm);
				}

				// Construct view model for the consultant
				ConsultantIndexViewModel consultant = new ConsultantIndexViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Project = pvm,
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
					SkillViewModel = new ConsultantSkillViewModel(),
					AvailableProjects = projectViewModels,
					ProjectSelectListItems = listItems
				};

				return View(consultant);
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region Projects

		// GET: Consultant/CreateProject
		/// <summary>
		/// GET: Page for new project creation.
		/// </summary>
		/// <returns></returns>
		public ActionResult CreateProject()
		{
			return PartialView("_CreateProject");
		}


		// POST: Consultant/UpdateProject
		/// <summary>
		/// POST: Updates a project.
		/// </summary>
		/// <param name="model">The model containing the project information to update.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateProject(ConsultantIndexViewModel model)
		{
			try
			{
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				// Finding the project in the database
				ProjectRepository projectRepo = new ProjectRepository();
				Project project = projectRepo.FindByProjectId(model.Project.ProjectId);

				UserStore userStore = new UserStore();


				// If the project doesn't exist in the database, clear user's project
				if (project == null)
				{
					userStore.RemoveProject(user);
				}
				else
				{
					// Setting the user's current project to the one selected
					userStore.AddProject(user, project);
				}

				return RedirectToAction("Index");
			}
			catch
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
				return View("Index");
			}
		}

		#endregion

		#region FileUpload

		// GET: Consultant/UploadFile
		/// <summary>
		/// GET: Page for uploading a file.
		/// </summary>
		/// <returns></returns>
		public ActionResult UploadFile()
		{
			return PartialView("_UploadFile", new FileUploadViewModel());
		}


		// POST: Consultant/UploadFile
		/// <summary>
		/// POST: Uploads a file.
		/// </summary>
		/// <param name="model">The model containing the file to upload.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UploadFile(FileUploadViewModel model)
		{
			try
			{
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				string[] validFileTypes = new string[]
			    {
				    "application/pdf",		// .pdf
				    "application/msword",	// .doc
					"application/vnd.openxmlformats-officedocument.wordprocessingml.document",	// .docx
				    "application/rtf",		// .rtf
				    "application/x-rtf",	// .rtf
				    "text/richtext"			// .rtf
			    };

				string[] validFileExtensions = new string[]
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
					if (!validFileExtensions.Contains(Path.GetExtension(model.FileUpload.FileName)))
					{
						ModelState.AddModelError("FileUpload", "Please choose a valid file type (PDF, DOC, DOCX, RTF)");
					}
				}

				if (ModelState.IsValid)
				{
					if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
					{
						string uploadDirectoryBase = ConfigurationManager.AppSettings["FileUploadPath"];
						string uploadDirectory = Path.Combine("~", uploadDirectoryBase, user.UserId.ToString());

						if (!Directory.Exists(Server.MapPath(uploadDirectory)))
						{
							Directory.CreateDirectory(Server.MapPath(uploadDirectory));
						}

						string fileName = model.FileUpload.FileName.Replace(" ", "_");

						// This is the path used to save the file to the application directory
						string filePath = Path.Combine(Server.MapPath(uploadDirectory), fileName);
						model.FileUpload.SaveAs(filePath);

						// This is the tail of the url that will be saved in the database
						string fileUrl = Path.Combine(uploadDirectoryBase, user.UserId.ToString(), fileName);

						// Create the FileInfo instance to add to the database
						FileInfo file = new FileInfo()
						{
							Title = model.Title,
							Description = model.Description,
							UserId = user.UserId,
							Path = fileUrl
						};

						FileRepository repository = new FileRepository();
						repository.Create(file);

						return RedirectToAction("Index");
					}

					ModelState.AddModelError("FileUpload", "Please choose a file to upload");
				}

				return PartialView("_UploadFile");
			}
			catch
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region Skills

		/// <summary>
		/// POST: Adds a skill.
		/// </summary>
		/// <param name="model">The model containing the user and skill list.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddSkill(ConsultantIndexViewModel model)
		{
			try
			{
				if (model.SkillViewModel.SkillName == null)
				{
					HttpContext.AddError(new HttpException(500, "Internal server error."));
				}

				// Find the user in the database and retrieve basic account information
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				// Get current skills and add the newest addition, then update the database
				IList<string> currentSkills = UserManager.GetUserSkills(user);
				currentSkills.Add(model.SkillViewModel.SkillName);
				UserManager.SetUserSkills(user, currentSkills);

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// POST: Deletes a skill.
		/// </summary>
		/// <param name="name">The name of the skill to be deleted.</param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteSkill(string name)
		{
			try
			{
				// Find the user in the database and retrieve basic account information
				BeachUser user = UserManager.FindById(User.Identity.GetUserId());

				// Get current skills and remove specified skill, then update the database
				IList<string> currentSkills = UserManager.GetUserSkills(user);
				currentSkills.Remove(name);
				UserManager.SetUserSkills(user, currentSkills);

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
			;
		}

		#endregion


	}
}
