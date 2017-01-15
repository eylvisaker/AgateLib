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

using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;

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
				Coordinates = coordinates ?? new NativeCoordinates()
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
		public static CreateWindowParams FullScreen(string title, IResolution resolution, ICoordinateSystem coordinates)
		{
			var result = new CreateWindowParams
			{
				IsFullScreen = true,
				Title = title,
				Resolution = resolution,
				Coordinates = coordinates ?? new NativeCoordinates()
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
				Coordinates = coordinates ?? new NativeCoordinates()
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
				Coordinates = coordinates ?? new NativeCoordinates()
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
				Coordinates = coordinates ?? new NativeCoordinates()
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
		///     The object which will be used to set the coordinate system for the window
		///     at the beginning of each frame.
		/// </summary>
		public ICoordinateSystem Coordinates { get; set; }
	}
}