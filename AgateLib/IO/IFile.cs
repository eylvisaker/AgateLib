using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public interface IFile
	{
		bool Exists(string path);

		System.IO.Stream OpenRead(string filename);
		System.IO.Stream OpenWrite(string filename, bool append = false);
	}
}
