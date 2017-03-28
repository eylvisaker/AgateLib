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
using System.IO;
using AgateLib.IO;
using AgateLib.Quality;

namespace AgateLib
{
	/// <summary>
	/// Provides extensions for file provider objects.
	/// </summary>
	public static class FileProviderExtensions
	{
		/// <summary>
		/// Synchronous opening of files for reading.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static Stream OpenRead(this IReadFileProvider provider, string filename)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenReadAsync(filename);

			return task.GetAwaiter().GetResult();
		}

		/// <summary>
		/// Synchronous opening of files for writing.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static Stream OpenWrite(this IReadWriteFileProvider provider, string filename, FileOpenMode mode = FileOpenMode.Create)
		{
			Condition.Requires<ArgumentNullException>(provider != null, "provider");
			Condition.Requires<ArgumentNullException>(filename != null, "filename");

			var task = provider.OpenWriteAsync(filename, mode);

			return task.GetAwaiter().GetResult();
		}

		/// <summary>
		/// Returns an IReadWriteFileProvider object which works from a subdirectory of this IReadWriteFileProvider object.
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="subpath"></param>
		/// <returns></returns>
		public static IReadWriteFileProvider Subdirectory(this IReadWriteFileProvider fileProvider, string subpath)
		{
			return new SubdirectoryProviderReadWrite(fileProvider, subpath);
		}

		/// <summary>
		/// Returns an IReadFileProvider object which works from a subdirectory of this IReadFileProvider object.
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="subpath"></param>
		/// <returns></returns>
		public static IReadFileProvider Subdirectory(this IReadFileProvider fileProvider, string subpath)
		{
			return new SubdirectoryProviderReadOnly(fileProvider, subpath);
		}
	}
}