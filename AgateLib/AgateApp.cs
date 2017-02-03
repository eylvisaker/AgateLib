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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.AudioLib;
using AgateLib.Configuration.State;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Drivers;
using AgateLib.InputLib;
using AgateLib.IO;
using AgateLib.Platform;
using AgateLib.Quality;
using AgateLib.Settings;

namespace AgateLib
{
	/// <summary>
	/// Class which contains methods related to the application state.
	/// </summary>
	public static class AgateApp
	{
		#region --- Error Reporting ---

		/// <summary>
		/// Static class which is used to handle all error reports.
		/// </summary>
		public static class ErrorReporting
		{
			/// <summary>
			/// Gets or sets the file name to which errors are recorded.  Defaults
			/// to "errorlog.txt"
			/// </summary>
			public static string ErrorFile
			{
				get { return State.App.ErrorReporting.ErrorFile; }
				set { State.App.ErrorReporting.ErrorFile = value; }
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
			///     AgateLib.AgateApp.AutoStackTrace = true;
			/// #endif
			/// </code>
			/// </example>
			public static bool AutoStackTrace
			{
				get { return State.App.ErrorReporting.AutoStackTrace; }
				set { State.App.ErrorReporting.AutoStackTrace = value; }
			}

			/// <summary>
			/// Gets or sets a value indicating how AgateLib should deal with issues that may
			/// cause problems when porting to another platform.
			/// </summary>
			public static CrossPlatformDebugLevel CrossPlatformDebugLevel
			{
				get { return State.App.CrossPlatformDebugLevel; }
				set { State.App.CrossPlatformDebugLevel = value; }
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
			/// Reports a cross platform error, according to the setting of AgateApp.CrossPlatformDebugLevel.
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
					if (State.App.ErrorReporting.WroteHeader == true)
					{
						Stream stream = FileSystem.File.OpenWrite(ErrorFile, true);

						return new StreamWriter(stream);
					}
					else
					{
						var stream = FileSystem.File.OpenWrite(ErrorFile);
						StreamWriter writer = new StreamWriter(stream);

						WriteHeader(writer);

						State.App.ErrorReporting.WroteHeader = true;

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

		internal static AgateLibState State { get; private set; } = new AgateLibState();

		/// <summary>
		/// Initializes AgateApp class with a platform factory.
		/// Can be called multiple times without adverse effects.
		/// </summary>
		public static void Initialize(IAgateFactory factory)
		{
			Require.ArgumentNotNull(factory, nameof(factory));

			if (State?.App.Inititalized ?? false)
				return;

			State = new AgateLibState();
			State.Factory = factory;
			State.App.Platform = factory.PlatformFactory.Info;
			State.App.Time = factory.PlatformFactory.CreateStopwatch();

			Assets = factory.PlatformFactory.ApplicationFolderFiles;
			UserFiles = factory.PlatformFactory.OpenUserAppStorage("");

			FileSystem.Initialize(factory.PlatformFactory);

			Display.Initialize(factory.DisplayFactory.DisplayCore);
			Audio.Initialize(factory.AudioFactory.AudioCore);
			Input.Initialize(factory.InputFactory.InputCore);

			AgateConsole.Initialize();

			State.App.Inititalized = true;
		}

		public static void Dispose()
		{
			Display.Dispose();
			Audio.Dispose();
			Input.Dispose();

			State = null;
		}

		/// <summary>
		/// Adds an action to a queue that is executed when AgateApp.KeepAlive is called.
		/// </summary>
		/// <param name="action"></param>
		public static void QueueWorkItem(Action action)
		{
			lock (State.App.WorkItems)
			{
				State.App.WorkItems.Add(action);
			}
		}

		/// <summary>
		/// Gets an object which describes details about the current platform.
		/// </summary>
		public static IPlatformInfo Platform => State.App.Platform;

		/// <summary>
		/// Gets an object which contains the persistant settings for the application.
		/// </summary>
		public static PersistantSettings Settings => State?.App.Settings;

		/// <summary>
		/// Gets or sets a bool value which indicates whether or not your
		/// app is the focused window.  This will be automatically set if
		/// you created DisplayWindows by specifying a title and size, but not
		/// if they are attached to a control.
		/// </summary>
		public static bool IsActive
		{
			get { return State.App.IsActive; }
			set { State.App.IsActive = value; }
		}

		/// <summary>
		/// Gets or sets a bool value indicating whether or not Agate
		/// should automatically pause execution when the application
		/// loses focus.
		/// 
		/// The automatic pause will occur during App.KeepAlive().  This
		/// will prevent the DisplayWindow from being updated at all.  As 
		/// such, this should not be used in production builds if your app
		/// is windowed.  Instead check the IsActive property and respond 
		/// accordingly if your application is windowed..
		/// </summary>
		public static bool AutoPause
		{
			get { return State.App.AutoPause; }
			set { State.App.AutoPause = value; }
		}

		/// <summary>
		/// The application is alive. When this value returns false, all application loops should terminate.
		/// </summary>
		public static bool IsAlive
		{
			get { return State?.App?.IsAlive ?? false; }
			set
			{
				if (!value)
					State.App.IsAlive = false;
			}
		}

		/// <summary>
		/// Gets the file provider for the programs assets folder.
		/// </summary>
		public static IReadFileProvider Assets
		{
			get { return State.App?.Assets; }
			set
			{
				Require.ArgumentNotNull(value, nameof(Assets));

				State.App.Assets = value;
			}
		}

		/// <summary>
		/// Gets the file provider that allows reading and writing to 
		/// the user's app data folder for this application. 
		/// </summary>
		/// <remarks>
		/// Typically on Windows this is in C:\Users\XXXX\AppData\Roaming\Company Name\Product Name
		/// </remarks>
		public static IReadWriteFileProvider UserFiles
		{
			get { return State.App?.UserFiles; }
			set
			{
				Require.ArgumentNotNull(value, nameof(UserFiles));

				State.App.UserFiles = value;
			}
		}
		
		public static void SetAssetPath(string path)
		{
			Assets = State.Factory.PlatformFactory.OpenAppFolder(path);
		}

		public static void SetUserAppStoragePath(string path)
		{
			UserFiles = State.Factory.PlatformFactory.OpenUserAppStorage(path);
		}
		/// <summary>
		/// Plays nice with the OS, by allowing events to be handled.
		/// This also handles user input events associated with the application,
		/// and polls joysticks if needed.
		/// </summary>
		public static void KeepAlive()
		{
			while (IsActive == false && AutoPause)
			{
				AudioLib.Audio.Update();

				if (Display.CurrentWindow == null)
					break;
				if (Display.CurrentWindow.IsClosed)
					break;
				if (IsAlive == false)
					break;
			}

			// Update Audio Engine.
			Audio.Update();

			// Poll for joystick input.
			Input.PollJoysticks();

			Input.DispatchQueuedEvents();

			ExecuteWorkItemQueue();
		}

		/// <summary>
		/// returns time since agatelib was initialized in milliseconds.
		/// </summary>
		/// <returns></returns>
		internal static double GetTime()
		{
			return State.App.Time.TotalMilliseconds;
		}

		private static void ExecuteWorkItemQueue()
		{
			while (State.App.WorkItems.Count > 0)
			{
				Action workItem = null;

				lock (State.App.WorkItems)
				{
					workItem = State.App.WorkItems.First();
					State.App.WorkItems.RemoveAt(0);
				}

				workItem();
			}
		}
	}
}