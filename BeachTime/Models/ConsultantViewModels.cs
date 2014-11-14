﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class ConsultantIndexViewModel
	{
		[DisplayName("First Name")]
		public string FirstName { get; set; }
		
		[DisplayName("Last Name")]
		public string LastName { get; set; }
		
		[DisplayName("Current Projects")]
		public ICollection<ProjectViewModel> Projects { get; set; }
		
		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }
	}

	public class ConsultantEditViewModel
	{

		[DisplayName("Current Projects")]
		public ICollection<ProjectViewModel> Projects { get; set; }

		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }

	}


}