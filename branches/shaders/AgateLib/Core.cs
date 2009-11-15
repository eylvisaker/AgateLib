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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AgateLib.AudioLib;
using AgateLib.DisplayLib;

namespace AgateLib
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
		Comment,
		/// <summary>
		/// Indicates that the error message is not severe, and the program may
		/// continue.  However, unexpected behavior may occur due to the result of
		/// this error.
		/// </summary>
		Warning,
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
	/// Enum used to inidicate the level of cross-platform debugging that should occur.
	/// </summary>
	public enum CrossPlatformDebugLevel
	{
		/// <summary>
		/// Ignores any issues related to cross platform deployment.
		/// </summary>
		None,

		/// <summary>
		/// Outputs comments using Core.Report with a comment level.
		/// </summary>
		Comment,

		/// <summary>
		/// Throws exceptions on issues that may cause problems when operating on another platform.
		/// </summary>
		Exception,
	}

	/// <summary>
	/// Class which contains methods commonly used by the entire library.
	/// </summary>
	public static class Core
	{
		private static bool mAutoPause = false;
		private static bool mIsActive = true;
		private static bool mInititalized = false;
		private static Platform mPlatform;

		#region --- Error Reporting ---

		private static CrossPlatformDebugLevel mCrossPlatform = CrossPlatformDebugLevel.Comment;
		private static System.Diagnostics.Stopwatch mTime = Stopwatch.StartNew();

		public static class ErrorReporting
		{
			private static string mErrorFile = "errorlog.txt";
			private static bool mAutoStackTrace = false;
			private static bool mWroteHeader = false;

			/// <summary>
			/// Gets or sets the file name to which errors are recorded.  Defaults
			/// to "errorlog.txt"
			/// </summary>
			public static string ErrorFile
			{
				get { return mErrorFile; }
				set { mErrorFile = value; }
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
			///     AgateLib.Core.AutoStackTrace = true;
			/// #endif
			/// </code>
			/// </example>
			public static bool AutoStackTrace
			{
				get { return mAutoStackTrace; }
				set { mAutoStackTrace = value; }
			}

			/// <summary>
			/// Gets or sets a value indicating how AgateLib should deal with issues that may
			/// cause problems when porting to another platform.
			/// </summary>
			public static CrossPlatformDebugLevel CrossPlatformDebugLevel
			{
				get { return mCrossPlatform; }
				set { mCrossPlatform = value; }
			}

			/// <summary>
			/// Saves an error message to the ErrorFile.
			/// Outputs a stack trace and shows a dialog box if the ErrorLevel 
			/// is Bug or Fatal.
			/// </summary>
			/// <param name="message">A message to print out before the 
			/// exception's message.</param>
			/// <param name="e"></param>
			/// <param name="level"></param>
			public static void Report(ErrorLevel level, string message, Exception e)
			{

				switch (level)
				{
					case ErrorLevel.Bug:
					case ErrorLevel.Fatal:
						Report(level, message, e, true, true);
						break;

					case ErrorLevel.Comment:
					case ErrorLevel.Warning:
						Report(level, message, e, AutoStackTrace, false);
						break;
				}
			}

			/// <summary>
			/// Saves an error message to the ErrorFile.
			/// </summary>
			/// <param name="message">A message to print out before the 
			/// exception's message.</param>
			/// <param name="e"></param>
			/// <param name="level"></param>
			/// <param name="printStackTrace">Bool value indicating whether or not 
			/// a stack trace should be written out.  </param>
			/// <param name="showDialog">Bool value indicating whether or not a 
			/// message box should pop up with an OK button, informing the user about the 
			/// error.  If false, the error is silently written to the ErrorFile.</param>
			public static void Report(ErrorLevel level, string message, Exception e, bool printStackTrace, bool showDialog)
			{
				StringBuilder b = new StringBuilder();

				b.Append(LevelText(level));
				b.Append(": ");
				b.AppendLine(message);

				if (e != null)
				{
					b.Append(e.GetType().Name);
					b.Append(": ");
					b.AppendLine(e.Message);

					if (printStackTrace)
						b.AppendLine(e.StackTrace);
				}

				b.AppendLine();

				string text = b.ToString();

				// show the error dialog if AgateWinForms.dll is present.
				if (showDialog && Drivers.Registrar.WinForms != null)
				{
					Drivers.Registrar.WinForms.ShowErrorDialog(message, e, level);
				}

				using (StreamWriter filewriter = OpenErrorFile())
				{
					if (filewriter != null)
						filewriter.Write(text);
				}

				Trace.WriteLine(text);
			}

			/// <summary>
			/// Reports a cross platform error, according to the setting of Core.CrossPlatformDebugLevel.
			/// </summary>
			/// <param name="message"></param>
			public static void ReportCrossPlatformError(string message)
			{
				switch (CrossPlatformDebugLevel)
				{
					case CrossPlatformDebugLevel.Comment:
						Report(ErrorLevel.Warning, message, null);
						break;

					case CrossPlatformDebugLevel.Exception:
						throw new AgateCrossPlatformException(message);

				}
			}

			private static StreamWriter OpenErrorFile()
			{
				try
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
				catch (Exception e)
				{
					string message = "Could not open file " + ErrorFile + ".\r\n" +
						"Error message: " + e.Message + "\r\n" +
						"Errors cannot be saved to a text file.";

					Debug.WriteLine(message);
					Trace.WriteLine(message);

					return null;
				}
			}

			private static void WriteHeader(StreamWriter writer)
			{
				writer.WriteLine("Error Log started " + DateTime.Now.ToString());
				writer.WriteLine("");
			}
			private static string LevelText(ErrorLevel level)
			{
				switch (level)
				{
					case ErrorLevel.Comment: return "COMMENT";
					case ErrorLevel.Warning: return "WARNING";
					case ErrorLevel.Fatal: return "ERROR";
					case ErrorLevel.Bug: return "BUG";
				}

				return "ERROR";
			}


		}

		#endregion

		static Core()
		{
			mPlatform = new Platform();
		}
		/// <summary>
		/// Initializes Core class.
		/// Can be called multiple times without adverse effects.
		/// </summary>
		public static void Initialize()
		{
			if (mInititalized)
				return;

			Drivers.Registrar.Initialize();


			mInititalized = true;
		}

		/// <summary>
		/// Gets an object which describes details about the current platform.
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
		/// Delegate for types which attach to the KeepAliveEvent.
		/// </summary>
		public delegate void KeepAliveDelegate();
		/// <summary>
		/// Event which occurs when KeepAlive is called.
		/// This allows subsystems (such as GUI, Input, Sound) to make updates each frame.
		/// </summary>
		public static event KeepAliveDelegate KeepAliveEvent;
		/// <summary>
		/// Plays nice with the OS, by allowing events to be handled.
		/// This also handles user input events associated with the application,
		/// and polls joysticks if needed.
		/// </summary>
		public static void KeepAlive()
		{
			// Some tests indicate that using the Win32 platform-specific call 
			// to PeekMessage before calling DoEvents is about 1 fps faster,
			// when there are no events to process.  It's not clear whether or
			// not this is worth it when there lots of events being generated
			// (ie lots of mouse move events) but it does seem to speed up for
			// Direct3D.
			if (Display.IsAppIdle == false)
			{
				Display.ProcessEvents();

				while (IsActive == false && AutoPause)
				{
					System.Threading.Thread.Sleep(25);
					Display.ProcessEvents();

					// Update Audio Engine, if necessary
					AudioLib.Audio.Update();

					if (Display.CurrentWindow == null)
						break;
					else if (Display.CurrentWindow.IsClosed)
						break;
				}
			}

			if (KeepAliveEvent != null)
				KeepAliveEvent();

			// Update Audio Engine.
			AudioLib.Audio.Update();

			// Poll for joystick input.
			InputLib.JoystickInput.PollTimer();
		}

		/// <summary>
		/// Returns the directory the application that was started resides in.
		/// </summary>
		public static string BaseDirectory
		{
			get
			{
				return System.AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		/// <summary>
		/// returns time since agatelib was initialized in milliseconds.
		/// </summary>
		/// <returns></returns>
		internal static double GetTime()
		{
			return mTime.ElapsedMilliseconds;
		}


	}
}