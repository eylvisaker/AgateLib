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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AgateLib.Platform.WinForms.PlatformImplementation
{
	/// <summary>
	/// Class which contains known information about the platform.
	/// This class also contains the folders where the application should store its data,
	/// which are automatically created from the AssemblyCompanhy and AssemblyProduct
	/// attributes for the assembly where the entry point for the application is.
	/// </summary>
	internal class FormsPlatformInfo : IPlatformInfo
	{
		WindowsVersion mWindowsVersion;
		string mDocuments;
		string mAppData;
		string mAppDir;
		bool m64Bit;

		internal FormsPlatformInfo()
		{
			PlatformType = DetectPlatformType();
			DeviceType = DeviceType.Computer;
			Runtime = DetectRuntime();
			m64Bit = Detect64Bit();

			if (PlatformType == PlatformType.Windows)
				mWindowsVersion = DetectWindowsVersion();

			// According to http://msdn.microsoft.com/query/dev10.query?appId=Dev10IDEF1&l=EN-US&k=k%28SYSTEM.DIAGNOSTICS.DEBUG.LISTENERS%29;k%28TargetFrameworkMoniker-%22.NETFRAMEWORK%2cVERSION%3dV3.5%22%29;k%28DevLang-CSHARP%29&rd=true
			//		The Listeners collection is shared by both the Debug and the Trace classes; 
			//		adding a trace listener to either class adds the listener to both.
			// So we will just use the Trace.Listeners class.
			if (PlatformType != PlatformType.Windows)
			{
				Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			}

			//CheckOSVersion();

			//if (mType == PlatformType.Windows)
			//	mWindowsVersion = DetectWindowsVersion();

			SetFolders();

			string debugLog = "agate-debuglog.txt";

			if (HasWriteAccessToAppDirectory())
			{
				debugLog = Path.Combine(mAppDir, debugLog);
			}
			else
			{
				debugLog = Path.Combine(mAppData, debugLog);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(debugLog));

			try
			{
				Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(debugLog)));
			}
			catch (Exception)
			{
				Trace.WriteLine("Could not open debug or trace log for writing.");
			}

			Trace.WriteLine("64-bit platform: " + m64Bit.ToString());

		}
		/// <summary>
		/// Returns the version of windows being used, if the current platform is Windows.
		/// An exception is thrown if this property is checked when the platform is not Windows.
		/// </summary>
		public WindowsVersion WindowsVersion
		{
			get
			{
				if (PlatformType != PlatformType.Windows)
					return WindowsVersion.Unknown;

				return mWindowsVersion;
			}
		}

		public PlatformType PlatformType { get; private set; }

		public DeviceType DeviceType { get; private set; }

		public DotNetRuntime Runtime { get; private set; }

		private bool Detect64Bit()
		{
			unsafe
			{
				int size = sizeof(IntPtr);

				switch (size)
				{
					case 4: return false;
					case 8: return true;
					default:
						throw new AgateException($"Size of IntPtr is {size}.");
				}
			}
		}

		private bool HasWriteAccessToAppDirectory()
		{
			return false;
			/*
			 * TODO: Fix this!!
			// TODO: Maybe there is a better way to inspect permissions?
			// here we just try to write and see if we fail.
			string filename = Path.GetTempFileName();

			try
			{
				string targetFile = Path.Combine(mAppDir, Path.GetFileName(filename));

				using (var w = new StreamWriter( targetFile))
				{
					w.WriteLine("x");
				}

				File.Delete(targetFile);
				return true;
			}
			catch
			{
				return false;
			}*/
		}

		static T GetCustomAttribute<T>(Assembly ass) where T : Attribute 	
		{
			try
			{
				return ass.GetCustomAttributes(typeof(T), false)[0] as T;
			}
			catch
			{
				return null;
			}
		}

		private void SetFolders()
		{
			Assembly entryPt = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
			string fqn = entryPt.GetLoadedModules()[0].FullyQualifiedName;

			var companyAttribute = GetCustomAttribute<AssemblyCompanyAttribute>(entryPt);
			var nameAttribute = GetCustomAttribute<AssemblyProductAttribute>(entryPt);

			mAppDir = Path.GetDirectoryName(fqn);
			Console.WriteLine("App Dir: {0}", mAppDir);

			string companyName = companyAttribute != null ? companyAttribute.Company : string.Empty;
			string product = nameAttribute != null ? nameAttribute.Product : string.Empty;

			SetFolderPaths(companyName, product);
		}

		/// <summary>
		/// Sets the folder paths for data based on the company name and application name.
		/// This only needs to be called if the values used in the AssemblyCompany and
		/// AssemblyProduct are not what you want to use to define these locations.
		/// </summary>
		/// <param name="companyName"></param>
		/// <param name="appName"></param>
		private void SetFolderPaths(string companyName, string appName)
		{
			string combDir = Path.Combine(companyName, appName);

			if (string.IsNullOrEmpty(combDir))
			{
				mAppData = mAppDir;
				Trace.WriteLine("Warning: No assembly level company / product name attributes were found.");
			}
			else
				mAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			mAppData = Path.Combine(mAppData, combDir);
			mDocuments = Path.Combine(mDocuments, combDir);

			Console.WriteLine("App Data: {0}", mAppData);
			Console.WriteLine("Documents: {0}", mDocuments);

		}

		private DotNetRuntime DetectRuntime()
		{
			DotNetRuntime runtime = DotNetRuntime.MicrosoftDotNet;

			if (Type.GetType("Mono.Runtime") != null)
				runtime = DotNetRuntime.Mono;

			return runtime;
		}

		PlatformType DetectPlatformType()
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.WinCE:
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
					return PlatformType.Windows;

				case PlatformID.Unix:
					string kernel = DetectUnixKernel();

					return kernel == "Darwin" ? PlatformType.MacOS : PlatformType.Linux;

				case PlatformID.MacOSX:
					return PlatformType.MacOS;

				default:
					return PlatformType.Unknown;
			}
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
			private string extraJustInCase;

		}

		 /// <summary>
		 /// Detects the unix kernel by p/invoking the uname call in libc.
		 /// </summary>
		 /// <returns></returns>
		private static string DetectUnixKernel()
		{
			Debug.WriteLine("Size: {0}", Marshal.SizeOf(typeof(utsname)).ToString());
			Debug.Flush();
			
			utsname uts = new utsname();
			uname(out uts);

			Debug.WriteLine("System:");
			Debug.WriteLine(uts.sysname);
			Debug.WriteLine(uts.nodename);
			Debug.WriteLine(uts.release);
			Debug.WriteLine(uts.version);
			Debug.WriteLine(uts.machine);

			return uts.sysname.ToString();
		}

		[DllImport("libc")]
		private static extern void uname(out utsname uname_struct);

		#endregion


		private void CheckOSVersion()
		{
			var version = System.Environment.OSVersion.Version;


			Debug.WriteLine("OS Version: {0}", System.Environment.OSVersion.VersionString);
			Debug.IndentLevel++;
			Debug.WriteLine("Major: {0}", version.Major);
			Debug.WriteLine("Major revision: {0}", version.MajorRevision);
			Debug.WriteLine("Minor: {0}", version.Minor);
			Debug.WriteLine("Minor revision: {0}", version.MinorRevision);
			Debug.WriteLine("Revision: {0}", version.Revision);
			Debug.WriteLine("BuildEllipse: {0}", version.Build);
			Debug.WriteLine("Service Pack: {0}", System.Environment.OSVersion.ServicePack);
			Debug.IndentLevel--;
		}

		private WindowsVersion DetectWindowsVersion()
		{
			WindowsVersion result = WindowsVersion.WindowsVista;
			
			switch (System.Environment.OSVersion.Version.Major)
			{
				case 4:
					result = WindowsVersion.Windows98;
					break;
				case 5:
					result = WindowsVersion.WindowsXP;
					break;
				case 6:
					result = WindowsVersion.WindowsVista;
					break;
				case 7:
					result = WindowsVersion.Windows7;
					break;
				case 8:
					result = WindowsVersion.Windows8;
					break;
				case 10:
					result = WindowsVersion.Windows10;
					break;
			}

			return result;
		}
	}
}
