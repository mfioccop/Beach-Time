using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BeachTime.Data.Tests {
	[TestFixture]
	internal class TestProjectRepository : DbTestFixture {
		private IProjectRepository projectRepository { get; set; }

		[SetUp]
		public override void SetUp() {
			base.SetUp();
			projectRepository = new ProjectRepository(dbContext.ConnectionStringName);
		}

		[Test]
		public void TestCreateProject() {
			var expected = new Project {
				Completed = true,
				Name = "a project",
				UserId = 1
			};
			projectRepository.Create(expected);
			var actual = projectRepository.FindByProjectId(expected.ProjectId);
			Assert.AreNotEqual(0, expected.ProjectId);
			AssertEx.PropertyValuesAreEquals(expected, actual);
		}

		[Test]
		public void TestDeleteProject() {
			var project = projectRepository.FindByProjectId(1);
			Assert.NotNull(project);
			projectRepository.Delete(project);
			Assert.Null(projectRepository.FindByProjectId(1));
		}

		[Test]
		public void TestFindAll() {
			var projects = projectRepository.FindAll();
			Assert.Greater(projects.Count(), 1);
		}

		[Test]
		public void TestUpdate() {
			var expected = projectRepository.FindByProjectId(1);
			expected.Name = "name1";
			expected.Completed = !expected.Completed;
			projectRepository.Update(expected);
			var actual = projectRepository.FindByProjectId(1);
			AssertEx.PropertyValuesAreEquals(expected, actual);
		}
	}
}
