using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.IO;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Quality;

namespace AgateLib.Platform.WinForms
{
	public class AgateSetupWinForms : AgateSetup
	{
		static double desktopScale;

		private static double DesktopScaling
		{
			get
			{
				if (desktopScale == 0)
				{
					using (var form = new System.Windows.Forms.Form())
					{
						using (var graphics = form.CreateGraphics())
						{
							desktopScale = graphics.DpiY / 96.0;
						}
					}
				}

				return desktopScale;
			}
		}

		Thread agateThread;
		FormsFactory factory;
		Assembly entryAssembly;
		IPrimaryWindow primaryWindow;
		bool windowClosed;

		public AgateSetupWinForms()
		{

		}

		public AgateSetupWinForms(string[] commandLineArguments) : this()
		{
			ParseCommandLineArgs(commandLineArguments);
		}

		protected override void Dispose(bool disposing)
		{
			primaryWindow.ExitMessageLoop();

			while (agateThread?.ThreadState == ThreadState.Running)
			{
				Thread.Sleep(0);
			}
		}

		public override void AgateLibInitialize()
		{
			entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			FillMissingProperties(entryAssembly);

			agateThread = new Thread(AgateThread);
			agateThread.Start();

			var watch = System.Diagnostics.Stopwatch.StartNew();
			const int maxTime = 10;

			while (Configuration == null)
			{
				Thread.Sleep(0);

				if (watch.Elapsed.TotalSeconds > maxTime)
				{
					var choice = System.Windows.Forms.MessageBox.Show(
						$"Failed to initialize AgateLib after {maxTime} seconds.",
						"Failed to Initialize",
						System.Windows.Forms.MessageBoxButtons.RetryCancel,
						System.Windows.Forms.MessageBoxIcon.Stop);

					if (choice == System.Windows.Forms.DialogResult.Cancel)
						throw new ExitGameException();
					else
						watch = System.Diagnostics.Stopwatch.StartNew();
				}
			}

			CreateContextForThread(Configuration.DisplayWindows.First());

			if (InitializeConsole)
			{
				AgateConsole.Initialize();
			}
		}

		private void AgateThread()
		{
			Initialize(Path.GetDirectoryName(Path.GetFullPath(entryAssembly.Location)));

			primaryWindow.RunApplication();
		}

		private void FillMissingProperties(Assembly assembly)
		{
			if (string.IsNullOrWhiteSpace(ApplicationName))
			{
				var product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
				var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
				var version = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
				var fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

				ApplicationName = title?.Title ?? product.Product + (" " + fileVersion?.Version + version?.Version).Trim();
			}
		}

		/// <summary>
		/// Initializes AgateLib.
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="assemblyPath">Path to the folder where the application files reside in.</param>
		private void Initialize(string appRootPath)
		{
			Condition.Requires<ArgumentNullException>(appRootPath != null, "appRootPath");
			Condition.Requires<InvalidOperationException>(factory == null, "Initialize should only be called once.");

			var result = new AgateConfig();

			factory = new FormsFactory(appRootPath);

			Core.Initialize(factory);
			Core.InitAssetLocations(AssetLocations);

			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			FileProvider.UserFiles = new FileSystemProvider(Path.Combine(appData, ApplicationName));

			CreateDisplayWindow(result);

			Configuration = result;
		}

		private static void CreateContextForThread(DisplayWindow window)
		{
			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
			var primaryWindow = window.Impl as IPrimaryWindow;

			primaryWindow.CreateContextForThread();
		}

		private void CreateDisplayWindow(AgateConfig config)
		{
			if (AutoCreateDisplayWindow == false)
				return;

			if (DesiredDisplayWindowResolution.Height == 0 || DesiredDisplayWindowResolution.Width == 0)
			{
				throw new AgateException("DesiredDisplayWindowResolution must be set to a non-zero value.");
			}

			DisplayWindow window;

			if (CreateFullScreenWindow)
			{
				var fullScreenSize = Display.Caps.NativeScreenResolution;
				var coords = CreateFullScreenCoords(fullScreenSize);

				window = DisplayWindow.CreateFullScreen(
					ApplicationName,
					fullScreenSize,
					coords);
			}
			else
			{
				var size = DesiredDisplayWindowResolution;

				var scale = IgnoreDesktopScaling ? 1.0 : DesktopScaling;

				var windowSize = new Size(
					(int)(size.Width * scale),
					(int)(size.Height * scale));

				window = DisplayWindow.CreateWindowed(
					ApplicationName,
					windowSize,
					new FixedCoordinateSystem(new Rectangle(Point.Empty, size)));
			}

			Display.RenderState.WaitForVerticalBlank = VerticalSync;

			window.Closing += window_Closing;

			config.DisplayWindows = new List<DisplayWindow> { window };

			primaryWindow = window.Impl as IPrimaryWindow;

			Core.State.Core.KeepAlive += KeepAlive;
		}

		private void KeepAlive()
		{
			if (windowClosed)
				throw new ExitGameException();
		}

		private void window_Closing(object sender, ref bool cancel)
		{
			windowClosed = true;
			primaryWindow.ExitMessageLoop();
		}

		private ICoordinateSystem CreateFullScreenCoords(Size fullScreenSize)
		{
			var desired = this.DesiredDisplayWindowResolution;
			if (fullScreenSize == desired)
				return new NativeCoordinates();

			var aspectRatio = fullScreenSize.AspectRatio;
			var desiredAspectRatio = DesiredDisplayWindowResolution.AspectRatio;

			if (Math.Abs(aspectRatio - desiredAspectRatio) < 1e-6)
				return new FixedCoordinateSystem(new Rectangle(Point.Empty, desired));

			switch (this.DisplayWindowExpansionType)
			{
				case WindowExpansionType.VerticalSizeFixed:
					return new FixedCoordinateSystem(new Rectangle(Point.Empty,
						new Size((int)(aspectRatio * desired.Height), desired.Height)));
				case WindowExpansionType.HorizontalSizeFixed:

					return new FixedCoordinateSystem(new Rectangle(Point.Empty,
						new Size(desired.Width, (int)(desired.Width / aspectRatio))));
				case WindowExpansionType.Scale:
					return new FixedCoordinateSystem(new Rectangle(Point.Empty, desired));

				default:
					throw new NotImplementedException();
			}
		}

	}
}
