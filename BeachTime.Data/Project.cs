using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public class Project {
		public int ProjectId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? LastUpdated { get; set; }
	}
}
