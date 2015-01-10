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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
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
using AgateLib.Settings;
using AgateLib.Platform;
using AgateLib.Drivers;
using AgateLib.Diagnostics;
using AgateLib.IO;
using AgateLib.InputLib;
using AgateLib.ApplicationModels;
using System.Threading.Tasks;
using AgateLib.DefaultAssets;
using AgateLib.Quality;

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
		private static bool sAutoPause = false;
		private static bool sIsActive = true;
		private static bool sInititalized = false;
		private static PlatformInfo mPlatform;
		private static PersistantSettings sSettings;
		private static IAgateFactory mFactory;

		#region --- Error Reporting ---

		private static CrossPlatformDebugLevel mCrossPlatform = CrossPlatformDebugLevel.Comment;
		private static IStopwatch mTime;

		/// <summary>
		/// Static class which is used to handle all error reports.
		/// </summary>
		public static class ErrorReporting
		{
			private static string sErrorFile = "errorlog.txt";
			private static bool sAutoStackTrace = false;
			private static bool sWroteHeader = false;

			/// <summary>
			/// Gets or sets the file name to which errors are recorded.  Defaults
			/// to "errorlog.txt"
			/// </summary>
			public static string ErrorFile
			{
				get { return sErrorFile; }
				set { sErrorFile = value; }
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
				get { return sAutoStackTrace; }
				set { sAutoStackTrace = value; }
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
			/// It is recommended to use an overload which takes an exception parameter,
			/// if there is an exception available which provides more information.
			/// </summary>
			/// <param name="level"></param>
			/// <param name="message"></param>
			public static void Report(ErrorLevel level, string message)
			{
				Report(level, message, null);
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
				//if (showDialog && Drivers.Registrar.WinForms != null)
				//{
				//	Drivers.Registrar.WinForms.ShowErrorDialog(message, e, level);
				//}

				using (StreamWriter filewriter = OpenErrorFile())
				{
					if (filewriter != null)
						filewriter.Write(text);
				}

				Log.WriteLine(text);
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
					if (sWroteHeader == true)
					{
						Stream stream = FileSystem.File.OpenWrite(ErrorFile, true);

						return new StreamWriter(stream);
					}
					else
					{
						var stream = FileSystem.File.OpenWrite(ErrorFile);
						StreamWriter writer = new StreamWriter(stream);

						WriteHeader(writer);

						sWroteHeader = true;

						return writer;
					}
				}
				catch (Exception e)
				{
					string message = "Could not open file " + ErrorFile + ".\r\n" +
						"Error message: " + e.Message + "\r\n" +
						"Errors cannot be saved to a text file.";

					Log.WriteLine(message);

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

		/// <summary>
		/// Initializes Core class. Also causes the Registrar to probe drivers.
		/// Can be called multiple times without adverse effects.
		/// </summary>
		public static void Initialize(IAgateFactory factory)
		{
			Condition.Requires<ArgumentNullException>(factory != null);

			if (sInititalized)
				return;

			mFactory = factory;
			mPlatform = factory.PlatformFactory.Info;
			mTime = factory.PlatformFactory.CreateStopwatch();

			FileSystem.File = factory.PlatformFactory.CreateFile();
			FileSystem.Path = factory.PlatformFactory.CreatePath();

			Display.Initialize(factory.DisplayFactory.DisplayImpl);
			Audio.Initialize(factory.AudioFactory.AudioImpl);
			JoystickInput.Initialize(factory.InputFactory.CreateJoystickInputImpl());

			InitializeDefaultResources();

			sInititalized = true;
		}

		public static void InitializeDefaultResources()
		{
			DefaultResources res = new DefaultResources();

			var task = mFactory.DisplayFactory.InitializeDefaultResourcesAsync(res);
			Fonts.Initialize(res);

			Task.WaitAll(task);
		}

		public static void InitAssetLocations(AssetLocations assets)
		{
			Condition.Requires<ArgumentNullException>(assets != null);

			FileProvider.Initialize(mFactory.PlatformFactory.ApplicationFolderFileProvider, assets);
		}

		public static IAgateFactory Factory
		{
			get { return mFactory; }
		}

		/// <summary>
		/// Gets an object which describes details about the current platform.
		/// </summary>
		public static PlatformInfo Platform
		{
			get { return mPlatform; }
		}

		/// <summary>
		/// Gets an object which contains the persistant settings for the application.
		/// </summary>
		public static PersistantSettings Settings
		{
			get
			{
				if (sSettings == null)
				{
					sSettings = new PersistantSettings();
				}

				return sSettings;
			}
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not your
		/// app is the focused window.  This will be automatically set if
		/// you created DisplayWindows by specifying a title and size, but not
		/// if they are attached to a control.
		/// </summary>
		public static bool IsActive
		{
			get { return sIsActive; }
			set { sIsActive = value; }
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
			get { return sAutoPause; }
			set { sAutoPause = value; }
		}

		/// <summary>
		/// Plays nice with the OS, by allowing events to be handled.
		/// This also handles user input events associated with the application,
		/// and polls joysticks if needed.
		/// </summary>
		public static void KeepAlive()
		{
			var appmodel = AgateAppModel.Instance;

			if (appmodel == null)
				return;

			appmodel.KeepAlive();

			while (IsActive == false && AutoPause)
			{
				appmodel.KeepAlive();

				AudioLib.Audio.Update();

				if (Display.CurrentWindow == null)
					break;
				else if (Display.CurrentWindow.IsClosed)
					break;
			}

			// Update Audio Engine.
			AudioLib.Audio.Update();

			// Poll for joystick input.
			InputLib.JoystickInput.PollTimer();
		}


		/// <summary>
		/// returns time since agatelib was initialized in milliseconds.
		/// </summary>
		/// <returns></returns>
		internal static double GetTime()
		{
			return mTime.TotalMilliseconds;
		}


	}
}