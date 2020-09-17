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
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AgateLib
{
	/// <summary>
	/// Class which contains known information about the platform.
	/// This class also contains the folders where the application should store its data,
	/// which are automatically created from the AssemblyCompanhy and AssemblyProduct
	/// attributes for the assembly where the entry point for the application is.
	/// </summary>
	public class Platform
	{
		PlatformType mType;
		DotNetRuntime mRuntime;
		WindowsVersion mWindowsVersion;
		string mDocuments;
		string mAppData;
		string mAppDir;
		bool m64Bit;

		internal Platform()
		{
			mType = DetectPlatformType();
			mRuntime = DetectRuntime();
			m64Bit = Detect64Bit();

			if (PlatformType != PlatformType.Windows)
			{
				Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
				Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			}

			CheckOSVersion();

			if (mType == PlatformType.Windows)
				mWindowsVersion = DetectWindowsVersion();

			SetFolders();

			string debugLog = "agate-debuglog.txt";
			string traceLog = "debuglog.txt";

			if (HasWriteAccessToAppDirectory())
			{
				debugLog = Path.Combine(mAppDir, debugLog);
				traceLog = Path.Combine(mAppDir, traceLog);
			}
			else
			{
				debugLog = Path.Combine(mAppData, debugLog);
				traceLog = Path.Combine(mAppData, traceLog);
			}

			try
			{
				Debug.Listeners.Add(new TextWriterTraceListener(new StreamWriter(debugLog)));
				Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(traceLog)));
			}
			catch (Exception)
			{
				Trace.WriteLine("Could not open debug or trace log for writing.");
			}

			Trace.WriteLine("64-bit platform: " + m64Bit.ToString());
		}

		private bool Detect64Bit()
		{
			int size = Marshal.SizeOf(typeof(IntPtr));

			switch (size)
			{
				case 4: return false;
				case 8: return true;
				default:
					throw new AgateException(string.Format("Size of IntPtr is {0}.", size));
			}
		}

		private bool HasWriteAccessToAppDirectory()
		{
			// TODO: Maybe there is a better way to inspect permissions?
			// here we just stry to write and see if we fail.
			string filename = Path.GetTempFileName();

			try
			{
				string targetFile = Path.Combine(mAppDir, Path.GetFileName(filename));

				using (var w = new StreamWriter(targetFile))
				{
					w.WriteLine("x");
				}

				File.Delete(targetFile);
				return true;
			}
			catch
			{
				return false;
			}
		}

		internal void EnsureAppDataDirectoryExists()
		{
			if (Directory.Exists(AppDataDirectory))
				return;

			Directory.CreateDirectory(AppDataDirectory);
		}

		/// <summary>
		/// Gets the directory where the application should store its configuration data.
		/// </summary>
		public string AppDataDirectory
		{
			get { return mAppData; }
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
			Assembly entryPt = Assembly.GetEntryAssembly();
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
		public void SetFolderPaths(string companyName, string appName)
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

		/// <summary>
		/// Returns the version of windows being used, if the current platform is Windows.
		/// An exception is thrown if this property is checked when the platform is not Windows.
		/// </summary>
		public WindowsVersion WindowsVersion
		{
			get
			{
				if (PlatformType != PlatformType.Windows)
					throw new AgateCrossPlatformException(
						"Current platform is not Windows, but the WindowsVersion propety was checked.");

				return mWindowsVersion;
			}
		}

		/// <summary>
		/// Gets the platform type.
		/// </summary>
		public PlatformType PlatformType
		{
			get { return mType; }
		}
		/// <summary>
		/// Gets the runtime being used.
		/// </summary>
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


		private void CheckOSVersion()
		{
			var version = System.Environment.OSVersion.Version;

			Debug.Print("OS Version: {0}", System.Environment.OSVersion.VersionString);
			Debug.IndentLevel++;
			Debug.Print("Major: {0}", version.Major);
			Debug.Print("Major revision: {0}", version.MajorRevision);
			Debug.Print("Minor: {0}", version.Minor);
			Debug.Print("Minor revision: {0}", version.MinorRevision);
			Debug.Print("Revision: {0}", version.Revision);
			Debug.Print("Build: {0}", version.Build);
			Debug.Print("Service Pack: {0}", System.Environment.OSVersion.ServicePack);
			Debug.IndentLevel--;
		}

		private WindowsVersion DetectWindowsVersion()
		{
			WindowsVersion retval = WindowsVersion.WindowsVista;
			
			switch (System.Environment.OSVersion.Version.Major)
			{
				case 4:
					retval = WindowsVersion.Windows98;
					break;
				case 5:
					retval = WindowsVersion.WindowsXP;
					break;
				case 6:
					retval = WindowsVersion.WindowsVista;
					break;
				case 7:
					retval = WindowsVersion.Windows7;
					break;
			}

			return retval;
		}
	
	}
}
