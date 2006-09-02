using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.PlatformSpecific
{
    /// <summary>
    /// This class encapsulates methods which much be implemented for
    /// each platform given.  It provides default implementations that 
    /// are "most conservative."
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// Only sub classes are allowed to initialized this class.
        /// </summary>
        protected Platform()
        {
        }
        /// <summary>
        /// Creates an object which encapsulates platform specific methods.
        /// </summary>
        /// <returns></returns>
        public static Platform CreatePlatformMethods()
        {
            // TODO: It'd be nice to have some nifty #if statements
            // that detect whether we are compiling on MS .NET, 
            // Mono on Windows, Linux, etc. to choose the correct
            // set of methods.

            if (true)
                return new PlatformSpecific.Win32Platform();

            // if no platform specific implementation is available, this
            // class should provide some default routines which allow
            // for correct behavior.
            //else
             //   return new Platform();
        }

        /// <summary>
        /// Returns true if the application is idle, so DoEvents does
        /// not need to be called.
        /// 
        /// If there is no platform-specific implementation, this
        /// just returns false always.
        /// </summary>
        public virtual bool IsAppIdle
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Contains platform-specific methods which are required for the initialization
        /// of the application.
        /// </summary>
        public virtual void Initialize()
        {
        }
        /// <summary>
        /// Returns the current time in milliseconds.
        /// This may be the amount of time since the application began, the system started,
        /// or any other zero-point.
        /// </summary>
        /// <returns></returns>
        public virtual double GetTime()
        {
            return System.Environment.TickCount;
        }
    }
}
