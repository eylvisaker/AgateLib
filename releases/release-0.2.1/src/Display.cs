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
using System.IO;
using System.Text;

using ERY.AgateLib.Drivers;
using ERY.AgateLib.ImplBase;
using ERY.AgateLib.Utility;

namespace ERY.AgateLib
{
    /// <summary>
    /// Static class which contains all basic functions for drawing onto the display.
    /// This class is most central to game rendering.  At the beginning and end of each frame
    /// Display.BeginFrame() and Display.EndFrame() must be called.  All drawing calls must
    /// occur between BeginFrame and EndFrame.
    /// 
    /// Display.Dispose() must be called before the program exits.
    /// 
    /// </summary>
    /// 
    /// <example> This example shows how a basic render loop works.
    /// <code>
    /// void MyRenderLoop()
    /// {
    ///     Display.BeginFrame();
    ///     Display.Clear(Color.Black);
    /// 
    ///     Display.DrawRect(new Rectangle(10, 10, 30, 30), Color.Red);
    /// 
    ///     Display.EndFrame();
    ///     Core.KeepAlive();
    /// }
    /// </code>
    /// </example>
    public static class Display 
    {
        private static DisplayImpl impl;
        private static DisplayWindow mCurrentWindow;
        private static SurfacePacker mSurfacePacker;
        
        /// <summary>
        /// Gets the object which handles all of the actual calls to Display functions.
        /// This may be cast to a surface object in whatever rendering library
        /// is being used (eg. if using the MDX_1_1 library, this can be cast
        /// to an MDX1_Display object).  You only need to use this if you
        /// want to access features which are specific to the graphics library
        /// you're using.
        /// </summary>
        public static DisplayImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Initializes the display by instantiating the driver with the given
        /// DisplayTypeID.  The display driver must be registered with the Registrar
        /// class.
        /// 
        /// It is recommended that you instantiate a SetupDisplay object from within
        /// a using block, to ensure that the Display is disposed of properly.
        /// </summary>
        /// <param name="displayType"></param>
        public static void Initialize(DisplayTypeID displayType)
        {
            Core.Initialize();

            impl = Registrar.DisplayDriverInfo.CreateDriver(displayType);

            mSurfacePacker = new SurfacePacker();

        }
        /// <summary>
        /// Disposes of the display.
        /// </summary>
        public static void Dispose()
        {
            OnDispose();

            if (impl != null)
            {
                impl.Dispose();
                impl = null;
            }
        }


        /// <summary>
        /// Delegate type for functions which are called when Display.Dispose is called
        /// at the end of execution of the program.
        /// </summary>
        public delegate void DisposeDisplayHandler();
        /// <summary>
        /// Event that is called when Display.Dispose() is invoked, to shut down the
        /// display system and release all resources.
        /// </summary>
        public static event DisposeDisplayHandler DisposeDisplay;

        private static void OnDispose()
        {
            if (DisposeDisplay != null)
                DisposeDisplay();
        }

        /// <summary>
        /// Searches for the fully qualified path for the specified file, in the following order:
        /// 1. Look in current directory
        /// 2. Look in ImagePath
        /// </summary>
        /// <param name="imageFileName">Filename to search for</param>
        /// <returns></returns>
        [Obsolete("Use methods in FileManager class instead.")]
        public static string FindQualifiedPath(string imageFileName)
        {
            string fullPath;

            // check current path first
            if (TestPath(imageFileName, out fullPath)) return fullPath;       
            //if (TestPath(Path.Combine(FileManager.ImagePath.ImagePath, imageFileName ), out fullPath )) return fullPath;

            // can't find it.  just return the filename.
            return imageFileName;
        }
        private static bool TestPath(string filename, out string fullPath)
        {
            if (File.Exists(filename))
            {
                fullPath = Path.GetFullPath(filename);
                return true;
            }
            else
            {
                fullPath = "";
                return false;
            }
        }



        /// <summary>
        /// Gets or sets the current render target.
        /// Must be called outside of BeginFrame..EndFrame blocks
        /// (usually just before BeginFrame).
        /// </summary>
        public static IRenderTarget RenderTarget
        {
            get
            {
                return impl.RenderTarget;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("RenderTarget cannot be null.");
                
                impl.RenderTarget = value;

                if (value is DisplayWindow)
                    mCurrentWindow = value as DisplayWindow;
                
            }
        }
        /// <summary>
        /// Gets the last render target used which was a DisplayWindow.
        /// 
        /// </summary>
        public static DisplayWindow CurrentWindow
        {
            get { return mCurrentWindow; }
        }

        /// <summary>
        /// Gets or sets the threshold value for alpha transparency below which
        /// pixels are considered completely transparent, and may not be drawn.
        /// 
        /// Whether or not this flag is actually used is driver-dependent.
        /// </summary>
        public static double AlphaThreshold
        {
            get { return impl.AlphaThreshold; }
            set { impl.AlphaThreshold = value; }
        }

        /// <summary>
        /// Clears the buffer to black.
        /// </summary>
        public static void Clear()
        {
            Clear(Color.Black);
        }
        /// <summary>
        /// Clears the buffer to the specified color.
        /// </summary>
        /// <param name="a">Alpha value</param>
        /// <param name="b">Blue value</param>
        /// <param name="g">Green value</param>
        /// <param name="r">Red value</param>
        public static void Clear(byte a, byte r, byte g, byte b)
        {
            Clear(Color.FromArgb(a, r, g, b));
        }
        /// <summary>
        /// Clears the buffer to the specified color.
        /// </summary>
        /// <param name="color"></param>
        public static void Clear(Color color)
        {
            impl.Clear(color);
        }
        /// <summary>
        /// Clears the buffer to the specified color.
        /// </summary>
        /// <param name="color"></param>
        public static void Clear(int color)
        {
            impl.Clear(Color.FromArgb(color));
        }
        /// <summary>
        /// Clears a region of the buffer to the specified color.
        /// Should be essentially the same as DrawRect(dest, color), except
        /// that alpha is not significant in the use of Clear.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="dest"></param>
        public static void Clear(Color color, Rectangle dest)
        {
            impl.Clear(color, dest);
        }
        /// <summary>
        /// Clears a region of the buffer to the specified color.
        /// Should be essentially the same as DrawRect(dest, color), except
        /// that alpha is not significant in the use of Clear.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="dest"></param>
        public static void Clear(int color, Rectangle dest)
        {
            impl.Clear(Color.FromArgb(color), dest);
        }
        // BeginFrame and EndFrame must be called at the start and end of each frame.
        /// <summary>
        /// Must be called at the start of each frame.
        /// </summary>
        public static void BeginFrame()
        {
            impl.BeginFrame();
        }
        /// <summary>
        /// EndFrame must be called at the end of each frame.
        /// By default, this waits for the vertical blank before rendering.
        /// However, some renderers (ie. System.Drawing) may not support that. 
        /// </summary>
        public static void EndFrame()
        {
            impl.EndFrame(true);
        }
        /// <summary>
        /// A version of EndFrame must be called at the end of each frame.
        /// This version allows the caller to indicate to the driver whether 
        /// it is preferred to wait for the vertical blank to do the drawing.
        /// The driver may or may not honor the value of waitVSync.
        /// </summary>
        /// <param name="waitVSync"></param>
        public static void EndFrame(bool waitVSync)
        {
            impl.EndFrame(waitVSync);
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has passed between this frame
        /// and the last one.
        /// </summary>
        public static double DeltaTime { get { return impl.DeltaTime; } }
        /// <summary>
        /// Provides a means to set the value returned by DeltaTime.
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void SetDeltaTime(double deltaTime)
        {
            impl.SetDeltaTime(deltaTime);
        }

        /// <summary>
        /// Gets the framerate
        /// </summary>
        public static double FramesPerSecond { get { return impl.FramesPerSecond; } }

        /// <summary>
        /// Set the current clipping rect.
        /// </summary>
        /// <param name="newClipRect"></param>
        public static void SetClipRect(Rectangle newClipRect)
        {
            impl.SetClipRect(newClipRect);
        }
        /// <summary>
        /// Pushes a clip rect onto the clip rect stack.
        /// </summary>
        /// <param name="newClipRect"></param>
        public static void PushClipRect(Rectangle newClipRect)
        {
            impl.PushClipRect(newClipRect);
        }
        /// <summary>
        /// Pops the clip rect and restores the previous clip rect.
        /// </summary>
        public static void PopClipRect()
        {
            impl.PopClipRect();
        }
        /// <summary>
        /// Returns the maximum size a surface object can be.
        /// </summary>
        public static Size MaxSurfaceSize
        {
            get { return impl.MaxSurfaceSize; }
        }
        /// <summary>
        /// Gets the object which handles packing of all surfaces.
        /// </summary>
        public static SurfacePacker SurfacePacker
        {
            get { return mSurfacePacker; }
        }

        /// <summary>
        /// Takes all surfaces and packs them into a large surface.
        /// This should minimize swapping of surfaces, and may result in a performance
        /// increase when using Direct3D or OpenGL.  
        /// 
        /// If you use this, it is best to load all your surfaces into memory, 
        /// mark any you don't want packed (surfaces which may be used as render targets,
        /// for example), then call Display.PackAllSurfaces().
        /// </summary>
        public static void PackAllSurfaces()
        {
            mSurfacePacker.ClearQueue();

            if (PackAllSurfacesEvent != null)
                PackAllSurfacesEvent(null, EventArgs.Empty);

            mSurfacePacker.PackQueue();

            GC.Collect();
        }

        /// <summary>
        /// Event fired when PackAllSurfacesEvent
        /// </summary>
        internal static event EventHandler PackAllSurfacesEvent;
       
        internal static Surface BuildPackedSurface(Size size, SurfacePacker.RectPacker<Surface> packedRects)
        {
            Surface retval = impl.BuildPackedSurface(size, packedRects);
            retval.ShouldBePacked = false;

            return retval;
            
        }

        #region --- Drawing Functions ---

        /// <summary>
        /// Draws an ellispe within the specified rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void DrawEllipse(Rectangle rect, Color color)
        {
            impl.DrawEllipse(rect, color);
        }
        /// <summary>
        /// Draws a line between the two points specified.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        public static void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            impl.DrawLine(x1, y1, x2, y2, color);
        }
        /// <summary>
        /// Draws a line between the two points specified.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public static void DrawLine(Point a, Point b, Color color)
        {
            impl.DrawLine(a, b, color);
        }
        /// <summary>
        /// Draws a bunch of connected lines.  The last point and the
        /// first point are not connected.
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="color"></param>
        public static void DrawLines(Point[] pts, Color color)
        {
            impl.DrawLines(pts, color);
        }
        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void DrawRect(Rectangle rect, Color color)
        {
            impl.DrawRect(rect, color);
        }
        /// <summary>
        /// Draws the outline of a rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void DrawRect(int x, int y, int width, int height, Color color)
        {
            impl.DrawRect(new Rectangle(x, y, width, height), color);
        }
        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void FillRect(Rectangle rect, Color color)
        {
            impl.FillRect(rect, color);
        }
        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void FillRect(int x, int y, int width, int height, Color color)
        {
            impl.FillRect(new Rectangle(x, y, width, height), color);
        }

        #endregion

    }


  
}