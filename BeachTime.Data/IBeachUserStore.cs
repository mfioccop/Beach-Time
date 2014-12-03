using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace BeachTime.Data {
	public interface IBeachUserStore : IUserStore<BeachUser> {
		Task<IEnumerable<BeachUser>> FindAll();
	}
}
