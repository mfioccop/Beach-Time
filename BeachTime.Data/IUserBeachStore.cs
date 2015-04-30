using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	using Microsoft.AspNet.Identity;

	public interface IUserBeachStore {
		/// <summary>
		/// Check if a user is on the beach.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns><c>true</c> if user is beached, <c>false</c> otherwise.</returns>
		bool OnBeach(BeachUser user);

		/// <summary>
		/// Gets all beached users.
		/// </summary>
		/// <returns>IEnumerable&lt;BeachUser&gt; of beached users.</returns>
		IEnumerable<BeachUser> GetBeachedUsers();
	}
}
