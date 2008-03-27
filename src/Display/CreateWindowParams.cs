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
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
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

        private bool isFullScreen = false;
        private Size size = new Size(1024, 768);
        private WindowPosition position;
        private int bpp = 32;
        private bool isResizable = false;
        
        private bool hasFrame = true;
        private bool hasMaximize = false;
        private bool hasMinimize = true;

        private string iconFile = "";

        private bool renderToControl = false;
        private object renderTarget = null;

        private string title = "AgateLib Application";

        #endregion
        #region --- Properties ---

        /// <summary>
        /// Title of the window.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// Whether or not the window should be created as a full screen window.  Defaults to false.
        /// </summary>
        public bool IsFullScreen
        {
            get { return isFullScreen; }
            set { isFullScreen = value; }
        }
        /// <summary>
        /// Size of the window.  Defaults to 1024x768.
        /// </summary>
        public Size Size
        {
            get { return size; }
            set { size = value; }
        }
        /// <summary>
        /// Width of the window
        /// </summary>
        public int Width
        {
            get { return size.Width; }
            set { size.Width = value; }
        }
        /// <summary>
        /// Height of the window
        /// </summary>
        public int Height
        {
            get { return size.Height; }
            set { size.Height = value; }
        }
        /// <summary>
        /// Sets the initial position of the window.
        /// </summary>
        public WindowPosition WindowPosition
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// Bit depth for the framebuffer for the window.  This defaults to 32.  This
        /// field is ignored if we are not creating a full-screen window.
        /// </summary>
        public int Bpp
        {
            get { return bpp; }
            set { bpp = value; }
        }

        /// <summary>
        /// Whether or not the user can manually resize the window.  Defaults to false.  Ignored
        /// for full-screen windows.
        /// </summary>
        public bool IsResizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        /// <summary>
        /// Whether or not the window is drawn with a frame and titlebar.  This property is ignored
        /// for fullscreen windows.  Defaults to true.
        /// </summary>
        public bool HasFrame
        {
            get { return hasFrame; }
            set { hasFrame = value; }
        }

        /// <summary>
        /// Whether or not the window has a maximize button.  In general, this should be equal to the
        /// IsResizable property.
        /// </summary>
        public bool HasMaximize
        {
            get { return hasMaximize; }
            set { hasMaximize = value; }
        }

        /// <summary>
        /// Whether or not the window has a minimize button.  This should generally be true.
        /// </summary>
        public bool HasMinimize
        {
            get { return hasMinimize; }
            set { hasMinimize = value; }
        }

        /// <summary>
        /// Path to a .ico file to use for the window icon.
        /// Setting this field resolves the path to the icon using the FileManager.ImagePath
        /// </summary>
        public string IconFile
        {
            get { return iconFile; }
            set
            {
                iconFile = FileManager.ImagePath.FindFileName(value);
            }
        }

        /// <summary>
        /// True if we are in fact rendering to a WinForms control, rather than creating
        /// a window to be managed by AgateLib.
        /// </summary>
        public bool RenderToControl
        {
            get { return renderToControl; }
            set { renderToControl = value; }
        }

        /// <summary>
        /// Control to be rendered to.  This is ignored if RenderToControl is false.
        /// </summary>
        public object RenderTarget
        {
            get { return renderTarget; }
            set { renderTarget = value; }
        }

        #endregion

        #region --- Static creation methods ---

        /// <summary>
        /// Creates a CreateWindowParams object which describes rendering into a WinForms control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static CreateWindowParams FromControl(object control)
        {
            CreateWindowParams retval = new CreateWindowParams();

            retval.RenderToControl = true;
            retval.RenderTarget = control;

            return retval;
        }

        /// <summary>
        /// Creates a CreateWindowParams object which describes a fullscreen window.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bpp"></param>
        /// <returns></returns>
        public static CreateWindowParams FullScreen(string title, int width, int height, int bpp)
        {
            CreateWindowParams retval = new CreateWindowParams();

            retval.Title = title;
            retval.Width = width;
            retval.Height = height;
            retval.bpp = bpp;

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
        /// <returns></returns>
        public static CreateWindowParams Windowed(string title, int width, int height, string iconFile, bool allowResize)
        {
            CreateWindowParams retval = new CreateWindowParams();

            retval.Title = title;
            retval.Width = width;
            retval.Height = height;
            retval.IconFile = iconFile;
            retval.IsResizable = allowResize;
            retval.HasMaximize = allowResize;
            
            return retval;
        }
        /// <summary>
        /// Creates a CreateWindowParams object which describes a desktop window with no frame or
        /// titlebar.  This is typical for showing a splashscreen as the application loads.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static CreateWindowParams NoFrame(string title, int width, int height)
        {
            CreateWindowParams retval = new CreateWindowParams();

            retval.Title = title;
            retval.Width = width;
            retval.Height = height;
            retval.IsResizable = false;
            retval.HasFrame = false;

            return retval;
        }

        #endregion
    }
}
