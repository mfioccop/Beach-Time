using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
	public interface IBeachUserRoleStore : IUserRoleStore<BeachUser> {
		/// <summary>
		/// Add a role change request to the database
		/// </summary>
		/// <param name="request">The request. UserId and RoleName must be specified</param>
		void RequestRoleChange(RoleChangeRequest request);

		/// <summary>
		/// Find all role change requests by a user
		/// </summary>
		/// <param name="userId"/>
		/// <returns/>
		IEnumerable<RoleChangeRequest> GetRoleChangeRequests(string userId);

		/// <summary>
		/// Retrieve all roles from the database
		/// </summary>
		/// <returns/>
		IEnumerable<string> GetAllRoles();

		/// <summary>
		/// Retrieve a role change request by request ID
		/// </summary>
		/// <param name="requestId"/>
		/// <returns/>
		IEnumerable<RoleChangeRequest> GetRoleChangeRequestById(string requestId);

		/// <summary>
		/// Retrieve all role change requests
		/// </summary>
		/// <returns/>
		IEnumerable<RoleChangeRequest> GetAllRoleChangeRequests();

		/// <summary>
		/// Removes a role request by request ID
		/// </summary>
		/// <param name="requestId"/>
		/// <returns/>
		Task RemoveRoleRequestAsync(string requestId);

	}
}
