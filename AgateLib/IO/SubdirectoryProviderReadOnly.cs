//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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

		public IEnumerable<string> GetAllFiles()
		{
			return parent.GetAllFiles(subdir + "**");
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			var results = parent.GetAllFiles(subdir + searchPattern);

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
