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
using System.Text;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a Surface structure.
	/// </summary>
	public abstract class SurfaceImpl : IRenderTargetImpl, IDisposable
	{
		#region --- Private Fields ---

		private bool mIsDisposed = false;
		private bool mShouldBePacked = true;

		private int mTesselate = 1;

		#endregion

		#region --- Creation / Destruction ---

		/// <summary>
		/// Constructs a SurfaceImpl object.
		/// </summary>
		public SurfaceImpl()
		{
		}
		/// <summary>
		/// Frees unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		#endregion

		#region --- Drawing the surface to the screen ---

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
		/// <param name="surface"></param>
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
		public virtual InterpolationMode InterpolationHint
		{
			get { return mInterpolationHint; }
			set { mInterpolationHint = value; }
		}
		/// <summary>
		/// Gets or sets how many squares the surface should be broken into when drawn.
		/// </summary>
		public int TesselateFactor
		{
			get { return mTesselate; }
			set
			{
				if (value < 1) value = 1;

				mTesselate = value;
			}
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
		protected bool IsRowBlankScanARGB(IntPtr pixelData, int row, int cols, int strideInBytes,
			int alphaThreshold, uint alphaMask, int alphaShift)
		{
			unsafe
			{
				uint* ptr = (uint*)pixelData;

				int start = row * strideInBytes / sizeof(uint);

				for (int i = 0; i < cols; i++)
				{
					int index = start + i;
					uint pixel = ptr[index];
					byte alpha = (byte)((pixel & alphaMask) >> alphaShift);

					if (alpha > alphaThreshold)
					{
						return false;
					}

				}
			}

			return true;
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
		protected bool IsColBlankScanARGB(IntPtr pixelData, int col, int rows, int strideInBytes,
			int alphaThreshold, uint alphaMask, int alphaShift)
		{
			unsafe
			{
				uint* ptr = (uint*)pixelData;


				for (int i = 0; i < rows; i++)
				{
					int index = col + i * strideInBytes / sizeof(uint);
					uint pixel = ptr[index];
					byte alpha = (byte)((pixel & alphaMask) >> alphaShift);

					if (alpha > alphaThreshold)
					{
						return false;
					}

				}
			}

			return true;

		}
		#endregion



		#region --- IRenderTargetImpl Members ---

		/// <summary>
		/// Utility function which can be called by BeginFrame to begin
		/// a render pass.
		/// </summary>
		public abstract void BeginRender();
		/// <summary>
		/// Utility function which can be called by EndFrame to end a render pass.
		/// </summary>
		public abstract void EndRender();

		Size IRenderTargetImpl.Size
		{
			get { return SurfaceSize; }
		}

		int IRenderTargetImpl.Width
		{
			get { return SurfaceWidth; }
		}

		int IRenderTargetImpl.Height
		{
			get { return SurfaceHeight; }
		}

		#endregion

	};

}
