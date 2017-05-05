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

		public IEnumerable<string> GetAllFiles(FileSearchOption searchOption)
		{
			return files.Keys;
		}

		public IEnumerable<string> GetAllFiles(string searchPattern, FileSearchOption searchOption)
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
