using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public interface IProjectRepository {
		/// <summary>
		/// Creates the specified project.
		/// </summary>
		/// <param name="project">The project to create.</param>
		void Create(Project project);

		/// <summary>
		/// Deletes the specified project.
		/// </summary>
		/// <param name="project">The project to delete.</param>
		void Delete(Project project);

		/// <summary>
		/// Finds all projects.
		/// </summary>
		/// <returns>IEnumerable&lt;Project&gt; of all projects.</returns>
		IEnumerable<Project> FindAll();

		/// <summary>
		/// Finds the project by its ID.
		/// </summary>
		/// <param name="projectId">The project ID.</param>
		/// <returns>Project with the specified ID.</returns>
		Project FindByProjectId(int projectId);

		/// <summary>
		/// Finds the user by their ID.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <returns>User with the specified ID.</returns>
		Project FindByUserId(int userId);

		/// <summary>
		/// Finds the project by its name.
		/// </summary>
		/// <param name="projectName">Name of the project to find.</param>
		/// <returns>Project with the specified name.</returns>
		Project FindByName(string projectName);

		/// <summary>
		/// Updates the specified project.
		/// </summary>
		/// <param name="project">The project to update.</param>
		void Update(Project project);
	}
}
