using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class SysIoPath : IPath
	{
		public string Combine(string p1, string p2)
		{
			return Path.Combine(p1, p2);
		}

		public string GetFileNameWithoutExtension(string filename)
		{
			return Path.GetFileNameWithoutExtension(filename);
		}

		public string GetDirectoryName(string filename)
		{
			return Path.GetDirectoryName(filename);
		}

		public string GetFileName(string p)
		{
			return Path.GetFileName(p);
		}

		public string GetExtension(string filename)
		{
			return Path.GetExtension(filename);
		}

		public string GetTempPath()
		{
			return Path.GetTempPath();
		}
	}
}
