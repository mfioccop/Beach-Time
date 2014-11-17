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
		
		[DisplayName("Projects")]
		public IList<ProjectViewModel> Projects { get; set; }
		
		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }
	}

	public class ConsultantEditViewModel
	{

		[DisplayName("Projects")]
		public IList<ProjectViewModel> Projects { get; set; }

		public string SkillsString { get; set; }

		[DisplayName("Skills")]
		public List<string> SkillList { get; set; }

		[DisplayName("Status")]
		public string Status { get; set; }

	}


}