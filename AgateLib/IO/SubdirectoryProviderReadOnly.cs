//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace AgateLib.IO
{
	class SubdirectoryProviderReadOnly : IReadFileProvider
	{
		private IReadFileProvider parent;
		private string subdir;

		public SubdirectoryProviderReadOnly(IReadFileProvider parent, string subdir)
		{
			Require.ArgumentNotNull(parent, nameof(parent));
			Require.True<ArgumentException>(string.IsNullOrWhiteSpace(subdir) == false,
				"subdir must not be null or empty.");

			this.parent = parent;
			this.subdir = subdir.Replace('\\', '/');

			if (this.subdir.EndsWith("/") == false)
				this.subdir += "/";
		}

		public bool IsLogicalFilesystem => parent.IsLogicalFilesystem;

		public override string ToString()
		{
			return System.IO.Path.Combine(parent.ToString(), subdir);
		}

		public Task<System.IO.Stream> OpenReadAsync(string filename)
		{
			return parent.OpenReadAsync(MapFilename(filename));
		}

		public bool FileExists(string filename)
		{
			return parent.FileExists(MapFilename(filename));
		}

		public IEnumerable<string> GetAllFiles(FileSearchOption searchOption)
		{
			return parent.GetAllFiles(subdir + "**", searchOption);
		}

		public IEnumerable<string> GetAllFiles(string searchPattern, FileSearchOption searchOption)
		{
			var results = parent.GetAllFiles(subdir + searchPattern, searchOption);

			foreach (var result in results)
			{
				if (result.StartsWith(subdir))
					yield return result.Substring(subdir.Length);
				else
					yield return result;
			}
		}

		public string ReadAllText(string filename)
		{
			return parent.ReadAllText(MapFilename(filename));
		}

		public bool IsRealFile(string filename)
		{
			return parent.IsRealFile(MapFilename(subdir + filename));
		}

		public string ResolveFile(string filename)
		{
			return parent.ResolveFile(MapFilename(filename));
		}

		protected string MapFilename(string filename)
		{
			if (IsRooted(filename))
				return filename;
			else
				return subdir + filename;
		}

		protected bool IsRooted(string filename)
		{
			return System.IO.Path.IsPathRooted(filename);
		}
	}
}
