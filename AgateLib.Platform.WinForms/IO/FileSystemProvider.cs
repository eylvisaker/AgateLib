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
using AgateLib.IO;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.IO
{
	/// <summary>
	/// FileSystemProvider implements IFileProvider, providing access to files
	/// from the operating system file system.
	/// </summary>
	public class FileSystemProvider : IReadWriteFileProvider
	{
		string mPath;
		Uri mPathUri;

		/// <summary>
		/// Constructs a FileSystemProvider object directing it to read files from the
		/// specified path.
		/// </summary>
		/// <param name="path"></param>
		public FileSystemProvider(string path)
		{
			mPath = path;

			if (path.EndsWith("/") == false && path.EndsWith("\\") == false)
				path += "/";

			path = path.Replace('\\', '/');

			if (path.StartsWith("./"))
				path = Directory.GetCurrentDirectory() + path.Substring(1);

			try
			{
				mPathUri = new Uri(path);
			}
			catch (Exception e)
			{
				throw new AgateException("Failed to understand path value. If your path is relative, it must start with './'",
					e);
			}
		}
		/// <summary>
		/// Opens a file.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public Task<Stream> OpenReadAsync(string filename)
		{
			string resolvedName = FindFileName(filename);
			if (resolvedName == null)
				throw new FileNotFoundException($"The file {filename} was not found in the path {mPath}.", filename);

			var result = File.OpenRead(resolvedName);
			return Task.FromResult<Stream>(result);
		}
		/// <summary>
		/// Returns true if the specified file exists.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool FileExists(string filename)
		{
			if (FindFileName(filename) != null)
				return true;
			else
				return false;
		}


		public bool IsRealFile(string filename)
		{
			if (FindFileName(filename) != null)
				return true;
			else
				return false;
		}

		public string ResolveFile(string filename)
		{
			var result = FindFileName(filename);

			if (result == null)
				throw new FileNotFoundException(filename);

			return result;
		}

		/// <summary>
		/// Searches through all directories in the SearchPathList object for the specified
		/// filename.  The search is performed in the order directories have been added,
		/// and the first result is returned.  If no file is found, null is returned.
		/// </summary>
		/// <param name="filename">Filename to search for.</param>
		/// <returns>The full path of the file, if it exists.  Null if no file is found.</returns>
		string FindFileName(string filename)
		{
			if (filename == null)
				return null;

			DebugCrossPlatform(filename);

			string path = Path.Combine(mPath, filename);

			if (File.Exists(path) == false)
				return null;

			if (AgateApp.ErrorReporting.CrossPlatformDebugLevel != CrossPlatformDebugLevel.Comment)
			{
				string[] files = Directory.GetFiles(mPath);
				string badMatch = "";
				bool badCaseMatch = false;
				int matchCount = 0;

				for (int i = 0; i < files.Length; i++)
				{
					string shortFilename = Path.GetFileName(files[i]);
					if (shortFilename.Equals(filename, StringComparison.OrdinalIgnoreCase) == false)
						continue;

					matchCount++;

					if (shortFilename.Equals(filename, StringComparison.Ordinal) == false)
					{
						badCaseMatch = true;
						badMatch = shortFilename;
					}
				}

				if (matchCount > 1)
					AgateApp.ErrorReporting.ReportCrossPlatformError(
						"The file " + filename + " located in directory \"" + mPath + "\"" +
						" has multiple case-insensitive matches.  This will present problems " +
						"when porting to an operating system with a case-insensitive filesystem (eg. Windows, MacOS).");
				if (badCaseMatch)
					AgateApp.ErrorReporting.ReportCrossPlatformError(
						"The search file " + filename + " does not have an exact case match in directory \"" +
						mPath + "\".  The closest match was " + badMatch +
						".  This will present problems when porting to an operating system " +
						"with a case-sensitive filesystem (eg. Linux, MacOS).");

			}

			return path;
		}


		private void DebugCrossPlatform(string filename)
		{
			if (filename == null)
				return;

			if (CheckCrossPlatformFilename(filename))
				return;

			StringBuilder b = new StringBuilder();
			b.Append("The path \"");
			b.Append(filename);
			b.Append("\" is not entered in a cross-platform manner.");
			b.AppendLine();

			b.Append("Use only forward slash (/) for path separators, and avoid using the following characters:  ");
			b.AppendLine(NonCrossPlatformChars);

			string text = b.ToString();

			AgateApp.ErrorReporting.ReportCrossPlatformError(text);
		}

		/// <summary>
		/// Returns a list of characters which may be valid file path characters
		/// on some platforms, but not others.
		/// </summary>
		private static string NonCrossPlatformChars
		{
			get
			{
				return @"\:|*?""<>";
			}
		}

		/// <summary>
		/// Checks to see if a filepath is entered in a cross-platform 
		/// manner, and returns true if it is.
		/// </summary>
		/// <param name="path">The path to check.</param>
		/// <returns>True if the passed path is cross-platform.</returns>
		private static bool CheckCrossPlatformFilename(string path)
		{
			if (path.Contains(Path.GetTempPath()))
				return true;

			string chars = NonCrossPlatformChars;

			for (int i = 0; i < chars.Length; i++)
			{
				if (path.Contains(chars[i].ToString()))
					return false;
			}

			return true;
		}
		/// <summary>
		/// Gets the path that this FileSystemProvider searches in.
		/// </summary>
		public string SearchPath
		{
			get
			{
				return mPath;
			}
		}

		/// <summary>
		/// Gets all files in all paths.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(FileSearchOption searchOption)
		{
			List<string> files = new List<string>();

			files.AddRange(Directory.GetFiles(mPath, "*.*", SearchOptionOf(searchOption)));

			return files;
		}
		/// <summary>
		/// Gets all files in all paths that match the specified search pattern.
		/// </summary>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(string searchPattern, FileSearchOption searchOption)
		{
			List<string> files = new List<string>();

			try
			{
				files.AddRange(Directory.GetFiles(mPath, searchPattern, SearchOptionOf(searchOption))
					.Select(x => MakeRelativePath(x)));
			}
			catch (DirectoryNotFoundException)
			{ }

			return files;
		}

		/// <summary>
		/// Constructs a path relative to the root of this file system provider.
		/// </summary>
		/// <param name="fullpath"></param>
		/// <returns></returns>
		public string MakeRelativePath(string fullpath)
		{
			var uri = new Uri(fullpath);
			var rel = mPathUri.MakeRelativeUri(uri);

			return rel.ToString();
		}

		/// <summary>
		/// Returns a string representing the FileSystemProvider object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("FileSystemProvider: {0}", mPath);
		}

		/// <summary>
		/// Returns a string containing all the text in the specified file.
		/// </summary>
		/// <param name="filename">The name of the file to read from.</param>
		/// <returns></returns>
		public string ReadAllText(string filename)
		{
			Stream s = OpenReadAsync(filename).Result;

			return new StreamReader(s).ReadToEnd();
		}


		public bool IsLogicalFilesystem
		{
			get { return true; }
		}

		public Task<Stream> OpenWriteAsync(string filename, FileOpenMode mode = FileOpenMode.Create)
		{
			string resolvedName = Path.Combine(mPath, filename);

			var dir = Path.GetDirectoryName(resolvedName);
			Directory.CreateDirectory(dir);

			var result = File.Open(resolvedName, ConvertOpenMode(mode));

			return Task.FromResult<Stream>(result);
		}

		private static FileMode ConvertOpenMode(FileOpenMode mode)
		{
			switch (mode)
			{
				case FileOpenMode.Create:
					return FileMode.Create;

				case FileOpenMode.Append:
					return FileMode.Append;

				default:
					throw new ArgumentException("Unknown FileOpenMode value.",
						nameof(mode));
			}
		}

		private SearchOption SearchOptionOf(FileSearchOption searchOption)
		{
			switch (searchOption)
			{
				case FileSearchOption.CurrentDirectory:
					return SearchOption.TopDirectoryOnly;

				case FileSearchOption.AllFolders:
					return SearchOption.AllDirectories;

				default:
					throw new ArgumentException("Unknown FileSearchOption value.", nameof(searchOption));
			}
		}
	}
}
