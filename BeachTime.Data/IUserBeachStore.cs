using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	using Microsoft.AspNet.Identity;

	public interface IUserBeachStore {
		bool OnBeach(BeachUser user);

		IEnumerable<BeachUser> GetBeachedUsers();
	}
}
