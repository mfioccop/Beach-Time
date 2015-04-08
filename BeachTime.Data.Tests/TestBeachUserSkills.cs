using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BeachTime.Data.Tests {
	[TestFixture]
	class TestBeachUserSkills : DbTestFixture {

		private UserStore Store { get; set; }
		private BeachUser User1 { get; set; }

		[SetUp]
		public override void SetUp() {
			base.SetUp();
			Store = new UserStore(dbContext.ConnectionStringName);
			User1 = GetUser1();
		}

		[Test]
		public async Task TestSkillGet() {
			var skills = await Store.GetSkillsAsync(User1);

			Assert.AreEqual(3, skills.Count);
		}

		[Test]
		public async Task TestSkillsSet() {
			IList<string> expected = new[] { "skill a", "skill b" };
			await Store.SetSkillsAsync(User1, expected);
			var actual = (await Store.GetSkillsAsync(User1)).OrderBy(s => s);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public async Task TestSkillClear() {
			await Store.ClearSkillsAsync(User1);
			var skills = await Store.GetSkillsAsync(User1);
			Assert.AreEqual(0, skills.Count);
		}
	}
}
