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
using System.Text;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Utility;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which describes how a DisplayWindow should be created.
	/// Several static methods exist to allow 
	/// </summary>
	public sealed class CreateWindowParams
	{
		#region --- Static creation methods ---

		/// <summary>
		/// Creates a CreateWindowParams object which describes rendering into a WinForms control.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FromControl(object control, ICoordinateSystem coordinates)
		{
			CreateWindowParams result = new CreateWindowParams();

			result.RenderToControl = true;
			result.RenderTarget = control;
			result.Coordinates = coordinates ?? new NativeCoordinates();

			return result;
		}

		/// <summary>
		/// Creates a CreateWindowParams object which describes a fullscreen window.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FullScreen(string title, IResolution resolution, ICoordinateSystem coordinates)
		{
			CreateWindowParams result = new CreateWindowParams();

			result.IsFullScreen = true;
			result.Title = title;
			result.Resolution = resolution;
			result.Coordinates = coordinates ?? new NativeCoordinates();

			return result;
		}

		/// <summary>
		/// Creates a CreateWindowParams object which describes a fullscreen window.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FullScreen(string title, int width, int height, int bpp, ICoordinateSystem coordinates)
		{
			CreateWindowParams result = new CreateWindowParams();

			result.IsFullScreen = true;
			result.Title = title;
			result.Resolution = new Resolution(width, height, null);
			result.Coordinates = coordinates ?? new NativeCoordinates();

			return result;
		}
		/// <summary>
		/// Creates a CreateWindowParams object which describes a typical window for non-fullscreen use.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="iconFile"></param>
		/// <param name="allowResize"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams Windowed(string title, int width, int height, bool allowResize, string iconFile, ICoordinateSystem coordinates)
		{
			CreateWindowParams result = new CreateWindowParams();

			result.Title = title;
			result.Resolution = new Resolution(width, height, null);
			result.IconFile = iconFile;
			result.IsResizable = allowResize;
			result.HasMaximize = allowResize;
			result.Coordinates = coordinates ?? new NativeCoordinates();

			return result;
		}

		/// <summary>
		/// Creates a CreateWindowParams object which describes a desktop window with no frame or
		/// titlebar.  This might be used for showing a splashscreen as the application loads.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams NoFrame(string title, int width, int height, ICoordinateSystem coordinates)
		{
			CreateWindowParams result = new CreateWindowParams();

			result.Title = title;
			result.Resolution = new Resolution(width, height);
			result.IsResizable = false;
			result.HasFrame = false;
			result.Coordinates = coordinates ?? new NativeCoordinates();

			return result;
		}

		#endregion

		#region --- Private Fields ---

		private bool mIsFullScreen = false;
		private Size mSize = new Size(1024, 768);
		private WindowPosition mPosition;
		private bool mIsResizable = false;

		private bool mHasFrame = true;
		private bool mHasMaximize = false;
		private bool mHasMinimize = true;

		private string mIconFile = "";

		private bool mRenderToControl = false;
		private object mRenderTarget = null;

		private string mTitle = "AgateLib Application";

		#endregion
		#region --- Properties ---

		/// <summary>
		/// Title of the window.
		/// </summary>
		public string Title
		{
			get { return mTitle; }
			set { mTitle = value; }
		}
		/// <summary>
		/// Whether or not the window should be created as a full screen window.  Defaults to false.
		/// </summary>
		public bool IsFullScreen
		{
			get { return mIsFullScreen; }
			set { mIsFullScreen = value; }
		}

		/// <summary>
		/// The information about the window resolution.
		/// </summary>
		public IResolution Resolution { get; set; }

		/// <summary>
		/// Sets the initial position of the window.
		/// </summary>
		public WindowPosition WindowPosition
		{
			get { return mPosition; }
			set { mPosition = value; }
		}

		/// <summary>
		/// Whether or not the user can manually resize the window.  Defaults to false.  Ignored
		/// for full-screen windows.
		/// </summary>
		public bool IsResizable
		{
			get { return mIsResizable; }
			set { mIsResizable = value; }
		}

		/// <summary>
		/// Whether or not the window is drawn with a frame and titlebar.  This property is ignored
		/// for fullscreen windows.  Defaults to true.
		/// </summary>
		public bool HasFrame
		{
			get { return mHasFrame; }
			set { mHasFrame = value; }
		}

		/// <summary>
		/// Whether or not the window has a maximize button.  In general, this should be equal to the
		/// IsResizable property.
		/// </summary>
		public bool HasMaximize
		{
			get { return mHasMaximize; }
			set { mHasMaximize = value; }
		}

		/// <summary>
		/// Whether or not the window has a minimize button.  This should generally be true.
		/// </summary>
		public bool HasMinimize
		{
			get { return mHasMinimize; }
			set { mHasMinimize = value; }
		}

		/// <summary>
		/// Path to a .ico file to use for the window icon.
		/// </summary>
		public string IconFile
		{
			get { return mIconFile; }
			set
			{
				mIconFile = value;
			}
		}

		/// <summary>
		/// True if we are in fact rendering to a WinForms control, rather than creating
		/// a window to be managed by AgateLib.
		/// </summary>
		public bool RenderToControl
		{
			get { return mRenderToControl; }
			set { mRenderToControl = value; }
		}

		/// <summary>
		/// Control to be rendered to.  This is ignored if RenderToControl is false.
		/// </summary>
		public object RenderTarget
		{
			get { return mRenderTarget; }
			set { mRenderTarget = value; }
		}

		/// <summary>
		/// The object which will be used to set the coordinate system for the window
		/// at the beginning of each frame.
		/// </summary>
		public ICoordinateSystem Coordinates { get; set; }

		#endregion

	}
}
