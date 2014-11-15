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

			const string CreateQuery =
				@"insert into Projects (
	UserId,
	Name,
	Completed
) output Inserted.ProjectId values (
	@userId,
	@name,
	@completed)";

			using (var con = GetConnection())
				project.ProjectId = con.Query<int>(CreateQuery, project).Single();
		}

		public void Delete(Project project) {
			if (project == null)
				throw new ArgumentNullException("project");

				using (var con = GetConnection())
					con.Execute("delete from Projects where ProjectId = @projectId", new { project.ProjectId });
		}

		public IEnumerable<Project> FindAll() {
			using (var con = GetConnection())
				return con.Query<Project>("select * from Projects");
		}

		public Project FindById(int projectId) {
			using (var con = GetConnection())
					return con.Query<Project>("select * from Projects where ProjectId = @projectId", new { projectId }).SingleOrDefault();
		}

		public Project FindByName(string projectName) {
			if (string.IsNullOrEmpty(projectName))
				throw new ArgumentNullException("projectName");

			using (var con = GetConnection())
				return con.Query<Project>("select * from Projects where Name = @projectName", new { projectName }).SingleOrDefault();
		}

		public void Update(Project project) {
			if (project == null)
				throw new ArgumentNullException("project");

			const string UpdateQuery =
				@"update Projects set
	UserId = @userId,	
	Name = @name,
	Completed = @completed
where ProjectId = @projectId";

			using (var con = GetConnection())
				con.Execute(UpdateQuery, project);
		}
	}
}
