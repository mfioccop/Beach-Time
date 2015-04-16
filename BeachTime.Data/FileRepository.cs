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

			var p = new DynamicParameters();
			p.Add("@userId", fileInfo.UserId);
			p.Add("@path", fileInfo.Path);
			p.Add("@title", fileInfo.Title);
			p.Add(@"description", fileInfo.Description);

			using (var con = GetConnection())
				fileInfo.FileId = con.Query<int>("spFileCreate", p,
					commandType: CommandType.StoredProcedure).Single();
		}

		public void Delete(FileInfo fileInfo) {
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");

			using (var con = GetConnection())
				con.Execute("spFileDelete", new { fileInfo.FileId },
					commandType: CommandType.StoredProcedure);
		}

		public FileInfo FindByFileId(int fileId) {
			using (var con = GetConnection())
				return con.Query<FileInfo>("spFileFindByFileId", new { fileId },
					commandType: CommandType.StoredProcedure).SingleOrDefault();
		}

		public IEnumerable<FileInfo> FindByUserId(int userId) {
			using (var con = GetConnection())
				return con.Query<FileInfo>("spFileFindByUserId", new { userId },
					commandType: CommandType.StoredProcedure);
		}

		public void Update(FileInfo fileInfo) {
			if (fileInfo == null)
				throw new ArgumentNullException("fileInfo");

			using (var con = GetConnection()) {
				var lastUpdated = con.Query<DateTime?>("spFileUpdate", fileInfo,
					commandType: CommandType.StoredProcedure).SingleOrDefault();
				// if update fails lastUpdated is null
				if (lastUpdated.HasValue)
					fileInfo.LastUpdated = lastUpdated.Value;
			}
		}
	}
}
