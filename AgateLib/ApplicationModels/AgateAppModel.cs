using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public abstract class AgateAppModel
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

		#endregion

		DisplayWindow window;

		public AgateAppModel(ModelParameters parameters)
		{
			Parameters = parameters;

			if (IsAlive)
				throw new AgateException("Cannot create a new application model when an existing one is active.");

			Instance = this;
		}

		protected int RunModel(Func<int> entryPoint)
		{
			try
			{
				Initialize();
				AutoCreateDisplayWindow();

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
			return Display.Caps.ScreenResolution;
		}

		protected abstract void Initialize();
		protected abstract void Dispose();

		public ModelParameters Parameters { get; set; }
	}
}
