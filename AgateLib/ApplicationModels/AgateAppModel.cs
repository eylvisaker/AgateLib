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
		/// Processes command line arguments. 
		/// </summary>
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

				if (arg.StartsWith("--"))
				{
					ProcessArgument(arg, p);

					i += p.Count;
				}
			}
		}

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
					break;
			}
		}

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

		public DisplayWindow AutoCreatedWindow { get { return window; } }

		public ModelParameters Parameters { get; set; }

		public virtual void KeepAlive()
		{
			Input.DispatchQueuedEvents();
		}
	}
}
