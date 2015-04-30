using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
	public interface IBeachUserStore : IUserStore<BeachUser> {
		/// <summary>
		/// Retrieves all users.
		/// </summary>
		/// <returns>Task&lt;IEnumerable&lt;BeachUser&gt;&gt;.</returns>
		Task<IEnumerable<BeachUser>> FindAll();
	}
}
