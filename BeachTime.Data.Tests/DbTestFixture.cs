using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BeachTime.Data.Tests {

	[TestFixture]
	public class DbTestFixture {
		protected AppDbContext dbContext;

		[SetUp]
		public virtual void SetUp() {
			dbContext = new AppDbContext("TestConnection");
			dbContext.ResetDbAndPopulate();
		}
	}
}
