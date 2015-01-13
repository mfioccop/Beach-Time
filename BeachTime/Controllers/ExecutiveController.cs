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
        public ActionResult Index()
        {
            // Find the user in the database and retrieve basic account information
            var user = UserManager.FindById(User.Identity.GetUserId());

            // Get all consultants on the beach
            UserStore beachedUserStore = new UserStore();
            var beachUsers = beachedUserStore.GetBeachedUsers();
            int numBeach = beachUsers.Count();

            // Get all consultants on projects
            var occupiedUsers = beachedUserStore.FindAll().Result;
            int numOccupied = occupiedUsers.Count() - numBeach;
            
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
        #endregion

        #region Beach
        // GET: Executive/Beach
        public ActionResult Beach()
        {
            // Get all consultants on the beach as ViewModels
            UserStore beachedUserStore = new UserStore();
            var beachUsers = beachedUserStore.GetBeachedUsers().ToList();
            var beachViewModelList = new List<ConsultantIndexViewModel>();
            var projectRepo = new ProjectRepository();
            var fileRepo = new FileRepository();

            foreach (var user in beachUsers)
            {
                // Get all projects
                var projects = projectRepo.FindByUserId(user.UserId);
                var projectViewModels = new List<ProjectViewModel>();

                // Create the ProjectViewModels
                foreach (var project in projects)
                {
                    var pvm = new ProjectViewModel()
                    {
                        ProjectName = project.Name,
                        IsCompleted = project.Completed
                    };
                    projectViewModels.Add(pvm);
                }

                // Get all files
                
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
                    Id = user.UserId,
                    Projects = projectViewModels,
                    SkillList = UserManager.GetUserSkills(user).ToList(),
                    Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
                    FileList = fileViewModels
                };
                beachViewModelList.Add(consultant);
            }
            
            // Construct view model for the beach
            var executive = new ExecutiveUserListViewModel()
            {
                BeachConsultantViewModels = beachViewModelList
            };

            return View(executive);
        }
        #endregion

        #region Occupied

		// GET: Executive/Occupied
        public ActionResult Occupied()
        {
            // Get all consultants on working on projects as ViewModels
            UserStore beachedUserStore = new UserStore();
            var allUsers = beachedUserStore.FindAll().Result;
            var beachUsers = beachedUserStore.GetBeachedUsers();
            var occupiedUsers = allUsers.Except(beachUsers); // Subtract beach users from all users
            var occupiedViewModelList = new List<ConsultantIndexViewModel>();
            var projectRepo = new ProjectRepository();
            var fileRepo = new FileRepository();

            foreach (var user in occupiedUsers)
            {
                // Get all projects
                var projects = projectRepo.FindByUserId(user.UserId);
                var projectViewModels = new List<ProjectViewModel>();

                // Create the ProjectViewModels
                foreach (var project in projects)
                {
                    var pvm = new ProjectViewModel()
                    {
                        ProjectName = project.Name,
                        IsCompleted = project.Completed
                    };
                    projectViewModels.Add(pvm);
                }

                // Get all files
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
                    Id = user.UserId,
                    Projects = projectViewModels,
                    SkillList = UserManager.GetUserSkills(user).ToList(),
                    Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
                    FileList = fileViewModels
                };
                occupiedViewModelList.Add(consultant);
            }

            // Construct view model for occupied users
            var executive = new ExecutiveUserListViewModel()
            {
                BeachConsultantViewModels = occupiedViewModelList
            };

            return View(executive);
        }
        #endregion

        #region Details
        // GET: Executive/Details/5
        public ActionResult Details(int id)
        {
            var user = UserManager.FindById(id.ToString());

			// URL id doesn't match a user in the database, 404
			if (user == null)
			{
				return RedirectToAction("PageNotFound", "Home");
			}

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
                    IsCompleted = project.Completed
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
                Id = user.UserId,
                Projects = projectViewModels,
                SkillList = UserManager.GetUserSkills(user).ToList(),
                Status = UserManager.UserOnBeach(user) ? "On the beach" : "On a project",
                FileList = fileViewModels
            };
	      
            return View(consultant);
        }
    }
        #endregion
}
