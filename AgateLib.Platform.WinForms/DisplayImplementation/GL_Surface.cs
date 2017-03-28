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
using Drawing = System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Mathematics.Geometry;
using AgateLib.Utility;
using AgateLib.OpenGL;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	public sealed class GL_Surface : SurfaceImpl, IGL_Surface
	{
		DesktopGLDisplay mDisplay;

		string mFilename;

		/// <summary>
		/// Refrence counting for the texture id's.
		/// </summary>
		static Dictionary<int, int> mTextureIDs = new Dictionary<int, int>();
		int mTextureID;


		Rectangle mSourceRect;

		/// <summary>
		/// OpenGL's texture size (always a power of 2, unles NPOT extension is available.).
		/// </summary>
		Size mTextureSize;

		TextureCoordinates mTexCoord;


		public GL_Surface(string filename)
		{
			mDisplay = Display.Impl as DesktopGLDisplay;

			mFilename = filename;

			Load();
		}
		public GL_Surface(Stream st)
		{
			mDisplay = Display.Impl as DesktopGLDisplay;

			// Load The Bitmap
			using (Drawing.Bitmap sourceImage = new Drawing.Bitmap(st))
			{
				LoadFromBitmap(sourceImage);
			}
		}
		public GL_Surface(Size size)
		{
			mDisplay = Display.Impl as DesktopGLDisplay;

			mSourceRect = new Rectangle(Point.Zero, size);

			mTextureSize = GetOGLSize(size);

			int textureId;
			GL.GenTextures(1, out textureId);

			AddTextureRef(textureId);

			IntPtr fake = IntPtr.Zero;

			try
			{
				fake = Marshal.AllocHGlobal(mTextureSize.Width * mTextureSize.Height * Marshal.SizeOf(typeof(int)));

				// Typical Texture Generation Using Data From The Bitmap
				GL.BindTexture(TextureTarget.Texture2D, mTextureID);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
					mTextureSize.Width, mTextureSize.Height, 0, OTKPixelFormat.Rgba,
					PixelType.UnsignedByte, IntPtr.Zero);

				mTexCoord = GetTextureCoords(mSourceRect);
			}
			finally
			{
				if (fake != IntPtr.Zero)
					Marshal.FreeHGlobal(fake);
			}
		}

		private GL_Surface(int textureID, Rectangle sourceRect, Size textureSize)
		{
			mDisplay = Display.Impl as DesktopGLDisplay;

			AddTextureRef(textureID);

			mSourceRect = sourceRect;
			mTextureSize = textureSize;

			mTexCoord = GetTextureCoords(mSourceRect);
		}

		private GLDrawBuffer DrawBuffer => mDisplay.DrawBuffer;

		private void AddTextureRef(int textureID)
		{
			mTextureID = textureID;

			if (mTextureIDs.ContainsKey(mTextureID))
				mTextureIDs[mTextureID] += 1;
			else
				mTextureIDs.Add(mTextureID, 1);
		}
		private void ReleaseTextureRef()
		{
			if (mTextureID == 0)
				return;

			mTextureIDs[mTextureID]--;

			if (mTextureIDs[mTextureID] == 0)
			{
				int[] array = new int[1];
				array[0] = mTextureID;

				if (OpenTK.Graphics.GraphicsContext.CurrentContext == null)
				{
					mDisplay.QueueDeleteTexture(array[0]);
				}
				else
					GL.DeleteTextures(1, array);

				mTextureIDs.Remove(mTextureID);
			}

			mTextureID = 0;
		}
		protected override void Dispose(bool disposing)
		{
			ReleaseTextureRef();
		}

		public override void Draw(SurfaceState state)
		{
			for (int i = 0; i < state.DrawInstances.Count; i++)
			{
				Draw(state, state.DrawInstances[i]);
			}
		}

		private void Draw(SurfaceState state, SurfaceDrawInstance inst)
		{
			float destX = inst.DestLocation.X;
			float destY = inst.DestLocation.Y;
			Rectangle srcRect = inst.GetSourceRect(SurfaceSize);
			SizeF displaySize = state.GetDisplaySize(srcRect.Size);
			PointF rotationCenter = state.GetRotationCenter(displaySize);
			float mRotationCos = (float)Math.Cos(state.RotationAngle);
			float mRotationSin = (float)Math.Sin(state.RotationAngle);
			SizeF dispSize = new SizeF(
				srcRect.Width * (float)state.ScaleWidth,
				srcRect.Height * (float)state.ScaleHeight);

			srcRect.X += mSourceRect.X;
			srcRect.Y += mSourceRect.Y;

			if (displaySize.Width < 0)
			{
				destX -= dispSize.Width;
				rotationCenter.X += dispSize.Width;
			}
			if (displaySize.Height < 0)
			{
				destY -= dispSize.Height;
				rotationCenter.Y += dispSize.Height;
			}

			mTexCoord = GetTextureCoords(srcRect);

			DrawBuffer.SetInterpolationMode(InterpolationHint);

			BufferQuad(destX, destY, rotationCenter.X, rotationCenter.Y,
				dispSize.Width, dispSize.Height, mTexCoord, state.ColorGradient,
				state.DisplayAlignment, mRotationCos, mRotationSin);
		}

		PointF[] cachePt = new PointF[4];

		private void BufferQuad(float destX, float destY, float rotationCenterX, float rotationCenterY,
			float displayWidth, float displayHeight, TextureCoordinates texCoord, Gradient color,
			 OriginAlignment DisplayAlignment, float mRotationCos, float mRotationSin)
		{

			// order is 
			//  1 -- 2
			//  |    |
			//  4 -- 3
			PointF[] pt = cachePt;

			SetPoints(pt, destX, destY,
				rotationCenterX, rotationCenterY, displayWidth, displayHeight,
				DisplayAlignment, mRotationCos, mRotationSin);

			//RectangleF destRect = new RectangleF(new PointF(-rotationCenterX, -rotationCenterY),
			//                     new SizeF(displayWidth, displayHeight));


			DrawBuffer.AddQuad(mTextureID, color, texCoord, pt);
		}

		private void SetPoints(PointF[] pt, float destX, float destY, float rotationCenterX, float rotationCenterY,
							   float destWidth, float destHeight, OriginAlignment DisplayAlignment,
							   float mRotationCos, float mRotationSin)
		{
			const int index = 0;
			PointF centerPoint = Origin.CalcF(DisplayAlignment, new SizeF(destWidth, destHeight));

			destX += rotationCenterX - centerPoint.X;
			destY += rotationCenterY - centerPoint.Y;

			// Point at (0, 0) local coordinates
			pt[index].X = mRotationCos * (-rotationCenterX) +
						 mRotationSin * (-rotationCenterY) + destX;

			pt[index].Y = -mRotationSin * (-rotationCenterX) +
						  mRotationCos * (-rotationCenterY) + destY;

			// Point at (DisplayWidth, 0) local coordinates
			pt[index + 1].X = mRotationCos * (-rotationCenterX + destWidth) +
						 mRotationSin * (-rotationCenterY) + destX;

			pt[index + 1].Y = -mRotationSin * (-rotationCenterX + destWidth) +
						  mRotationCos * (-rotationCenterY) + destY;

			// Point at (DisplayWidth, DisplayHeight) local coordinates
			pt[index + 2].X = mRotationCos * (-rotationCenterX + destWidth) +
						 mRotationSin * (-rotationCenterY + destHeight) + destX;

			pt[index + 2].Y = -mRotationSin * (-rotationCenterX + destWidth) +
						  mRotationCos * (-rotationCenterY + destHeight) + destY;

			// Point at (0, DisplayHeight) local coordinates
			pt[index + 3].X = mRotationCos * (-rotationCenterX) +
						 mRotationSin * (-rotationCenterY + destHeight) + destX;

			pt[index + 3].Y = (-mRotationSin * (-rotationCenterX) +
						   mRotationCos * (-rotationCenterY + destHeight)) + destY;

		}

		public override void SaveTo(string filename, ImageFileFormat format)
		{
			PixelBuffer buffer = ReadPixels(PixelFormat.Any);
			buffer.SaveTo(filename, format);
		}

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			srcRect.X += mSourceRect.X;
			srcRect.Y += mSourceRect.Y;

			return new GL_Surface(mTextureID, srcRect, mTextureSize);
		}

		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			ReleaseTextureRef();
			AddTextureRef((surf as GL_Surface).mTextureID);

			mTextureSize = (surf as GL_Surface).mTextureSize;
			mSourceRect = srcRect;

			mTexCoord = GetTextureCoords(mSourceRect);

		}

		public override PixelBuffer ReadPixels(PixelFormat format)
		{
			return ReadPixels(format, new Rectangle(Point.Zero, SurfaceSize));
		}
		public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
		{
			if (format == PixelFormat.Any)
				format = PixelFormat.RGBA8888;

			rect.X += mSourceRect.X;
			rect.Y += mSourceRect.Y;

			int pixelStride = 4;
			int size = mTextureSize.Width * mTextureSize.Height * pixelStride;
			int memStride = pixelStride * mTextureSize.Width;
			IntPtr memory = Marshal.AllocHGlobal(size);

			GL.BindTexture(TextureTarget.Texture2D, mTextureID);
			GL.GetTexImage(TextureTarget.Texture2D, 0, OTKPixelFormat.Rgba,
				 PixelType.UnsignedByte, memory);

			byte[] data = new byte[rect.Width * rect.Height * pixelStride];

			unsafe
			{
				for (int i = rect.Top; i < rect.Bottom; i++)
				{
					int row = i - rect.Top;

					if (FlipVertical)
					{
						row = rect.Height - row - 1;
					}

					int dataIndex = (row) * pixelStride * rect.Width;
					IntPtr memPtr = (IntPtr)((byte*)memory + i * memStride + rect.Left * pixelStride);

					Marshal.Copy(memPtr, data, dataIndex, pixelStride * rect.Width);
				}
			}

			Marshal.FreeHGlobal(memory);

			if (format == PixelFormat.RGBA8888)
				return new PixelBuffer(format, rect.Size, data);
			else
				return new PixelBuffer(format, rect.Size, data, PixelFormat.RGBA8888);
		}

		public override void WritePixels(PixelBuffer buffer)
		{
			if (buffer.PixelFormat != PixelFormat.RGBA8888 ||
				buffer.Size.Equals(mTextureSize) == false)
			{
				buffer = buffer.ConvertTo(PixelFormat.RGBA8888, mTextureSize);
			}

			unsafe
			{
				fixed (byte* ptr = buffer.Data)
				{
					// Typical Texture Generation Using Data From The Bitmap
					GL.BindTexture(TextureTarget.Texture2D, mTextureID);
					GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
						mTextureSize.Width, mTextureSize.Height, 0, OTKPixelFormat.Rgba,//, GL.GL_BGRA, 
						PixelType.UnsignedByte, (IntPtr)ptr);

					GL.TexParameter(TextureTarget.Texture2D,
									 TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.TexParameter(TextureTarget.Texture2D,
									 TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				}
			}
		}

		public override Size SurfaceSize
		{
			get { return mSourceRect.Size; }
		}
		public override bool IsLoaded
		{
			// We don't currently support background loading for OpenGL.
			// So return true because if the surface is constructed, it's been loaded.
			get { return true; }
		}
		public override event EventHandler LoadComplete
		{
			add
			{
				// since loading is synchronously performed in the constructor, just
				// execute any delegate that comes in immediately.
				value(this, EventArgs.Empty);
			}
			remove { }
		}

		public int GLTextureID { get { return mTextureID; } }

		private void Load()
		{
			if (mFilename == "")
				return;


			// Load The Bitmap
			Drawing.Bitmap sourceImage = new Drawing.Bitmap(mFilename);

			LoadFromBitmap(sourceImage);

			sourceImage.Dispose();
		}

		private void LoadFromBitmap(Drawing.Bitmap sourceImage)
		{
			Drawing.Bitmap textureImage;
			Size newSize = GetOGLSize(sourceImage);

			mSourceRect.Size = sourceImage.Size.ToGeometry();

			if (newSize != mSourceRect.Size)
			{
				// create a new bitmap of the size OpenGL expects, and copy the source image to it.
				textureImage = new Drawing.Bitmap(newSize.Width, newSize.Height);
				Drawing.Graphics g = Drawing.Graphics.FromImage(textureImage);

				g.Transform = new System.Drawing.Drawing2D.Matrix();
				g.Clear(Drawing.Color.FromArgb(0, 0, 0, 0));
				g.DrawImage(sourceImage, new Drawing.Rectangle(new Drawing.Point(0, 0), sourceImage.Size));
				g.Dispose();
			}
			else
				textureImage = sourceImage;

			mTextureSize = textureImage.Size.ToGeometry();

			mTexCoord = GetTextureCoords(mSourceRect);

			// Rectangle For Locking The Bitmap In Memory
			Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);

			// Get The Bitmap's Pixel Data From The Locked Bitmap
			BitmapData bitmapData = textureImage.LockBits(rectangle.ToDrawing(),
				ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);

			// use a pixelbuffer to do format conversion.
			PixelBuffer buffer = new PixelBuffer(PixelFormat.RGBA8888, mTextureSize);

			buffer.SetData(bitmapData.Scan0, PixelFormat.BGRA8888, bitmapData.Stride);

			// Create The GL Texture object
			int textureID;

			GL.GenTextures(1, out textureID);
			AddTextureRef(textureID);

			WritePixels(buffer);
			textureImage.UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
			textureImage.Dispose();                                  // Dispose The Bitmap
		}

		private Size GetOGLSize(System.Drawing.Bitmap image)
		{
			return GetOGLSize(image.Size.ToGeometry());
		}
		private Size GetOGLSize(Size size)
		{
			Size result = size;

			if (mDisplay.SupportsNonPowerOf2Textures)
				return result;

			if (IsPowerOfTwo(result.Width) == false)
				result.Width = NextPowerOfTwo(result.Width);
			if (IsPowerOfTwo(result.Height) == false)
				result.Height = NextPowerOfTwo(result.Height);

			return result;
		}
		private bool IsPowerOfTwo(int value)
		{
			if (value < 0)
				throw new ArgumentException("value cannot be negative.");

			while (value > 1)
			{
				if ((value & 1) == 1)
					return false;

				value >>= 1;
			}

			return true;
		}
		private int NextPowerOfTwo(int p)
		{
			return (int)Math.Pow(2, (int)(Math.Log(p) / Math.Log(2)) + 1);
		}
		private TextureCoordinates GetTextureCoords(Rectangle srcRect)
		{
			TextureCoordinates coords = new TextureCoordinates(
				(srcRect.Left) / (float)mTextureSize.Width,
				(srcRect.Top) / (float)mTextureSize.Height,
				(srcRect.Right) / (float)mTextureSize.Width,
				(srcRect.Bottom) / (float)mTextureSize.Height);

			if (FlipVertical)
			{
				var t = coords.Top;
				coords.Top = coords.Bottom;
				coords.Bottom = t;
			}

			return coords;
		}
		private TextureCoordinates GetTextureCoords(RectangleF srcRect)
		{
			TextureCoordinates coords = new TextureCoordinates(
				(srcRect.Left) / (float)mTextureSize.Width,
				(srcRect.Top) / (float)mTextureSize.Height,
				(srcRect.Right) / (float)mTextureSize.Width,
				(srcRect.Bottom) / (float)mTextureSize.Height);

			if (FlipVertical)
			{
				var t = coords.Top;
				coords.Top = coords.Bottom;
				coords.Bottom = t;
			}

			return coords;
		}

		/// <summary>
		/// Used for framebuffer surfaces which need to be flipped vertically for some reason.
		/// </summary>
		public bool FlipVertical { get; set; }

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
		protected override bool IsRowBlankScanARGB(IntPtr pixelData, int row, int cols, int strideInBytes,
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
		protected override bool IsColBlankScanARGB(IntPtr pixelData, int col, int rows, int strideInBytes,
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
	}
}
