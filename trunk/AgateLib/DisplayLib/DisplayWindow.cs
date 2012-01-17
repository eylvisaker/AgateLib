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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// A class representing a screen region which is used as a RenderTarget.
	/// </summary>
	/// <remarks>
	/// Creating a DisplayWindow can be done in two ways.  By specifying
	/// a title and width and height, the DisplayWindow will create and manage
	/// a window.
	/// 
	/// Alternatively, a control may be specified to render into.
	/// </remarks>
	public sealed class DisplayWindow : IDisposable
	{
		DisplayWindowImpl mImpl;
		FrameBuffer mFrameBuffer;

		/// <summary>
		/// Constructs a DisplayWindow from a resource.
		/// </summary>
		/// <param name="resources"></param>
		/// <param name="name"></param>
		public DisplayWindow(Resources.AgateResourceCollection resources, string name)
		{
			Resources.AgateResource res = resources[name];
			Resources.DisplayWindowResource disp = res as Resources.DisplayWindowResource;

			if (disp == null)
				throw new Resources.AgateResourceException("Resource " + name + " was found, but was of type " + name.GetType().ToString() + ", not DisplayWindowResource.");


			if (disp.FullScreen)
			{
				CreateWindowParams par = CreateWindowParams.FullScreen(
					disp.Title, disp.Size.Width, disp.Size.Height, disp.Bpp);

				mImpl = Display.Impl.CreateDisplayWindow(par);
			}
			else
			{
				CreateWindowParams par = CreateWindowParams.Windowed(
					disp.Title, disp.Size.Width, disp.Size.Height, disp.AllowResize, null);

				mImpl = Display.Impl.CreateDisplayWindow(par);
			}

			Display.RenderTarget = FrameBuffer;
			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
		}
		/// <summary>
		/// Creates a DisplayWindow object using the specified CreateWindowParams to create
		/// the window.
		/// </summary>
		/// <param name="windowParams"></param>
		public DisplayWindow(CreateWindowParams windowParams)
		{
			if (Display.Impl == null)
				throw new AgateException(
					"Display has not been initialized." + Environment.NewLine +
					"Did you forget to call AgateSetup.Initialize or Display.Initialize?");

			mImpl = Display.Impl.CreateDisplayWindow(windowParams);

			Display.RenderTarget = FrameBuffer;
			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);

			// TODO: Fix this hack
			Display.CurrentWindow = this;
		}

		#region --- Static Creation Methods ---

		/// <summary>
		/// Creates a DisplayWindow object using the specified System.Windows.Forms.Control
		/// object as a render context.  A DisplayWindow made in this manner cannot be made
		/// into a full-screen DisplayWindow.
		/// </summary>
		/// <remarks>Calling this function is equivalent to calling
		/// new DisplayWindow(CreateWindowParams.FromControl(control)).</remarks>
		/// <param name="control">Windows.Forms control which should be used as the
		/// render target.</param>
		public static DisplayWindow CreateFromControl(object control)
		{
			return new DisplayWindow(CreateWindowParams.FromControl(control));
		}
		/// <summary>
		/// Creates a DisplayWindow object which renders to the entire screen, setting
		/// the resolution to the value specified.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static DisplayWindow CreateFullScreen(string title, int width, int height)
		{
			return new DisplayWindow(CreateWindowParams.FullScreen(title, width, height, 32));
		}
		/// <summary>
		/// Creates a DisplayWindow object which generates a desktop window to render into.
		/// This overload creates a window which has the default icon and is not resizeable.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static DisplayWindow CreateWindowed(string title, int width, int height)
		{
			return DisplayWindow.CreateWindowed(title, width, height, false, null);
		}
		/// <summary>
		/// Creates a DisplayWindow object which generates a desktop window to render into.
		/// This overload creates a window which has the default icon and is not resizeable.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="allowResize"></param>
		/// <returns></returns>
		public static DisplayWindow CreateWindowed(string title, int width, int height, bool allowResize)
		{
			return DisplayWindow.CreateWindowed(title, width, height, allowResize, null);
		}
		/// <summary>
		/// Creates a DisplayWindow object which generates a desktop window to render into.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="iconFile"></param>
		/// <param name="allowResize"></param>
		/// <returns></returns>
		public static DisplayWindow CreateWindowed(string title, int width, int height, bool allowResize, string iconFile)
		{
			return new DisplayWindow(CreateWindowParams.Windowed(title, width, height, allowResize, iconFile));
		}
		/// <summary>
		/// Creates a DisplayWindow object which is a desktop window with no frame or
		/// titlebar.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static DisplayWindow CreateNoFrame(string title, int width, int height)
		{
			return new DisplayWindow(CreateWindowParams.NoFrame(title, width, height));
		}

		#endregion

		/// <summary>
		/// Gets the FrameBuffer object which represents the memory to draw to.
		/// </summary>
		public FrameBuffer FrameBuffer
		{
			get
			{
				if (mFrameBuffer == null || mFrameBuffer.Impl != Impl.FrameBuffer)
				{
					mFrameBuffer = new FrameBuffer(Impl.FrameBuffer);
				}

				return mFrameBuffer;
			}
		}

		/// <summary>
		/// Compatibility conversion.  Read from the FrameBuffer property instead.
		/// </summary>
		/// <param name="wind"></param>
		/// <returns></returns>
		[Obsolete("Read from wind.FrameBuffer instead.")]
		public static implicit operator FrameBuffer(DisplayWindow wind)
		{
			return wind.FrameBuffer;
		}

		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (mImpl != null)
			{
				mImpl.Dispose();
				mImpl = null;
			}
		}
		/// <summary>
		/// Returns true if this DisplayWindow has been closed, either
		/// by a call to Dispose(), or perhaps the user clicked the close
		/// box in a form.
		/// </summary>
		public bool IsClosed
		{
			get
			{
				if (mImpl == null)
					return true;

				return mImpl.IsClosed;
			}
		}

		/// <summary>
		/// Gets or sets the size of the client area in pixels.
		/// </summary>
		public Size Size
		{
			get { return mImpl.Size; }
			set { mImpl.Size = value; }
		}
		/// <summary>
		/// Gets or sets the width of the client area in pixels.
		/// </summary>
		public int Width
		{
			get { return Size.Width; }
			set
			{
				Size = new Size(value, Size.Height);
			}
		}
		/// <summary>
		/// Gets or sets the height of the client area in pixels.
		/// </summary>
		public int Height
		{
			get { return Size.Height; }
			set
			{
				Size = new Size(Size.Width, value);
			}
		}

		/// <summary>
		/// Gets or sets the position of the cursor, in the 
		/// client coordinates of the window.
		/// </summary>
		public Point MousePosition
		{
			get { return mImpl.MousePosition; }
			set
			{
				mImpl.MousePosition = value;
			}
		}
		/// <summary>
		/// Returns the DisplayWindowImpl object.
		/// </summary>
		public DisplayWindowImpl Impl
		{
			get { return mImpl; }
		}

		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		public string Title
		{
			get { return mImpl.Title; }
			set { mImpl.Title = value; }
		}

		/// <summary>
		/// Returns true if this window is displayed fullscreen.
		/// </summary>
		public bool IsFullScreen
		{
			get { return mImpl.IsFullScreen; }
		}
		/// <summary>
		/// Sets the display to windowed.  Does nothing if the display is already
		/// windowed.  The DisplayWindow retains the same height and width as the
		/// previous full screen resolution.
		/// </summary>
		public void SetWindowed()
		{
			mImpl.SetWindowed();
		}

		/// <summary>
		/// Sets the display to a full screen Display.  This overload uses the
		/// desktop resolution for the full-screen Display.
		/// </summary>
		/// <remarks>
		/// This call is not guaranteed to work; some drivers (eg. GDI) don't support 
		/// fullscreen displays.  If this fails it returns without any error
		/// thrown.  Check to see if it worked by examining IsFullScreen property.
		/// </remarks>
		public void SetFullScreen()
		{
			mImpl.SetFullScreen();
		}
		/// <summary>
		/// Sets the display to a full screen Display.  The resolution chosen is 
		/// driver/video card/monitor dependent, but it should be fairly close to
		/// values specified.
		/// </summary>
		/// <remarks>
		/// This call is not guaranteed to work; some drivers (eg. GDI) don't support 
		/// fullscreen displays.  If this fails it returns without any error
		/// thrown.  Check to see if it worked by examining IsFullScreen property.
		/// </remarks>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		public void SetFullScreen(int width, int height, int bpp)
		{
			mImpl.SetFullScreen(width, height, bpp);
		}

		/// <summary>
		/// Creates an orthogonal projection matrix that maps drawing units onto pixels.
		/// </summary>
		public Matrix4x4 OrthoProjection
		{
			get { return Matrix4x4.Ortho(new RectangleF(0, 0, Width, Height), -1, 1); }
		}

		/// <summary>
		/// Event raised when the window is resized by the user.
		/// </summary>
		public event EventHandler Resize
		{
			add { mImpl.Resize += value; }
			remove { mImpl.Resize -= value; }
		}

		/// <summary>
		/// Event raised when the window is closed by the user.
		/// </summary>
		public event EventHandler Closed
		{
			add { mImpl.Closed += value; }
			remove { mImpl.Closed -= value; }
		}

		/// <summary>
		/// Event raised when the user clicks the close box but before the window is closed.
		/// </summary>
		public event CancelEventHandler Closing
		{
			add { mImpl.Closing += value; }
			remove { mImpl.Closing -= value; }
		}

	}

	public delegate void CancelEventHandler(object sender, ref bool cancel);

}