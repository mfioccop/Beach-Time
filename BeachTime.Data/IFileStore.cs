using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachTime.Data {
	public interface IFileStore {

		/// <summary>
		/// Insert a new <see cref="FileInfo"/>
		/// </summary>
		/// <param name="fileInfo"/>
		void Create(FileInfo fileInfo);

		/// <summary>
		/// Find a <see cref="FileInfo"/>
		/// </summary>
		/// <param name="fileId"/>
		/// <returns/>
		FileInfo FindByFileId(int fileId);

		/// <summary>
		/// Find all <see cref="FileInfo"/>s for a user
		/// </summary>
		/// <param name="userId"/>
		/// <returns/>
		IEnumerable<FileInfo> FindByUserId(int userId);

		/// <summary>
		/// Update a <see cref="FileInfo"/>
		/// </summary>
		/// <param name="fileInfo"/>
		void Update(FileInfo fileInfo);

		/// <summary>
		/// Delete a <see cref="FileInfo"/>
		/// </summary>
		/// <param name="fileInfo"/>
		void Delete(FileInfo fileInfo);
	}
}
