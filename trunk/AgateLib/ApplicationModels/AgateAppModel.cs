using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
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
		public void Initialize()
		{
			ProcessArguments();

			InitializeImpl();
		}

		protected virtual void InitializeImpl()
		{ }

		public void Dispose()
		{
			Dispose(true);

			if (Instance == this)
				Instance = null;
		}
		protected virtual void Dispose(bool disposing)
		{ }


		public int Run(Action entry)
		{
			return RunImpl(entry);
		}
		public int Run(Func<int> entry)
		{
			return RunImpl(entry);
		}
		public int Run(string[] args, Action entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}
		public int Run(string[] args, Func<int> entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}

		private int RunImpl(Action entry)
		{
			return RunImpl(ActionToFunc(entry));
		}
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

		protected virtual void ProcessArguments()
		{
			if (Parameters.Arguments == null) return;

			for(int i = 0; i < Parameters.Arguments.Length; i++)
			{
				var arg = Parameters.Arguments[i];
				int extraArguments = Parameters.Arguments.Length - i- 1;
				bool nextArgIsParam = extraArguments > 0 && Parameters.Arguments[i+1].StartsWith("--") == false;

				if (arg.StartsWith("--"))
				{
					if (nextArgIsParam)
					{
						ProcessArgument(arg, Parameters.Arguments[i + 1]);
						i++;
					}
					else
						ProcessArgument(arg, "");
				}
			}
		}

		protected virtual void ProcessArgument(string arg, string parm)
		{
			switch(arg)
			{
				case "--window":
					Parameters.CreateFullScreenWindow = false;
					Parameters.DisplayWindowSize = Size.FromString(parm);
					break;

				case "--novsync":
					Parameters.VerticalSync = false;
					break;

				default:
					break;
			}
		}

		private Size ParseSize(string parm)
		{
			Size.FromString(parm);
			throw new NotImplementedException();
		}

		protected int RunModel(Func<int> entryPoint)
		{
			try
			{
				Initialize();
				AutoCreateDisplayWindow();
				Display.RenderState.WaitForVerticalBlank = Parameters.VerticalSync;

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
			
			if (Parameters.DisplayWindowSize.Width == 0 ) throw new AgateException("Cannot create a display window with width 0.");
			if (Parameters.DisplayWindowSize.Height == 0) throw new AgateException("Cannot create a display window with height 0.");

			return Parameters.DisplayWindowSize;
		}

		private Size GetScreenSize()
		{
			return Display.Caps.NativeScreenResolution;
		}

		protected DisplayWindow AutoCreatedWindow { get { return window; } }

		public ModelParameters Parameters { get; set; }

		public virtual void KeepAlive()
		{
			Input.DispatchEvents();
		}
	}
}
