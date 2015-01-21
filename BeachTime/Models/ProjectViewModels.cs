using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{

	public class ProjectCreateViewModel
	{
		[Required]
		public string ProjectName { get; set; }
		[Required]
		public bool IsCompleted { get; set; }

	}
	public class ProjectViewModel
	{
		[Required]
		public string ProjectName { get; set; }
		[Required]
		public bool IsCompleted { get; set; }

		[Required]
		public int ProjectId { get; set; }
	}
}