using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WinForms.Factories
{
	class SysIoFile : IFile
	{
		public bool Exists(string path)
		{
			return File.Exists(path);
		}

		public Stream OpenRead(string filename)
		{
			return File.Open(filename, FileMode.Open);
		}

		public Stream OpenWrite(string filename, bool append = false)
		{
			return File.Open(filename, append ? FileMode.Append : FileMode.Create);
		}
	}
}
