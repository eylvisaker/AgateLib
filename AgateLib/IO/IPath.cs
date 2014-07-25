using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public interface IPath
	{
		string Combine(string p1, string p2);

		string GetFileNameWithoutExtension(string filename);

		string GetDirectoryName(string filename);

		string GetFileName(string p);

		string GetExtension(string filename);

		string GetTempPath();
	}
}
