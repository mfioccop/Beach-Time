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

	public class FileRepository : IFileRepository {

		private readonly string connectionString;

		public FileRepository() {
			connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		}

		public FileRepository(string connectionStringName) {
			if (string.IsNullOrWhiteSpace(connectionStringName))
				throw new ArgumentNullException("connectionStringName");

			connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		private IDbConnection GetConnection() {
			IDbConnection con = new SqlConnection(connectionString);
			con.Open();
			return con;
		}

		public void Create(FileInfo fileInfo) {
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");

			const string CreateQuery =
				@"insert into FileInfo (
	UserId,
	Path,
	Title,
	Description
) output Inserted.FileId values (
	@userId,
	@path,
	@title,
	@description)";

			using (var con = GetConnection())
				fileInfo.FileId = con.Query<int>(CreateQuery, fileInfo).Single();
		}

		public void Delete(FileInfo fileInfo) {
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");

			using (var con = GetConnection())
				con.Execute("delete from FileInfo where FileId = @fileId", new { fileInfo.FileId });
		}

		public FileInfo FindByFileId(int fileId) {
			using (var con = GetConnection())
				return con.Query<FileInfo>("select * from FileInfo where FileId = @fileId", new { fileId }).SingleOrDefault();
		}

		public IEnumerable<FileInfo> FindByUserId(int userId) {
			using (var con = GetConnection())
				return con.Query<FileInfo>("select * from FileInfo where UserId = @userId", new { userId });
		}

		public void Update(FileInfo fileInfo) {
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");

			const string UpdateQuery =
				@"update FileInfo set
	UserId = @userId,	
	Path = @path,
	Title = @title,
	Description = @description
where FileId = @fileId";

			using (var con = GetConnection())
				con.Execute(UpdateQuery, fileInfo);
		}
	}
}
