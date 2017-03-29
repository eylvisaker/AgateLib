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
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.InputLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Interface for a display window. A display window is an area on the screen where the images
	/// drawn by the game will appear for the user.
	/// </summary>
	public interface IDisplayWindow : IDisposable, IDisplayContext
	{
		/// <summary>
		/// Gets the FrameBuffer object which represents the memory to draw to.
		/// </summary>
		FrameBuffer FrameBuffer { get; }

		/// <summary>
		/// Returns true if this DisplayWindow has been closed, either
		/// by a call to Dispose(), or perhaps the user clicked the close
		/// box in a form.
		/// </summary>
		bool IsClosed { get; }

		/// <summary>
		/// Gets or sets the size of the client area in pixels.
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Returns true if this window is displayed fullscreen.
		/// </summary>
		bool IsFullScreen { get; }

		/// <summary>
		/// Gets or sets the IResolution object that describes the backbuffer.
		/// </summary>
		IResolution Resolution { get; set; }

		/// <summary>
		/// Gets the "physical" size of the DisplayWindow - the size in pixels it is
		/// on the desktop.
		/// </summary>
		Size PhysicalSize { get; }

		/// <summary>
		/// Gets the screen this display window was originally created on. This property is not updated
		/// if the window is moved to another monitor.
		/// </summary>
		ScreenInfo Screen { get; }

		/// <summary>
		/// Event raised when the window is resized by the user.
		/// </summary>
		event EventHandler Resize;

		/// <summary>
		/// Event raised when the window is closed by the user.
		/// </summary>
		event EventHandler Closed;

		/// <summary>
		/// Event raised when the user clicks the close box but before the window is closed.
		/// </summary>
		event CancelEventHandler Closing;
	}

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
	public sealed class DisplayWindow : IDisplayWindow
	{
		DisplayWindowImpl mImpl;
		FrameBuffer mFrameBuffer;

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

			mImpl = AgateApp.State.Factory.DisplayFactory.CreateDisplayWindow(this, windowParams);
			mImpl.InputEvent += InputEvent;

			Display.RenderTarget = FrameBuffer;
			Display.DisposeDisplay += Dispose;

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
		/// <param name="coordinates"></param>
		public static DisplayWindow CreateFromControl(object control, ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.FromControl(control, coordinates));
		}
		/// <summary>
		/// Creates a DisplayWindow object which renders to the entire screen, setting
		/// the resolution to the value specified.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="size"></param>
		/// <param name="coordinates"></param>
		/// <returns></returns>
		public static DisplayWindow CreateFullScreen(string title, Size size, ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.FullScreen(title, size.Width, size.Height, 32, coordinates));
		}
		/// <summary>
		/// Creates a DisplayWindow object which renders to the entire screen, setting
		/// the resolution to the value specified.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static DisplayWindow CreateFullScreen(string title, int width, int height, ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.FullScreen(title, width, height, 32, null));
		}

		/// <summary>
		/// Creates a DisplayWindow object which renders to the entire screen, setting
		/// the resolution to the value specified.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="resolution"></param>
		/// <returns></returns>
		public static DisplayWindow CreateFullScreen(string title, IResolution resolution,
			ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.FullScreen(title, resolution, coordinates));
		}

		/// <summary>
		/// Creates a DisplayWindow object which generates a desktop window to render into.
		/// This overload creates a window which has the default icon and is not resizeable.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static DisplayWindow CreateWindowed(string title, Size size, ICoordinateSystem coordinates = null)
		{
			return DisplayWindow.CreateWindowed(title, size.Width, size.Height, false, null, coordinates);
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
		public static DisplayWindow CreateWindowed(string title, int width, int height, bool allowResize = false, string iconFile = null, ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.Windowed(title, width, height, allowResize, iconFile, coordinates));
		}

		/// <summary>
		/// Creates a DisplayWindow object which is a desktop window with no frame or
		/// titlebar.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static DisplayWindow CreateNoFrame(string title, int width, int height, ICoordinateSystem coordinates = null)
		{
			return new DisplayWindow(CreateWindowParams.NoFrame(title, width, height, null));
		}

		#endregion

		FrameBuffer IDisplayContext.RenderTarget => FrameBuffer;

		IPrimitiveRenderer IDisplayContext.Primitives => Display.Primitives;

		/// <summary>
		/// Gets the FrameBuffer object which represents the memory to draw to.
		/// </summary>
		public FrameBuffer FrameBuffer
		{
			get
			{
				if (Impl == null)
					return null;

				if (mFrameBuffer == null || mFrameBuffer.Impl != Impl.FrameBuffer)
				{
					mFrameBuffer = new FrameBuffer(Impl.FrameBuffer);
				}

				return mFrameBuffer;
			}
		}

		/// <summary>
		/// Disposes of the DisplayWindow.
		/// </summary>
		public void Dispose()
		{
			if (mImpl != null)
			{
				mImpl.Dispose();
				mImpl = null;
			}

			Display.DisposeDisplay -= Dispose;
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
		}

		/// <summary>
		/// Gets the width of the client area in pixels.
		/// </summary>
		public int Width
		{
			get { return Size.Width; }
		}

		/// <summary>
		/// Gets the height of the client area in pixels.
		/// </summary>
		public int Height
		{
			get { return Size.Height; }
		}

		/// <summary>
		/// Returns the DisplayWindowImpl object.
		/// </summary>
		public DisplayWindowImpl Impl => mImpl;

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
		public bool IsFullScreen => mImpl.IsFullScreen;

		/// <summary>
		/// Creates an orthogonal projection matrix that maps drawing units onto pixels.
		/// </summary>
		public Matrix4x4 OrthoProjection
		{
			get { return Matrix4x4.Ortho(new RectangleF(0, 0, Width, Height), -1, 1); }
		}

		/// <summary>
		/// Gets or sets the IResolution object that describes the backbuffer.
		/// </summary>
		public IResolution Resolution
		{
			get { return mImpl.Resolution; }
			set { mImpl.Resolution = value; }
		}

		/// <summary>
		/// Gets the screen this DisplayWindow was created on.
		/// </summary>
		public ScreenInfo Screen => Impl.Screen;

		/// <summary>
		/// Gets the "physical" size of the DisplayWindow - the size in pixels it is
		/// on the desktop.
		/// </summary>
		public Size PhysicalSize => mImpl.PhysicalSize;

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

		private void InputEvent(object sender, AgateInputEventArgs e)
		{
			switch (e.InputEventType)
			{
				case InputEventType.MouseDown:
				case InputEventType.MouseMove:
				case InputEventType.MouseUp:
				case InputEventType.MouseWheel:
					e.MouseWindow = this;
					break;
			}

			Input.QueueInputEvent(e);
		}

	}

	public delegate void CancelEventHandler(object sender, ref bool cancel);

}