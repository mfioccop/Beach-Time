using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace BeachTime.Data {
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;

	public class ProjectRepository : IProjectRepository {

		private readonly string connectionString;

		public ProjectRepository() {
			connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		}

		public ProjectRepository(string connectionStringName) {
			if (string.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException("connectionStringName");

			connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		private IDbConnection GetConnection() {
			IDbConnection con = new SqlConnection(connectionString);
			con.Open();
			return con;
		}

		public void Create(Project project) {
			if (project == null)
				throw new ArgumentNullException("project");

			var p = new DynamicParameters();
			p.Add("@name", project.Name);
			p.Add("@description", project.Description);
			p.Add("@code", project.Code);
			p.Add("@startDate", project.StartDate);
			p.Add("@endDate", project.EndDate);

			using (var con = GetConnection())
				project.ProjectId = con.Query<int>("spProjectCreate", p,
					commandType: CommandType.StoredProcedure).Single();
		}

		public void Delete(Project project) {
			if (project == null)
				throw new ArgumentNullException("project");

				using (var con = GetConnection())
					con.Execute("spProjectDelete", new { project.ProjectId },
						commandType: CommandType.StoredProcedure);
		}

		public IEnumerable<Project> FindAll() {
			using (var con = GetConnection())
				return con.Query<Project>("spProjectFindAll", commandType: CommandType.StoredProcedure);
		}

		public Project FindByProjectId(int projectId) {
			using (var con = GetConnection())
				return con.Query<Project>("spProjectFindByProjectId", new { projectId },
					commandType: CommandType.StoredProcedure).SingleOrDefault();
		}

		public Project FindByUserId(int userId) {
			using (var con = GetConnection())
				return con.Query<Project>("spProjectFindByUserId", new { userId },
					commandType: CommandType.StoredProcedure).SingleOrDefault();
		}

		public Project FindByName(string projectName) {
			if (string.IsNullOrEmpty(projectName))
				throw new ArgumentNullException("projectName");

			using (var con = GetConnection())
				return con.Query<Project>("spProjectFindByName", new { projectName },
					commandType: CommandType.StoredProcedure).SingleOrDefault();
		}

		public void Update(Project project) {
			if (project == null)
				throw new ArgumentNullException("project");

			using (var con = GetConnection()) {
				var lastUpdated = con.Query<DateTime?>("spProjectUpdate", project,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
				// if update fails lastUpdate is null
				if (lastUpdated.HasValue)
					project.LastUpdated = lastUpdated.Value;

			}
		}
	}
}
