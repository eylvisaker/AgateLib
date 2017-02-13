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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;

namespace AgateLib.Platform.Test
{
	public class FakeReadFileProvider : IReadFileProvider
	{
		protected class FileInfo
		{
			public FileInfo(byte[] contents)
			{
				Contents = contents;
			}

			public byte[] Contents { get; set; }
			public int ReadCount { get; set; }
		}

		Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();

		protected Dictionary<string, FileInfo> Files => files;

		public int ReadCount(string filename)
		{
			return files[NormalizePath(filename)].ReadCount;
		}

		public void Add(string filename, string fileContents)
		{
			Add(NormalizePath(NormalizePath(filename)), Encoding.UTF8.GetBytes(fileContents));
		}

		private void Add(string filename, byte[] fileContents)
		{
			files.Add(NormalizePath(filename), new FileInfo(fileContents));
		}

		public Task<Stream> OpenReadAsync(string filename)
		{
			var normalFilename = NormalizePath(filename);

			VerifyFileExists(normalFilename);

			return Task.FromResult<Stream>(new MemoryStream(files[normalFilename].Contents));
		}

		public bool FileExists(string filename)
		{
			return files.ContainsKey(filename);
		}

		public IEnumerable<string> GetAllFiles()
		{
			return files.Keys;
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			return files.Keys.Where(file => SearchPatternMatches(searchPattern, file));
		}

		public string ReadAllText(string filename)
		{
			var normalFilename = NormalizePath(filename);
			
			VerifyFileExists(normalFilename);
			var data = files[normalFilename].Contents;

			return Encoding.UTF8.GetString(data, 0, data.Length);
		}

		public bool IsRealFile(string filename)
		{
			return true;
		}

		public string ResolveFile(string filename)
		{
			return filename;
		}

		public bool IsLogicalFilesystem => true;

		private void VerifyFileExists(string filename)
		{
			var normalFilename = NormalizePath(filename);

			if (files.ContainsKey(normalFilename) == false)
				throw new FileNotFoundException($"Could not find file {filename} in {nameof(FakeReadFileProvider)}.");

			files[normalFilename].ReadCount++;
		}

		private bool SearchPatternMatches(string searchPattern, string file)
		{
			return true;
		}

		private string NormalizePath(string filename)
		{
			return filename.Replace("\\", "/");
		}

	}
}
