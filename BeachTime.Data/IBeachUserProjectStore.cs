using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	 public interface IBeachUserProjectStore {
		 void AddProject(BeachUser user, Project project);

		 void RemoveProject(BeachUser user);
	 }
}
