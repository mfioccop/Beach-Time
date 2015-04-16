using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public class FileInfo {
		public int FileId { get; set; }
		public int UserId { get; set; }
		public string Path { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? LastUpdated { get; set; }
	}
}
