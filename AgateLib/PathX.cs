//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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

namespace AgateLib
{
	/// <summary>
	/// Provides some basic methods for manipulating paths.
	/// </summary>
	public static class PathX
	{
		/// <summary>
		/// Returns the relative path from the origin to the destination.
		/// </summary>
		/// <param name="originDirectory"></param>
		/// <param name="destinationPath"></param>
		/// <returns></returns>
		public static string MakeRelativePath(string originDirectory, string destinationPath, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (originDirectory == null)
				throw new ArgumentNullException(nameof(originDirectory));

			if (destinationPath == null)
				throw new ArgumentNullException(nameof(destinationPath));

			bool isRooted = (Path.IsPathRooted(originDirectory) && Path.IsPathRooted(destinationPath));

			originDirectory = originDirectory.Replace('\\', '/');
			destinationPath = destinationPath.Replace('\\', '/');

			if (isRooted)
			{
				bool isDifferentRoot = (string.Compare(Path.GetPathRoot(originDirectory), Path.GetPathRoot(destinationPath), comparisonType) != 0);

				if (isDifferentRoot)
					return destinationPath;
			}

			List<string> relativePath = new List<string>();

			string[] fromDirectories = originDirectory.Split('/');
			string[] toDirectories = destinationPath.Split('/');

			int length = Math.Min(fromDirectories.Length, toDirectories.Length);

			int lastCommonRoot = -1;

			// find common root
			for (int x = 0; x < length; x++)
			{
				if (string.Compare(fromDirectories[x], toDirectories[x], comparisonType) != 0)
					break;

				lastCommonRoot = x;
			}

			if (lastCommonRoot == -1)
				return destinationPath;

			// add relative folders in from path
			for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
			{
				if (fromDirectories[x].Length > 0)
					relativePath.Add("..");
			}

			// add to folders to path
			for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
			{
				relativePath.Add(toDirectories[x]);
			}

			// create relative path
			string[] relativeParts = new string[relativePath.Count];
			relativePath.CopyTo(relativeParts, 0);

			string newPath = string.Join("/", relativeParts);

			return newPath;
		}

		public static string GetDirectoryName(string path)
		{
			var result = Path.GetDirectoryName(path);

			return NormalizePath(result);
		}

		public static string NormalizePath(string path)
		{
			var result = path.Replace('\\', '/');

			var elements = result.Split('/').ToList();

			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i] == ".." && i > 0)
				{
					elements.RemoveAt(i);
					elements.RemoveAt(i - 1);
					i -= 2;
				}
			}

			return string.Join("/", elements);
		}
	}
}
