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
using System.Runtime.InteropServices;

namespace AgateLib.PlatformSpecific
{
    /// <summary>
    /// Contains X11 platform specific methods.
    /// </summary>
    [Obsolete("Relevant methods moved to display plugin.")]
    public class X11Platform : Platform 
    {
        IntPtr display;
        private const string dllname = "libX11";

        [DllImport(dllname)]
        extern static int XPending(IntPtr display);

        /// <summary>
        /// Constructs the X11 platform methods.
        /// </summary>
        public X11Platform()
        {
            Console.WriteLine("Created X11 platform driver.");
            System.Diagnostics.Trace.WriteLine("Created X11 platform driver.");

            GetXConnection();
        }

        private void GetXConnection()
        {
            Type xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms", true);

            display = (IntPtr)xplatui.GetField("DisplayHandle",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.NonPublic).GetValue(null);
        }

        private bool XConnectionMessage = false;

        /// <summary>
        /// Returns whether or not the application is idle; that is, if there are no messages
        /// waiting to be processed.
        /// </summary>
        public override bool IsAppIdle
        {
            get
            {
                if (display.Equals(IntPtr.Zero))
                {
                    GetXConnection();

                    if (display.Equals(IntPtr.Zero) && XConnectionMessage == false)
                    {
                        Core.ReportError(ErrorLevel.Warning, "Could not get connection to the X Server.  Reverting to " +
                            "platform independent methods.", null);

                        XConnectionMessage = true;
                        return base.IsAppIdle;
                    }
                    else if (XConnectionMessage)
                    {
                        Core.ReportError(ErrorLevel.Comment, "Connection to X server is now available.  Using platform " +
                            "specific idle checking method.", null);

                        XConnectionMessage = false;
                    }
                }

                return XPending(display) == 0;
            }
        }
    }
}
