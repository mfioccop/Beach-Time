using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	 public interface IBeachUserProjectStore {
		 /// <summary>
		 /// Add a user to a project.
		 /// </summary>
		 /// <param name="user">The user.</param>
		 /// <param name="project">The project.</param>
		 void AddProject(BeachUser user, Project project);

		 /// <summary>
		 /// Removes a user from their project.
		 /// </summary>
		 /// <param name="user">The user.</param>
		 void RemoveProject(BeachUser user);
	 }
}
