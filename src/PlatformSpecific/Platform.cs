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
