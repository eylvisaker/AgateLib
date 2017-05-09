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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Fluent interface for creating a display window.
	/// </summary>
	public class DisplayWindowBuilder
	{
		private CreateWindowParams createParams = new CreateWindowParams();
		private string[] args;
		private bool quitOnClose;
		private bool autoResize;
		private ICoordinateSystem coords;

		/// <summary>
		/// Constructs a DisplayWindowBuilder object. For creating your primary
		/// DisplayWindows, it is recommended that you
		/// use the <code>(string[] args)</code> overload instead to allow
		/// for command line parameters to override the window construction 
		/// behaviore.
		/// </summary>
		public DisplayWindowBuilder()
		{
			createParams.IsFullScreen = true;
			createParams.IsResizable = true;
		}

		/// <summary>
		/// Constructs a DisplayWindowBuilder object.
		/// </summary>
		/// <param name="args">The command line arguments the user
		/// passed to your application.</param>
		public DisplayWindowBuilder(string[] args) : this()
		{
			this.args = args;
		}

		/// <summary>
		/// The CreateWindowParams object being constructed, which will be
		/// used to create the DisplayWindow.
		/// </summary>
		public CreateWindowParams CreateWindowParams => createParams;

		/// <summary>
		/// Constructs a single DisplayWindow object.
		/// </summary>
		/// <returns></returns>
		public DisplayWindow Build()
		{
			ParseCommandLineArgs();

			if (createParams.TargetScreen == null)
				createParams.TargetScreen = Display.Screens.PrimaryScreen;

			var result = new DisplayWindow(createParams);

			if (Display.RenderTarget == null)
				Display.RenderTarget = result.FrameBuffer;

			if (quitOnClose)
				result.Closed += DisplayWindow_Closed;

			ApplyProperties(result);

			return result;
		}

		/// <summary>
		/// Constructs a DisplayWindow object with a single canvas that spans
		/// all physical monitors.
		/// </summary>
		/// <remarks>The aspect ratio of the back buffer is modified to match
		/// the aspect ratio of the desktop.</remarks>
		/// <returns></returns>
		public DisplayWindow BuildForAllScreens()
		{
			ParseCommandLineArgs();

			createParams.CompleteDesktop = true;

			var result = new DisplayWindow(createParams);

			if (Display.RenderTarget == null)
				Display.RenderTarget = result.FrameBuffer;

			if (quitOnClose)
				result.Closed += DisplayWindow_Closed;

			ApplyProperties(result);

			return result;
		}

		/// <summary>
		/// Constructs a DisplayWindow object for each physical monitor
		/// attached to the system.
		/// </summary>
		/// <returns></returns>
		public DisplayWindowCollection BuildSeparateWindowsForAllScreens()
		{
			ParseCommandLineArgs();

			DisplayWindowCollection results = new DisplayWindowCollection();

			foreach (var screen in Display.Screens.AllScreens)
			{
				createParams.TargetScreen = screen;

				var result = new DisplayWindow(createParams);

				if (Display.RenderTarget == null)
					Display.RenderTarget = result.FrameBuffer;

				ApplyProperties(result);

				results.Add(result);
			}

			return results;
		}

		/// <summary>
		/// Sets the window title.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public DisplayWindowBuilder Title(string title)
		{
			createParams.Title = title;

			return this;
		}

		/// <summary>
		/// Sets the size of the DisplayWindow's back buffer. 
		/// </summary>
		/// <param name="size">The size of the buffer in pixels.</param>
		/// <param name="renderMode">An IRenderMode object which indicates
		/// how to transform the backbuffer when rendering it to the screen.
		/// If left null, then RenderMode.RetainAspectRation will be used
		/// to preserve the backbuffer's aspect ratio while scaling to take
		/// the full physical size of the DisplayWindow.</param>
		/// <returns></returns>
		public DisplayWindowBuilder BackbufferSize(Size size, IRenderMode renderMode = null)
		{
			createParams.Resolution = new Resolution(
				size,
				renderMode ?? createParams?.Resolution?.RenderMode ?? RenderMode.RetainAspectRatio);

			if (createParams.PhysicalSize == null)
			{
				createParams.PhysicalSize = new Size(
					(int)(Display.Screens.PrimaryScreen.Scaling * size.Width),
					(int)(Display.Screens.PrimaryScreen.Scaling * size.Height));
			}

			return this;
		}

		/// <summary>
		/// Sets the size of the DisplayWindow's back buffer. 
		/// </summary>
		/// <param name="width">The width of the buffer in pixels.</param>
		/// <param name="height">The height of the buffer in pixels.</param>
		/// <param name="renderMode">An IRenderMode object which indicates
		/// how to transform the backbuffer when rendering it to the screen.
		/// If left null, then RenderMode.RetainAspectRation will be used
		/// to preserve the backbuffer's aspect ratio while scaling to take
		/// the full physical size of the DisplayWindow.</param>
		/// <returns></returns>
		public DisplayWindowBuilder BackbufferSize(int width, int height, IRenderMode renderMode = null)
		{
			return BackbufferSize(new Size(width, height), renderMode);
		}

		/// <summary>
		/// Sets the physical size in pixels of the DisplayWindow.
		/// Command line arguments will override this. If this property is not
		/// set, the DisplayWindow's physical size will be chosen automaticall.
		/// If this is set, the user's desktop scaling preferences will be 
		/// ignored.
		/// </summary>
		/// <param name="size">Size of the display window in pixels</param>
		/// <returns></returns>
		public DisplayWindowBuilder PhysicalSize(Size size)
		{
			createParams.PhysicalSize = size;

			return this;
		}

		/// <summary>
		/// Indicates a full screen window should be created.
		/// This is the default behavior, and does not need to 
		/// be explicitly specified.
		/// </summary>
		/// <returns></returns>
		public DisplayWindowBuilder FullScreen()
		{
			createParams.IsFullScreen = true;

			return this;
		}

		/// <summary>
		/// Indicates a desktop window should be created, allowing the
		/// user to move the window around and have it overlap with other
		/// desktop windows.
		/// </summary>
		/// <returns></returns>
		public DisplayWindowBuilder Windowed()
		{
			createParams.IsFullScreen = false;

			return this;
		}

		/// <summary>
		/// Indicates the application should quit automatically when the 
		/// window is closed by the user.
		/// </summary>
		/// <returns></returns>
		public DisplayWindowBuilder QuitOnClose()
		{
			quitOnClose = true;

			return this;
		}

		/// <summary>
		/// Indicates which screen this display window should be created for.
		/// Default is to use the user's primary screen.
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public DisplayWindowBuilder TargetScreen(ScreenInfo screen)
		{
			createParams.TargetScreen = screen;

			return this;
		}

		/// <summary>
		/// Allows the user to resize the window by dragging the borders.
		/// This is ignored for full screen or render to control situations.
		/// This is true by default.
		/// </summary>
		public DisplayWindowBuilder AllowResize(bool value = true)
		{
			createParams.IsResizable = value;

			return this;
		}

		/// <summary>
		/// If true, this will automatically
		/// resize the backbuffer in response to window size change events.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public DisplayWindowBuilder AutoResizeBackBuffer(bool value = true)
		{
			autoResize = value;

			return this;
		}

		/// <summary>
		/// Sets the coordinate system for the display window.
		/// </summary>
		/// <param name="coords"></param>
		/// <returns></returns>
		public DisplayWindowBuilder WithCoordinates(ICoordinateSystem coords)
		{
			this.coords = coords;

			return this;
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
					Windowed();
					if (parameters.Count > 0)
						PhysicalSize(Size.FromString(parameters[0]));
					break;

				default:
					ProcessCustomArgument(arg, parameters);
					break;
			}
		}

		private void DisplayWindow_Closed(object sender, EventArgs e)
		{
			AgateApp.IsAlive = false;
		}

		private void ApplyProperties(DisplayWindow window)
		{
			ApplyAutoResize(window);
			ApplyCoordinateSystem(window);
		}

		private void ApplyCoordinateSystem(DisplayWindow window)
		{
			if (coords != null)
				window.FrameBuffer.CoordinateSystem = coords;
		}

		private void ApplyAutoResize(DisplayWindow window)
		{
			if (autoResize)
			{
				window.Resize += DisplayWindow_Resized;
			}
		}

		private void DisplayWindow_Resized(object sender, EventArgs e)
		{
			DisplayWindow window = (DisplayWindow)sender;

			window.Resolution = window.Resolution.Clone(window.PhysicalSize);
		}


		/// <summary>
		/// Processes command line arguments. 
		/// </summary>
		/// <remarks>
		/// Arguments are only processed if they start
		/// with a dash (-). Any arguments which do not start with a dash are considered
		/// parameters to the previous argument. For example, the argument string 
		/// <code>-window 640,480 test -novsync</code> would call ProcessArgument once
		/// for -window with the parameters <code>640,480 test</code> and again for
		/// -novsync.
		/// </remarks>
		private void ParseCommandLineArgs()
		{
			if (args == null)
				return;

			List<string> parameters = new List<string>();

			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				int extraArguments = args.Length - i - 1;

				parameters.Clear();

				for (int j = i + 1; j < args.Length; j++)
				{
					if (args[j].StartsWith("-") == false)
						parameters.Add(args[j]);
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

		private void ProcessCustomArgument(string arg, IList<string> parameters)
		{
		}
	}
}
