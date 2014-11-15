using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public interface IProjectRepository {
		void Create(Project project);

		void Delete(Project project);

		IEnumerable<Project> FindAll();

		Project FindById(int projectId);

		Project FindByName(string projectName);

		void Update(Project project);
	}
}
