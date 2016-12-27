using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.IO;

namespace AgateLib.Configuration
{
	public abstract class AgateSetup : IAgateSetup
	{
		public void Dispose()
		{
			Dispose(true);
		}

		protected abstract void Dispose(bool disposing);

		/// <summary>
		/// Sets the title of the display windows.
		/// If this is empty, AgateLib will automatically 
		/// set the title from the assembly attributes.
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// Sets file paths for AgateLib.IO.Assets.
		/// </summary>
		public AssetLocations AssetLocations { get; set; } = new AssetLocations();

		/// <summary>
		/// Value that indicates the size of the smallest dimension of the
		/// auto created display windows. This is usually the vertical 
		/// dimension. If this value is not set, the display windows will 
		/// simply have a coordinate system that matches their size in pixels.
		/// </summary>
		public Size DesiredDisplayWindowResolution { get; set; }

		/// <summary>
		/// A value which indicates how a display window should be expanded.
		/// This is used if DesiredDisplayWindowResolution does not match the
		/// physical aspect ratio of the user's monitor.
		/// </summary>
		public WindowExpansionType DisplayWindowExpansionType { get; set; }

		/// <summary>
		/// Set to true to force video refreshes to wait for the vertical blank.
		/// Defaults to true. This can be set to false by the user 
		/// via a command line argument of -novsync.
		/// </summary>
		public bool VerticalSync { get; set; } = true;

		/// <summary>
		/// Sets the physical size of the created window. This parameter is
		/// not required to be set programmatically. 
		/// </summary>
		/// <remarks>Unless this is set programmatically, this will usually match 
		/// the value of DesiredDisplayWindowResolution with the following exceptions:
		/// <list type="bullet">
		/// <item><description>The user specifies -window &lt;widthxheight&gt;</description></item>
		/// <item><description>The desktop scaling is defined for this machine - this will
		/// result in the physical size of the window being scaled by the same amount.</description></item>
		/// </list>
		/// </remarks>
		public Size? DisplayWindowPhysicalSize { get; set; }

		/// <summary>
		/// Set to false to prevent the setup system from automatically creating display
		/// windows. In this case you must manage your own DisplayWindow objects.
		/// </summary>
		public bool AutoCreateDisplayWindow { get; set; } = true;

		/// <summary>
		/// Set to indicate whether the window created will be full screen. This
		/// can be turned off on the command line by specifying '-window'
		/// </summary>
		public bool CreateFullScreenWindow { get; set; } = true;

		/// <summary>
		/// Set to true to create a display window for each monitor.
		/// </summary>
		public bool CreateWindowForEachMonitor { get; set; } = false;

		/// <summary>
		/// Call this to execute initialiation of AgateLib.
		/// </summary>
		public abstract void AgateLibInitialize();

		/// <summary>
		/// Gets the configuration of AgateLib that resulted from the
		/// call to AgateLibInitialize.
		/// </summary>
		public AgateConfig Configuration { get; protected set; }

		/// <summary>
		/// Processes command line arguments. Override this to completely replace the 
		/// comand line argument processor. 
		/// </summary>
		/// <remarks>
		/// Arguments are only processed if they start
		/// with a dash (-). Any arguments which do not start with a dash are considered
		/// parameters to the previous argument. For example, the argument string 
		/// <code>-window 640,480 test -novsync</code> would call ProcessArgument once
		/// for -window with the parameters <code>640,480 test</code> and again for
		/// -novsync.
		/// </remarks>
		public virtual void ParseCommandLineArgs(string[] commandLineArguments)
		{
			if (commandLineArguments == null)
				return;

			List<string> parameters = new List<string>();

			for (int i = 0; i < commandLineArguments.Length; i++)
			{
				var arg = commandLineArguments[i];

				int extraArguments = commandLineArguments.Length - i - 1;

				parameters.Clear();

				for (int j = i + 1; j < commandLineArguments.Length; j++)
				{
					if (commandLineArguments[j].StartsWith("-") == false)
						parameters.Add(commandLineArguments[j]);
					else
						break;
				}

				if (arg.StartsWith("-"))
				{
					ProcessArgument(arg, parameters);

					i += parameters.Count;
				}
			}
		}

		/// <summary>
		/// Processes a single command line argument. Override this to replace how
		/// command line arguments interact with AgateLib. Unrecognized arguments will be
		/// passed to ProcessCustomArgument. 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="parameters"></param>
		protected virtual void ProcessArgument(string arg, IList<string> parameters)
		{
			switch (arg)
			{
				case "-window":
					CreateFullScreenWindow = false;
					if (parameters.Count > 0)
						DisplayWindowPhysicalSize = Size.FromString(parameters[0]);
					break;

				case "-novsync":
					VerticalSync = false;
					break;
					
				default:
					ProcessCustomArgument(arg, parameters);
					break;
			}
		}

		/// <summary>
		/// Method called if ProcessArgument doesn't know what to do with an argument.
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="parameters"></param>
		protected virtual void ProcessCustomArgument(string arg, IList<string> parameters)
		{
		}
	}
}
