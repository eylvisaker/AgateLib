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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AgateLib.IO
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
		IEnumerable<string> GetAllFiles(FileSearchOption searchOption = FileSearchOption.CurrentDirectory);
		/// <summary>
		/// Enumerates through all filenames which match the specified search pattern.
		/// </summary>
		/// <remarks>The search pattern is not regex style pattern matching, rather it should 
		/// be bash pattern matching, so a searchPattern of "*" would match all files, and
		/// "*.*" would match all filenames with a period in them.</remarks>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		IEnumerable<string> GetAllFiles(string searchPattern, FileSearchOption searchOption = FileSearchOption.CurrentDirectory);
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
}
