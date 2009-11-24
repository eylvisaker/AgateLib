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
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;
using AgateLib.Utility;
using AgateLib.WinForms;

namespace AgateOTK
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	public sealed class GL_Surface : SurfaceImpl
	{
		GL_Display mDisplay;
		GLDrawBuffer mDrawBuffer;

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
			mDisplay = Display.Impl as GL_Display;
			mDrawBuffer = mDisplay.DrawBuffer;

			mFilename = filename;

			Load();
		}
		public GL_Surface(Stream st)
		{
			mDisplay = Display.Impl as GL_Display;
			mDrawBuffer = mDisplay.DrawBuffer;

			// Load The Bitmap
			Drawing.Bitmap sourceImage = new Drawing.Bitmap(st);

			LoadFromBitmap(sourceImage);
		}
		public GL_Surface(Size size)
		{
			mDisplay = Display.Impl as GL_Display;
			mDrawBuffer = mDisplay.DrawBuffer;

			mSourceRect = new Rectangle(Point.Empty, size);

			mTextureSize = GetOGLSize(size);

			int textureID;
			GL.GenTextures(1, out textureID);

			AddTextureRef(textureID);

			IntPtr fake = IntPtr.Zero;

			try
			{
				fake = Marshal.AllocHGlobal(mTextureSize.Width * mTextureSize.Height * Marshal.SizeOf(typeof(int)));

				// Typical Texture Generation Using Data From The Bitmap
				GL.BindTexture(TextureTarget.Texture2D, mTextureID);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
					mTextureSize.Width, mTextureSize.Height, 0, OTKPixelFormat.Rgba,
					PixelType.UnsignedByte, fake);

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
			mDisplay = Display.Impl as GL_Display;
			mDrawBuffer = mDisplay.DrawBuffer;

			AddTextureRef(textureID);

			mSourceRect = sourceRect;
			mTextureSize = textureSize;

			mTexCoord = GetTextureCoords(mSourceRect);

		}

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
		public override void Dispose()
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
			SizeF displaySize = state.GetDisplaySize(SurfaceSize);
			PointF rotationCenter = state.GetRotationCenter(displaySize);
			float mRotationCos = (float)Math.Cos(state.RotationAngle);
			float mRotationSin = (float)Math.Sin(state.RotationAngle);
			SizeF dispSize = new SizeF(
				srcRect.Width * (float)state.ScaleWidth,
				srcRect.Height * (float)state.ScaleHeight);

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

			mDrawBuffer.SetInterpolationMode(InterpolationHint);

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


			mDrawBuffer.AddQuad(mTextureID, color, texCoord, pt);
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
			return ReadPixels(format, new Rectangle(Point.Empty, SurfaceSize));
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
					int dataIndex = (i - rect.Top) * pixelStride * rect.Width;
					IntPtr memPtr = (IntPtr)((byte*)memory + i * memStride + rect.Left * pixelStride);

					Marshal.Copy(memPtr, data, dataIndex, pixelStride * rect.Width);
				}
			}

			Marshal.FreeHGlobal(memory);

			if (format == PixelFormat.RGBA8888)
				return new PixelBuffer(format, SurfaceSize, data);
			else
				return new PixelBuffer(format, SurfaceSize, data, PixelFormat.RGBA8888);
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

		internal int GLTextureID { get { return mTextureID; } }

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

			mSourceRect.Size = Interop.Convert(sourceImage.Size);

			Size newSize = GetOGLSize(sourceImage);

			// create a new bitmap of the size OpenGL expects, and copy the source image to it.
			Drawing.Bitmap textureImage = new Drawing.Bitmap(newSize.Width, newSize.Height);
			Drawing.Graphics g = Drawing.Graphics.FromImage(textureImage);

			g.Transform = new System.Drawing.Drawing2D.Matrix();
			g.Clear(Drawing.Color.FromArgb(0, 0, 0, 0));
			g.DrawImage(sourceImage, new Drawing.Rectangle(new Drawing.Point(0, 0), sourceImage.Size));
			g.Dispose();

			mTextureSize = Interop.Convert(textureImage.Size);

			mTexCoord = GetTextureCoords(mSourceRect);

			// Rectangle For Locking The Bitmap In Memory
			Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);

			// Get The Bitmap's Pixel Data From The Locked Bitmap
			BitmapData bitmapData = textureImage.LockBits(Interop.Convert(rectangle),
				ImageLockMode.ReadOnly, Drawing.Imaging.PixelFormat.Format32bppArgb);

			// use a pixelbuffer to do format conversion.
			PixelBuffer buffer = new PixelBuffer(PixelFormat.RGBA8888, mTextureSize,
				bitmapData.Scan0, PixelFormat.BGRA8888, bitmapData.Stride);

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
			return GetOGLSize(Interop.Convert(image.Size));
		}
		private Size GetOGLSize(Size size)
		{
			Size retval = size;

			if (mDisplay.SupportsNonPowerOf2Textures)
				return retval;

			if (IsPowerOfTwo(retval.Width) == false)
				retval.Width = NextPowerOfTwo(retval.Width);
			if (IsPowerOfTwo(retval.Height) == false)
				retval.Height = NextPowerOfTwo(retval.Height);

			return retval;
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

			return coords;
		}
		private TextureCoordinates GetTextureCoords(RectangleF srcRect)
		{
			TextureCoordinates coords = new TextureCoordinates(
				(srcRect.Left) / (float)mTextureSize.Width,
				(srcRect.Top) / (float)mTextureSize.Height,
				(srcRect.Right) / (float)mTextureSize.Width,
				(srcRect.Bottom) / (float)mTextureSize.Height);

			return coords;
		}

	}
}
