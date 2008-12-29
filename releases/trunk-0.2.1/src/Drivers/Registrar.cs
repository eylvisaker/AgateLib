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
using System.Reflection;
using System.Text;
using ERY.AgateLib.ImplBase;
using ERY.AgateLib.PlatformSpecific;

namespace ERY.AgateLib.Drivers
{


    /// <summary>
    /// Static class with which drivers register themselves so that the library can
    /// instantiate them.
    /// </summary>
    public static class Registrar
    {
        private static DriverInfoList<DisplayImpl, DisplayTypeID> mDisplayDrivers = new DriverInfoList<DisplayImpl, DisplayTypeID>();
        private static DriverInfoList<AudioImpl, AudioTypeID> mAudioDrivers = new DriverInfoList<AudioImpl, AudioTypeID>();
        private static DriverInfoList<InputImpl, InputTypeID> mInputDrivers = new DriverInfoList<InputImpl, InputTypeID>();
        private static DriverInfoList<Platform, PlatformTypeID> mPlatformDrivers = new DriverInfoList<Platform, PlatformTypeID>();

        private static DisplayTypeID mCreatedDisplayTypeID = null;
        private static AudioTypeID mCreatedAudioTypeID = null;
        private static InputTypeID mCreatedInputTypeID = null;

        private static bool mIsInitialized = false;

        static Registrar()
        {
        }
        /// <summary>
        /// Searches through FileManager.AssemblyPath for all *.dll files.  These files
        /// are loaded and searched for classes which derive from DisplayImpl, AudioImpl, etc.
        /// 
        /// </summary>
        internal static void Initialize()
        {
            if (mIsInitialized)
                return;

            NullSoundImpl.Register();
            NullInputImpl.Register();

            FileManager.Initialize();

            IEnumerable<string> files = FileManager.AssemblyPath.GetAllFiles("*.dll");

            foreach (string file in files)
            {
                Assembly ass;
                Type[] types;

                try
                {
                    ass = Assembly.LoadFrom(file);

                    // the library DLL should be in the same directory, make sure to skip it.
                    if (ass == Assembly.GetExecutingAssembly())
                        continue;

                    types = ass.GetTypes();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading assembly " + file);
                    System.Diagnostics.Debug.WriteLine(e.Message);

                    continue;
                }

                foreach (Type t in types)
                {
                    if (t.BaseType == null || t.BaseType == typeof(Object))
                        continue;

                    if (typeof(DriverImplBase).IsAssignableFrom(t))
                    {
                        MethodInfo m = t.GetMethod("Register", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                        if (m == null)
                        {
                            throw new Exception("Error:  The assembly " + System.IO.Path.GetFileName(file) +
                                " has class " + t + " which derives from " + t.BaseType.Name + ", but does not " +
                                "have a static Register() method!");
                        }

                        m.Invoke(null, new object[] { });
                    }
                }
            }

            mIsInitialized = true;
        }

        /// <summary>
        /// Returns a collection with all the DriverInfo&lt;DisplayTypeID&gt; structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
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
        public static DriverInfoList<InputImpl, InputTypeID> InputDriverInfo
        {
            get
            {
                return mInputDrivers;
            }
        }

        /// <summary>
        /// Returns a collection with all the DriverInfo&lt;PlatformTypeID&gt; structures
        /// for registered platform drivers.
        /// </summary>
        public static DriverInfoList<Platform, PlatformTypeID> PlatformDriverInfo
        {
            get
            {
                return mPlatformDrivers;
            }
        }

        /// <summary>
        /// Registers a display driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the display driver's own DisplayImpl derived class.</param>
        public static void RegisterDisplayDriver(DriverInfo<DisplayTypeID> info)
        {
            mDisplayDrivers.Add(info);
        }
        /// <summary>
        /// Registers an audio driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the audio driver's own AudioImpl derived class.</param>
        public static void RegisterAudioDriver(DriverInfo<AudioTypeID> info)
        {
            mAudioDrivers.Add(info);
        }
        /// <summary>
        /// Registers an input driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the input driver's own InputImpl derived class.</param>
        public static void RegisterInputDriver(DriverInfo<InputTypeID> info)
        {
            mInputDrivers.Add(info);
        }

        /// <summary>
        /// Registers a platform driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the platform specific method class.</param>
        public static void RegisterPlatformDriver(DriverInfo<PlatformTypeID> info)
        {
            mPlatformDrivers.Add(info);
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
            SetSystemsForm frm = new SetSystemsForm(chooseDisplay, chooseAudio, chooseInput);
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

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return false;
            }

            selectedDisplay = frm.DisplayType;
            selectedAudio = frm.AudioType;
            selectedInput = frm.InputType;

            return true;

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

    }
}