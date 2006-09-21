//     ``The contents of this file are subject to the Mozilla Public License
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

namespace ERY.AgateLib
{
    public abstract class DriverTypeIDBase
        : IEquatable<int>
    {
        #region --- IEquatable<int> Members ---

        public abstract bool Equals(int other);

        #endregion
    }
    /// <summary>
    /// List of identifiers of known or planned display drivers.
    /// </summary>
    public class DisplayTypeID:DriverTypeIDBase , IEquatable <DisplayTypeID >
    {
        private int value;

        private DisplayTypeID ()
        {}
        private DisplayTypeID (int value)
        {
            this.value = value;
        }

        public override bool Equals(int other)
        {
            return value == other;
        }
        public bool Equals(DisplayTypeID other)
        {
            return Equals(other.value);
        }

        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// display driver for the system.
        /// </summary>
        public static DisplayTypeID AutoSelect 
        {
            get {return new DisplayTypeID ( 0);}
        }
        /// <summary>
        /// The reference driver is implemented using System.Drawing.  This is useful for
        /// debugging the development of a new driver, as it should behave exactly like the
        /// reference driver (but hopefully be much faster).
        /// </summary>
        public static  DisplayTypeID Reference 
        {
            get {return new DisplayTypeID (1);}
        }
        

        /// <summary>
        /// Driver Implementation using Managed DirectX 1.1.
        /// </summary>
        public static DisplayTypeID Direct3D_MDX_1_1
        {
            get
            {
                return new DisplayTypeID(0x100);
            }
        }

        /// <summary>
        /// Driver implementation using Managed DirectX 2.0 beta.  Since Microsoft has discontinued
        /// development on MDX2.0 in favor of the XNA framework);}} this driver is obsolete.
        /// </summary>
        [Obsolete]
        public static DisplayTypeID Direct3D_MDX_2_0_Beta {
            get { return new DisplayTypeID(0x101); } }

        /// <summary>
        /// Driver Implementation using XNA Studio.
        /// </summary>
        public static DisplayTypeID Direct3D_XNA
        { 
            get { return new DisplayTypeID(0x110); } }

        /// <summary>
        /// Driver implementation using OpenGL);}} with WGL for creation of windows and management of
        /// memory.
        /// </summary>
        public static DisplayTypeID WGL
        {    
           get {return new DisplayTypeID ( 0x200);}}

        /// <summary>
        /// Driver implememtation using OpenGL);}} with some platform-independent library for window
        /// creation.
        /// </summary>
        public static DisplayTypeID OpenGL { get { return new DisplayTypeID(0x210); } }

        /// <summary>
        /// Driver implementation using SDL.  SDL.NET does not support many of the basic features
        /// of this library (notably);}} rotation of images) so is not considered an adequate driver
        /// for general purpose use.
        /// </summary>
        [Obsolete]
        public static DisplayTypeID SDL { get { return new DisplayTypeID(0x300); } }

    };

    /// <summary>
    /// List of identifiers of known or planned audio drivers.
    /// </summary>
    public class AudioTypeID : DriverTypeIDBase, IEquatable<AudioTypeID>
    {

        private int value;

        private AudioTypeID ()
        {}
        private AudioTypeID(int value)
        {
            this.value = value;
        }

        public override bool Equals(int other)
        {
            return value == other;
        }
        public bool Equals(AudioTypeID other)
        {
            return Equals(other.value);
        }
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        public static AudioTypeID AutoSelect { get { return new AudioTypeID(0); } }

        /// <summary>
        /// A driver which does nothing.
        /// </summary>
        public static AudioTypeID Silent { get { return new AudioTypeID(-0x100); } }

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        public static AudioTypeID DirectSound { get { return new AudioTypeID(0x100); } }
        /// <summary>
        /// Implementation using XNA Studio
        /// (what will this be called);}} anyway?)
        /// </summary>
        public static AudioTypeID XAct { get { return new AudioTypeID(0x110); } }

        /// <summary>
        /// Implementation using the cross-platform OpenAL library.
        /// </summary>
        public static AudioTypeID OpenAL { get { return new AudioTypeID(0x200); } }
    }

    /// <summary>
    /// List of identifiers of known or planned input drivers.
    /// </summary>
    public class InputTypeID : DriverTypeIDBase, IEquatable<InputTypeID>
    {

        private int value;

        private InputTypeID ()
        {}
        private InputTypeID(int value)
        {
            this.value = value;
        }

        public override bool Equals(int other)
        {
            return value == other;
        }
        public bool Equals(InputTypeID  other)
        {
            return Equals(other.value);
        }

        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        public static InputTypeID AutoSelect { get { return new InputTypeID(0); } }

        /// <summary>
        /// A driver with no joysticks.
        /// </summary>
        public static InputTypeID Silent { get { return new InputTypeID(-0x100); } }

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        public static InputTypeID DirectInput { get { return new InputTypeID(0x100); } }
        /// <summary>
        /// Implementation using the XNA framework.
        /// </summary>
        public static InputTypeID XInput { get { return new InputTypeID(0x110); } }
    }

    /// <summary>
    /// List of identifiers for known or planned platforms to support.
    /// </summary>
    public class PlatformTypeID : DriverTypeIDBase, IEquatable<PlatformTypeID>
    {


        private int value;

        private PlatformTypeID ()
        {}
        private PlatformTypeID(int value)
        {
            this.value = value;
        }

        public override bool Equals(int other)
        {
            return value == other;
        }
        public bool Equals(PlatformTypeID other)
        {
            return Equals(other.value);
        }

        /// <summary>
        /// Indicates that no platform specific methods are available.
        /// Managed equivalents are used where available.
        /// </summary>
        public static PlatformTypeID None { get { return new PlatformTypeID(0); } }
        /// <summary>
        /// Indicates that Windows platform specific methods are provided
        /// by the driver.
        /// </summary>
        public static PlatformTypeID Windows { get { return new PlatformTypeID(1); } }
        /// <summary>
        /// indicates that X-Windows provides windowing functions.
        /// </summary>
        public static PlatformTypeID Linux { get { return new PlatformTypeID(2); } }
    }
    /// <summary>
    /// Class which describes what's required to instantiate a driver.
    /// </summary>
    public abstract class DriverInfo<T>
        // \cond
        where T : DriverTypeIDBase ,IEquatable<T >
    // \endcond
    {
        private Type mMyClass;
        private string mName;
        private int mPriority = 1;
        private T mTypeID;

        /// <summary>
        /// Gets or sets the driver type identifier.
        /// </summary>
        public T TypeID
        {
            get { return mTypeID; }
            set { mTypeID = value; }
        }

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo()
        { }

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass, T type)
        {
            mMyClass = myClass;
            mTypeID = type;
        }

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass, T type, string name)
        {
            mMyClass = myClass;
            mName = name;
            mTypeID = type;
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass, T type, string name, int priority)
        {
            mMyClass = myClass;
            mName = name;
            mPriority = priority;
            mTypeID = type;
        }

        /// <summary>
        /// Type which should be instantiated to use the driver.
        /// This type must derive from DisplayImpl.
        /// </summary>
        public Type MyClass
        {
            get { return mMyClass; }
            set { mMyClass = value; }
        }

        /// <summary>
        /// Name of the class to display to the user if asked to do so.
        /// </summary>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        /// <summary>
        /// Drivers which are registered are sorted by priority, and the 
        /// highest priority is selected and used if an application asks for
        /// DisplayImplementations.AutoSelect.  The reference System.Drawing
        /// implementation has a priority of zero.
        /// </summary>
        public int Priority
        {
            get { return mPriority; }
            set { mPriority = value; }
        }

    }

    /// <summary>
    /// Class which describes what's required to instantiate a display driver.
    /// </summary>
    public class DisplayDriverInfo : DriverInfo<DisplayTypeID>
    {

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo()
        { }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID)
            : base(myDisplayClass, displayTypeID)
        {
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID, string name)
            : base(myDisplayClass, displayTypeID, name)
        {

        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID, string name, int priority)
            : base(myDisplayClass, displayTypeID, name, priority)
        {

        }

    }
    /// <summary>
    /// Class which describes what's required to instantiate a audio driver.
    /// </summary>
    public class AudioDriverInfo : DriverInfo<AudioTypeID>
    {

        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo()
        { }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID audioTypeID)
            : base(myAudioClass, audioTypeID)
        {
        }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID audioTypeID, string name)
            : base(myAudioClass, audioTypeID, name)
        {

        }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID audioTypeID, string name, int priority)
            : base(myAudioClass, audioTypeID, name, priority)
        {

        }

    }
    /// <summary>
    /// Class which describes what's required to instantiate an input driver.
    /// </summary>
    public class InputDriverInfo : DriverInfo<InputTypeID>
    {

        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo()
        { }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID inputTypeID)
            : base(myInputClass, inputTypeID)
        {
        }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID inputTypeID, string name)
            : base(myInputClass, inputTypeID, name)
        {
        }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID inputTypeID, string name, int priority)
            : base(myInputClass, inputTypeID, name, priority)
        {
        }

    }

    /// <summary>
    /// Class which contains a list of drivers and the code required to
    /// instantiate them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DriverInfoList<TBase, T> : List<DriverInfo<T>>
        // \cond
        where T : DriverTypeIDBase, IEquatable<T>
        where TBase : DriverImplBase 
    // \endcond
    {
        T mCreatedTypeID;
            
            /// <summary>
        /// Instantiates the chosen  driver.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        internal TBase CreateDriver(T type)
        {
            if (Count == 0)
                throw new Exception("No  Drivers registered.");

            if (type.Equals (0)) 
            {
                Sort(
                    delegate(DriverInfo<T> a, DriverInfo<T> b)
                    {
                        return a.Priority.CompareTo(b.Priority);
                    }
                    );

                return InstantiateDriver(this[Count - 1]);
            }
            else
            {
                DriverInfo<T> selected = Find(
                    delegate(DriverInfo<T> info)
                    {
                        if (info.TypeID == type)
                            return true;
                        else
                            return false;
                    });

                if (selected != null)
                {
                    return InstantiateDriver(selected);
                }
                else
                {
                    throw new Exception(" Implementation not supported.\n" +
                        "Either a reference has been left out or the implementation needs to have" +
                        "its setup routine called.");
                }
            }
        }

        private TBase InstantiateDriver(DriverInfo<T> driverInfo)
        {
            // get default constructor
            ConstructorInfo constructor = driverInfo.MyClass.GetConstructor(Type.EmptyTypes);

            mCreatedTypeID = driverInfo.TypeID;

            // do the instantiation
            TBase result = constructor.Invoke(new object[] { }) as TBase;
            result.Initialize();

            return result;
        }
    }

    /// <summary>
    /// Static class with which drivers register themselves so that the library can
    /// instantiate them.
    /// </summary>
    public static class Registrar
    {
        private static DriverInfoList<DisplayImpl, DisplayTypeID> mDisplayDrivers = new DriverInfoList<DisplayImpl, DisplayTypeID>();
            private static DriverInfoList<AudioImpl ,AudioTypeID> mAudioDrivers = new DriverInfoList<AudioImpl,AudioTypeID>();
        private static DriverInfoList<InputImpl, InputTypeID> mInputDrivers = new DriverInfoList<InputImpl, InputTypeID>();

        private static DisplayTypeID mCreatedDisplayTypeID = null ;
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
        public static void Initialize()
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

                    if (t.BaseType.Equals(typeof(DisplayImpl)) ||
                        t.BaseType.Equals(typeof(AudioImpl)) ||
                        t.BaseType.Equals(typeof(InputImpl)))
                    {
                        MethodInfo m = t.GetMethod("Register");

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
        /// Returns a collection with all the DisplayDriverInfo structures for
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
        /// Returns a collection with all the AudioDriverInfo structures for
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
        /// Returns a collection with all the InputDriverInfo structures for
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
        /// Registers a display driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the display driver's own DisplayImpl derived class.</param>
        public static void RegisterDisplayDriver(DisplayDriverInfo info)
        {
            mDisplayDrivers.Add(info);
        }
        /// <summary>
        /// Registers an audio driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the audio driver's own AudioImpl derived class.</param>
        public static void RegisterAudioDriver(AudioDriverInfo info)
        {
            mAudioDrivers.Add(info);
        }
        /// <summary>
        /// Registers an input driver as being available.
        /// </summary>
        /// <param name="info">Structure which contains enough information to instantiate
        /// the input driver's own InputImpl derived class.</param>
        public static void RegisterInputDriver(InputDriverInfo info)
        {
            mInputDrivers.Add(info);
        }


        /*
        /// <summary>
        /// Instantiates the chosen audio driver.
        /// </summary>
        /// <param name="audioType"></param>
        /// <returns></returns>
        internal static AudioImpl CreateAudioDriver(AudioTypeID audioType)
        {
            if (audioType == AudioTypeID.AutoSelect)
            {
                mAudioDrivers.Sort(
                    delegate(AudioDriverInfo a, AudioDriverInfo b)
                    {
                        return a.Priority.CompareTo(b.Priority);
                    }
                    );

                return InstantiateAudioDriver(mAudioDrivers[mAudioDrivers.Count - 1]);
            }
            else
            {
                AudioDriverInfo selected = mAudioDrivers.Find(
                    delegate(AudioDriverInfo info)
                    {
                        if (info.AudioTypeID == audioType)
                            return true;
                        else
                            return false;
                    });

                if (selected != null)
                {
                    return InstantiateAudioDriver(selected);
                }
                else
                {
                    throw new Exception("Audio Implementation not supported.\n" +
                        "Either a reference has been left out or the implementation needs to have" +
                        "its setup routine called.");
                }
            }
        }

        private static AudioImpl InstantiateAudioDriver(AudioDriverInfo selected)
        {
            // get default constructor
            ConstructorInfo constructor = selected.MyClass.GetConstructor(new Type[] { });

            mCreatedAudioTypeID = selected.AudioTypeID;

            // do the instantiation
            AudioImpl result = constructor.Invoke(new object[] { }) as AudioImpl;
            result.Initialize();

            return result;
        }

        /// <summary>
        /// Instantiates the chosen input driver.
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        internal static InputImpl CreateInputDriver(InputTypeID inputType)
        {
            if (inputType == InputTypeID.AutoSelect)
            {
                mInputDrivers.Sort(
                    delegate(InputDriverInfo a, InputDriverInfo b)
                    {
                        return a.Priority.CompareTo(b.Priority);
                    }
                    );

                return InstantiateInputDriver(mInputDrivers[mInputDrivers.Count - 1]);
            }
            else
            {
                InputDriverInfo selected = mInputDrivers.Find(
                    delegate(InputDriverInfo info)
                    {
                        if (info.InputTypeID == inputType)
                            return true;
                        else
                            return false;
                    });

                if (selected != null)
                {
                    return InstantiateInputDriver(selected);
                }
                else
                {
                    throw new Exception("Input Implementation not supported.\n" +
                        "Either a reference has been left out or the implementation needs to have" +
                        "its setup routine called.");
                }
            }
        }

        private static InputImpl InstantiateInputDriver(InputDriverInfo selected)
        {
            // get default constructor
            ConstructorInfo constructor = selected.MyClass.GetConstructor(new Type[] { });

            mCreatedInputTypeID = selected.InputTypeID;

            // do the instantiation
            InputImpl result = constructor.Invoke(new object[] { }) as InputImpl;
            result.Initialize();

            return result;
        }
        */

        /// <summary>
        /// Allows the user to select which drivers to use.
        /// Returns true if the user clicked ok, false otherwise.
        /// </summary>
        /// <param name="chooseAudio"></param>
        /// <param name="chooseDisplay"></param>
        /// <param name="chooseInput"></param>
        /// <param name="selectedAudio"></param>
        /// <param name="selectedDisplay"></param>
        /// <param name="selectedInput"></param>
        /// <returns></returns>
        public static bool UserSelectDrivers(bool chooseDisplay, bool chooseAudio, bool chooseInput,
            out DisplayTypeID selectedDisplay, out AudioTypeID selectedAudio, out InputTypeID selectedInput)
        {
            SetSystemsForm frm = new SetSystemsForm(chooseDisplay, chooseAudio, chooseInput);
            DisplayDriverInfo highestDisplay = null;
            AudioDriverInfo highestAudio = null;
            InputDriverInfo highestInput = null;

            // set default values.
            selectedDisplay = DisplayTypeID.AutoSelect;
            selectedAudio = AudioTypeID.AutoSelect;
            selectedInput = InputTypeID.AutoSelect;

            foreach (DisplayDriverInfo info in mDisplayDrivers)
            {
                frm.AddDisplayType(info);

                if (highestDisplay == null || info.Priority > highestDisplay.Priority)
                {
                    highestDisplay = info;
                }
            }
            foreach (AudioDriverInfo info in mAudioDrivers)
            {
                frm.AddAudioType(info);

                if (highestAudio == null || info.Priority > highestAudio.Priority)
                {
                    highestAudio = info;
                }
            }
            foreach (InputDriverInfo info in mInputDrivers)
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