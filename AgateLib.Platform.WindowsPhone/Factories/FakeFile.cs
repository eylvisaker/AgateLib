using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsPhone.Factories
{
	class FakeFile : IFile
	{
		public bool Exists(string path)
		{
			throw new NotImplementedException();
		}

		public System.IO.Stream OpenRead(string filename)
		{
			throw new NotImplementedException();
		}

		public System.IO.Stream OpenWrite(string filename, bool append = false)
		{
			throw new NotImplementedException();
		}
	}
}
