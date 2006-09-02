using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.PlatformSpecific;

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which contains objects commonly used by the entire library.
    /// </summary>
    public static class Core
    {
        private static Platform mPlatform;
        private static bool mAutoPause = false;
        private static bool mIsActive = true;

        static Core()
        {
            mPlatform = Platform.CreatePlatformMethods();
            mPlatform.Initialize();

        }
        /// <summary>
        /// Initializes Core class.
        /// Can be called multiple times.
        /// </summary>
        public static void Initialize() { }

        /// <summary>
        /// Gets platform-specific methods.
        /// </summary>
        public static Platform Platform
        {
            get { return mPlatform; }
        }

        /// <summary>
        /// Gets or sets a bool value which indicates whether or not your
        /// app is the focused window.  This will be automatically set if
        /// you created DisplayWindows by specifying a title and size, but not
        /// if they are attached to a control.
        /// </summary>
        public static bool IsActive
        {
            get { return mIsActive; }
            set { mIsActive = value; }
        }
        /// <summary>
        /// Gets or sets a bool value indicating whether or not Agate
        /// should automatically pause execution when the application
        /// loses focus.
        /// 
        /// The automatic pause will occur during Core.KeepAlive().  This
        /// will prevent the DisplayWindow from being updated at all.  As 
        /// such, this should not be used in production builds if your app
        /// is windowed.  Instead check the IsActive property and respond 
        /// accordingly if your application is windowed..
        /// </summary>
        public static bool AutoPause
        {
            get { return mAutoPause; }
            set { mAutoPause = value; }
        }
        /// <summary>
        /// Plays nice with the OS, by allowing events to be handled.
        /// This also handles user input events associated with the application,
        /// and polls joysticks if needed.
        /// </summary>
        public static void KeepAlive()
        {
            //System.Windows.Forms.Application.DoEvents();

            // Some tests indicate that using the Win32 platform-specific call 
            // to PeekMessage before calling DoEvents is about 1 fps faster,
            // when there are no events to process.  It's not clear whether or
            // not this is worth it when there lots of events being generated
            // (ie lots of mouse move events) but it does seem to speed up for
            // Direct3D.

            if (mPlatform.IsAppIdle == false)
            {
                System.Windows.Forms.Application.DoEvents();

                while (IsActive == false && AutoPause)
                {
                    System.Threading.Thread.Sleep(25);
                    System.Windows.Forms.Application.DoEvents();
                }
            }

            // Poll joystick input, if the time is right.
            Input.PollTimer();


        }

    }
}
