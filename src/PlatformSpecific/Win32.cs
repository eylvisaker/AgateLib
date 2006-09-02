using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ERY.AgateLib.PlatformSpecific
{
    /// <summary>
    /// Contains Win32 platform specific methods.
    /// </summary>
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
            // public WindowMessage msg;
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
        private static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        /// <summary>
        /// QueryPerformanceCounter gets the performance counter.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(out long x);
        /// <summary>
        /// Gets the performance frequency
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceFrequency(out long x);

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
        /// <summary>
        /// Initializes Win32 specific methods.
        /// </summary>
        public override void Initialize()
        {
            // this just tells windows to make the form and controls look nice on WinXP
            System.Windows.Forms.Application.EnableVisualStyles();

            long freq;

            if (QueryPerformanceFrequency(out freq) != 0)
            {
                performanceFrequency = 1000.0 / freq;

                QueryPerformanceCounter(out start);
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
