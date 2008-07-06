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
using System.Runtime.InteropServices;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.Drivers;

namespace ERY.AgateLib.PlatformSpecific
{
    /// <summary>
    /// Contains Win32 platform specific methods.
    /// </summary>
    [Obsolete("Methods have been moved to display plug-ins.")]
    public class Win32Platform : Platform
    {
        // taken from DirectX framework.
        //And the declarations for those two native methods members:
        /// <summary>
        /// Window Message structuer
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Message
        {
            public IntPtr hWnd;
            public int msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        /// <summary>
        /// PeekMessage checks to see if there are messages waiting.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="hWnd"></param>
        /// <param name="messageFilterMin"></param>
        /// <param name="messageFilterMax"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        /// <summary>
        /// QueryPerformanceCounter gets the performance counter.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        extern static int QueryPerformanceCounter(out long x);
        /// <summary>
        /// Gets the performance frequency
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        extern static int QueryPerformanceFrequency(out long x);

        /// <summary>
        /// Initializes Win32 specific methods.
        /// </summary>
        public Win32Platform()
        {
            // this just tells windows to make the form and controls look nice on WinXP
            System.Windows.Forms.Application.EnableVisualStyles();
            Console.WriteLine("Created Windows platform driver.");
            System.Diagnostics.Trace.WriteLine("Created Windows platform driver.");

            long freq;

            if (QueryPerformanceFrequency(out freq) != 0)
            {
                performanceFrequency = 1000.0 / freq;

                QueryPerformanceCounter(out start);
            }
        }
        
        /// <summary>
        /// Returns true if there are no messages waiting.
        /// This allows us to avoid an expensive Application.DoEvents() call.
        /// </summary>
        public override bool IsAppIdle
        {
            get
            {
                Message msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
        

        double performanceFrequency = 0;
        long start = 0;

        /// <summary>
        /// Gets the time in milliseconds, using QueryPerformanceCounter
        /// </summary>
        /// <returns></returns>
        public override double GetTime()
        {
            if (performanceFrequency != 0.0)
            {
                long count;

                if (QueryPerformanceCounter(out count) != 0)
                {
                    return (count - start) * performanceFrequency;
                }
            }
            
            return base.GetTime();
        }
    }
}
