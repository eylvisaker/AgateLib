using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public interface IDirectory
	{
		string[] GetFiles(string mPath);

		IEnumerable<string> GetFiles(string mPath, string searchPattern);
	}
}
