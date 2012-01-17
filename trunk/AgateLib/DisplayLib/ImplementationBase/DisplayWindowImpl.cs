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
using AgateLib.Utility;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Implementation of DisplayWindow class.
	/// </summary>
	public abstract class DisplayWindowImpl : IDisposable
	{
		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public abstract void Dispose();
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

		/// <summary>
		/// Sets the display to windowed.  Does nothing if the display is already
		/// windowed.  The DisplayWindow retains the same height and width as the
		/// previous full screen resolution.
		/// </summary>
		public abstract void SetWindowed();

		/// <summary>
		/// Sets the display to a full screen Display.  This overload should use the
		/// same resolution as the desktop environment.
		/// </summary>
		public abstract void SetFullScreen();
		/// <summary>
		/// Sets the display to a full screen Display.  The resolution chosen is 
		/// driver/video card/monitor dependent, but it should be fairly close to
		/// values specified.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bpp"></param>
		public abstract void SetFullScreen(int width, int height, int bpp);

		/// <summary>
		/// Gets or sets the size of the render area.
		/// </summary>
		public abstract Size Size { get; set; }

		/// <summary>
		/// Gets or sets the width of the render area.
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
		/// Gets or sets the height of the render area.
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
		/// Gets or sets the window title.
		/// </summary>
		public abstract string Title { get; set; }
		/// <summary>
		/// Gets or sets the mouse position within the render area.
		/// </summary>
		public abstract Point MousePosition { get; set; }


		/// <summary>
		/// Event raised when the window is resized by the user.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnResize()
		{
			if (Resize != null)
				Resize(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when the window is closed by the user.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnClosed()
		{
			if (Closed != null)
				Closed(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when the user clicks the close box but before the window is closed.
		/// Be sure to call the base class method so that client events are raised.
		/// </summary>
		protected virtual void OnClosing(ref bool cancel)
		{
			if (Closing != null)
			{
				Closing(this, ref cancel);
			}
		}

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

	}
}