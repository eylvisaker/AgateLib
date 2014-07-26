using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public static class FileSystem
	{
		public static IFile File { get; set; }
		public static IPath Path { get; set; }
		public static IDirectory Directory { get; set; }
	}
}
