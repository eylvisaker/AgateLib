﻿//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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

		public override string ToString()
		{
			return parent.ToString() + subdir;
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
			if (IsRooted(filename))
				return parent.ResolveFile(filename);

			return parent.ResolveFile(subdir + filename);
		}

		private bool IsRooted(string filename)
		{
			if (char.IsLetter(filename[0]) && filename[1] == ':')
				return true;
			if (filename.StartsWith("/"))
				return true;
			if (Core.Platform.PlatformType == Platform.PlatformType.Windows &&
				filename.StartsWith("\\"))
				return true;

			return false;
		}
	}
}
