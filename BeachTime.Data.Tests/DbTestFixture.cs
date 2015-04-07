using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BeachTime.Data.Tests {

	public class DbTestFixture : TestBase {
		protected AppDbContext dbContext;

		[SetUp]
		public virtual void SetUp() {
			dbContext = new AppDbContext("TestConnection");
			dbContext.ResetDbAndPopulate();
		}
	}

	public class TestBase {
		protected BeachUser GenerateUser() {
			return new BeachUser {
				UserId = 42,
				UserName = "test-name",
				Email = "test-email",
				PhoneNumber = "8675309",
				// 8/17/2014 03:19:03.0456585
				LockoutEndDateUtc = new DateTime(635438421156814829),
				PasswordHash = "test-hash",
				SecurityStamp = Guid.Parse("2b849a46-e49f-4129-9944-009d6e2be6c5")
			};
		}

		protected BeachUser GetUser1() {
			return new BeachUser {
				UserId = 1,
				UserName = "billy.bob@gmail.com",
				FirstName = "Billy",
				LastName = "Bob",
				Email = "billy.bob@gmail.com",
				LockoutEnabled = true,
				PasswordHash =
					"ABA8B6l2iak7EMHAz4ij9x5juf0b06FnEbkneLdxltj5Sp5rWI+mGcrmnT4gf1C8nw==",
				SecurityStamp = Guid.Parse("D97CBD09-B55C-49F4-890E-9C08488CF400")
			};
		}
	}
}
