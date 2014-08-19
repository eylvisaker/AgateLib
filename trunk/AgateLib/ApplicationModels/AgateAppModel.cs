﻿//     The contents of this file are subject to the Mozilla Public License
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
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public abstract class AgateAppModel : IDisposable
	{
		#region --- Static Members ---

		public static AgateAppModel Instance { get; private set; }

		public static bool IsAlive
		{
			get
			{
				if (Instance == null)
					return false;

				if (Instance.window != null)
					return Instance.window.IsClosed == false;

				if (Display.CurrentWindow == null)
					return false;

				return Display.CurrentWindow.IsClosed == false;
			}
		}


		static Func<int> ActionToFunc(Action entry)
		{
			return () => { entry(); return 0; };
		}

		#endregion

		DisplayWindow window;

		public AgateAppModel(ModelParameters parameters)
		{
			Parameters = parameters;

			if (IsAlive)
				throw new AgateException("Cannot create a new application model when an existing one is active.");

			Instance = this;
		}
		/// <summary>
		/// Initializes the applicatin model. This will process command line arguments and initialize AgateLib.
		/// It is not required to call this function manually, it will be called by the Run method if it has not
		/// been called.
		/// </summary>
		public void Initialize()
		{
			ProcessArguments();

			InitializeImpl();
		}

		/// <summary>
		/// Override this to provide proper platform initialization of AgateLib.
		/// </summary>
		protected virtual void InitializeImpl()
		{ }

		public void Dispose()
		{
			Dispose(true);

			if (Instance == this)
				Instance = null;
		}
		/// <summary>
		/// Override this to clean up the platform initialization of AgateLib.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{ }

		/// <summary>
		/// Runs the application model with the specified entry point for your application.
		/// </summary>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns 0.</returns>
		public int Run(Action entry)
		{
			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point for your application.
		/// </summary>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns the return value from the <c>entry</c> parameter.</returns>
		public int Run(Func<int> entry)
		{
			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point and command line arguments for your application.
		/// </summary>
		/// <param name="args">The command arguments to process.</param>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns 0.</returns>
		public int Run(string[] args, Action entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point and command line arguments for your application.
		/// </summary>
		/// <param name="args">The command arguments to process.</param>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns the return value from the <c>entry</c> parameter.</returns>
		public int Run(string[] args, Func<int> entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}

		private int RunImpl(Action entry)
		{
			return RunImpl(ActionToFunc(entry));
		}
		/// <summary>
		/// Runs the application model by calling RunModel. If you override this, make
		/// sure to catch the ExitGameException and return 0 in the exception handler.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		protected virtual int RunImpl(Func<int> entry)
		{
			try
			{
				return RunModel(entry);
			}
			catch (ExitGameException)
			{
				return 0;
			}
		}

		/// <summary>
		/// Processes command line arguments. Override this to completely replace the 
		/// comand line argument processor. 
		/// </summary>
		/// <remarks>
		/// Arguments are only processed if they start
		/// with a dash (-). Any arguments which do not start with a dash are considered
		/// parameters to the previous argument. For example, the argument string 
		/// <code>--window 640,480 test --novsync</code> would call ProcessArgument once
		/// for --window with the parameters <code>640,480 test</code> and again for
		/// --novsync.
		/// </remarks>
		protected virtual void ProcessArguments()
		{
			if (Parameters.Arguments == null) return;

			List<string> p = new List<string>();

			for (int i = 0; i < Parameters.Arguments.Length; i++)
			{
				var arg = Parameters.Arguments[i];
				int extraArguments = Parameters.Arguments.Length - i - 1;

				p.Clear();
				for (int j = i+1; j < Parameters.Arguments.Length; j++)
				{
					if (Parameters.Arguments[j].StartsWith("-") == false)
						p.Add(Parameters.Arguments[j]);
					else
						break;
				}

				if (arg.StartsWith("-"))
				{
					ProcessArgument(arg, p);

					i += p.Count;
				}
			}
		}

		/// <summary>
		/// Processes a single command line argument. Override this to replace how
		/// command line arguments interact with AgateLib. Unrecognized arguments will be
		/// passed to ProcessCustomArgument. 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="parm"></param>
		protected virtual void ProcessArgument(string arg, IList<string> parm)
		{
			switch (arg)
			{
				case "--window":
					Parameters.CreateFullScreenWindow = false;
					if (parm.Count > 0)
						Parameters.DisplayWindowSize = Size.FromString(parm[0]);
					break;

				case "--novsync":
					Parameters.VerticalSync = false;
					break;

				case "--emulate-device":
					if (parm.Count > 0)
						Parameters.EmulateDeviceType = (DeviceType)Enum.Parse(typeof(DeviceType), parm[0], true);
					break;

				default:
					ProcessCustomArgument(arg, parm);
					break;
			}
		}

		/// <summary>
		/// Called when a command-line argument is unhandled. This function logs the argument
		/// to AgateLib.Diagnostics.Log. Override this to handle custom arguments.
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="parm"></param>
		/// <remarks>
		/// All arguments to AgateLib start with two dashes (--). Custom arguments for your
		/// application can safely start with a single dash (-) and they won't clash with AgateLib
		/// arguments.
		/// </remarks>
		protected virtual void ProcessCustomArgument(string arg, IList<string> parm)
		{
			Log.WriteLine("Unknown command line argument:");
			Log.WriteLine("    {0} {1}", arg, string.Join(" ", parm));
		}

		/// <summary>
		/// Runs the application model.
		/// </summary>
		/// <param name="entryPoint"></param>
		/// <returns></returns>
		protected int RunModel(Func<int> entryPoint)
		{
			try
			{
				Initialize();
				AutoCreateDisplayWindow();
				SetPlatformEmulation();

				int retval = BeginModel(entryPoint);

				return retval;
			}
			finally
			{
				if (window != null)
					window.Dispose();

				Dispose();
			}
		}

		private void SetPlatformEmulation()
		{
			if (Parameters.EmulateDeviceType != DeviceType.Unknown)
			{
				Core.Platform.DeviceType = Parameters.EmulateDeviceType;
			}
		}

		/// <summary>
		/// Override this to implement the application model. This function
		/// should call the entry point and return its return value.
		/// It should not catch ExitGameException.
		/// </summary>
		/// <param name="entryPoint">The application entry point to call.</param>
		/// <returns></returns>
		protected abstract int BeginModel(Func<int> entryPoint);

		private void AutoCreateDisplayWindow()
		{
			if (Parameters.AutoCreateDisplayWindow == false)
				return;

			if (Parameters.CreateFullScreenWindow)
			{
				window = DisplayWindow.CreateFullScreen(
					Parameters.ApplicationName,
					GetFullScreenSize());
			}
			else
			{
				window = DisplayWindow.CreateWindowed(
					Parameters.ApplicationName,
					GetWindowedScreenSize());
			}

			window.FrameBuffer.CoordinateSystem =
				Parameters.CoordinateSystem.DetermineCoordinateSystem(window.Size);

			Display.RenderState.WaitForVerticalBlank = Parameters.VerticalSync;

			window.Closing += window_Closing;
		}

		/// <summary>
		/// Method called when the autocreated display window is closing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="cancel"></param>
		protected virtual void window_Closing(object sender, ref bool cancel)
		{
		}

		private Size GetWindowedScreenSize()
		{
			if (Parameters.DisplayWindowSize.IsEmpty)
			{
				var size = Display.Caps.NativeScreenResolution;
				size.Width -= 60;
				size.Height -= 60;

				return size;
			}
			else
			{
				return Parameters.DisplayWindowSize;
			}
		}

		private Size GetFullScreenSize()
		{
			if (Parameters.DisplayWindowSize.IsEmpty)
				return GetScreenSize();

			if (Parameters.DisplayWindowSize.Width == 0) throw new AgateException("Cannot create a display window with width 0.");
			if (Parameters.DisplayWindowSize.Height == 0) throw new AgateException("Cannot create a display window with height 0.");

			return Parameters.DisplayWindowSize;
		}

		private Size GetScreenSize()
		{
			return Display.Caps.NativeScreenResolution;
		}

		/// <summary>
		/// Returns the auto created display window. If the application model did not
		/// automatically create a display window, this is null.
		/// </summary>
		public DisplayWindow AutoCreatedWindow { get { return window; } }

		/// <summary>
		/// These are the parameters used to initialize and start up the application model.
		/// </summary>
		public ModelParameters Parameters { get; set; }

		/// <summary>
		/// Processes input events.
		/// </summary>
		public virtual void KeepAlive()
		{
			Input.DispatchQueuedEvents();
		}
	}
}
