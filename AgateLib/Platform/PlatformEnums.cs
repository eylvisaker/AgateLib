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

namespace AgateLib.Platform
{
	/// <summary>
	/// Enumeration listing the known platform types.
	/// </summary>
	public enum PlatformType
	{
		/// <summary>
		/// Default value.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The Microsoft Windows platform, including Windows 98, Windows NT, Windows XP, Windows Vista, etc.
		/// </summary>
		Windows,
		/// <summary>
		/// Some Linux / Unix platform, typically running with an X windowing system.
		/// </summary>
		Linux,
		/// <summary>
		/// Mac OS 10.3 or later.
		/// </summary>
		MacOS,

		/// <summary>
		/// An device running the Android operating system.
		/// </summary>
		Android,
	}

	/// <summary>
	/// Enum which indicates which type of device we are running on.
	/// </summary>
	public enum DeviceType
	{
		/// <summary>
		/// Undetermined device type
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// A desktop or laptop computer with a mouse and keyboard for input.
		/// </summary>
		Computer,
		/// <summary>
		/// A tablet with a large screen and the main input method is the touch screen.
		/// </summary>
		Tablet,
		/// <summary>
		/// A handheld device such as a smart phone with a small but possibly high resolution screen.
		/// </summary>
		Handheld,
		/// <summary>
		/// A console system that primarily uses a gamepad as input
		/// </summary>
		Console,
	}

	/// <summary>
	/// Enum indicating which version of Microsoft Windows is currently being run.
	/// </summary>
	public enum WindowsVersion
	{
		/// <summary>
		/// An unknown version of Windows.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Windows 98.
		/// </summary>
		Windows98,
		/// <summary>
		/// Windows XP or Server 2003.
		/// </summary>
		WindowsXP,
		/// <summary>
		/// Windows Vista or Server 2008.
		/// </summary>
		WindowsVista,
		/// <summary>
		/// Windows 7.
		/// </summary>
		Windows7,
		/// <summary>
		/// Windows 8.
		/// </summary>
		Windows8,
		/// <summary>
		/// Windows 10.
		/// </summary>
		Windows10,
	}

	/// <summary>
	/// Enum indicating which .NET runtime is currently in use.
	/// </summary>
	public enum DotNetRuntime
	{
		/// <summary>
		/// An unknown runtime.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Microsoft's runtime.
		/// </summary>
		MicrosoftDotNet = 1,
		/// <summary>
		/// The runtime of the Mono project.
		/// </summary>
		Mono = 2,
		/// <summary>
		/// The DotGnu / Portable.NET runtime.
		/// Note that presence of this enumeration value does not indicate
		/// that using AgateLib on DotGnu is supported.
		/// </summary>
		DotGnu = 9999,
	}
}
