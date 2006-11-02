using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ERY.AgateLib.PlatformSpecific
{
    /// <summary>
    /// Contains X11 platform specific methods.
    /// </summary>
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

                    if (display.Equals(IntPtr.Zero))
                    {
                        Core.ReportError(new Exception(
                            "Could not get connection to the X Server.  Reverting to " +
                            "platform independent methods."), ErrorLevel.Warning);

                        XConnectionMessage = true;
                        return base.IsAppIdle;
                    }
                    else if (XConnectionMessage)
                    {
                        Core.ReportError(new Exception(
                            "Connection to X server is now available.  Using platform " +
                            "specific idle checking method."), ErrorLevel.Comment);

                        XConnectionMessage = false;
                    }
                }

                return XPending(display) == 0;
            }
        }
    }
}
