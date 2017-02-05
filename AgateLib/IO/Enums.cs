using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.IO
{

	/// <summary>
	/// Enum indicating how a file should be opened.
	/// </summary>
	public enum FileOpenMode
	{
		/// <summary>
		/// Specifies a new file should always be created. If the file does not exist
		/// it will be created, if it does exist it will be overwritten.
		/// </summary>
		Create,

		/// <summary>
		/// Specifies that if the file exists, it should be opened for appending.
		/// If the file does not exist, it will be created.
		/// </summary>
		Append,
	}
}
