using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class ProjectViewModel
	{
		public string ProjectName { get; set; }
		public bool IsCompleted { get; set; }

		public int ProjectId { get; set; }
	}
}