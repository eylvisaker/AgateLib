using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.IO
{
	public class SubdirectoryProvider : IReadFileProvider
	{
		private IReadFileProvider parent;
		private string subdir;

		public SubdirectoryProvider(IReadFileProvider parent, string subdir)
		{
			// TODO: Complete member initialization
			this.parent = parent;
			this.subdir = subdir.Replace('\\', '/');

			if (this.subdir.EndsWith("/") == false)
				this.subdir += "/";
		}

		public System.IO.Stream OpenRead(string filename)
		{
			return parent.OpenRead(subdir + filename);
		}

		public bool FileExists(string filename)
		{
			return parent.FileExists(subdir + filename);
		}

		public IEnumerable<string> GetAllFiles()
		{
			return parent.GetAllFiles(subdir + "**");
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			return parent.GetAllFiles(subdir + searchPattern);
		}

		public string ReadAllText(string filename)
		{
			return parent.ReadAllText(subdir + filename);
		}

		public bool IsRealFile(string filename)
		{
			return parent.IsRealFile(subdir + filename);
		}

		public string ResolveFile(string filename)
		{
			return parent.ResolveFile(subdir + filename);
		}
	}
}
