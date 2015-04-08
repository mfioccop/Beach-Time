using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeachTime.Data;
using Microsoft.AspNet.Identity;
using NUnit.Framework;

namespace BeachTime.Data.Tests {
	[TestFixture]
	internal class TestBeachUser : DbTestFixture {

		private UserStore Store { get; set; }
		private BeachUser User1 { get; set; }

		[SetUp]
		public override void SetUp() {
			base.SetUp();
			Store = new UserStore(dbContext.ConnectionStringName);
			User1 = GetUser1();
		}

		[Test]
		public async Task TestCreate() {
			var user = new BeachUser {
				UserName = "test-user",
				Email = "test-email",
				LockoutEnabled = true,
				EmailTwoFactorEnabled = true,
				GoogleAuthenticatorEnabled = true,
				GoogleAuthenticatorSecretKey = "super-secret-key",
				PasswordHash =
					"ABA8B6l2iak7EMHAz4ij9x5juf0b06FnEbkneLdxltj5Sp5rWI+mGcrmnT4gf1C8nw==",
				SecurityStamp = Guid.NewGuid()
			};

			await Store.CreateAsync(user);

			Assert.AreEqual(user, await Store.FindByIdAsync(user.Id));
		}

		[Test]
		public async Task TestDelete() {
			await Store.DeleteAsync(User1);

			Assert.Null(await Store.FindByIdAsync(User1.Id));
		}

		[Test]
		public async Task TestFindById() {
			var user = await Store.FindByIdAsync(User1.Id);

			Assert.NotNull(user);
			Assert.AreEqual(User1, user);
		}

		[Test]
		public async Task TestFindByName() {
			var user = await Store.FindByNameAsync(User1.UserName);

			Assert.NotNull(user);
			Assert.AreEqual(User1, user);
		}

		[TestCase("5558675309", "updated-email")]
		[TestCase("05558675309", "updated-email")]
		[TestCase("995005558675309",
			"some.really.long.email.limit.is.255.chars.some.really.long.email.limit.is.255." +
			"chars.some.really.long.email.limit.is.255.chars.some.really.long.email.limit.is.255.chars." +
			"some.really.long.email.limit.is.255.chars.some.really.long.email.limit.is.255.chars1234")]
		public async Task TestUpdate(string phoneNumber, string email) {
			User1.LockoutEndDateUtc = DateTime.UtcNow;
			User1.EmailConfirmed = true;
			User1.PhoneNumber = phoneNumber;
			User1.Email = email;
			User1.EmailTwoFactorEnabled = true;
			User1.GoogleAuthenticatorEnabled = true;
			User1.GoogleAuthenticatorSecretKey = "updated-key";

			await Store.UpdateAsync(User1);
			var user = await Store.FindByIdAsync(User1.Id);

			Assert.NotNull(user);
			Assert.AreEqual(User1, user);
		}

		[TestCase("asdf")]
		[TestCase("asdf123")]
		[TestCase("123asdf")]
		[TestCase("123asdf123")]
		[TestCase("123.456")]
		[TestCase("123-456")]
		[TestCase("123@456")]
		[TestCase("+123456")]
		[TestCase("-123456")]
		[TestCase("too-long-and-violates-constraint")]
		[TestCase("123456789123456789")]
		public async Task TestUpdateBadPhoneNumbers(string phoneNumber) {
			User1.LockoutEndDateUtc = DateTime.UtcNow;
			User1.EmailConfirmed = true;
			User1.PhoneNumber = phoneNumber;
			User1.Email = "test-email";

			var validNumbers = new[] {
				(int) SqlExceptionReason.CheckConstraintViolation,
				(int) SqlExceptionReason.StringTooLong
			};

			try {
				await Store.UpdateAsync(User1);
			} catch (SqlException ex) {
				if (validNumbers.Contains(ex.Number)) {
					Assert.Pass();
				} else {
					Assert.Fail("Update threw exception with number {0}", ex.Number);
				}
			}

			Assert.Fail("Update did not throw");
		}

		[Test]
		public async void TestFindByEmail() {
			var expectedUser = GetUser1();

			var actualUser = await Store.FindByEmailAsync(expectedUser.Email);

			Assert.NotNull(actualUser);
			Assert.AreEqual(expectedUser, actualUser);
		}

		[Test]
		public async void TestGetEmail() {
			var expectedUser = GenerateUser();

			string actualEmail = await Store.GetEmailAsync(expectedUser);

			Assert.AreEqual(expectedUser.Email, actualEmail);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async void TestGetEmailConfirmed(bool emailConfirmed) {
			var expectedUser = GenerateUser();
			expectedUser.EmailConfirmed = emailConfirmed;

			bool actualConfirmed = await Store.GetEmailConfirmedAsync(expectedUser);

			Assert.AreEqual(emailConfirmed, actualConfirmed);
		}

		[Test]
		public async void TestSetEmail() {
			var actualUser = GenerateUser();
			const string expectedEmail = "updated-test-email";

			await Store.SetEmailAsync(actualUser, expectedEmail);

			Assert.AreEqual(expectedEmail, actualUser.Email);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async void TestSetEmailConfirmed(bool emailConfirmed) {
			var actualUser = GenerateUser();

			await Store.SetEmailConfirmedAsync(actualUser, emailConfirmed);

			Assert.AreEqual(emailConfirmed, actualUser.EmailConfirmed);
		}

		[Test]
		public async Task TestUserOnBeach() {
			var user5 = await Store.FindByIdAsync("5");
			var user6 = await Store.FindByIdAsync("6");
			Assert.IsTrue(Store.OnBeach(user5));
			Assert.IsFalse(Store.OnBeach(user6));
		}

		[Test]
		public void TestGetBeachedUsers() {
			var beachedUsers = Store.GetBeachedUsers();
			var beachUsers = beachedUsers as IList<BeachUser> ?? beachedUsers.ToList();
			Assert.IsNull(beachUsers.SingleOrDefault(u => u.UserId == 6));
			Assert.IsNotNull(beachUsers.SingleOrDefault(u => u.UserId == 5));
		}

		private enum SqlExceptionReason {
			CheckConstraintViolation = 547,
			StringTooLong = 8152
		}
	}
}