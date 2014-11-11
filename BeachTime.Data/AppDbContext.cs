using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Dapper;

namespace BeachTime.Data {
	public class AppDbContext : IDisposable {
		public string ConnectionString { get; private set; }

		private const string ScriptPath = "Scripts";
		private const string HostedBinPath = "bin";
		private const string CreateScript = "CreateDatabase";
		private const string PopulateScript = "PopulateDatabase";

		private static bool DapperConfigured { get; set; }
		private static bool DbReset { get; set; }
		private bool DbResetEnabled { get; set; }

		public AppDbContext()
			: this("DefaultConnection") {
		}

		public AppDbContext(bool dbResetEnabled)
			: this("DefaultConnection", dbResetEnabled) {
		}

		public AppDbContext(string connectionStringName, bool dbResetEnabled = false) {
			ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			DbResetEnabled = dbResetEnabled;

			ConfigureDapper();
			if (DbResetEnabled && !DbReset) {
				DbReset = true;
				ResetDbAndPopulate();
			}
		}

		public static AppDbContext Create() {
			return new AppDbContext(true);
		}

		public static void ConfigureDapper() {
			if (DapperConfigured) return;

			SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
			SqlMapper.AddTypeMap(typeof(DateTime?), DbType.DateTime2);
			DapperConfigured = true;
		}

		public void ResetDatabase() {
			ExecuteScript(CreateScript);
		}

		public void PopulateWithTestData() {
			ExecuteScript(PopulateScript);
		}

		public void ResetDbAndPopulate() {
			ResetDatabase();
			PopulateWithTestData();
		}

		private static string ReadSqlScript(string scriptName) {
			string absolutePath = HostingEnvironment.IsHosted
				? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HostedBinPath, ScriptPath, scriptName + ".sql")
				: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScriptPath, scriptName + ".sql");

			return File.ReadAllText(absolutePath);
		}

		private void ExecuteScript(string scriptName) {
			string scriptSql = ReadSqlScript(scriptName);

			using (var con = new SqlConnection(ConnectionString))
				con.Execute(scriptSql);
		}

		public void Dispose() {
		}
	}
}
