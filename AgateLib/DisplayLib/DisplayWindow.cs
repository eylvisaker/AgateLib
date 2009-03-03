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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.Geometry;
using AgateLib.ImplementationBase;

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
    public sealed class DisplayWindow : IRenderTarget, IDisposable
    {
        DisplayWindowImpl impl;

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

                impl = Display.Impl.CreateDisplayWindow(par);
            }
            else
            {
                CreateWindowParams par = CreateWindowParams.Windowed(
                    disp.Title, disp.Size.Width, disp.Size.Height, null, disp.AllowResize);

                impl = Display.Impl.CreateDisplayWindow(par);
            }

            Display.RenderTarget = this;
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

            impl = Display.Impl.CreateDisplayWindow(windowParams);

            Display.RenderTarget = this;
            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
        }
        /// <summary>
        /// Creates a DisplayWindow object using the specified System.Windows.Forms.Control
        /// object as a render context.  A DisplayWindow made in this manner cannot be made
        /// into a full-screen DisplayWindow.
        /// </summary>
        /// <remarks>
        /// [Experimental - The API may be changed in the future.]
        /// </remarks>
        /// <param name="renderTarget">Windows.Forms control which should be used as the
        /// render target.</param>
        [Obsolete("Use the DisplayWindow.CreateFromControl static method instead.")]
        public DisplayWindow(object renderTarget)
        {
            if (Display.Impl == null)
                throw new AgateException(
                    "Display has not been initialized." + Environment.NewLine +
                    "Did you forget to call AgateSetup.Initialize or Display.Initialize?");

            impl = Display.Impl.CreateDisplayWindow(CreateWindowParams.FromControl(renderTarget));

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
        [Obsolete("Use the DisplayWindow.CreateWindowed static method instead.")]
        public DisplayWindow(string title, int clientWidth, int clientHeight)
            : this(CreateWindowParams.Windowed(title, clientWidth, clientHeight, "", false))
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
        [Obsolete("Use the DisplayWindow.CreateWindowed static method instead.")]
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
        [Obsolete("Use either the DisplayWindow.CreateWindowed or DisplayWindow.CreateFullScreen static method instead.")]
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
        [Obsolete("Use either the DisplayWindow.CreateWindowed or DisplayWindow.CreateFullScreen static method instead.")]
        public DisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullscreen, bool allowResize)
        {
            //impl = Display.Impl.CreateDisplayWindow(title, clientWidth, clientHeight, 
            //    FileManager.ImagePath.FindFileName(iconFile), startFullscreen, allowResize);

            if (startFullscreen)
                impl = Display.Impl.CreateDisplayWindow(
                    CreateWindowParams.FullScreen(title, clientWidth, clientHeight, 32));
            else
                impl = Display.Impl.CreateDisplayWindow(
                    CreateWindowParams.Windowed(title, clientWidth, clientHeight, iconFile, allowResize));

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
        [Obsolete("Use either the DisplayWindow.Windowed or DisplayWindow.FullScreen static method instead.")]
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
        [Obsolete("Use either the DisplayWindow.Windowed or DisplayWindow.FullScreen static method instead.")]
        public DisplayWindow(string title, int clientWidth, int clientHeight, bool startFullscreen, bool allowResize)
            : this(title, clientWidth, clientHeight, "", startFullscreen, allowResize)
        {
        }

        #region --- Static Creation Methods ---

        [Obsolete("Use CreateFromControl static method.")]
        public static DisplayWindow FromControl(object control)
        {
            return CreateFromControl(control);
        }
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
        /// <param name="bpp"></param>
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
            return new DisplayWindow(CreateWindowParams.Windowed(title, width, height, iconFile, allowResize));
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
        /// Disposes of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (impl != null)
            {
                impl.Dispose();
                impl = null;
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
                if (impl == null)
                    return true;

                return impl.IsClosed;
            }
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
        /// Sets the display to windowed.  Does nothing if the display is already
        /// windowed.  The DisplayWindow retains the same height and width as the
        /// previous full screen resolution.
        /// </summary>
        public void SetWindowed()
        {
            impl.SetWindowed();
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
            impl.SetFullScreen();
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