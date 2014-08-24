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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.Geometry;
using AgateLib.Utility;
using AgateLib.ApplicationModels;
using AgateLib.Geometry.CoordinateSystems;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Enum which describes what position the window should be created at on screen.
	/// </summary>
	public enum WindowPosition
	{
		/// <summary>
		/// Lets AgateLib choose where to position the window.  
		/// </summary>
		DefaultAgate,

		/// <summary>
		/// Let the runtime decide where the window is placed.
		/// </summary>
		DefaultOS,

		/// <summary>
		/// Center the window horizontally on screen, but vertically above center.
		/// This often looks better because the vertical center of the monitor is usually 
		/// positioned below eye-level.
		/// </summary>
		AboveCenter,

		/// <summary>
		/// Center the window on the screen.
		/// </summary>
		CenterScreen,



	}
	/// <summary>
	/// Class which describes how a DisplayWindow should be created.
	/// Several static methods exist to allow 
	/// </summary>
	public sealed class CreateWindowParams
	{
		#region --- Private Fields ---

		private bool mIsFullScreen = false;
		private Size mSize = new Size(1024, 768);
		private WindowPosition mPosition;
		private int mBpp = 32;
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
		/// Size of the window.  Defaults to 1024x768.
		/// </summary>
		public Size Size
		{
			get { return mSize; }
			set { mSize = value; }
		}
		/// <summary>
		/// Width of the window
		/// </summary>
		public int Width
		{
			get { return mSize.Width; }
			set { mSize.Width = value; }
		}
		/// <summary>
		/// Height of the window
		/// </summary>
		public int Height
		{
			get { return mSize.Height; }
			set { mSize.Height = value; }
		}
		/// <summary>
		/// Sets the initial position of the window.
		/// </summary>
		public WindowPosition WindowPosition
		{
			get { return mPosition; }
			set { mPosition = value; }
		}


		/// <summary>
		/// Bit depth for the framebuffer for the window.  This defaults to 32.  This
		/// field is (usually) ignored if we are not creating a full-screen window.
		/// </summary>
		public int Bpp
		{
			get { return mBpp; }
			set { mBpp = value; }
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
		public ICoordinateSystemCreator Coordinates { get; set; }

		#endregion

		#region --- Static creation methods ---

		/// <summary>
		/// Creates a CreateWindowParams object which describes rendering into a WinForms control.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="coordinates">Coordinate system creator object. May be null</param>
		/// <returns></returns>
		public static CreateWindowParams FromControl(object control, ICoordinateSystemCreator coordinates)
		{
			CreateWindowParams retval = new CreateWindowParams();

			retval.RenderToControl = true;
			retval.RenderTarget = control;
			retval.Coordinates = coordinates ?? new NativeCoordinates();

			return retval;
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
		public static CreateWindowParams FullScreen(string title, int width, int height, int bpp, ICoordinateSystemCreator coordinates)
		{
			CreateWindowParams retval = new CreateWindowParams();

			retval.IsFullScreen = true;
			retval.Title = title;
			retval.Width = width;
			retval.Height = height;
			retval.mBpp = bpp;
			retval.Coordinates = coordinates ?? new NativeCoordinates();

			return retval;
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
		public static CreateWindowParams Windowed(string title, int width, int height, bool allowResize, string iconFile, ICoordinateSystemCreator coordinates)
		{
			CreateWindowParams retval = new CreateWindowParams();

			retval.Title = title;
			retval.Width = width;
			retval.Height = height;
			retval.IconFile = iconFile;
			retval.IsResizable = allowResize;
			retval.HasMaximize = allowResize;
			retval.Coordinates = coordinates ?? new NativeCoordinates();

			return retval;
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
		public static CreateWindowParams NoFrame(string title, int width, int height, ICoordinateSystemCreator coordinates)
		{
			CreateWindowParams retval = new CreateWindowParams();

			retval.Title = title;
			retval.Width = width;
			retval.Height = height;
			retval.IsResizable = false;
			retval.HasFrame = false;
			retval.Coordinates = coordinates ?? new NativeCoordinates();

			return retval;
		}

		#endregion

	}
}
