using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
  public interface IUserSkillStore<in TUser, in TKey> : IDisposable where TUser : class, IUser<TKey> {
	  /// <summary>
	  /// Gets a users skills.
	  /// </summary>
	  /// <param name="user">The user.</param>
	  /// <returns>Asynchronous Task&lt;IList&lt;System.String&gt;&gt; of a users skills.</returns>
	  Task<IList<string>> GetSkillsAsync(TUser user);
	  /// <summary>
	  /// Sets the users skills.
	  /// </summary>
	  /// <param name="user">The user.</param>
	  /// <param name="skills">The skills.</param>
	  /// <returns>Asynchronous task setting the users skills.</returns>
	  Task SetSkillsAsync(TUser user, IList<string> skills);
	  /// <summary>
	  /// Clears users skills.
	  /// </summary>
	  /// <param name="user">The user.</param>
	  /// <returns>Asynchronous task clearing the users skills.</returns>
	  Task ClearSkillsAsync(TUser user);
  }
}
