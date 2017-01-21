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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
	/// <summary>
	/// Provides initialization of the AgateLib WinForms platform.
	/// </summary>
	public class AgateSetup : AgateSetupCore
	{
		#region --- Static Members ---

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

		#endregion

		FormsFactory factory;
		Assembly entryAssembly;
		bool windowClosed;

		public AgateSetup(string[] commandLineArguments = null)
		{
			ParseCommandLineArgs(commandLineArguments);

			InitializeLibrary();
		}

		protected override void Dispose(bool disposing)
		{
			Core.IsAlive = false;

			if (Configuration != null)
			{
				foreach (var displayWindow in Configuration.DisplayWindows)
				{
					displayWindow.Dispose();
				}
			}

			EventThread.Dispose();

			Core.Dispose();
		}

		private WinFormsEventThread EventThread => factory.DisplayFactory.FullDisplayImpl.EventThread;

		public void InitializeAgateLib()
		{
			var result = new AgateConfig();
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			Core.InitAssetLocations(AssetLocations,
				new FileSystemProvider(GetAppRootPath()));

			Core.UserFiles =
				new FileSystemProvider(Path.Combine(appData, ApplicationName));

			InitializeDisplayWindow(result);

			Configuration = result;
		}

		private string GetAppRootPath()
		{
			entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			FillMissingProperties(entryAssembly);

			return Path.GetDirectoryName(Path.GetFullPath(entryAssembly.Location));
		}

		private void FillMissingProperties(Assembly assembly)
		{
			if (string.IsNullOrWhiteSpace(ApplicationName))
			{
				var product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
				var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
				var version = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
				var fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

				ApplicationName = $"{title?.Title ?? product.Product} {fileVersion?.Version ?? version?.Version}"
					.Trim();
			}
		}

		private void InitializeLibrary()
		{
			Condition.Requires<InvalidOperationException>(factory == null, "InitializeLibrary should only be called once.");

			factory = new FormsFactory(GetAppRootPath());

			Core.Initialize(factory);
		}

		private void InitializeDisplayWindow(AgateConfig config)
		{
			if (base.CreateDisplayWindow == false)
				return;

			if (DesiredDisplayWindowResolution.Height == 0 || DesiredDisplayWindowResolution.Width == 0)
			{
				throw new AgateException("DesiredDisplayWindowResolution must be set to a non-zero value.");
			}

			DisplayWindow primaryDisplayWindow = FullScreen
				? CreateFullScreenDisplay(config)
				: CreateWindowedDisplay(config);

			Display.RenderState.WaitForVerticalBlank = VerticalSync;

			primaryDisplayWindow.Closed += window_Closed;
		}

		private DisplayWindow CreateWindowedDisplay(AgateConfig config)
		{
			DisplayWindow primaryDisplayWindow;

			var size = DesiredDisplayWindowResolution;

			var scale = IgnoreDesktopScaling ? 1.0 : DesktopScaling;

			var windowSize = new Size(
				(int)(size.Width * scale),
				(int)(size.Height * scale));

			var createParams = CreateWindowParams.Windowed(
				ApplicationName, size.Width, size.Height, false, null, null);

			createParams.PhysicalSize = DisplayWindowPhysicalSize ?? windowSize;

			primaryDisplayWindow = new DisplayWindow(createParams);

			config.DisplayWindows = new List<DisplayWindow> { primaryDisplayWindow };
			return primaryDisplayWindow;
		}

		private DisplayWindow CreateFullScreenDisplay(AgateConfig config)
		{
			DisplayWindow primaryDisplayWindow = null;

			switch (FullScreenCaptureMode)
			{
				case FullScreenCaptureMode.PrimaryScreenOnly:
					primaryDisplayWindow = DisplayWindow.CreateFullScreen(
						ApplicationName,
						new Resolution(DesiredDisplayWindowResolution, FullScreenRenderMode));

					config.DisplayWindows = new List<DisplayWindow> { primaryDisplayWindow };

					break;

				default:
					var results = new List<DisplayWindow>();

					foreach (var screen in Display.Screens.AllScreens)
					{
						var windowParams = CreateWindowParams.FullScreen(ApplicationName,
							new Resolution(DesiredDisplayWindowResolution, FullScreenRenderMode), null);

						windowParams.TargetScreen = screen;

						var displayWindow = new DisplayWindow(windowParams);

						if (screen.IsPrimary)
							primaryDisplayWindow = displayWindow;

						results.Add(displayWindow);
					}

					config.DisplayWindows = results;

					break;
			}

			return primaryDisplayWindow;
		}

		private void window_Closed(object sender, EventArgs args)
		{
			windowClosed = true;

			Core.IsAlive = false;
		}
	}
}
