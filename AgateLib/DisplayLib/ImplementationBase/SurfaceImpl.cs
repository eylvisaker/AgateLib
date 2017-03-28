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

using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a Surface structure.
	/// </summary>
	public abstract class SurfaceImpl : IDisposable
	{
		#region --- Private Fields ---

		private bool mIsDisposed = false;
		private bool mShouldBePacked = true;

		#endregion

		#region --- Creation / Destruction ---

		/// <summary>
		/// Constructs a SurfaceImpl object.
		/// </summary>
		protected SurfaceImpl()
		{
		}
		/// <summary>
		/// Finalizes a SurfaceImpl object
		/// </summary>
		~SurfaceImpl()
		{
			Dispose(false);
		}

		/// <summary>
		/// Frees unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			mIsDisposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Implemented by deriving classes to clean up unmanaged resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected abstract void Dispose(bool disposing);

		#endregion

		#region --- Drawing the surface to the screen ---

		/// <summary>
		/// Draws the surface to the screen using the specified state information.
		/// </summary>
		/// <param name="state"></param>
		public abstract void Draw(SurfaceState state);

		#endregion

		#region --- Surface Data Manipulations ---

		/// <summary>
		/// Saves the surface data to the specified file in the specified format.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public abstract void SaveTo(string filename, ImageFileFormat format);

		/// <summary>
		/// Creates a new SurfaceImpl object which comes from a small sub-rectangle on this surface.
		/// </summary>
		/// <param name="srcRect"></param>
		/// <returns></returns>
		public abstract SurfaceImpl CarveSubSurface(Rectangle srcRect);

		/// <summary>
		/// Used by Display.BuildPackedSurface.
		/// </summary>
		/// <param name="surf"></param>
		/// <param name="srcRect"></param>
		public abstract void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect);


		/// <summary>
		/// Creates a PixelBuffer object with a copy of the pixel data, in the specified format.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public virtual PixelBuffer ReadPixels(PixelFormat format)
		{
			return ReadPixels(format, new Rectangle(Point.Zero, SurfaceSize));
		}
		/// <summary>
		/// Creates a PixelBuffer object with a copy of the pixel data in the 
		/// specified rectangle, in the specified format.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="rect"></param>
		/// <returns></returns>
		public abstract PixelBuffer ReadPixels(PixelFormat format, Rectangle rect);
		/// <summary>
		/// Writes pixel data to the surface.
		/// </summary>
		/// <param name="buffer"></param>
		public abstract void WritePixels(PixelBuffer buffer);
		/// <summary>
		/// Writes pixel data to the surface.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="startPoint"></param>
		public virtual void WritePixels(PixelBuffer buffer, Point startPoint)
		{
			// poor man's method
			PixelBuffer pixels = ReadPixels(PixelFormat.RGBA8888);

			pixels.CopyFrom(buffer, new Rectangle(Point.Zero, buffer.Size), startPoint, false);

			WritePixels(pixels);
		}

		#endregion
		#region --- Surface properties ---


		private InterpolationMode mInterpolationHint;
		/// <summary>
		/// Gets or sets the InterpolationMode value, indicating 
		/// what sampling to use for surfaces.  This property may 
		/// be replaced by something else in the future.
		/// </summary>
		public virtual InterpolationMode InterpolationHint
		{
			get { return mInterpolationHint; }
			set { mInterpolationHint = value; }
		}
		/// <summary>
		/// Returns true if Dispose() has been called on this surface.
		/// </summary>
		public bool IsDisposed
		{
			get { return mIsDisposed; }
		}

		/// <summary>
		/// Gets or sets a bool value which indicates whether or not this surface
		/// should be considered for packing when Display.PackAllSurfaces() is 
		/// called.
		/// </summary>
		public bool ShouldBePacked
		{
			get { return mShouldBePacked; }
			set { mShouldBePacked = value; }
		}

		/// <summary>
		/// Gets the width of the source surface in pixels.
		/// </summary>
		public virtual int SurfaceWidth { get { return SurfaceSize.Width; } }
		/// <summary>
		/// Gets the height of the source surface in pixels.
		/// </summary>
		public virtual int SurfaceHeight { get { return SurfaceSize.Height; } }
		/// <summary>
		/// Gets the Size of the source surface in pixels.
		/// </summary>
		public abstract Size SurfaceSize { get; }


		#endregion

		#region --- Protected Methods ---
		
		/// <summary>
		/// Scans a memory area to see if it entirely contains pixels which won't be
		/// seen when drawn.
		/// </summary>
		/// <param name="pixelData">Pointer to the data</param>
		/// <param name="row">Which row to check</param>
		/// <param name="cols">How many columns to check</param>
		/// <param name="strideInBytes">The stride of each row</param>
		/// <param name="alphaThreshold">The maximum value of alpha to consider a pixel transparent.</param>
		/// <param name="alphaMask">The mask to use to extract the alpha value from the data.</param>
		/// <param name="alphaShift">How many bits to shift it to get alpha in the range of 0-255.
		/// For example, if alphaMask = 0xff000000 then alphaShift should be 24.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		protected virtual bool IsRowBlankScanARGB(IntPtr pixelData, int row, int cols, int strideInBytes,
			int alphaThreshold, uint alphaMask, int alphaShift)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Scans a memory area to see if it entirely contains pixels which won't be
		/// seen when drawn.
		/// </summary>
		/// <param name="pixelData">Pointer to the data</param>
		/// <param name="col">Which col to check</param>
		/// <param name="rows">How many columns to check</param>
		/// <param name="strideInBytes">The stride of each row</param>
		/// <param name="alphaThreshold">The maximum value of alpha to consider a pixel transparent.</param>
		/// <param name="alphaMask">The mask to use to extract the alpha value from the data.</param>
		/// <param name="alphaShift">How many bits to shift it to get alpha in the range of 0-255.
		/// For example, if alphaMask = 0xff000000 then alphaShift should be 24.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		protected virtual bool IsColBlankScanARGB(IntPtr pixelData, int col, int rows, int strideInBytes,
			int alphaThreshold, uint alphaMask, int alphaShift)
		{
			throw new NotImplementedException();
		}

		#endregion

		/// <summary>
		/// Returns true if loading of the surface data is complete.
		/// </summary>
		public abstract bool IsLoaded { get; }
		/// <summary>
		/// Event which is raised when loading is completed. If loading is already complete,
		/// the delegate gets executed immediately.
		/// </summary>
		/// <remarks>Delegates added to this event are executed exactly once, after which 
		/// they are removed. Event handlers may be executed on the thread that is loading
		/// the surface, or they may be executed on the thread that adds them to the delegate.
		/// </remarks>
		public abstract event EventHandler LoadComplete;
	};
}
