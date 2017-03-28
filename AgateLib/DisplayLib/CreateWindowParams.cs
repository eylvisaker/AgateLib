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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	///     Class which describes how a DisplayWindow should be created.
	///     Several static methods exist to allow
	/// </summary>
	public sealed class CreateWindowParams
	{
		#region --- Static creation methods ---

		/// <summary>
		///     Creates a CreateWindowParams object which describes rendering into a WinForms control.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FromControl(object control, ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				RenderToControl = true,
				RenderTarget = control,
			};

			return result;
		}

		/// <summary>
		///     Creates a CreateWindowParams object which describes a fullscreen window.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="resolution"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FullScreen(string title, IResolution resolution, ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				IsFullScreen = true,
				Title = title,
				Resolution = resolution,
			};

			return result;
		}

		/// <summary>
		///     Creates a CreateWindowParams object which describes a fullscreen window.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FullScreen(string title, int width, int height, int bpp,
			ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				IsFullScreen = true,
				Title = title,
				Resolution = new Resolution(width, height, null),
			};

			return result;
		}

		/// <summary>
		///     Creates a CreateWindowParams object which describes a typical window for non-fullscreen use.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="iconFile"></param>
		/// <param name="allowResize"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams Windowed(string title, int width, int height, bool allowResize, string iconFile,
			ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				Title = title,
				Resolution = new Resolution(width, height, null),
				IconFile = iconFile,
				IsResizable = allowResize,
				HasMaximize = allowResize,
			};

			return result;
		}

		/// <summary>
		///     Creates a CreateWindowParams object which describes a desktop window with no frame or
		///     titlebar.  This might be used for showing a splashscreen as the application loads.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams NoFrame(string title, int width, int height, ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				Title = title,
				Resolution = new Resolution(width, height),
				IsResizable = false,
				HasFrame = false,
			};

			return result;
		}

		#endregion
		
		/// <summary>
		///     Title of the window.
		/// </summary>
		public string Title { get; set; } = "AgateLib Application";

		/// <summary>
		///     Whether or not the window should be created as a full screen window.  Defaults to false.
		/// </summary>
		public bool IsFullScreen { get; set; }
		
		/// <summary>
		/// Sets the physical size of the display window. This is ignored for
		/// full screen or render-to-control situations. If set to null (the 
		/// default), the physical size of the display window will be set 
		/// according to the user's desktop scaling.
		/// </summary>
		public Size? PhysicalSize { get; set; }

		/// <summary>
		/// Indicates which screen the display winodw should be created on.
		/// </summary>
		public ScreenInfo TargetScreen { get; set; } = Display.Screens?.PrimaryScreen;

		/// <summary>
		///     The information about the window resolution.
		/// </summary>
		public IResolution Resolution { get; set; }

		/// <summary>
		///     Sets the initial position of the window.
		/// </summary>
		public WindowPosition WindowPosition { get; set; }

		/// <summary>
		///     Whether or not the user can manually resize the window.  Defaults to false.  Ignored
		///     for full-screen windows.
		/// </summary>
		public bool IsResizable { get; set; }

		/// <summary>
		///     Whether or not the window is drawn with a frame and titlebar.  This property is ignored
		///     for fullscreen windows.  Defaults to true.
		/// </summary>
		public bool HasFrame { get; set; } = true;

		/// <summary>
		///     Whether or not the window has a maximize button.  In general, this should be equal to the
		///     IsResizable property.
		/// </summary>
		public bool HasMaximize { get; set; }

		/// <summary>
		///     Whether or not the window has a minimize button.  This should generally be true.
		/// </summary>
		public bool HasMinimize { get; set; } = true;

		/// <summary>
		///     Path to a .ico file to use for the window icon.
		/// </summary>
		public string IconFile { get; set; } = "";

		/// <summary>
		///     True if we are in fact rendering to a WinForms control, rather than creating
		///     a window to be managed by AgateLib.
		/// </summary>
		public bool RenderToControl { get; set; }

		/// <summary>
		///     Control to be rendered to.  This is ignored if RenderToControl is false.
		/// </summary>
		public object RenderTarget { get; set; }

		/// <summary>
		/// If CompleteDesktop == true and IsFullScreen == true, this instruct the platform
		/// to build a DisplayWindow with a canvas that covers all monitors.
		/// </summary>
		public bool CompleteDesktop { get; set; }
	}
}