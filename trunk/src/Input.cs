using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Static class which contains basic functions for handling non-keyboard and mouse
    /// input.
    /// </summary>
    public static class Input
    {
        private static InputImpl impl;
        private static List<Joystick> mJoysticks = new List<Joystick>();
        private static int mMinJoysticks = 0;
        private static double mPollInterval = 50;
        private static double mLastPoll;
        private static int mPollCount = 0;

        /// <summary>
        /// Gets the object which handles all of the actual calls to Input functions.
        /// </summary>
        public static InputImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Initializes the input system by instantiating the driver with the given
        /// InputTypeID.  The input driver must be registered with the Registrar
        /// class.
        /// </summary>
        /// <param name="inputType"></param>
        public static void Initialize(InputTypeID inputType)
        {
            Core.Initialize();

            impl = Registrar.CreateInputDriver(inputType);

            InitializeJoysticks();
        }

        /// <summary>
        /// Disposes of the input driver.
        /// </summary>
        public static void Dispose()
        {
            OnDispose();

            if (impl != null)
            {
                impl.Dispose();
                impl = null;
            }
        }
        private static void OnDispose()
        {
            if (DisposeInput != null)
                DisposeInput();
        }

        /// <summary>
        /// Delegate for DisposeInput event.
        /// </summary>
        public delegate void InputSystemDelegate();

        /// <summary>
        /// Event raised when Input.Dispose() is called.
        /// </summary>
        public static event InputSystemDelegate DisposeInput;

        /// <summary>
        /// Initializes all joysticks.
        /// </summary>
        public static void InitializeJoysticks()
        {
            mJoysticks.Clear();

            IEnumerable<JoystickImpl> joys = impl.CreateJoysticks();

            foreach (JoystickImpl i in joys)
            {
                mJoysticks.Add(new Joystick(i));
            }

            while (mJoysticks.Count < mMinJoysticks)
            {
                mJoysticks.Add(new Joystick(new NullJoystickImpl()));
            }
        }
        /// <summary>
        /// Counts the number of joysticks attached to the system.
        /// </summary>
        public static int JoystickCount
        {
            get 
            {
                return mJoysticks.Count;
            }
        }
        /// <summary>
        /// Returns a list of joysticks which can be iterated through,
        /// or accessed like an array.
        /// </summary>
        public static IList<Joystick> Joysticks
        {
            get { return mJoysticks; }
        }

        /// <summary>
        /// Minimum number of joysticks for the system to have.
        /// If there aren't enough physical joysticks attached to
        /// the system, NullJoysticks will be created to emulate
        /// a joystick which does nothing. (this is to avoid
        /// NullReferenceExceptions).
        /// </summary>
        public static int MinJoysticks
        {
            get { return mMinJoysticks; }
            set
            {
                mMinJoysticks = value;

                InitializeJoysticks();
            }
        }

        /// <summary>
        /// Polls all joysticks for input now.
        /// </summary>
        public static void PollJoysticks()
        {
            if (mPollCount % 10 == 0)
            {
                if (impl.CountJoysticks() != mJoysticks.Count)
                    InitializeJoysticks();
            }

            foreach (Joystick j in mJoysticks)
                j.Poll();

            mLastPoll = Core.Platform.GetTime();

            mPollCount++;

            if (JoystickPollEvent != null)
                JoystickPollEvent();
        }

        /// <summary>
        /// Gets or sets the interval in milliseconds for the time between
        /// automatic joystick polls.
        /// </summary>
        public static double PollInterval
        {
            get { return mPollInterval; }
            set { mPollInterval = value; }
        }
        /// <summary>
        /// Gets or sets the number of times per second that joysticks should
        /// be polled.
        /// </summary>
        public static double PollFrequency
        {
            get
            {
                return 1000.0 / mPollInterval; 
            }
            set
            {
                mPollInterval = 1000.0 / value; 
            }
        }

        internal static void PollTimer()
        {
            if (impl == null)
                return;
            if (mLastPoll + mPollInterval > Core.Platform.GetTime())
                return;

            PollJoysticks();
        }

        internal delegate void JoystickPollDelegate ();
        internal static event JoystickPollDelegate JoystickPollEvent;
    }
}
