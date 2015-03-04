using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeachTime.Models
{
	/// <summary>
	/// ViewModel for a file upload.
	/// </summary>
	public class FileUploadViewModel
	{
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[Required]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		[Required]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the file upload.
		/// </summary>
		/// <value>
		/// The file upload.
		/// </value>
		[Required]
		[DataType(DataType.Upload)]
		public HttpPostedFileBase FileUpload { get; set; }
	}

	/// <summary>
	/// ViewModel for the information about a file.
	/// </summary>
	public class FileIndexViewModel
	{
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[Required]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		[Required]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		[Required]
		public string Path { get; set; }
	}
}