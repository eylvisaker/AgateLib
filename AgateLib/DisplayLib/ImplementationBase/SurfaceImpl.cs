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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

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
			return ReadPixels(format, new Rectangle(Point.Empty, SurfaceSize));
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

			pixels.CopyFrom(buffer, new Rectangle(Point.Empty, buffer.Size), startPoint, false);

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

		public abstract bool IsLoaded { get; }
	};
}
