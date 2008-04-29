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
using System.Text;

using ERY.AgateLib.Drivers;

namespace ERY.AgateLib.PlatformSpecific 
{
    /// <summary>
    /// This class encapsulates methods which much be implemented for
    /// each platform given.  It provides default implementations that 
    /// are "most conservative."
    /// </summary>
    [Obsolete]
    public class Platform  : IDisposable 
    {
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Only sub classes are allowed to initialized this class.
        /// </summary>
        protected Platform()
        {
            // only do this if we are this class, not a derivative of it.
            if (this.GetType().Equals(typeof(Platform)))
            {
                System.Diagnostics.Debug.WriteLine("Created Platform Independent platform driver.");
            }

            watch.Start();
        }
        /// <summary>
        /// Creates an object which encapsulates platform specific methods.
        /// </summary>
        /// <returns></returns>
        public static Platform CreatePlatformMethods()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                    return new Win32Platform();

                case (PlatformID)128:
                case PlatformID.Unix:
                    //return new X11Platform();

                default:
                    Console.WriteLine("Encountered unsupported platform ID: {0} = {1}",
                        Environment.OSVersion.Platform, (int)Environment.OSVersion.Platform);

                    break;
            }

            return new Platform();

        }

        /// <summary>
        /// Dipsoses of the platform-specific methods.
        /// </summary>
        public virtual void Dispose()
        {
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

        long lastTickCount;

        /// <summary>
        /// Returns the current time in milliseconds.
        /// This may be the amount of time since the application began, the system started,
        /// or any other zero-point.
        /// </summary>
        /// <returns></returns>
        public virtual double GetTime()
        {

            if (watch.ElapsedMilliseconds < lastTickCount)
                throw new Exception(string.Format(
                    "Tick count has gone down!  old: {0}   new: {1}",
                    lastTickCount, watch.ElapsedMilliseconds));

            lastTickCount = watch.ElapsedMilliseconds;
            return lastTickCount;
        }
    }
}
