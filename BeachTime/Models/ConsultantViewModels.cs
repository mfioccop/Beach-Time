using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class ConsultantIndexViewModel
	{
		public string FirstName = "John";
		public string LastName = "Doe";

		public string CurrentProject = "Current proj";
		
		public List<string> SkillList = new List<string>
		{
			"C#",
			"Java",
			"Git"
		};
	}


}