//     ``The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.Geometry;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
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
    public class DisplayWindow : IRenderTarget, IDisposable
    {
        DisplayWindowImpl impl;

        /// <summary>
        /// Creates a DisplayWindow object using the specified System.Windows.Forms.Control
        /// object as a render context.  A DisplayWindow made in this manner cannot be made
        /// into a full-screen DisplayWindow.
        /// </summary>
        /// <param name="renderTarget">Windows.Forms control which should be used as the
        /// render target.</param>
        public DisplayWindow(System.Windows.Forms.Control renderTarget)
        {
            impl = Display.Impl.CreateDisplayWindow(renderTarget);

            Display.RenderTarget = this;
            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
        }
        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed Form.
        /// By default, this window does not allow the user to resize it.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="clientWidth"></param>
        /// <param name="clientHeight"></param>
        public DisplayWindow(string title, int clientWidth, int clientHeight)
            : this(title, clientWidth, clientHeight, "", false, false)
        {

        }
        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed Form.
        /// By default, this window does not allow the user to resize it.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of the drawing area in pixels.</param>
        /// <param name="clientHeight">Height of the drawing area in pixels.</param>
        /// <param name="iconFile">File name of a Win32 .ico file to use for the window icon.  Pass
        /// null or "" to not use an icon.</param>
        public DisplayWindow(string title, int clientWidth, int clientHeight, string iconFile)
            : this(title, clientWidth, clientHeight, "", false, false)
        {

        }
        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed or fullscreen Form.
        /// By default, this window does not allow the user to resize it.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of the drawing area in pixels.</param>
        /// <param name="clientHeight">Height of the drawing area in pixels.</param>
        /// <param name="iconFile">File name of a Win32 .ico file to use for the window icon.</param>
        /// <param name="startFullscreen">True to start as a full screen window.</param>
        public DisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullscreen)
            : this(title, clientWidth, clientHeight, iconFile, startFullscreen, false)
        {

        }
        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed or fullscreen Form.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of the drawing area in pixels.</param>
        /// <param name="clientHeight">Height of the drawing area in pixels.</param>
        /// <param name="iconFile">File name of a Win32 .ico file to use for the window icon.</param>
        /// <param name="startFullscreen">True to start as a full screen window.</param>
        /// <param name="allowResize">True to allow the user to manually resize the window by
        /// dragging the border.</param>
        public DisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullscreen, bool allowResize)
        {
            impl = Display.Impl.CreateDisplayWindow(title, clientWidth, clientHeight, 
                FileManager.ImagePath.FindFileName(iconFile), startFullscreen, allowResize);

            Display.RenderTarget = this;
            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
        }

        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed or fullscreen Form.
        /// By default, this window does not allow the user to resize it.
        /// 
        /// <para><b>Deprecated.</b>  Use an overload which includes the icon File argument.</para>
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of the drawing area in pixels.</param>
        /// <param name="clientHeight">Height of the drawing area in pixels.</param>
        /// <param name="startFullscreen">True to start as a full screen window.</param>
        [Obsolete("Use an overload which includes the iconFile argument, and pass an empty string.")]
        public DisplayWindow(string title, int clientWidth, int clientHeight, bool startFullscreen)
            : this(title, clientWidth, clientHeight, startFullscreen, false)
        {

        }
        /// <summary>
        /// Creates a DisplayWindow object by creating a windowed or fullscreen Form.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="clientWidth"></param>
        /// <param name="clientHeight"></param>
        /// <param name="startFullscreen"></param>
        /// <param name="allowResize"></param>
        [Obsolete("Use an overload which includes the iconFile argument.")]
        public DisplayWindow(string title, int clientWidth, int clientHeight, bool startFullscreen, bool allowResize)
            : this(title, clientWidth, clientHeight, "", startFullscreen, allowResize)
        {
            //impl = Display.Impl.CreateDisplayWindow(title, clientWidth, clientHeight, "", startFullscreen, allowResize);

            //Display.RenderTarget = this;
            //Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
        }
        
        /// <summary>
        /// Destructs a DisplayWindow
        /// </summary>
        ~DisplayWindow()
        {
            Dispose(false);
        }
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (impl != null)
            {
                impl.Dispose();
                impl = null;
            }

            if (disposing)
                GC.SuppressFinalize(this);
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
                return impl.IsClosed;
            }
        }
        /// <summary>
        /// OBSOLETE: Use IsClosed property instead. 
        /// <para>
        /// Returns true if this DisplayWindow has been closed, either
        /// by a call to Dispose(), or perhaps the user clicked the close
        /// box in a form.</para>
        /// </summary>
        [Obsolete("Use IsClosed property instead.")]
        public bool Closed
        {
            get { return IsClosed; }
        }
        /// <summary>
        /// Gets or sets the size of the client area in pixels.
        /// </summary>
        public Size Size
        {
            get { return impl.Size; }
            set { impl.Size = value; }
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
                Size = new Size(Size.Width, Size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the position of the cursor, in the 
        /// client coordinates of the window.
        /// </summary>
        public Point MousePosition
        {
            get { return impl.MousePosition; }
            set
            {
                impl.MousePosition = value;
            }
        }
        /// <summary>
        /// Returns the DisplayWindowImpl object.
        /// </summary>
        public DisplayWindowImpl Impl
        {
            get { return impl; }
        }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get { return impl.Title; }
            set { impl.Title = value; }
        }

        /// <summary>
        /// Returns true if this window is displayed fullscreen.
        /// </summary>
        public bool IsFullScreen
        {
            get { return impl.IsFullScreen; }
        }
        /// <summary>
        /// Toggles windowed and full screen.
        /// Not guaranteed to work; some drivers (eg. GDI) don't support 
        /// fullscreen displays.  If this fails it returns without any error
        /// thrown.  Check to see if it worked by examining IsFullScreen property.
        /// </summary>
        public void ToggleFullScreen()
        {
            impl.ToggleFullScreen();
        }
        /// <summary>
        /// Toggles windowed and full screen.
        /// Not guaranteed to work; some drivers (eg. GDI) don't support 
        /// fullscreen displays.  If this fails it returns without any error
        /// thrown.  Check to see if it worked by examining IsFullScreen property.
        /// </summary>
        [Obsolete("Use SetWindowed / SetFullScreen instead.")]
        public void ToggleFullScreen(int width, int height, int bpp)
        {
            impl.ToggleFullScreen(width, height, bpp);
        }

        /// <summary>
        /// Sets the display to windowed.  Does nothing if the display is already
        /// windowed.  The DisplayWindow retains the same height and width as the
        /// previous full screen resolution.
        /// </summary>
        public void SetWindowed()
        {
            impl.SetWindowed();
        }

        /// <summary>
        /// Sets the display to a full screen display.  This overload uses the
        /// desktop resolution for the full-screen display.
        /// </summary>
        /// <remarks>
        /// This call is not guaranteed to work; some drivers (eg. GDI) don't support 
        /// fullscreen displays.  If this fails it returns without any error
        /// thrown.  Check to see if it worked by examining IsFullScreen property.
        /// </remarks>
        public void SetFullScreen()
        {
            impl.SetFullScreen();
        }
        /// <summary>
        /// Sets the display to a full screen display.  The resolution chosen is 
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
            impl.SetFullScreen(width, height, bpp);
        }

        #region --- IRenderTarget Members ---

        IRenderTargetImpl IRenderTarget.Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Event raised when the window is resized by the user.
        /// </summary>
        public event EventHandler Resize;

        internal void OnResize()
        {
            if (Resize != null)
                Resize(this, EventArgs.Empty);
        }

        #endregion
    }
}