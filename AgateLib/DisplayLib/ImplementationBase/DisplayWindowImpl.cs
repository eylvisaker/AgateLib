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
using System.Text;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Implementation of DisplayWindow class.
	/// </summary>
	public abstract class DisplayWindowImpl : IDisposable
	{
		/// <summary>
		/// Event raised when the window is resized by the user.
		/// </summary>
		public event EventHandler Resize;

		/// <summary>
		/// Event raised when the window is closed by the user.
		/// </summary>
		public event EventHandler Closed;

		/// <summary>
		/// Event raised when the user clicks the close box but before the window is closed.
		/// </summary>
		public event CancelEventHandler Closing;

		/// <summary>
		/// Event raised when there is an input event.
		/// </summary>
		public event EventHandler<AgateInputEventArgs> InputEvent;

		/// <summary>
		/// Disposes of the DisplayWindowImpl object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Override to clean up resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Closed?.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Returns true if the DisplayWindowImpl has been closed.
		/// This happens if the user clicks the close box, or Dispose is called.
		/// </summary>
		public abstract bool IsClosed { get; }

		/// <summary>
		/// Returns true if this DisplayWindowImpl is being used as a full-screen
		/// device.
		/// </summary>
		public abstract bool IsFullScreen { get; }

		/// <summary>
		/// Returns the frame buffer that is rendered to for rendering to this
		/// window.
		/// </summary>
		public abstract FrameBufferImpl FrameBuffer { get; }

		public abstract IResolution Resolution { get; set; }

		/// <summary>
		/// Gets or sets the size of the render area.
		/// </summary>
		public Size Size
		{
			get { return Resolution.Size; }
			set
			{
				var newRes = Resolution.Clone(value);

				Resolution = newRes;
			}
		}

		/// <summary>
		/// Gets or sets the width of the render area.
		/// </summary>
		public int Width => Size.Width;

		/// <summary>
		/// Gets or sets the height of the render area.
		/// </summary>
		public int Height => Size.Height;

		/// <summary>
		/// Gets or sets the window title.
		/// </summary>
		public abstract string Title { get; set; }

		public abstract Size PhysicalSize { get; }
		public abstract ScreenInfo Screen { get; }

		/// <summary>
		/// Event raised when the window is resized by the user.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnResize(IDisplayWindow owner)
		{
			Resize?.Invoke(owner, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when the window is closed by the user.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnClosed(IDisplayWindow owner)
		{
			Closed?.Invoke(owner, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when the user clicks the close box but before the window is closed.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnClosing(IDisplayWindow owner, ref bool cancel)
		{
			Closing?.Invoke(owner, ref cancel);
		}

		/// <summary>
		/// Event raised when there is any mouse/keyboard input from the user.
		/// </summary>
		/// <param name="args"></param>
		protected virtual void OnInputEvent(IDisplayWindow owner, AgateInputEventArgs args)
		{
			InputEvent?.Invoke(owner, args);
		}

		/// <summary>
		/// Converts a pixel location on screen to the logical coordinate system used by AgateLib.
		/// This function is primarily for supporting input mouse and touch events.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public virtual Point PixelToLogicalCoords(Point point)
		{
			var coords = FrameBuffer.CoordinateSystem.Coordinates;

			double x = point.X / (double)Width;
			double y = point.Y / (double)Height;

			Point result = new Point(
				(int)(x * coords.Width),
				(int)(y * coords.Height));

			result.X += coords.X;
			result.Y += coords.Y;

			return result;
		}

		/// <summary>
		/// Converts a logical coordinate to actual pixel coordinates.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public Point LogicalToPixelCoords(Point point)
		{
			var coords = FrameBuffer.CoordinateSystem.Coordinates;

			point.X -= coords.X;
			point.Y -= coords.Y;

			double x = point.X / (double)coords.Width;
			double y = point.Y / (double)coords.Height;

			return new Point(
				(int)(x * Width),
				(int)(y * Height));
		}
	}
}