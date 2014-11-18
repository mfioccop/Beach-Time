using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	public class FileUploadViewModel
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[DataType(DataType.Upload)]
		public HttpPostedFileBase FileUpload { get; set; }
	}
}