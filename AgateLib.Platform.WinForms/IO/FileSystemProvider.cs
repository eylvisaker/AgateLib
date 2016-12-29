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

			mPathUri = new Uri(path);
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
				throw new FileNotFoundException(string.Format("The file {0} was not found in the path {1}.",
					filename, mPath), filename);

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

			if (Core.ErrorReporting.CrossPlatformDebugLevel != CrossPlatformDebugLevel.Comment)
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
					Core.ErrorReporting.ReportCrossPlatformError(
						"The file " + filename + " located in directory \"" + mPath + "\"" +
						" has multiple case-insensitive matches.  This will present problems " +
						"when porting to an operating system with a case-insensitive filesystem (eg. Windows, MacOS).");
				if (badCaseMatch)
					Core.ErrorReporting.ReportCrossPlatformError(
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

			Core.ErrorReporting.ReportCrossPlatformError(text);
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
		public IEnumerable<string> GetAllFiles()
		{
			List<string> files = new List<string>();

			files.AddRange(Directory.GetFiles(mPath));

			return files;
		}
		/// <summary>
		/// Gets all files in all paths that match the specified search pattern.
		/// </summary>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			List<string> files = new List<string>();

			try
			{
				files.AddRange(Directory.GetFiles(mPath, searchPattern)
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

		public Task<Stream> OpenWriteAsync(string file)
		{
            string resolvedName = Path.Combine(mPath, file);
			var result = File.Open(resolvedName, FileMode.Create);

			return Task.FromResult<Stream>(result);
		}

		public void CreateDirectory(string folder)
		{
			DebugCrossPlatform(folder);
			Directory.CreateDirectory(Path.Combine(mPath, folder));
		}
	}
}
