using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsStore.Factories
{
	class FakePath : IPath
	{
		public string Combine(string p1, string p2)
		{
			throw new NotImplementedException();
		}

		public string GetFileNameWithoutExtension(string filename)
		{
			throw new NotImplementedException();
		}

		public string GetDirectoryName(string filename)
		{
			throw new NotImplementedException();
		}

		public string GetFileName(string p)
		{
			throw new NotImplementedException();
		}

		public string GetExtension(string filename)
		{
			throw new NotImplementedException();
		}

		public string GetTempPath()
		{
			throw new NotImplementedException();
		}
	}
}
