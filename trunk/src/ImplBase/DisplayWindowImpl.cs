using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib.Utility;

namespace ERY.AgateLib.ImplBase
{

    /// <summary>
    /// Implementation of DisplayWindow class.
    /// </summary>
    public abstract class DisplayWindowImpl : IRenderTargetImpl, IDisposable
    {
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public abstract void Dispose();
        /// <summary>
        /// Returns true if the DisplayWindowImpl has been closed.
        /// This happens if the user clicks the close box, or Dispose is called.
        /// </summary>
        public abstract bool Closed { get; }
        /// <summary>
        /// Returns true if this DisplayWindowImpl is being used as a full-screen
        /// device.
        /// </summary>
        public abstract bool IsFullScreen { get; }

        /// <summary>
        /// Toggles windowed/fullscreen.
        /// If this is unsupported, this method should silently return
        /// (do not throw an error).
        /// </summary>
        public abstract void ToggleFullScreen();
        /// <summary>
        /// Toggles windowed/fullscreen.
        /// If this is unsupported, this method should silently return
        /// (do not throw an error).
        /// 
        /// Attempts to match width, height and bpp as best as possible.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bpp"></param>
        public abstract void ToggleFullScreen(int width, int height, int bpp);

        /// <summary>
        /// Creates a System.Windows.Forms.Form object for rendering to.
        /// </summary>
        /// <param name="frm">Returns the created form.</param>
        /// <param name="renderTarget">Returns the control which is rendered into.</param>
        /// <param name="title">Title of the window.</param>
        /// <param name="clientWidth">Width of client area, in pixels.</param>
        /// <param name="clientHeight">Height of client area, in pixels.</param>
        /// <param name="startFullscreen">True if we should start with a full-screen window.</param>
        /// <param name="allowResize">True if we should allow the user to resize the window.</param>
        protected static void InitializeWindowsForm(
            out Form frm, 
            out Control renderTarget, 
            string title, int clientWidth, int clientHeight, bool startFullscreen, bool allowResize)
        {
            DisplayWindowForm mainForm = new DisplayWindowForm();

            // set output values
            frm = mainForm;
            renderTarget = mainForm.RenderTarget;

            
            // set properties
            frm.Text = title;
            frm.ClientSize = new System.Drawing.Size(clientWidth, clientHeight);
            frm.KeyPreview = true;

            if (allowResize == false)
            {
                frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                frm.MaximizeBox = false;
            }

        }
        /// <summary>
        /// Gets or sets the size of the render area.
        /// </summary>
        public abstract Size Size { get;            set;      }
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

        #region --- IRenderTargetImpl Members ---

        /// <summary>
        /// Utility function which may be called by the DisplayImpl when 
        /// rendering begins.
        /// </summary>
        public abstract void BeginRender();
        /// <summary>
        /// Utility function which may be called by the DisplayImpl when 
        /// rendering is done.
        /// </summary>
        /// <param name="waitVSync"></param>
        public abstract void EndRender(bool waitVSync);

        #endregion

    }

}
