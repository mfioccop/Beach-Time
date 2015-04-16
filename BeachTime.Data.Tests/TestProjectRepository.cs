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
				Name = "a project",
				Description = "a description",
				Code = "a code",
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(2)
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
			expected.Description = "desc1";
			expected.Code = "code1";
			expected.StartDate = DateTime.Now.AddDays(1);
			expected.EndDate = DateTime.Now.AddDays(2);
			projectRepository.Update(expected);
			var actual = projectRepository.FindByProjectId(1);
			AssertEx.PropertyValuesAreEquals(expected, actual);
		}
	}
}
