using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public class Project {
		public int ProjectId { get; set; }
		public int UserId { get; set; }
		public string Name { get; set; }
		public bool Completed { get; set; }
	}
}
