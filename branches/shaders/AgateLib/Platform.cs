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
//     Portions created by Erik Ylvisaker are Copyright (C) 2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AgateLib
{
	public class Platform
	{
		PlatformType mType;
		DotNetRuntime mRuntime;

		internal Platform() 
		{
			mType = DetectPlatformType();
			mRuntime = DetectRuntime();
		}

		private DotNetRuntime DetectRuntime()
		{
			DotNetRuntime runtime = DotNetRuntime.MicrosoftDotNet;

			if (Type.GetType("Mono.Runtime") != null)
				runtime = DotNetRuntime.Mono;

			return runtime;
		}

		public PlatformType PlatformType
		{
			get { return mType; }
		}
		public DotNetRuntime Runtime
		{
			get { return mRuntime; }
		}

		PlatformType DetectPlatformType()
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.WinCE:
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
					return AgateLib.PlatformType.Windows;

				case PlatformID.Unix:
					string kernel = DetectUnixKernel();

					if (kernel == "Darwin")
						return PlatformType.MacOS;
					else
						return PlatformType.Linux;

				case PlatformID.MacOSX:
					return PlatformType.MacOS;

				case PlatformID.Xbox:
					return PlatformType.XBox360;
			}

			return PlatformType.Unknown;
		}

		#region private static string DetectUnixKernel()

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		struct utsname
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string sysname;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string nodename;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string release;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string version;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string machine;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string extraJustInCase;

		}

		/// <summary>
		/// Detects the unix kernel by p/invoking the uname call in libc.
		/// </summary>
		/// <returns></returns>
		private static string DetectUnixKernel()
		{
			Debug.Print("Size: {0}", Marshal.SizeOf(typeof(utsname)).ToString());
			Debug.Flush();
			
			utsname uts = new utsname();
			uname(out uts);

			Debug.WriteLine("System:");
			Debug.Indent();
			Debug.WriteLine(uts.sysname);
			Debug.WriteLine(uts.nodename);
			Debug.WriteLine(uts.release);
			Debug.WriteLine(uts.version);
			Debug.WriteLine(uts.machine);
			Debug.Unindent();

			return uts.sysname.ToString();
		}

		[DllImport("libc")]
		private static extern void uname(out utsname uname_struct);

		#endregion

	}
}
