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
using System.Text;

namespace AgateLib
{
	/// <summary>
	/// Base exception class for exceptions which are thrown by AgateLib.
	/// </summary>
	public class AgateException : Exception
	{
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException() { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(string message) : base(message) { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(Exception inner, string message) : base(message, inner) { }
		/// <summary>
		/// Constructs an AgateException, calling string.Format on the arguments.
		/// </summary>
		public AgateException(string format, params object[] args)
			: base(string.Format(format, args)) { }
		/// <summary>
		/// Constructs an AgateException.
		/// </summary>
		public AgateException(Exception inner, string format, params object[] args)
			: base(string.Format(format, args), inner) { }
	}

	/// <summary>
	/// Exception which is thrown when AgateLib detects that it is used in a way that
	/// may not be portable to different platforms, 
	/// and AgateApp.CrossPlatformDebugLevel is set to Exception.
	/// </summary>
	public class AgateCrossPlatformException : AgateException
	{
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException() { }
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException(string message) : base(message) { }
		/// <summary>
		/// Constructs a new AgateCrossPlatformException object.
		/// </summary>
		public AgateCrossPlatformException(string message, Exception inner) : base(message, inner) { }
	}
}