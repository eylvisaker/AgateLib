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

using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using AgateLib.Utility;

namespace AgateLib.DisplayLib
{
    /// <summary>
    /// Enum which is used to indicate what format an image file is in.
    /// </summary>
    public enum ImageFileFormat
    {
        /// <summary>
        /// Portable Network Graphics (PNG) format.
        /// </summary>
        Png,
        /// <summary>
        /// Windows Bitmap (BMP) format.
        /// </summary>
        Bmp,
        /// <summary>
        /// Jpeg format.
        /// </summary>
        Jpg,
        /// <summary>
        /// Targa format.
        /// </summary>
        Tga,
    }
    /// <summary>
    /// Class which represents a pixel surface.
    /// There are several ways to create a Surface object.  The most common
    /// is to pass the name of an image file.
    /// 
    /// Using a surface to draw on the screen is very simple.  There are
    /// several overloaded Draw methods which do that.
    /// 
    /// You must have created a DisplayWindow object before creating any 
    /// Surface objects.
    /// <example>
    /// C# Example to create a new surface from an image file:
    /// <code>
    ///     Surface surface = new Surface("myimage.png");
    /// </code>
    /// VB Example to create a new surface from an image file:
    /// <code>
    ///     Dim surf as New Surface("myimage.png")
    /// </code>
    /// 
    /// C# Example to draw surface to screen.
    /// <code>
    ///     surface.Draw(20, 20);
    /// </code>
    /// VB Example to draw surface to screen.
    /// <code>
    ///     surf.Draw(20, 20)
    /// </code>
    /// </example>
    /// </summary>
    public sealed class Surface : IRenderTarget, IDisposable, ISurface
    {
        SurfaceImpl impl;

        /// <summary>
        /// Creates a surface object from a resource.
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="name"></param>
        public Surface(Resources.AgateResourceCollection resources, string name)
        {
            Resources.AgateResource res = resources[name];
            Resources.SurfaceResource surf = res as Resources.SurfaceResource;

            using (System.IO.Stream s = AgateFileProvider.ImageProvider.OpenRead(surf.Filename))
            {
                impl = Display.Impl.CreateSurface(s);
            }

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
            Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
        }
        /// <summary>
        /// Creates a surface object, from the specified image file.
        /// </summary>
        /// <param name="filename"></param>
        public Surface(string filename)
        {
            if (Display.Impl == null)
                throw new AgateException("AgateLib's display system has not been initialized.");

            using (System.IO.Stream s = AgateFileProvider.ImageProvider.OpenRead(filename))
            {
                impl = Display.Impl.CreateSurface(s);
            }

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
            Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
        }
        /// <summary>
        /// Creates a surface object from the data in the specified stream.
        /// </summary>
        /// <param name="st"></param>
        public Surface(Stream st)
        {
            impl = Display.Impl.CreateSurface(st);

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
            Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
        }
        /// <summary>
        /// Creates a surface object of the specified size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Surface(int width, int height)
            : this(new Size(width, height))
        {
            
        }
        /// <summary>
        /// Creates a surface object of the specified size.
        /// </summary>
        /// <param name="size"></param>
        public Surface(Size size)
        {
            impl = Display.Impl.CreateSurface(size);

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
            Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
        }
        /// <summary>
        /// Constructs a surface object from the specified PixelBuffer object.
        /// </summary>
        /// <param name="pixels"></param>
        public Surface(PixelBuffer pixels)
            : this(pixels.Size)
        {
            WritePixels(pixels);
        }
        /// <summary>
        /// Creates a surface object and to be ready to attach to an implemented object.
        /// Throws an Exception if there is a passed impl.
        /// (This is not intended for use by applications).
        /// </summary>
        /// <param name="fromImpl"></param>
        private Surface(SurfaceImpl fromImpl)
        {
            if (fromImpl != null)
                throw new Exception("fromImpl already has an owned implementation!");

            Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
            Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
        }
        /// <summary>
        /// Destroyes unmanaged resources associated with this surface.
        /// </summary>
        public void Dispose()
        {
            impl.Dispose();

            //Display.DisposeDisplay -= Dispose;
            Display.PackAllSurfacesEvent -= Display_PackAllSurfacesEvent;
        }
        /// <summary>
        /// Returns true if Dispose() has been called on this surface.
        /// </summary>
        public bool IsDisposed
        {
            get { return impl.IsDisposed; } 
        }

        void Display_PackAllSurfacesEvent(object sender, EventArgs e)
        {
            if (ShouldBePacked && !IsDisposed)
                Display.SurfacePacker.QueueSurface(this);
        }

        #region --- Surface properties ---

        /// <summary>
        /// Gets or sets how many times this surface should be broken down
        /// when drawn.  A TesselateFactor of 2 indicates that each time
        /// this surface is drawn, it's drawn in 4 (2x2) chunks.
        /// </summary>
        /// <remarks>
        /// This property is used to divide a surface drawn up into smaller
        /// surfaces which are drawn independently.  The reason to do this is
        /// lighting calculations (without using shaders) are done on a per-vertex 
        /// basis.  When a light source is close to a large surface, this will create
        /// noticably bad lighting, because only the lighting properties at the 
        /// corners are calculated, and then the lighting is interpolated between
        /// the end points.
        /// <para>
        /// Changing this value while using gradients can result in ugly transitions between
        /// different tesselation values.  The reason is AgateLib will interpolate the gradient
        /// color to the vertices used, and then this is capped to integer values.  So avoid
        /// doing this.</para>
        /// <para>
        /// Setting this value high may have a significant impact on performance.
        /// For each time a Surface.Draw() overload is called, the number of triangles
        /// which are calculated and sent to the rasterizer is 2 * TesselateFactor<sup>2</sup>.
        /// </para>
        /// </remarks>
        /// 
        public int TesselateFactor
        {
            get { return impl.TesselateFactor; }
            set { impl.TesselateFactor = value; }
        }
        /// <summary>
        /// Gets or sets a bool value that indicates whether or not this surface
        /// should be included in a call to Display.PackAllSurfaces.
        /// </summary>
        public bool ShouldBePacked
        {
            get { return impl.ShouldBePacked; }
            set { impl.ShouldBePacked = value; }
        }

        /// <summary>
        /// Gets the width of the source surface in pixels.
        /// </summary>
        public int SurfaceWidth { get { return impl.SurfaceWidth; } }
        /// <summary>
        /// Gets the height of the source surface in pixels.
        /// </summary>
        public int SurfaceHeight { get { return impl.SurfaceHeight; } }
        /// <summary>
        /// Gets the Size of the source surface in pixels.
        /// </summary>
        public Size SurfaceSize { get { return impl.SurfaceSize; } }


        /// <summary>
        /// Get or sets the width of the surface in pixels when it will be displayed on screen.
        /// </summary>
        public int DisplayWidth
        {
            get { return impl.DisplayWidth; }
            set { impl.DisplayWidth = value; }
        }
        /// <summary>
        /// Gets or sets the height of the surface in pixels when it is displayed on screen.
        /// </summary>
        public int DisplayHeight
        {
            get { return impl.DisplayHeight; }
            set { impl.DisplayHeight = value; }
        }
        /// <summary>
        /// Gets or sets the Size of the area used by this surface when displayed on screen.
        /// </summary>
        public Size DisplaySize
        {
            get
            {
                Size sz = new Size(DisplayWidth, DisplayHeight);

                return sz;
            }
            set
            {
                DisplayWidth = value.Width;
                DisplayHeight = value.Height;
            }
        }

        /// <summary>
        /// Alpha value for displaying this surface.
        /// Valid values range from 0.0 (completely transparent) to 1.0 (completely opaque).
        /// Internally stored as a byte, so granularity is only 1/255.0.
        /// </summary>
        public double Alpha
        {
            get { return impl.Alpha; }
            set { impl.Alpha = value; }
        }
        /// <summary>
        /// Gets or sets the rotation angle in radians.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public double RotationAngle
        {
            get { return impl.RotationAngle; }
            set { impl.RotationAngle = value; }
        }
        /// <summary>
        /// Gets or sets the rotation angle in degrees.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public double RotationAngleDegrees
        {
            get { return RotationAngle * 180.0 / Math.PI; }
            set { RotationAngle = value * Math.PI / 180.0; }
        }
        /// <summary>
        /// Gets or sets the point on the surface which is used to rotate around.
        /// </summary>
        public OriginAlignment RotationCenter
        {
            get { return impl.RotationCenter; }
            set { impl.RotationCenter = value; }
        }
        /// <summary>
        /// Gets or sets the point where the surface is aligned to when drawn.
        /// </summary>
        public OriginAlignment DisplayAlignment
        {
            get { return impl.DisplayAlignment; }
            set { impl.DisplayAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the amount the width is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        public double ScaleWidth
        {
            get { return impl.ScaleWidth; }
            set { impl.ScaleWidth = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleHeight
        {
            get { return impl.ScaleHeight; }
            set { impl.ScaleHeight = value; }
        }
        /// <summary>
        /// Sets the amount of scaling when this surface is drawn.  
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetScale(double width, double height)
        {
            impl.SetScale(width, height);
        }
        /// <summary>
        /// Gets the amount of scaling when this surface is drawn.
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void GetScale(out double width, out double height)
        {
            impl.GetScale(out width, out height);
        }

        
        /// <summary>
        /// Gets or sets the multiplicative color for this surface.
        /// Setting this is equivalent to setting the ColorGradient property
        /// with a gradient with the same color in all corners.  If a gradient
        /// is being used, getting this property returns the top-left color in the gradient.
        /// </summary>
        public Color Color
        {
            get { return impl.Color; }
            set { impl.Color = value; }
        }
        /// <summary>
        /// Gets or sets the gradient for this surface.
        /// </summary>
        public Gradient ColorGradient
        {
            get { return impl.ColorGradient; }
            set { impl.ColorGradient = value; }
        }

        /// <summary>
        /// Increments the rotation angle of this surface.
        /// </summary>
        /// <param name="radians">Value in radians to increase the rotation by.</param>
        public void IncrementRotationAngle(double radians)
        {
            impl.IncrementRotationAngle(radians);
        }
        /// <summary>
        /// Increments the rotation angle of this surface.  Value supplied is in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        public void IncrementRotationAngleDegrees(double degrees)
        {
            impl.IncrementRotationAngleDegrees(degrees);
        }

        /// <summary>
        /// Checks to see whether the surface pixels all have
        /// alpha value less than the value of the AlphaThreshold of the
        /// display object..
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use ReadPixels and PixelBuffer.Is*Blank methods instead.")]
        public bool IsSurfaceBlank() { return impl.IsSurfaceBlank(); }
        /// <summary>
        /// Checks to see whether the surface pixels all have
        /// alpha value less than the given value.
        /// </summary>
        /// <param name="alphaThreshold">The alpha value below which to consider 
        /// a pixel blank.  In the range 0 &lt;= alphaThreshold &lt;= 255.</param>
        /// <returns></returns>
        [Obsolete("Use ReadPixels and PixelBuffer.Is*Blank methods instead.")]
        public bool IsSurfaceBlank(int alphaThreshold) { return impl.IsSurfaceBlank(alphaThreshold); }

        /// <summary>
        /// Checks to see whether all the pixels along the given row are all
        /// transparent, within the threshold.
        /// </summary>
        /// <param name="row">Which row.  Valid range is between 0 and SurfaceSize.Height - 1.</param>
        /// <returns></returns>
        [Obsolete("Use ReadPixels and PixelBuffer.Is*Blank methods instead.")]
        public bool IsRowBlank(int row) { return impl.IsRowBlank(row); }
        /// <summary>
        /// Checks to see whether all the pixels along the given column are all
        /// transparent, within the threshold.
        /// </summary>
        /// <param name="col">Which column.  Valid range is between 0 and SurfaceSize.Width - 1.</param>
        /// <returns></returns>
        [Obsolete("Use ReadPixels and PixelBuffer.Is*Blank methods instead.")]
        public bool IsColumnBlank(int col) { return impl.IsColumnBlank(col); }

        #endregion

        #region --- Drawing to the screen ---

        /// <summary>
        /// Draws the surface to the top-left corner (0, 0) of the
        /// target.
        /// </summary>
        public void Draw()
        {
            Draw(Point.Empty);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        public void Draw(int destX, int destY)
        {
            impl.Draw((float) destX, (float)destY);
        }    
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        public void Draw(float destX, float destY)
        {
            impl.Draw(destX, destY);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// </summary>
        /// <param name="destPt"></param>
        public void Draw(Point destPt)
        {
            impl.Draw(destPt.X, destPt.Y);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// </summary>
        /// <param name="destPt"></param>
        public void Draw(Vector2 destPt)
        {
            impl.Draw(destPt.X, destPt.Y);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// </summary>
        /// <param name="destPt"></param>
        public void Draw(PointF destPt)
        {
            impl.Draw(destPt.X, destPt.Y);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// Ignores the value of RotationCenter and uses the specified
        /// point to rotate around instead.  
        /// </summary>
        /// <param name="destPt"></param>
        /// <param name="rotationCenter">Center of rotation to use, relative
        /// to the top-left of the surface.</param>
        public void Draw(PointF destPt, PointF rotationCenter)
        {
            impl.Draw(destPt.X, destPt.Y, rotationCenter.X, rotationCenter.Y);
        }
        /// <summary>
        /// Draws this surface to the screen at the specified point, 
        /// using all the state information defined in the properties 
        /// of this surface.
        /// Ignores the value of RotationCenter and uses the specified
        /// point to rotate around instead.
        /// </summary>
        public void Draw(float destX, float destY, float rotationCenterX, float rotationCenterY)
        {
            impl.Draw(destX, destY, rotationCenterX, rotationCenterY);
        }

        /// <summary>
        /// Draws a portion of this surface to the specified destination
        /// rectangle.  
        /// 
        /// State settings which are ignored are RotationAngle,
        /// DisplayAlignment and Scaling.  Color and alpha values
        /// are still used.
        /// </summary>
        /// <param name="srcRect"></param>
        /// <param name="destRect"></param>
        public void Draw(Rectangle srcRect, Rectangle destRect)
        {
            impl.Draw(srcRect, destRect);
        }

        /// <summary>
        /// Draws this surface to the specified destination
        /// rectangle.  
        /// 
        /// State settings which are ignored are RotationAngle,
        /// DisplayAlignment and Scaling.  Color and alpha values
        /// are still used.
        /// </summary>
        /// <param name="destRect"></param>
        public void Draw(Rectangle destRect)
        {
            impl.Draw(destRect);
        }

        /// <summary>
        /// Draws the surface using an array of source and destination rectangles.
        /// This method will throw an exception if the two arrays are not the same size.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        public void DrawRects(Rectangle[] srcRects, Rectangle[] destRects)
        {
            if (srcRects.Length != destRects.Length)
            {
                throw new ArgumentException(
                    "Source and dest rect arrays are not the same size!  Use overload which indicates length of arrays to use.");
            }
            impl.DrawRects(srcRects, destRects, 0, srcRects.Length);
        }
        /// <summary>
        /// Draws the surface using an array of source and destination rectangles.
        /// This method will throw an exception if the two arrays are not the same size.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        public void DrawRects(RectangleF[] srcRects, RectangleF[] destRects)
        {
            if (srcRects.Length != destRects.Length)
            {
                throw new ArgumentException(
                    "Source and dest rect arrays are not the same size!  Use overload which indicates length of arrays to use.");
            }
            impl.DrawRects(srcRects, destRects, 0, srcRects.Length);
        }

        /// <summary>
        /// Draws the surface using an array of source and destination rectangles.
        /// This method will throw an exception if the two arrays are not the same size.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        /// <param name="start">Element in the arrays to start at.</param>
        /// <param name="length">Number of elements in the arrays to use.</param>
        public void DrawRects(Rectangle[] srcRects, Rectangle[] destRects, int start, int length)
        {
            impl.DrawRects(srcRects, destRects, start, length);
        }
        /// <summary>
        /// Draws the surface using an array of source and destination rectangles.
        /// This method will throw an exception if the two arrays are not the same size.
        /// </summary>
        /// <param name="srcRects"></param>
        /// <param name="destRects"></param>
        /// <param name="start">Element in the arrays to start at.</param>
        /// <param name="length">Number of elements in the arrays to use.</param>
        public void DrawRects(RectangleF[] srcRects, RectangleF[] destRects, int start, int length)
        {
            impl.DrawRects(srcRects, destRects, start, length);
        }

        #endregion
 
        #region --- Surface Data Manipulation ---

        /// <summary>
        /// Saves the surface to the specified file.
        /// 
        /// Infers the file format from the extension.  If there
        /// is no extension present or it is unrecognized, PNG is
        /// assumed.
        /// </summary>
        /// <param name="filename"></param>
        public void SaveTo(string filename)
        {
            SaveTo(filename, FormatFromExtension(filename));
        }
        /// <summary>
        /// Saves the surface to the specified file, with the specified
        /// file format.  If the file has an extension such as ".png" or
        /// ".bmp" than the SaveTo(string) overload is prefered, as it
        /// chooses a file format which is consistent with the extension.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void SaveTo(string filename, ImageFileFormat format)
        {
            impl.SaveTo(filename, format);
        }

        /// <summary>
        /// Copies the pixels in this surface from the given source rectangle 
        /// to a new surface, and returns that.
        /// It is not recommended to call this between calls to 
        /// Display.BeginFrame..Display.EndFrame.
        /// </summary>
        /// <param name="srcRect">The rectangle of pixels to create a new surface from.</param>
        /// <returns>A Surface object containing only those pixels copied.</returns>
        public Surface CarveSubSurface(Rectangle srcRect)
        {
            Surface retval = new Surface((SurfaceImpl)null);
            SurfaceImpl newImpl = impl.CarveSubSurface(retval, srcRect);

            retval.impl = newImpl;

            return retval;
        }

        /// <summary>
        /// Used by the BuildPackedSurface 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="srcRect"></param>
        internal void SetSourceSurface(Surface surf, Rectangle srcRect)
        {
            impl.SetSourceSurface(surf.impl, srcRect);
        }

        /// <summary>
        /// Returns a pixel buffer which contains a copy of the pixel data in the surface. 
        /// The format of the pixel data is the same as of the raw data in the surface. 
        /// </summary>
        /// <returns></returns>
        public PixelBuffer ReadPixels()
        {
            return ReadPixels(PixelFormat.Any);
        }
        /// <summary>
        /// Returns a pixel buffer which contains a copy of the pixel data in the surface. 
        /// </summary>
        /// <param name="format">Format of the pixel data wanted.  Automatic conversion is
        /// performed, if necessary.  Use PixelFormat.Any to retrieve data in its original format,
        /// without conversion.</param>
        /// <returns></returns>
        public PixelBuffer ReadPixels(PixelFormat format)
        {
            return impl.ReadPixels(format);
        }
        /// <summary>
        /// Returns a pixel buffer which contains a copy of the pixel data in the surface,
        /// inside the rectangle requested.
        /// </summary>
        /// <param name="format">Format of the pixel data wanted.  Automatic conversion is
        /// performed, if necessary.  Use PixelFormat.Any to retrieve data in its original format,
        /// without conversion.</param>
        /// <param name="rect">Area of the Surface from which to retrieve data.</param>
        /// <returns></returns>
        public PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
        {
            return impl.ReadPixels(format, rect);
        }

        /// <summary>
        /// Copies the data directly from PixelBuffer. The PixelBuffer must be the same width 
        /// and height as the Surface's SurfaceWidth and SurfaceHeight.
        /// </summary>
        /// <param name="buffer">The PixelBuffer which contains pixel data to copy from.</param>
        public void WritePixels(PixelBuffer buffer)
        {
            if (buffer.Width != SurfaceWidth || buffer.Height != SurfaceHeight)
                throw new ArgumentOutOfRangeException(
                    "PixelBuffer is not the correct size to write to entire surface!");

            impl.WritePixels(buffer);
        }
        /// <summary>
        /// Copies the data directly from PixelBuffer, overwriting a portion of the surface's 
        /// pixel data.  The PixelBuffer must fit within the surface.
        /// </summary>
        /// <param name="buffer">The PixelBuffer which contains pixel data to copy from.</param>
        /// <param name="startPoint"></param>
        public void WritePixels(PixelBuffer buffer, Point startPoint)
        {
            if (startPoint.X + buffer.Width > SurfaceWidth ||
                startPoint.Y + buffer.Height > SurfaceHeight)
                throw new ArgumentOutOfRangeException(
                    "PixelBuffer is too large!");

            impl.WritePixels(buffer, startPoint);
        }
        /// <summary>
        /// Copies the data directly from the PixelBuffer to the surface, overwriting a portion
        /// of the surface's pixel data.  The selected source rectangle from the pixel buffer must
        /// fit within the surface.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="sourceRect"></param>
        /// <param name="destPt"></param>
        public void WritePixels(PixelBuffer buffer, Rectangle sourceRect, Point destPt)
        {
            PixelBuffer smallBuffer = new PixelBuffer(buffer, sourceRect);

            WritePixels(smallBuffer, destPt);
        }

        #endregion

        /// <summary>
        /// Returns a value in the ImageFileFormat enum based on the file
        /// extension of the given filename.  No checks are done to see
        /// if that file exists.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ImageFileFormat FormatFromExtension(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename);

            switch (ext)
            {
                case ".bmp":
                    return ImageFileFormat.Bmp;

                case ".jpg":
                case ".jpe":
                case ".jpeg":
                    return ImageFileFormat.Jpg;

                case ".tga":
                    return ImageFileFormat.Tga;

                case ".png":
                default:
                    return ImageFileFormat.Png;
            }

        }

        /// <summary>
        /// Gets the object which does actual rendering of this surface.
        /// This may be cast to a surface object in whatever rendering library
        /// is being used (eg. if using the MDX_1_1 library, this can be cast
        /// to an MDX1_Surface object).  You only need to use this if you
        /// want to access features which are specific to the graphics library
        /// you're using.
        /// </summary>
        public SurfaceImpl Impl
        {
            get { return impl; }
        }

        #region --- IRenderTarget Members ---

        IRenderTargetImpl IRenderTarget.Impl
        {
            get { return impl; }
        }

        Size IRenderTarget.Size
        {
            get { return SurfaceSize; }
        }

        EventHandler ResizeEventHandlers;

        event EventHandler IRenderTarget.Resize
        {
            add { ResizeEventHandlers += value; }
            remove { ResizeEventHandlers -= value; }
        }

        internal void OnResize()
        {
            if (ResizeEventHandlers != null)
            {
                ResizeEventHandlers(this, EventArgs.Empty);
            }
        }


        int IRenderTarget.Width
        {
            get { return SurfaceWidth; }
        }

        int IRenderTarget.Height
        {
            get { return SurfaceHeight; }
        }

        #endregion

        internal void Draw(float x, float y, Rectangle srcRect, float rotationCenterX, float rotationCenterY)
        {
            impl.Draw(x, y, srcRect, rotationCenterX, rotationCenterY);
        }
    }
}
