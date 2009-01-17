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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using AgateLib.ImplementationBase;
using AgateLib.PlatformSpecific;
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

        private static DriverInfoList<DisplayImpl, DisplayTypeID> mDisplayDrivers = new DriverInfoList<DisplayImpl, DisplayTypeID>();
        private static DriverInfoList<AudioImpl, AudioTypeID> mAudioDrivers = new DriverInfoList<AudioImpl, AudioTypeID>();
        private static DriverInfoList<InputImpl, InputTypeID> mInputDrivers = new DriverInfoList<InputImpl, InputTypeID>();

		private static DisplayTypeID mCreatedDisplayTypeID;
		private static AudioTypeID mCreatedAudioTypeID;
		private static InputTypeID mCreatedInputTypeID;

        private static bool mIsInitialized = false;

        private static IWinForms mDesktop;

        private static readonly string[] KnownNativeLibraries = new string[]
        {
            "SDL.dll",
        };

        static Registrar()
        {
        }
        /// <summary>
        /// Searches through FileManager.AssemblyPath for all *.dll files.  These files
        /// are loaded and searched for classes which derive from DisplayImpl, AudioImpl, etc.
        /// 
        /// </summary>
        public static void Initialize()
        {
            if (mIsInitialized)
                return;

            AppDomain sandbox = AppDomain.CreateDomain("AgateSandBox");

            AgateSandBoxLoader loader = (AgateSandBoxLoader)
                sandbox.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                typeof(AgateSandBoxLoader).FullName);

            IEnumerable<string> files = AgateFileProvider.AssemblyProvider.GetAllFiles("*.dll");

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
                            Core.ReportError(ErrorLevel.Warning, string.Format(
                                "Could not interpret DriverType returned by type {0} in assembly {1}.",
                                info.DriverTypeName, info.AssemblyFile), null);

                            break;
                    }
                }
            }

            AppDomain.Unload(sandbox);

            WriteCurrentLoadedAssemblies();

            SortDriverInfo(displayDrivers);
            SortDriverInfo(audioDrivers);
            SortDriverInfo(inputDrivers);
            SortDriverInfo(desktopDrivers);
           
        }

        private static void SortDriverInfo(List<AgateDriverInfo> driverList)
        {
            driverList.Sort(delegate(AgateDriverInfo a, AgateDriverInfo b)
            {
                return a.Priority.CompareTo(b.Priority);
            });
        }

        internal static void WriteCurrentLoadedAssemblies()
        {
            WriteCurrentLoadedAssemblies(AppDomain.CurrentDomain);
        }
        internal static void WriteCurrentLoadedAssemblies(AppDomain domain)
        {
            Assembly[] assemblies = domain.GetAssemblies();

            Debug.Print("Currently loaded assemblies in domain {0}", domain.FriendlyName);

            foreach (Assembly assembly in assemblies)
            {
                Debug.Print("Loaded:  {0}.", assembly.ManifestModule.Name);
            }
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
                System.IO.Path.GetFileName(file).ToLower().Contains("agatemdx.dll"))
            {
                Core.ReportError(ErrorLevel.Comment,
                    "DirectX not supported on Linux.  Remove "
                    + System.IO.Path.GetFileName(file) +
                    " to eliminate this message.", null);

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
        /// Returns a collection with all the DriverInfo&lt;DisplayTypeID&gt; structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static DriverInfoList<DisplayImpl, DisplayTypeID> DisplayDriverInfo
        {
            get
            {
                return mDisplayDrivers;
            }
        }

        /// <summary>
        /// Returns a collection with all the DriverInfo&lt;AudioTypeID&gt; structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static DriverInfoList<AudioImpl, AudioTypeID> AudioDriverInfo
        {
            get
            {
                return mAudioDrivers;
            }
        }

        /// <summary>
        /// Returns a collection with all the DriverInfo&lt;InputTypeID&gt; structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static DriverInfoList<InputImpl, InputTypeID> InputDriverInfo
        {
            get
            {
                return mInputDrivers;
            }
        }


        /// <summary>
        /// Registers a display driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the display driver's own DisplayImpl derived class.</param>
        [Obsolete]
        public static void RegisterDisplayDriver(DriverInfo<DisplayTypeID> info)
        {
            mDisplayDrivers.Add(info);
        }
        /// <summary>
        /// Registers an audio driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the audio driver's own AudioImpl derived class.</param>
        [Obsolete]
        public static void RegisterAudioDriver(DriverInfo<AudioTypeID> info)
        {
            mAudioDrivers.Add(info);
        }
        /// <summary>
        /// Registers an input driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the input driver's own InputImpl derived class.</param>
        [Obsolete]
        public static void RegisterInputDriver(DriverInfo<InputTypeID> info)
        {
            mInputDrivers.Add(info);
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
        public static bool UserSelectDrivers(bool chooseDisplay, bool chooseAudio, bool chooseInput,
            out DisplayTypeID selectedDisplay, out AudioTypeID selectedAudio, out InputTypeID selectedInput)
        {
            if (mDesktop == null)
            {
                CreateDesktopDriver();
                if (mDesktop == null)
                    SelectBestDrivers(chooseDisplay, chooseAudio, chooseInput,
                        out selectedDisplay, out selectedAudio, out selectedInput);
            }

            IUserSetSystems frm = mDesktop.CreateUserSetSystems();
            
            frm.SetChoices(chooseDisplay, chooseAudio, chooseInput);

            DriverInfo<DisplayTypeID> highestDisplay = null;
            DriverInfo<AudioTypeID> highestAudio = null;
            DriverInfo<InputTypeID> highestInput = null;

            // set default values.
            selectedDisplay = DisplayTypeID.AutoSelect;
            selectedAudio = AudioTypeID.AutoSelect;
            selectedInput = InputTypeID.AutoSelect;

            foreach (DriverInfo<DisplayTypeID> info in mDisplayDrivers)
            {
                frm.AddDisplayType(info);

                if (highestDisplay == null || info.Priority > highestDisplay.Priority)
                {
                    highestDisplay = info;
                }
            }
            foreach (DriverInfo<AudioTypeID> info in mAudioDrivers)
            {
                frm.AddAudioType(info);

                if (highestAudio == null || info.Priority > highestAudio.Priority)
                {
                    highestAudio = info;
                }
            }
            foreach (DriverInfo<InputTypeID> info in mInputDrivers)
            {
                frm.AddInputType(info);

                if (highestInput == null || info.Priority > highestInput.Priority)
                {
                    highestInput = info;
                }
            }

            frm.SetDefaultDisplay(highestDisplay);
            frm.SetDefaultAudio(highestAudio);
            frm.SetDefaultInput(highestInput);

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
            out DisplayTypeID selectedDisplay, out AudioTypeID selectedAudio, out InputTypeID selectedInput)
        {
            selectedDisplay = DisplayTypeID.AutoSelect;
            selectedAudio = AudioTypeID.AutoSelect;
            selectedInput = InputTypeID.AutoSelect;

            if (displayDrivers.Count > 0)
                selectedDisplay = (DisplayTypeID)displayDrivers[0].DriverTypeID;
            if (audioDrivers.Count > 0)
                selectedAudio = (AudioTypeID)audioDrivers[0].DriverTypeID;
            if (inputDrivers.Count > 0)
                selectedInput = (InputTypeID)inputDrivers[0].DriverTypeID;
        }

        private static void CreateDesktopDriver()
        {
            if (desktopDrivers.Count == 0)
                return;

            mDesktop = (IWinForms)CreateType(desktopDrivers[0]);
        }

        private static object CreateType(AgateDriverInfo info)
        {
            Assembly ass = Assembly.Load(info.AssemblyName);

            return ass.CreateInstance(info.DriverTypeName);
        }

        /// <summary>
        /// Returns the identifier for the DisplayImpl which was actually
        /// created.
        /// </summary>
        public static DisplayTypeID CreatedDisplayTypeID
        {
            get { return mCreatedDisplayTypeID; }
        }
        /// <summary>
        /// Returns the identifier for the AudioImpl which was actually
        /// created.
        /// </summary>
        public static AudioTypeID CreatedAudioTypeID
        {
            get { return mCreatedAudioTypeID; }
        }
        /// <summary>
        /// Returns the identifier for the InputImpl which was actually
        /// created.
        /// </summary>
        public static InputTypeID CreatedInputTypeID
        {
            get { return mCreatedInputTypeID; }
        }

        internal static IWinForms WinForms
        {
            get { return mDesktop; }
        }
        // This is kept, because a lot of the app domain stuff is not supported
        // on the compact framework or on XNA framework.
        private static void OldInitialize()
        {
            NullSoundImpl.Register();
            NullInputImpl.Register();

            IEnumerable<string> files = AgateFileProvider.AssemblyProvider.GetAllFiles("*.dll");

            foreach (string file in files)
            {
                Assembly ass;
                Type[] types;

                if (ShouldSkipLibrary(file))
                    continue;

                try
                {
                    ass = Assembly.LoadFrom(file);

                    // the AgateLib.dll file may be in the same directory, make sure to skip it.
                    if (ass == Assembly.GetExecutingAssembly())
                        continue;

                    types = ass.GetTypes();


                    foreach (Type t in types)
                    {
                        if (t.GetInterfaces().Length == 0 && t.BaseType == typeof(Object))
                            continue;

                        if (typeof(DriverImplBase).IsAssignableFrom(t))
                        {
                            MethodInfo m = t.GetMethod("Register", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                            if (m == null)
                            {
                                throw new Exception("The assembly " + System.IO.Path.GetFileName(file) +
                                    " has class " + t + " which derives from " + t.BaseType.Name + ", but does not " +
                                    "have a static Register() method!");
                            }

                            m.Invoke(null, new object[] { });
                        }
                        else if (typeof(IWinForms).IsAssignableFrom(t))
                        {
                            mDesktop = (IWinForms)Activator.CreateInstance(t);
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    Core.ReportError(ErrorLevel.Warning,
                        "An error occured while loading assembly " + file + ".  " +
                        "The following " + e.LoaderExceptions.Length + " warnings are the LoaderExceptions.",
                        e);

                    for (int i = 0; i < e.LoaderExceptions.Length; i++)
                    {
                        Core.ReportError(ErrorLevel.Warning,
                            "LoaderException " + (i + 1).ToString(),
                            e.LoaderExceptions[i]);
                    }

                    continue;
                }
                catch (Exception e)
                {
                    Core.ReportError(ErrorLevel.Warning, "An error occured while loading assembly " + file + ".", e);

                    continue;
                }
            }

            mIsInitialized = true;
        }

    }
}
