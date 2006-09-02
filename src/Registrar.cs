using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// List of identifiers of known or planned display drivers.
    /// </summary>
    public enum DisplayTypeID
    {
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// display driver for the system.
        /// </summary>
        AutoSelect = 0,
        /// <summary>
        /// The reference driver is implemented using System.Drawing.  This is useful for
        /// debugging the development of a new driver, as it should behave exactly like the
        /// reference driver (but hopefully be much faster).
        /// </summary>
        Reference = 1,

        /// <summary>
        /// Driver Implementation using Managed DirectX 1.1.
        /// </summary>
        Direct3D_MDX_1_1 = 0x100,

        /// <summary>
        /// Driver implementation using Managed DirectX 2.0 beta.  Since Microsoft has discontinued
        /// development on MDX2.0 in favor of the XNA framework, this driver is obsolete.
        /// </summary>
        [Obsolete]
        Direct3D_MDX_2_0_Beta = 0x101,

        /// <summary>
        /// Driver Implementation using XNA Studio.
        /// </summary>
        Direct3D_XNA = 0x110,

        /// <summary>
        /// Driver implementation using OpenGL, with WGL for creation of windows and management of
        /// memory.
        /// </summary>
        WGL = 0x200,

        /// <summary>
        /// Driver implememtation using OpenGL, with some platform-independent library for window
        /// creation.
        /// </summary>
        OpenGL = 0x210,

        /// <summary>
        /// Driver implementation using SDL.  SDL.NET does not support many of the basic features
        /// of this library (notably, rotation of images) so is not considered an adequate driver
        /// for general purpose use.
        /// </summary>
        [Obsolete]
        SDL = 0x300,

    };

    /// <summary>
    /// List of identifiers of known or planned audio drivers.
    /// </summary>
    public enum AudioTypeID
    {
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        AutoSelect = 0,

        /// <summary>
        /// A driver which does nothing.
        /// </summary>
        Silent = -0x100,

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        DirectSound = 0x100,
        /// <summary>
        /// Implementation using XNA Studio
        /// (what will this be called, anyway?)
        /// </summary>
        XAct = 0x110,

        /// <summary>
        /// Implementation using the cross-platform OpenAL library.
        /// </summary>
        OpenAL = 0x200,
    }

    /// <summary>
    /// List of identifiers of known or planned input drivers.
    /// </summary>
    public enum InputTypeID
    {
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        AutoSelect = 0,

        /// <summary>
        /// A driver with no joysticks.
        /// </summary>
        Silent = -0x100,

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        DirectInput = 0x100,
        /// <summary>
        /// Implementation using the XNA framework.
        /// </summary>
        XInput = 0x110,
    }

    /// <summary>
    /// Class which describes what's required to instantiate a driver.
    /// </summary>
    public abstract class DriverInfo
    {
        private Type mMyClass;
        private string mName;
        private int mPriority = 1;

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo()
        { }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass)
        {
            mMyClass = myClass;
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass, string name)
        {
            mMyClass = myClass;
            mName = name;
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DriverInfo(Type myClass, string name, int priority)
        {
            mMyClass = myClass;
            mName = name;
            mPriority = priority;
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
    public class DisplayDriverInfo : DriverInfo
    {
        private DisplayTypeID mDisplayTypeID;

        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo()
        { }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID)
            : base(myDisplayClass)
        {
            mDisplayTypeID = displayTypeID;
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID, string name)
            : base(myDisplayClass, name)
        {
            mDisplayTypeID = displayTypeID;
        }
        /// <summary>
        /// Constructs a DisplayDriverInfo object.
        /// </summary>
        public DisplayDriverInfo(Type myDisplayClass, DisplayTypeID displayTypeID, string name, int priority)
            : base(myDisplayClass, name, priority)
        {
            mDisplayTypeID = displayTypeID;
        }

        /// <summary>
        /// The unique identifier for the display implementation.
        /// </summary>
        public DisplayTypeID DisplayTypeID
        {
            get { return mDisplayTypeID; }
            set { mDisplayTypeID = value; }
        }
    }
    /// <summary>
    /// Class which describes what's required to instantiate a audio driver.
    /// </summary>
    public class AudioDriverInfo : DriverInfo
    {
        private AudioTypeID mAudioTypeID;

        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo()
        { }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID AudioTypeID)
            : base(myAudioClass)
        {
            mAudioTypeID = AudioTypeID;
        }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID AudioTypeID, string name)
            : base(myAudioClass, name)
        {
            mAudioTypeID = AudioTypeID;
        }
        /// <summary>
        /// Constructs a AudioDriverInfo object.
        /// </summary>
        public AudioDriverInfo(Type myAudioClass, AudioTypeID AudioTypeID, string name, int priority)
            : base(myAudioClass, name, priority)
        {
            mAudioTypeID = AudioTypeID;
        }

        /// <summary>
        /// The unique identifier for the Audio implementation.
        /// </summary>
        public AudioTypeID AudioTypeID
        {
            get { return mAudioTypeID; }
            set { mAudioTypeID = value; }
        }
    }
    /// <summary>
    /// Class which describes what's required to instantiate an input driver.
    /// </summary>
    public class InputDriverInfo : DriverInfo
    {
        private InputTypeID mInputTypeID;

        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo()
        { }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID InputTypeID)
            : base(myInputClass)
        {
            mInputTypeID = InputTypeID;
        }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID InputTypeID, string name)
            : base(myInputClass, name)
        {
            mInputTypeID = InputTypeID;
        }
        /// <summary>
        /// Constructs a InputDriverInfo object.
        /// </summary>
        public InputDriverInfo(Type myInputClass, InputTypeID InputTypeID, string name, int priority)
            : base(myInputClass, name, priority)
        {
            mInputTypeID = InputTypeID;
        }

        /// <summary>
        /// The unique identifier for the Input implementation.
        /// </summary>
        public InputTypeID InputTypeID
        {
            get { return mInputTypeID; }
            set { mInputTypeID = value; }
        }
    }

    /// <summary>
    /// Static class with which drivers register themselves so that the library can
    /// instantiate them.
    /// </summary>
    public static class Registrar
    {
        private static List<DisplayDriverInfo> mDisplayDrivers = new List<DisplayDriverInfo>();
        private static List<AudioDriverInfo> mAudioDrivers = new List<AudioDriverInfo>();
        private static List<InputDriverInfo> mInputDrivers = new List<InputDriverInfo>();

        private static DisplayTypeID mCreatedDisplayTypeID = 0;
        private static AudioTypeID mCreatedAudioTypeID = 0;
        private static InputTypeID mCreatedInputTypeID = 0;

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
                Assembly ass = Assembly.LoadFrom(file);

                // the library DLL should be in the same directory, make sure to skip it.
                if (ass == Assembly.GetExecutingAssembly())
                    continue;

                Type[] types = ass.GetTypes();

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
        public static ICollection<DisplayDriverInfo> GetAllDisplayDriverInfo()
        {
            return mDisplayDrivers;
        }

        /// <summary>
        /// Returns a collection with all the AudioDriverInfo structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
        public static ICollection<AudioDriverInfo> GetAllAudioDriverInfo()
        {
            return mAudioDrivers;
        }

        /// <summary>
        /// Returns a collection with all the InputDriverInfo structures for
        /// registered display drivers.
        /// </summary>
        /// <returns></returns>
        public static ICollection<InputDriverInfo> GetAllInputDriverInfo()
        {
            return mInputDrivers;
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

        /// <summary>
        /// Instantiates the chosen display driver.
        /// </summary>
        /// <param name="displayType"></param>
        /// <returns></returns>
        internal static DisplayImpl CreateDisplayDriver(DisplayTypeID displayType)
        {
            if (mDisplayDrivers.Count == 0)
                throw new Exception("No Display Drivers registered.");

            if (displayType == DisplayTypeID.AutoSelect)
            {
                mDisplayDrivers.Sort(
                    delegate(DisplayDriverInfo a, DisplayDriverInfo b)
                    {
                        return a.Priority.CompareTo(b.Priority);
                    }
                    );

                return InstantiateDisplayDriver(mDisplayDrivers[mDisplayDrivers.Count - 1]);
            }
            else
            {
                DisplayDriverInfo selected = mDisplayDrivers.Find(
                    delegate(DisplayDriverInfo info)
                    {
                        if (info.DisplayTypeID == displayType)
                            return true;
                        else
                            return false;
                    });

                if (selected != null)
                {
                    return InstantiateDisplayDriver(selected);
                }
                else
                {
                    throw new Exception("Display Implementation not supported.\n" +
                        "Either a reference has been left out or the implementation needs to have" +
                        "its setup routine called.");
                }
            }
        }

        private static DisplayImpl InstantiateDisplayDriver(DisplayDriverInfo displayDriverInfo)
        {
            // get default constructor
            ConstructorInfo constructor = displayDriverInfo.MyClass.GetConstructor(new Type[] { });

            mCreatedDisplayTypeID = displayDriverInfo.DisplayTypeID;

            // do the instantiation
            DisplayImpl result = constructor.Invoke(new object[] { }) as DisplayImpl;
            result.Initialize();

            return result;
        }

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