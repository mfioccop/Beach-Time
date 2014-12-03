using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public class RoleChangeRequest {
		public int RequestId { get; set; }
		public int UserId { get; set; }
		public string RoleName { get; set; }
		public DateTime RequestDate { get; set; }
	}
}
