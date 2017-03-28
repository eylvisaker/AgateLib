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

namespace AgateLib
{
	/// <summary>
	/// Used by AgateLib.AgateApp class's error reporting functions
	/// to indicate how severe an error is.
	/// </summary>
	public enum ErrorLevel
	{
		/// <summary>
		/// Indicates an message is just a comment, and safe to ignore.
		/// </summary>
		Comment,
		/// <summary>
		/// Indicates that the error message is not severe, and the program may
		/// continue.  However, unexpected behavior may occur due to the result of
		/// this error.
		/// </summary>
		Warning,
		/// <summary>
		/// Indicates that the error condition is too severe and the program 
		/// may not continue.
		/// </summary>
		Fatal,


		/// <summary>
		/// Indicates the error condition indicates some assumption
		/// has not held that should have.  This should only be used
		/// if the condition is caused by a bug in the code.
		/// </summary>
		Bug,
	}

	/// <summary>
	/// Enum used to inidicate the level of cross-platform debugging that should occur.
	/// </summary>
	public enum CrossPlatformDebugLevel
	{
		/// <summary>
		/// Ignores any issues related to cross platform deployment.
		/// </summary>
		None,

		/// <summary>
		/// Outputs comments using AgateApp.Report with a comment level.
		/// </summary>
		Comment,

		/// <summary>
		/// Throws exceptions on issues that may cause problems when operating on another platform.
		/// </summary>
		Exception,
	}

}
