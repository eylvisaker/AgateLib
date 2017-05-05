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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.IO
{

	/// <summary>
	/// Enum indicating how a file should be opened.
	/// </summary>
	public enum FileOpenMode
	{
		/// <summary>
		/// Specifies a new file should always be created. If the file does not exist
		/// it will be created, if it does exist it will be overwritten.
		/// </summary>
		Create,

		/// <summary>
		/// Specifies that if the file exists, it should be opened for appending.
		/// If the file does not exist, it will be created.
		/// </summary>
		Append,
	}

	/// <summary>
	/// Enum indicating whether directories should be traversed in file searches.
	/// </summary>
	public enum FileSearchOption
	{
		CurrentDirectory,
		AllFolders,
	}
}
