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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Utility;

namespace AgateLib.DisplayLib
{
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
	public sealed class Surface : IDisposable, ISurface
	{
		SurfaceImpl impl;
		SurfaceState mState = new SurfaceState();

		/// <summary>
		/// Creates a surface object from a resource.
		/// </summary>
		/// <param name="resources"></param>
		/// <param name="name"></param>
		public Surface(Resources.AgateResourceCollection resources, string name)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			Resources.SurfaceResource surf = resources.Surfaces[name];
					    
			impl = surf.CreateSurfaceImpl();

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
			Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
		}
		/// <summary>
		/// Creates a surface object, from the specified image file.
		/// </summary>
		/// <param name="filename">Path and file name for the image file to load.</param>
		public Surface(string filename)
			: this(AgateFileProvider.Images, filename)
		{
		}
		/// <summary>
		/// Creates a surface object using the specified file provider to open the image file.
		/// </summary>
		/// <param name="filename">Path and file name for the image file to load.</param>
		/// <param name="fileProvider">The IFileProvider object which will resolve the filename and open the stream</param>
		public Surface(IFileProvider fileProvider, string filename)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			using (System.IO.Stream s = fileProvider.OpenRead(filename))
			{
				impl = Display.Impl.CreateSurface(s);
			}

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
			Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
		}
		/// <summary>
		/// Creates a surface object from the data in the specified stream.
		/// </summary>
		/// <param name="st">Stream from which to load the surface data from.</param>
		public Surface(Stream st)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			impl = Display.Impl.CreateSurface(st);

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
			Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
		}
		/// <summary>
		/// Creates a surface object of the specified size.  
		/// The surface is initialized to contain pixels with ARGB = (0,0,0,0).
		/// </summary>
		/// <param name="width">Width of the newly created surface.</param>
		/// <param name="height">Height of the newly created surface.</param>
		public Surface(int width, int height)
			: this(new Size(width, height))
		{

		}
		/// <summary>
		/// Creates a surface object of the specified size.
		/// The surface is initialized to contain pixels with ARGB = (0,0,0,0).
		/// </summary>
		/// <param name="size">Size of the newly created surface.</param>
		public Surface(Size size)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			impl = Display.Impl.CreateSurface(size);

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
			Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);
		}
		/// <summary>
		/// Constructs a surface object from the specified PixelBuffer object.
		/// </summary>
		/// <param name="pixels">The PixelBuffer containing the pixel data to use.</param>
		public Surface(PixelBuffer pixels)
			: this(pixels.Size)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			WritePixels(pixels);
		}
		/// <summary>
		/// Creates a surface object and to be ready to attach to an implemented object.
		/// </summary>
		/// <param name="fromImpl"></param>
		internal Surface(SurfaceImpl fromImpl)
		{
			if (Display.Impl == null)
				throw new AgateException("AgateLib's display system has not been initialized.");

			if (fromImpl == null)
				throw new ArgumentNullException("The argument fromImpl must not be null.");

			Display.DisposeDisplay += new Display.DisposeDisplayHandler(Dispose);
			Display.PackAllSurfacesEvent += new EventHandler(Display_PackAllSurfacesEvent);

			impl = fromImpl;
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
		/// Gets or sets a value indicating how to sample points from this surface.
		/// </summary>
		public InterpolationMode InterpolationHint
		{
			get { return impl.InterpolationHint; }
			set { impl.InterpolationHint = value; }
		}

		/// <summary>
		/// Gets or sets the state of the surface.
		/// </summary>
		public SurfaceState State
		{
			get { return mState; }
			set { mState = value; }
		}

		/// <summary>
		/// Get or sets the width of the surface in pixels when it will be displayed on screen.
		/// </summary>
		public int DisplayWidth
		{
			get { return (int)(mState.ScaleWidth * SurfaceWidth); }
			set { ScaleWidth = value / (double)SurfaceWidth; }
		}
		/// <summary>
		/// Gets or sets the height of the surface in pixels when it is displayed on screen.
		/// </summary>
		public int DisplayHeight
		{
			get { return (int)(mState.ScaleHeight * SurfaceHeight); }
			set { ScaleHeight = value / (double)SurfaceHeight; }
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
		/// If a gradient is used, getting this property returns the alpha value for the top left
		/// corner of the gradient.
		/// </summary>
		public double Alpha
		{
			get { return Color.A / 255.0; }
			set
			{
				Gradient g = mState.ColorGradient;
				g.SetAlpha(value);

				mState.ColorGradient = g;
			}
		}
		/// <summary>
		/// Gets or sets the rotation angle in radians.
		/// Positive angles indicate rotation in the Counter-Clockwise direction.
		/// </summary>
		public double RotationAngle
		{
			get { return mState.RotationAngle; }
			set { mState.RotationAngle = value; }
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
			get { return mState.RotationCenter; }
			set { mState.RotationCenter = value; }
		}
		/// <summary>
		/// Gets or sets the point where the surface is aligned to when drawn.
		/// </summary>
		public OriginAlignment DisplayAlignment
		{
			get { return mState.DisplayAlignment; }
			set { mState.DisplayAlignment = value; }
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
			get { return mState.ScaleWidth; }
			set { mState.ScaleWidth = value; }
		}
		/// <summary>
		/// Gets or sets the amount the height is scaled when this surface is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		public double ScaleHeight
		{
			get { return mState.ScaleHeight; }
			set { mState.ScaleHeight = value; }
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
			ScaleWidth = width;
			ScaleHeight = height;
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
			width = mState.ScaleWidth;
			height = mState.ScaleHeight;
		}


		/// <summary>
		/// Gets or sets the multiplicative color for this surface.
		/// Setting this is equivalent to setting the ColorGradient property
		/// with a gradient with the same color in all corners.  If a gradient
		/// is being used, getting this property returns the top-left color in the gradient.
		/// </summary>
		public Color Color
		{
			get { return mState.Color; }
			set { mState.Color = value; }
		}
		/// <summary>
		/// Gets or sets the gradient for this surface.
		/// </summary>
		public Gradient ColorGradient
		{
			get { return mState.ColorGradient; }
			set { mState.ColorGradient = value; }
		}
		/// <summary>
		/// Increments the rotation angle of this surface.
		/// </summary>
		/// <param name="radians">Value in radians to increase the rotation by.</param>
		public void IncrementRotationAngle(double radians)
		{
			mState.RotationAngle += radians;
		}
		/// <summary>
		/// Increments the rotation angle of this surface.  Value supplied is in degrees.
		/// </summary>
		/// <param name="degrees"></param>
		public void IncrementRotationAngleDegrees(double degrees)
		{
			mState.IncrementRotationAngleDegrees(degrees);
		}

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
		/// <param name="destX">X position to draw to.</param>
		/// <param name="destY">Y position to draw to.</param>
		public void Draw(int destX, int destY)
		{
			mState.DrawInstances.SetCount(1);
			mState.DrawInstances[0] = new SurfaceDrawInstance(new PointF(destX, destY));

			impl.Draw(State);
		}
		/// <summary>
		/// Draws this surface to the screen at the specified point, 
		/// using all the state information defined in the properties 
		/// of this surface.
		/// </summary>
		/// <param name="destX">X position to draw to.</param>
		/// <param name="destY">Y position to draw to.</param>
		public void Draw(float destX, float destY)
		{
			mState.DrawInstances.SetCount(1);
			mState.DrawInstances[0] = new SurfaceDrawInstance(new PointF(destX, destY));

			impl.Draw(State);
		}
		/// <summary>
		/// Draws this surface to the screen at the specified point, 
		/// using all the state information defined in the properties 
		/// of this surface.
		/// </summary>
		/// <param name="destPt">Destination point to draw to.</param>
		public void Draw(Point destPt)
		{
			Draw(destPt.X, destPt.Y);
		}
		/// <summary>
		/// Draws this surface to the screen at the specified point, 
		/// using all the state information defined in the properties 
		/// of this surface.
		/// </summary>
		/// <param name="destPt">Destination point to draw to.</param>
		public void Draw(Vector2 destPt)
		{
			Draw(destPt.X, destPt.Y);
		}
		/// <summary>
		/// Draws this surface to the screen at the specified point, 
		/// using all the state information defined in the properties 
		/// of this surface.
		/// </summary>
		/// <param name="destPt">Destination point to draw to.</param>
		public void Draw(PointF destPt)
		{
			Draw(destPt.X, destPt.Y);
		}
		/// <summary>
		/// Draws this surface to the screen at the specified point, 
		/// using all the state information defined in the properties 
		/// of this surface.
		/// Ignores the value of RotationCenter and uses the specified
		/// point to rotate around instead.  
		/// </summary>
		/// <param name="destPt">Destination point to draw to.</param>
		/// <param name="rotationCenter">Center of rotation to use, relative
		/// to the top-left of the surface.</param>
		public void Draw(PointF destPt, PointF rotationCenter)
		{
			Draw(destPt, Rectangle.Empty, rotationCenter);
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
			Draw(new PointF(destX, destY), new PointF(rotationCenterX, rotationCenterY));
		}

		internal void Draw(PointF destPt, Rectangle srcRect, PointF rotationCenter)
		{
			OriginAlignment oldrotation = State.RotationCenter;
			PointF oldcenter = State.RotationCenterLocation;

			State.RotationCenter = OriginAlignment.Specified;
			State.RotationCenterLocation = rotationCenter;

			State.DrawInstances.SetCount(1);
			State.DrawInstances[0] = new SurfaceDrawInstance(destPt, srcRect);

			impl.Draw(State);

			State.RotationCenterLocation = oldcenter;
			State.RotationCenter = oldrotation;
		}

		/// <summary>
		/// Draws the surface using the parameters in the specified state object.
		/// </summary>
		/// <param name="state">The surface state information to use when drawing.</param>
		public void Draw(SurfaceState state)
		{
			impl.Draw(state);
		}

		SurfaceState rectState;
		/// <summary>
		/// Draws a portion of this surface to the specified destination
		/// rectangle.  
		/// 
		/// State settings which are ignored are RotationAngle,
		/// DisplayAlignment and Scaling.  Color and alpha values
		/// are still used.
		/// </summary>
		/// <param name="srcRect">Source rectangle on the surface to draw from.</param>
		/// <param name="destRect">Destination rectangle in the render target to draw to.</param>
		public void Draw(Rectangle srcRect, Rectangle destRect)
		{
			if (rectState == null)
				rectState = State.Clone();
			else
				State.CopyTo(rectState, false);

			rectState.RotationAngle = 0;
			rectState.ScaleWidth = destRect.Width / (double)srcRect.Width;
			rectState.ScaleHeight = destRect.Height / (double)srcRect.Height;

			rectState.DrawInstances[0] = new SurfaceDrawInstance
			{
				SourceRect = srcRect,
				DestLocation = new PointF(destRect.X, destRect.Y),
			};

			impl.Draw(rectState);
		}

		/// <summary>
		/// Draws this surface to the specified destination
		/// rectangle.  
		/// 
		/// State settings which are ignored are RotationAngle,
		/// DisplayAlignment and Scaling.  Color and alpha values
		/// are still used.
		/// </summary>
		/// <param name="destRect">Destination rectangle in the render target to draw to.</param>
		public void Draw(Rectangle destRect)
		{
			Draw(new Rectangle(0, 0, SurfaceWidth, SurfaceHeight), destRect);
		}

		void Draw(RectangleF srcRect, RectangleF destRect)
		{
			if (rectState == null)
				rectState = State.Clone();
			else
				State.CopyTo(rectState, false);

			rectState.RotationAngle = 0;
			rectState.ScaleWidth = destRect.Width / (double)srcRect.Width;
			rectState.ScaleHeight = destRect.Height / (double)srcRect.Height;

			rectState.DrawInstances[0] = new SurfaceDrawInstance
			{
				SourceRect = Rectangle.Round(srcRect),
				DestLocation = new PointF(destRect.X, destRect.Y),
			};

			impl.Draw(rectState);
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

			DrawRects(srcRects, destRects, 0, srcRects.Length);
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

			DrawRects(srcRects, destRects, 0, srcRects.Length);
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
			// TODO: optimize this
			for (int i = start; i < length; i++)
				Draw(srcRects[i], destRects[i]);
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
			// TODO: optimize this
			for (int i = start; i < length; i++)
				Draw(srcRects[i], destRects[i]);
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
		/// <param name="filename">File name to save to.</param>
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
		/// <param name="filename">File name to save to.</param>
		/// <param name="format">Image format for the target file.</param>
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
			SurfaceImpl newImpl = impl.CarveSubSurface(srcRect);

			return new Surface(newImpl);
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
				throw new ArgumentException(
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
				throw new ArgumentException(
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
		/// is being used (eg. if using the SDX library, this can be cast
		/// to an SDX_Surface object).  You only need to use this if you
		/// want to access features which are specific to the graphics library
		/// you're using.
		/// </summary>
		public SurfaceImpl Impl
		{
			get { return impl; }
		}
	}

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
	/// Enum for indicating the sampling mode used when stretching or shrinking surfaces.
	/// </summary>
	public enum InterpolationMode
	{
		/// <summary>
		/// Use whatever the driver default is.
		/// </summary>
		Default,
		/// <summary>
		/// Use the fastest method, usually nearest neighbor pixel sampling.
		/// </summary>
		Fastest,
		/// <summary>
		/// Use the nicest method, usually bilinear sampling.
		/// </summary>
		Nicest,
	}
}