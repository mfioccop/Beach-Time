using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BeachTime.Data;
using BeachTime.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease;
using WebGrease.Css.Extensions;

namespace BeachTime.Controllers
{
	[AuthorizeBeachUser(Roles = "Executive")]
	public class ExecutiveController : Controller
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
		// GET: Executive
		/// <summary>
		/// GET: Account Executive index page (dashboard)
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

				// Get all consultants on the beach
				UserStore beachedUserStore = new UserStore();
				List<BeachUser> beachUsers = beachedUserStore.GetBeachedUsers().ToList();

				List<BeachUser> beachList = new List<BeachUser>(beachUsers);

				foreach (BeachUser beached in beachUsers)
				{
					if (!beachedUserStore.IsInRoleAsync(beached, "Consultant").Result)
					{
						beachList.Remove(beached);
					}
				}

				int numBeach = beachList.Count();

				// Get all consultants on projects
				List<BeachUser> occupiedUsers = beachedUserStore.FindAll().Result.ToList();
				List<BeachUser> occupiedList = new List<BeachUser>(occupiedUsers);

				foreach (BeachUser occupied in occupiedUsers)
				{
					if (!beachedUserStore.IsInRoleAsync(occupied, "Consultant").Result)
					{
						occupiedList.Remove(occupied);
					}
				}

				int numOccupied = occupiedList.Count() - numBeach;

				// Get all skills on the beach
				List<string> beachSkillsList = new List<string>();
				foreach (BeachUser consultant in beachList)
				{
					BeachUser consultantManager = UserManager.FindById(consultant.Id);
					beachSkillsList.AddRange(UserManager.GetUserSkills(consultantManager).ToList());
				}

				// Construct view model for the executive
				ExecutiveIndexViewModel executive = new ExecutiveIndexViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					OccupiedConsultantsCount = numOccupied,
					BeachConsultantsCount = numBeach,
					SkillList = beachSkillsList,
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
						Id = user.UserId,
						Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project"
					}
				};

				return View(executive);
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Beach
		// GET: Executive/Beach
		/// <summary>
		/// GET: Page for viewing all consultants not currently working on any projects.
		/// </summary>
		/// <returns></returns>
		public ActionResult Beach()
		{
			try
			{
				// Get all consultants on the beach as ViewModels
				UserStore beachedUserStore = new UserStore();
				List<BeachUser> beachUsers = beachedUserStore.GetBeachedUsers().ToList();
				List<ConsultantIndexViewModel> beachViewModelList = new List<ConsultantIndexViewModel>();
				ProjectRepository projectRepo = new ProjectRepository();
				FileRepository fileRepo = new FileRepository();

				foreach (BeachUser user in beachUsers)
				{
					// If the user is not a consultant, don't bother adding them to the beach
					if (!beachedUserStore.IsInRoleAsync(user, "Consultant").Result)
					{
						continue;
					}
					// Get this user's current project
					Project project = projectRepo.FindByUserId(user.UserId);

					// Create the ProjectViewModel for the project
					ProjectViewModel pvm;

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
					else // dummy view model 
					{
						pvm = new ProjectViewModel()
						{
							ProjectId = -1,
							Name = "No project",
							Code = "NO_PROJ",
							Description = "No project",
							StartDate = DateTime.Today,
							EndDate = DateTime.Today,
							LastUpdated = DateTime.Today,
						};
					}

					// Get all files
					IEnumerable<FileInfo> files = fileRepo.FindByUserId(user.UserId);
					List<FileIndexViewModel> fileViewModels = new List<FileIndexViewModel>();

					// Create the FileIndexViewModels
					foreach (FileInfo file in files)
					{
						FileIndexViewModel fvm = new FileIndexViewModel()
						{
							Title = file.Title,
							Description = file.Description,
							Path = Server.MapPath(file.Path)
						};
						fileViewModels.Add(fvm);
					}

					// Construct view model for the consultant
					ConsultantIndexViewModel consultant = new ConsultantIndexViewModel()
					{
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
						Id = user.UserId,
						Project = pvm,
						SkillList = UserManager.GetUserSkills(user).ToList(),
						Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
						FileList = fileViewModels
					};
					beachViewModelList.Add(consultant);
				}

				BeachUser exec = UserManager.FindById(User.Identity.GetUserId());

				// Construct view model for the beach
				ExecutiveUserListViewModel executive = new ExecutiveUserListViewModel()
				{
					BeachConsultantViewModels = beachViewModelList,
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = exec.FirstName,
						LastName = exec.LastName,
						Email = exec.Email,
						Id = exec.UserId,
						Status = UserManager.UserOnBeach(exec) ? "On the beach" : "On a project"
					}
				};

				return View(executive);
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Occupied

		// GET: Executive/Occupied
		/// <summary>
		/// GET: Page for viewing all consultants working on a project.
		/// </summary>
		/// <returns></returns>
		public ActionResult Occupied()
		{
			try
			{
				// Get all consultants on working on projects as ViewModels
				UserStore beachedUserStore = new UserStore();
				IEnumerable<BeachUser> allUsers = beachedUserStore.FindAll().Result;
				IEnumerable<BeachUser> beachUsers = beachedUserStore.GetBeachedUsers();
				IEnumerable<BeachUser> occupiedUsers = allUsers.Except(beachUsers); // Subtract beach users from all users
				List<ConsultantIndexViewModel> occupiedViewModelList = new List<ConsultantIndexViewModel>();
				ProjectRepository projectRepo = new ProjectRepository();
				FileRepository fileRepo = new FileRepository();

				foreach (BeachUser user in occupiedUsers)
				{
					// If the user is not a consultant, don't bother adding them to the list of occupied
					if (!beachedUserStore.IsInRoleAsync(user, "Consultant").Result)
					{
						continue;
					}
					// Get this user's current project
					Project project = projectRepo.FindByUserId(user.UserId);


					// Create the ProjectViewModel for the project
					ProjectViewModel pvm;

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
					else // dummy view model 
					{
						pvm = new ProjectViewModel()
						{
							ProjectId = -1,
							Name = "No project",
							Code = "NO_PROJ",
							Description = "No project",
							StartDate = DateTime.Today,
							EndDate = DateTime.Today,
							LastUpdated = DateTime.Today,
						};
					}

					// Get all files
					IEnumerable<FileInfo> files = fileRepo.FindByUserId(user.UserId);
					List<FileIndexViewModel> fileViewModels = new List<FileIndexViewModel>();

					// Create the FileIndexViewModels
					foreach (FileInfo file in files)
					{
						FileIndexViewModel fvm = new FileIndexViewModel()
						{
							Title = file.Title,
							Description = file.Description,
							Path = Server.MapPath(file.Path)
						};
						fileViewModels.Add(fvm);
					}

					// Construct view model for the consultant
					ConsultantIndexViewModel consultant = new ConsultantIndexViewModel()
					{
						FirstName = user.FirstName,
						LastName = user.LastName,
						Id = user.UserId,
						Project = pvm,
						SkillList = UserManager.GetUserSkills(user).ToList(),
						Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
						FileList = fileViewModels
					};
					occupiedViewModelList.Add(consultant);
				}

				BeachUser exec = UserManager.FindById(User.Identity.GetUserId());

				if (exec == null)
				{
					HttpContext.AddError(new HttpException(403, "Not authorized."));
					return RedirectToAction("Index", "Home");
				}

				// Construct view model for occupied users
				ExecutiveUserListViewModel executive = new ExecutiveUserListViewModel()
				{
					BeachConsultantViewModels = occupiedViewModelList,
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = exec.FirstName,
						LastName = exec.LastName,
						Email = exec.Email,
						Id = exec.UserId,
						Status = UserManager.UserOnBeach(exec) ? "On the beach" : "On a project"
					}
				};

				return View(executive);
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Details
		// GET: Executive/Details/5
		/// <summary>
		/// GET: Page for viewing details of a consultant
		/// </summary>
		/// <param name="id">The userId of the consultant.</param>
		/// <returns></returns>
		public ActionResult Details(int id)
		{
			try
			{
				HttpContext.ClearError();
				BeachUser user = UserManager.FindById(id.ToString());

				// URL id doesn't match a user in the database, 404
				if (user == null)
				{
					HttpContext.AddError(new HttpException(404, "No consultant with that ID exists."));
				}

				// Get all projects
				ProjectRepository projectRepo = new ProjectRepository();
				// Get this user's current project
				Project project = projectRepo.FindByUserId(user.UserId);

				// Create the ProjectViewModel for the project
				ProjectViewModel pvm;

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
				else // dummy view model 
				{
					pvm = new ProjectViewModel()
					{
						ProjectId = -1,
						Name = "No project",
						Code = "NO_PROJ",
						Description = "No project",
						StartDate = DateTime.Today,
						EndDate = DateTime.Today,
						LastUpdated = DateTime.Today,
					};
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
						Path = Server.MapPath(file.Path)
					};
					fileViewModels.Add(fvm);
				}

				BeachUser exec = UserManager.FindById(User.Identity.GetUserId());

				// Construct view model for the consultant
				ConsultantIndexViewModel consultant = new ConsultantIndexViewModel()
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Id = user.UserId,
					Project = pvm,
					SkillList = UserManager.GetUserSkills(user).ToList(),
					Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
					FileList = fileViewModels,
					Navbar = new HomeNavbarViewModel()
					{
						FirstName = exec.FirstName,
						LastName = exec.LastName,
						Email = exec.Email,
						Id = exec.UserId,
						Status = UserManager.UserOnBeach(exec) ? "On the beach" : "On a project"
					}
				};

				return View(consultant);
			}
			catch (InvalidOperationException ioe)
			{
				HttpContext.AddError(new HttpException(404, "No consultant with that ID exists."));
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index", "Home");
		}
		#endregion


		public ActionResult CreateNewProject()
		{
			return View("_CreateNewProject");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateNewProject(ProjectCreateViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					ProjectRepository projectRepo = new ProjectRepository();

					Project project = new Project()
					{
						Code = model.Code,
						Description = model.Description,
						EndDate = model.EndDate,
						Name = model.Name,
						StartDate = model.StartDate
					};


					projectRepo.Create(project);

					return RedirectToAction("Index");

				}
				else
				{
					HttpContext.AddError(new HttpException(500, "Internal server error."));
					return RedirectToAction("Index");
				}
			}
			catch (Exception e)
			{
				HttpContext.AddError(new HttpException(500, "Internal server error."));
			}
			return RedirectToAction("Index");
		}

	}

}
