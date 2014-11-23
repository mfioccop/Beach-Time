using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data.Tests {
	using System.Globalization;

	using NUnit.Framework;

	class TestBeachUserRoleStore : DbTestFixture {
		private IBeachUserRoleStore roleStore;

		private RoleChangeRequest request;

		[SetUp]
		public override void SetUp() {
			base.SetUp();
			roleStore = new UserStore(dbContext.ConnectionStringName);
			request = new RoleChangeRequest { UserId = 1, RoleName = "Executive" };
		}

		[Test]
		public void TestRoleRequests() {
			roleStore.RequestRoleChange(request);
			var requests = roleStore.GetRoleChangeRequests(request.UserId.ToString(CultureInfo.InvariantCulture)).ToList();
			Assert.AreEqual(1, requests.Count);
			AssertEx.PropertyValuesAreEquals(request, requests[0]);
		}
	}
}
