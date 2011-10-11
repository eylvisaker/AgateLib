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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.Utility;

namespace AgateLib.Drivers
{
	/// <summary>
	/// Static class with which drivers register themselves so that the library can
	/// instantiate them.
	/// </summary>
	public static class Registrar
	{
		private static List<AgateDriverInfo>
			displayDrivers = new List<AgateDriverInfo>(),
			audioDrivers = new List<AgateDriverInfo>(),
			inputDrivers = new List<AgateDriverInfo>(),
			desktopDrivers = new List<AgateDriverInfo>();

		private static bool mIsInitialized = false;

		private static IDesktopDriver mDesktop;

		private static readonly string[] KnownNativeLibraries = new string[]
		{
			"SDL.dll",
		};


		static bool Contains(this IEnumerable<AgateDriverInfo> list, DisplayTypeID type)
		{
			return list.Any(
				x => Comparator(x, DriverType.Display, (int)type));
		}
		static bool Contains(this IEnumerable<AgateDriverInfo> list, AudioTypeID type)
		{
			return list.Any(
				x => Comparator(x, DriverType.Audio, (int)type));
		}
		static bool Contains(this IEnumerable<AgateDriverInfo> list, InputTypeID type)
		{
			return list.Any(
				x => Comparator(x, DriverType.Input, (int)type));
		}
		static bool Comparator(AgateDriverInfo info, DriverType driverType, int type)
		{
			return info.DriverType == driverType && info.DriverTypeID == type;
		}

		static Registrar()
		{
		}
		/// <summary>
		/// Searches through FileManager.AssemblyPath for all *.dll files.  These files
		/// are loaded and searched for classes which derive from DisplayImpl, AudioImpl, etc.
		/// </summary>
		internal static void Initialize()
		{
			if (mIsInitialized)
				return;

			RegisterNullDrivers();

			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
			AppDomain sandbox = AppDomain.CreateDomain("AgateSandBox");

			AgateSandBoxLoader loader = (AgateSandBoxLoader)
				sandbox.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
				typeof(AgateSandBoxLoader).FullName);

			IEnumerable<string> files = AgateFileProvider.Assemblies.GetAllFiles("*.dll");

			foreach (string file in files)
			{
				if (ShouldSkipLibrary(file))
					continue;

				foreach (AgateDriverInfo info in loader.ReportDrivers(file))
				{
					switch (info.DriverType)
					{
						case DriverType.Display:
							displayDrivers.Add(info);
							break;

						case DriverType.Audio:
							audioDrivers.Add(info);
							break;

						case DriverType.Input:
							inputDrivers.Add(info);
							break;

						case DriverType.Desktop:
							desktopDrivers.Add(info);
							break;

						default:
							Core.ErrorReporting.Report(ErrorLevel.Warning, string.Format(
								"Could not interpret DriverType returned by type {0} in assembly {1}.",
								info.DriverTypeName, info.AssemblyFile), null);

							break;
					}
				}
			}

			AppDomain.Unload(sandbox);

			SortDriverInfo(displayDrivers);
			SortDriverInfo(audioDrivers);
			SortDriverInfo(inputDrivers);
			SortDriverInfo(desktopDrivers);

			mIsInitialized = true;
		}
		private static void SortDriverInfo(List<AgateDriverInfo> driverList)
		{
			// sorts the driver list in reverse order.
			driverList.Sort(delegate(AgateDriverInfo a, AgateDriverInfo b)
			{
				return -a.Priority.CompareTo(b.Priority);
			});
		}

		private static void RegisterNullDrivers()
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();

			AgateDriverInfo nullAudioInfo = new AgateDriverInfo(AudioTypeID.Silent,
				typeof(NullSoundImpl), "No audio", -100);

			nullAudioInfo.AssemblyFile = thisAssembly.CodeBase;
			nullAudioInfo.AssemblyName = thisAssembly.FullName;

			audioDrivers.Add(nullAudioInfo);

			AgateDriverInfo nullInputInfo = new AgateDriverInfo(InputTypeID.Silent,
				typeof(NullInputImpl), "No input", -100);

			nullInputInfo.AssemblyFile = thisAssembly.CodeBase;
			nullInputInfo.AssemblyName = thisAssembly.FullName;

			inputDrivers.Add(nullInputInfo);
		}

		private static bool ShouldSkipLibrary(string file)
		{
			// Native libraries in the same directory will give an error when loaded, 
			// so skip any ones that we know about that will probably be in the same
			// directory as the drivers.
			if (IsKnownNativeLibrary(file))
				return true;

			// hack, because mono crashes if AgateMDX.dll is present.
			// annoying, because it should report a failure to load the types in the
			// assembly, and then the try catch should continue after that.
			// this seems unnecessary in the current version of Mono.
			if ((Environment.OSVersion.Platform == PlatformID.Unix ||
				 Environment.OSVersion.Platform == (PlatformID)128) &&
				(System.IO.Path.GetFileName(file).ToLower().Contains("agatemdx.dll") ||
				 System.IO.Path.GetFileName(file).ToLower().Contains("agatesdx.dll")))
			{
				Core.ErrorReporting.Report(ErrorLevel.Comment,
					string.Format("DirectX not supported on non-Windows platforms.  {0}Remove {1} to eliminate this message.",
					Environment.NewLine, System.IO.Path.GetFileName(file)), null);

				return true;
			}

			// Skip the agatelib dll.
			if (System.IO.Path.GetFileName(file).ToLower().Contains("agatelib.dll"))
				return true;

			return false;
		}
		private static bool IsKnownNativeLibrary(string path)
		{
			string filename = System.IO.Path.GetFileName(path).ToLowerInvariant();

			for (int i = 0; i < KnownNativeLibraries.Length; i++)
			{
				if (KnownNativeLibraries[i].ToLowerInvariant() == filename)
					return true;
			}
			return false;
		}


		/// <summary>
		/// Asks the user to select which drivers to use.
		/// </summary>
		/// <param name="chooseDisplay"></param>
		/// <param name="chooseAudio"></param>
		/// <param name="chooseInput"></param>
		/// <param name="selectedDisplay"></param>
		/// <param name="selectedAudio"></param>
		/// <param name="selectedInput"></param>
		/// <returns></returns>
		internal static bool UserSelectDrivers(bool chooseDisplay, bool chooseAudio, bool chooseInput,
			DisplayTypeID preferredDisplay, AudioTypeID preferredAudio, InputTypeID preferredInput,
			out DisplayTypeID selectedDisplay, out AudioTypeID selectedAudio, out InputTypeID selectedInput)
		{
			if (mDesktop == null)
			{
				CreateDesktopDriver();

				if (mDesktop == null)
				{
					SelectBestDrivers(chooseDisplay, chooseAudio, chooseInput,
						preferredDisplay, preferredAudio, preferredInput,
						out selectedDisplay, out selectedAudio, out selectedInput);

					return true;
				}
			}

			IUserSetSystems frm = mDesktop.CreateUserSetSystems();

			// set default values.
			selectedDisplay = DisplayTypeID.AutoSelect;
			selectedAudio = AudioTypeID.AutoSelect;
			selectedInput = InputTypeID.AutoSelect;

			foreach (AgateDriverInfo info in displayDrivers)
			{
				frm.AddDisplayType(info);
			}
			foreach (AgateDriverInfo info in audioDrivers)
			{
				frm.AddAudioType(info);
			}
			foreach (AgateDriverInfo info in inputDrivers)
			{
				frm.AddInputType(info);
			}

			frm.SetChoices(chooseDisplay, chooseAudio, chooseInput,
				preferredDisplay, preferredAudio, preferredInput);

			// run the dialog asking user which drivers to use.
			if (frm.RunDialog() == SetSystemsDialogResult.Cancel)
			{
				return false;
			}

			selectedDisplay = frm.DisplayType;
			selectedAudio = frm.AudioType;
			selectedInput = frm.InputType;

			return true;

		}

		private static void SelectBestDrivers(bool chooseDisplay, bool chooseAudio, bool chooseInput,
			DisplayTypeID preferredDisplay, AudioTypeID preferredAudio, InputTypeID preferredInput,
			out DisplayTypeID selectedDisplay, out AudioTypeID selectedAudio, out InputTypeID selectedInput)
		{
			// initial return values if a driver isn't selected.
			selectedDisplay = DisplayTypeID.AutoSelect;
			selectedAudio = AudioTypeID.AutoSelect;
			selectedInput = InputTypeID.AutoSelect;

			if (preferredDisplay != DisplayTypeID.AutoSelect && displayDrivers.Contains(preferredDisplay))
				selectedDisplay = preferredDisplay;
			else if (displayDrivers.Count > 0)
				selectedDisplay = (DisplayTypeID)displayDrivers[0].DriverTypeID;

			if (preferredAudio != AudioTypeID.AutoSelect && audioDrivers.Contains(preferredAudio))
				selectedAudio = preferredAudio;
			else if (audioDrivers.Count > 0)
				selectedAudio = (AudioTypeID)audioDrivers[0].DriverTypeID;

			if (preferredInput != InputTypeID.AutoSelect && inputDrivers.Contains(preferredInput))
				selectedInput = preferredInput;
			else if (inputDrivers.Count > 0)
				selectedInput = (InputTypeID)inputDrivers[0].DriverTypeID;
		}

		private static void CreateDesktopDriver()
		{
			if (desktopDrivers.Count == 0)
				return;

			mDesktop = (IDesktopDriver)CreateDriverInstance(desktopDrivers[0]);
		}

		internal static IDesktopDriver WinForms
		{
			get { return mDesktop; }
		}

		internal static DisplayImpl CreateDisplayDriver(DisplayTypeID displayType)
		{
			if (displayDrivers.Count == 0)
				throw new AgateException("No display drivers registered.");

			AgateDriverInfo info = FindDriverInfo(displayDrivers, (int)displayType);

			if (info == null)
				throw new AgateException(string.Format("Could not find the driver {0}.", displayType));

			return (DisplayImpl)CreateDriverInstance(info);
		}
		internal static AudioImpl CreateAudioDriver(AudioTypeID audioType)
		{
			if (audioDrivers.Count == 0)
				throw new AgateException("No audio drivers registered.");

			AgateDriverInfo info = FindDriverInfo(audioDrivers, (int)audioType);

			if (info == null)
				throw new AgateException(string.Format("Could not find the driver {0}.", audioType));

			return (AudioImpl)CreateDriverInstance(info);
		}
		internal static InputImpl CreateInputDriver(InputTypeID inputType)
		{
			if (inputDrivers.Count == 0)
				throw new AgateException("No audio drivers registered.");

			AgateDriverInfo info = FindDriverInfo(inputDrivers, (int)inputType);

			if (info == null)
				throw new AgateException(string.Format("Could not find the driver {0}.", inputType));

			return (InputImpl)CreateDriverInstance(info);
		}


		private static AgateDriverInfo FindDriverInfo(List<AgateDriverInfo> driverList, int typeID)
		{
			AgateDriverInfo theInfo = null;

			if (driverList.Count == 0)
				return null;

			// autoselect ID's are all zero
			if (typeID == 0)
				return driverList[0];

			foreach (AgateDriverInfo info in driverList)
			{
				if (info.DriverTypeID != typeID)
					continue;

				theInfo = info;
			}
			return theInfo;
		}
		private static AgateDriverInfo FindDriverInfo(IEnumerable<AgateDriverInfo> driverList, string assemblyFullName)
		{
			AgateDriverInfo theInfo = null;

			foreach (AgateDriverInfo info in driverList)
			{
				if (info.AssemblyName != assemblyFullName)
					continue;

				theInfo = info;
			}
			return theInfo;
		}

		private static object CreateDriverInstance(AgateDriverInfo info)
		{
			Assembly ass = Assembly.Load(info.AssemblyName);

			Type driverType = ass.GetType(info.DriverTypeName, false);

			if (driverType == null)
				throw new AgateException(string.Format(
					"Could not find the type {0} in the library {1}.",
					info.DriverTypeName,
					ass.FullName));

			return Activator.CreateInstance(driverType);
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			AgateDriverInfo info = null;

			info = info ?? FindDriverInfo(displayDrivers, args.Name);
			info = info ?? FindDriverInfo(audioDrivers, args.Name);
			info = info ?? FindDriverInfo(inputDrivers, args.Name);
			info = info ?? FindDriverInfo(desktopDrivers, args.Name);

			if (info == null)
				return null;

			return LoadAssemblyLoadFrom(info);
		}

		private static Assembly LoadAssemblyLoadFrom(AgateDriverInfo info)
		{
			Core.ErrorReporting.Report(ErrorLevel.Warning,
				string.Format("Assembly {0} was loaded in the LoadFrom context.  Move it to the application directory to load in the Load context.", info.AssemblyName), null);
			return Assembly.LoadFrom(info.AssemblyFile);
		}

		/// <summary>
		/// Returns a collection with all the DriverInfo&lt;DisplayTypeID&gt; structures for
		/// registered display drivers.
		/// </summary>
		/// <returns></returns>
		public static List<AgateDriverInfo> DisplayDrivers
		{
			get { return displayDrivers; }
		}
		/// <summary>
		/// Returns a collection with all the DriverInfo&lt;AudioTypeID&gt; structures for
		/// registered display drivers.
		/// </summary>
		/// <returns></returns>
		public static List<AgateDriverInfo> AudioDrivers
		{
			get { return audioDrivers; }
		}
		/// <summary>
		/// Returns a collection with all the DriverInfo&lt;InputTypeID&gt; structures for
		/// registered display drivers.
		/// </summary>
		/// <returns></returns>
		public static List<AgateDriverInfo> InputDrivers
		{
			get { return inputDrivers; }
		}


	}
}
