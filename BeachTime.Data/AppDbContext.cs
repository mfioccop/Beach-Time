﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using Dapper;

namespace BeachTime.Data {
	public class AppDbContext : IDisposable {
		public string ConnectionString { get; private set; }
		public string ConnectionStringName { get; private set; }

		private const string ScriptPath = "Scripts";
		private const string HostedBinPath = "bin";
		private const string CreateDatabaseScript = "CreateDatabase";
		private const string CreateProceduresScript = "CreateProcedures";
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
			ConnectionStringName = connectionStringName;
			ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			DbResetEnabled = dbResetEnabled;

			ConfigureDapper();
			if (DbResetEnabled && !DbReset) {
				DbReset = true;
				ResetDbAndPopulate();
			}
		}

		public static AppDbContext Create() {
			var resetDatabase = Convert.ToBoolean(ConfigurationManager.AppSettings["ResetDatabase"]);
			return new AppDbContext(resetDatabase);
		}

		public static void ConfigureDapper() {
			if (DapperConfigured) return;

			SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
			SqlMapper.AddTypeMap(typeof(DateTime?), DbType.DateTime2);
			DapperConfigured = true;
		}

		public void ResetDatabase() {
			ExecuteScript(CreateDatabaseScript);
			ExecuteScript(CreateProceduresScript);
		}

		public void PopulateWithTestData() {
			ExecuteScript(PopulateScript);
		}

		public void ResetDbAndPopulate() {
			ResetDatabase();
			PopulateWithTestData();
		}

		// Split sql batches at 'GO': http://stackoverflow.com/a/18597052/3875516
		private static IEnumerable<string> SplitSqlStatements(string sqlScript) {
			// Split by "GO" statements
			var statements = Regex.Split(
					sqlScript,
					@"^\s*GO\s* ($ | \-\- .*$)",
					RegexOptions.Multiline |
					RegexOptions.IgnorePatternWhitespace |
					RegexOptions.IgnoreCase);

			// Remove empties, trim, and return
			return statements
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.Trim(' ', '\r', '\n'));
		}
		
		private static string ReadSqlScript(string scriptName) {
			string absolutePath = HostingEnvironment.IsHosted
				? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HostedBinPath, ScriptPath, scriptName + ".sql")
				: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScriptPath, scriptName + ".sql");

			return File.ReadAllText(absolutePath);
		}

		private void ExecuteScript(string scriptName) {
			string scriptSql = ReadSqlScript(scriptName);
			var statements = SplitSqlStatements(scriptSql);
			foreach (var statement in statements)
				using (var con = new SqlConnection(ConnectionString))
					con.Execute(statement);
		}

		public void Dispose() {
		}
	}
}
