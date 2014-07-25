using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public static class FileSystem
	{
		public static Stream OpenRead(string filename)
		{
			throw new NotImplementedException();
		}
		public static Stream OpenWrite(string ErrorFile, bool append = false)
		{
			throw new NotImplementedException();
		}


		public static IFile File { get; set; }
		public static IPath Path { get; set; }
		public static IDirectory Directory { get; set; }

	}
}
