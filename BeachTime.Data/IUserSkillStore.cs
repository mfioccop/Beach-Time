using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
  public interface IUserSkillStore<in TUser, in TKey> : IDisposable where TUser : class, IUser<TKey> {
	  Task<IList<string>> GetSkillsAsync(TUser user);
	  Task SetSkillsAsync(TUser user, IList<string> skills);
	}
}
