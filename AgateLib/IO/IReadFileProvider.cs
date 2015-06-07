using AgateLib.Quality;
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib
{
	/// <summary>
	/// Public interface that should be implemented 
	/// </summary>
	public interface IReadFileProvider
	{
		/// <summary>
		/// Opens the specified file returning a stream.  Throws 
		/// FileNotFoundException if the file does not exist.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		Task<Stream> OpenReadAsync(string filename);
		/// <summary>
		/// Checks to if the specified file exists in the file provider.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		bool FileExists(string filename);

		/// <summary>
		/// Enumerates through all existing filenames in the file provider.
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetAllFiles();
		/// <summary>
		/// Enumerates through all filenames which match the specified search pattern.
		/// </summary>
		/// <remarks>The search pattern is not regex style pattern matching, rather it should 
		/// be bash pattern matching, so a searchPattern of "*" would match all files, and
		/// "*.*" would match all filenames with a period in them.</remarks>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		IEnumerable<string> GetAllFiles(string searchPattern);
		/// <summary>
		/// Returns a string containing the entire contents of the specified file.
		/// </summary>
		/// <param name="filename">The path and filename of the file to read from.</param>
		/// <returns></returns>
		string ReadAllText(string filename);

		/// <summary>
		/// Returns true if the specified filename points to an actual file on disk.
		/// If this method returns false, then ResolveFile will throw an exception
		/// for that file.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		bool IsRealFile(string filename);

		/// <summary>
		/// Returns the full path of the given filename.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		string ResolveFile(string filename);

		/// <summary>
		/// Returns true if the file system is not a physical file system.
		/// </summary>
		bool IsLogicalFilesystem { get; }
	}

	public static class FileProviderExtensions
	{
		public static Stream OpenRead(this IReadFileProvider provider, string filename)
		{
            Condition.Requires<ArgumentNullException>(provider != null, "provider");
            Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenReadAsync(filename);

			return task.Result;
		}
	}
}
