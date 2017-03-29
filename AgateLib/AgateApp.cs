//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.AudioLib;
using AgateLib.Configuration;
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
		internal static event EventHandler AfterKeepAlive
		{
			add { State.App.AfterKeepAlive += value; }
			remove { State.App.AfterKeepAlive -= value; }
		}

		internal static AgateLibState State { get; private set; } = new AgateLibState();

		/// <summary>
		/// Gets the object which handlers error reporting configuration.
		/// </summary>
		public static ErrorReporter ErrorReporting => State.App.ErrorReporting;

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

		/// <summary>
		/// Returns the amount of time that elapsed between the last two calls to KeepAlive.
		/// </summary>
		[Obsolete("Use ApplicationClock.Elapsed instead.", true)]
		public static TimeSpan DeltaTime => State.App.AppTime.DeltaTime;

		/// <summary>
		/// Returns a MasterClock object which only advances during calls to KeepAlive, and shows
		/// the amount of time that passed between the last two calls to KeepAlive.
		/// This should be used for advancing user interface and other out-of-game behavior.
		/// </summary>
		/// <returns></returns>
		public static MasterClock ApplicationClock => State.App.ApplicationClock;

		/// <summary>
		/// Returns a GameClock object. This is similar to the ApplicationClock except it can be sped up
		/// and slowed down. This should be used to advance the in-game action.
		/// </summary>
		public static GameClock GameClock => State.App.GameClock;

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

			Display.Initialize(factory.DisplayFactory.DisplayCore);
			Audio.Initialize(factory.AudioFactory.AudioCore);
			Input.Initialize(factory.InputFactory.InputCore);

			AgateConsole.Initialize();

			State.App.Inititalized = true;
		}

		/// <summary>
		/// Disposes of the AgateLib resources related to the application.
		/// After calling this, the display/audio/input subsystems will no longer
		/// operate.
		/// </summary>
		public static void Dispose()
		{
			Settings?.Save();

			Display.Dispose();
			Audio.Dispose();
			Input.Dispose();

			State = null;
		}

		/// <summary>
		/// Signals the application should quit by setting the IsAlive flag to false.
		/// </summary>
		public static void Quit()
		{
			IsAlive = false;
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
			while (IsAlive && IsActive == false && AutoPause)
			{
				Audio.Update();

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

			if (IsAlive)
			{
				State.App.AfterKeepAlive?.Invoke(null, EventArgs.Empty);
			}

			State.App.ApplicationClock.Advance();
		}

		/// <summary>
		/// Returns time in milliseconds since agatelib was initialized in milliseconds.
		/// </summary>
		/// <returns></returns>
		internal static double GetTimeInMilliseconds()
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