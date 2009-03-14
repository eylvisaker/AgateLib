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
using System.IO;
using System.Text;
using ERY.AgateLib.PlatformSpecific;

namespace ERY.AgateLib
{
    /// <summary>
    /// Used by AgateLib.Core class's error reporting functions
    /// to indicate how severe an error is.
    /// </summary>
    public enum ErrorLevel
    {

        /// <summary>
        /// Indicates an message is just a comment, and safe to ignore.
        /// </summary>
        Comment ,
        /// <summary>
        /// Indicates that the error message is not severe, and the program may
        /// continue.  However, unexpected behavior may occur due to the result of
        /// this error.
        /// </summary>
        Warning ,
        /// <summary>
        /// Indicates that the error condition is too severe and the program 
        /// may not continue.
        /// </summary>
        Fatal,


        /// <summary>
        /// Indicates the error condition indicates some assumption
        /// has not held that should have.  This should only be used
        /// if the condition is caused by a bug in the code.
        /// </summary>
        Bug,
    }

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

                    if (Display.CurrentWindow == null)
                        break;
                    else if (Display.CurrentWindow.Closed)
                        break;

                }
            }

            // Poll joystick input, if the time is right.
            Input.PollTimer();


        }

        #region --- Error Reporting ---

        private static string mErrorFile = "errorlog.txt";
        private static bool mAutoStackTrace = false;
        private static bool mWroteHeader = false;

        /// <summary>
        /// Gets or sets the file name to which errors are recorded.  Defaults
        /// to "errorlog.txt"
        /// </summary>
        public static string ErrorFile
        {
            get { return Core.mErrorFile; }
            set { Core.mErrorFile = value; }
        }

        /// <summary>
        /// Gets or sets whether or not a stack trace is automatically used.
        /// </summary>
        /// <example>
        /// You may find it useful to turn this on during a debug build, and
        /// then turn if off when building the release version.  The following
        /// code accomplishes that.
        /// <code>
        /// #if _DEBUG
        ///     ERY.AgateLib.Core.AutoStackTrace = true;
        /// #endif
        /// </code>
        /// </example>
        public static bool AutoStackTrace
        {
            get { return Core.mAutoStackTrace; }
            set { Core.mAutoStackTrace = value; }
        }

        /// <summary>
        /// Saves an error message to the ErrorFile.
        /// Outputs a stack trace and shows a dialog box if the ErrorLevel is Bug or Fatal.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        public static void ReportError(Exception e, ErrorLevel level)
        {
            switch (level)
            {
                case ErrorLevel.Bug:
                case ErrorLevel.Fatal:
                    ReportError(e, level, true, true);
                    break;

                case ErrorLevel.Comment:
                case ErrorLevel.Warning:
                    ReportError(e, level, AutoStackTrace, false);
                    break;
            }
        }
        /// <summary>
        /// Saves an error message to the ErrorFile.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <param name="printStackTrace">Bool value indicating whether or not 
        /// a stack trace should be written out.  </param>
        /// <param name="showDialog">Bool value indicating whether or not a 
        /// message box should pop up with an OK button, informing the user about the 
        /// error.  If false, the error is silently written to the ErrorFile.</param>
        public static void ReportError(Exception e, ErrorLevel level, bool printStackTrace, bool showDialog)
        {
            StreamWriter writer = OpenErrorFile();

            writer.Write(LevelText(level) + ": ");

            writer.WriteLine(e.Message);

            if (printStackTrace)
                writer.WriteLine(e.StackTrace);

            writer.WriteLine("");

            writer.Flush();
            writer.Dispose();

            if (showDialog)
            {
                DialogReport(e, level);
            }
        }

        private static StreamWriter OpenErrorFile()
        {
            if (mWroteHeader == true)
            {
                FileStream stream = File.Open(ErrorFile, FileMode.Append, FileAccess.Write);

                return new StreamWriter(stream);
            }
            else
            {
                FileStream stream = File.Open(ErrorFile, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);

                WriteHeader(writer);

                mWroteHeader = true;

                return writer;
            }
        }

        private static void WriteHeader(StreamWriter writer )
        {
            writer.WriteLine("Error Log started " + DateTime.Now.ToString());
            writer.WriteLine("");
        }

        private static void DialogReport(Exception e, ErrorLevel level)
        {
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK ;
            System.Windows.Forms.MessageBoxIcon icon = System.Windows.Forms.MessageBoxIcon.Asterisk;

            switch(level)
            {
                case ErrorLevel.Comment:
                    icon = System.Windows.Forms.MessageBoxIcon.Information;
                    break;

                case ErrorLevel.Warning:
                    icon = System.Windows.Forms.MessageBoxIcon.Warning;
                    break;

                case ErrorLevel.Fatal:
                    icon = System.Windows.Forms.MessageBoxIcon.Error;
                    break;

                case ErrorLevel.Bug:
                    icon = System.Windows.Forms.MessageBoxIcon.Hand;

                    break;
            }

            System.Windows.Forms.MessageBox.Show
                ("An error has occured: \r\n" + e.Message, level.ToString(), buttons, icon);
        }

        private static string LevelText(ErrorLevel level)
        {
            switch (level)
            {
                case ErrorLevel.Comment:
                    return "COMMENT";
                case ErrorLevel.Warning:
                    return "WARNING";
                case ErrorLevel.Fatal:
                    return "ERROR";
                case ErrorLevel.Bug:
                    return "BUG";
            }

            return "ERROR";
        }

        #endregion
    }
}