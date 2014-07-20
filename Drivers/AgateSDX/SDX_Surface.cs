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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using SlimDX;
using SlimDX.Direct3D9;
using Direct3D = SlimDX.Direct3D9;

using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using AgateLib.Utility;
using AgateLib.WinForms;

using Drawing = System.Drawing;
using ImageFileFormat = AgateLib.DisplayLib.ImageFileFormat;
using Surface = AgateLib.DisplayLib.Surface;
using Vector2 = AgateLib.Geometry.Vector2;

namespace AgateSDX
{
	public class SDX_Surface : SurfaceImpl
	{
		#region --- Private Variables ---

		SDX_Display mDisplay;
		D3DDevice mDevice;

		Ref<Texture> mTexture;

		string mFileName;

		Rectangle mSrcRect;
		Size mTextureSize;
		PointF mCenterPoint;

		PositionTextureColor[] mVerts = new PositionTextureColor[4];
		short[] mIndices = new short[] { 0, 2, 1, 1, 2, 3 };

		PositionTextureColor[] mExtraVerts = new PositionTextureColor[4];
		short[] mExtraIndices = new short[] { 0, 2, 1, 1, 2, 3 };

		#endregion

		public Texture D3dTexture
		{
			get { return mTexture.Value; }
		}

		#region --- TextureCoordinates structure ---

		struct TextureCoordinates
		{
			public float Left;
			public float Top;
			public float Right;
			public float Bottom;

			public TextureCoordinates(float left, float top, float right, float bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}

		#endregion

		#region --- Creation / Destruction ---

		public SDX_Surface()
		{
			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;

			InitVerts();
		}

		public SDX_Surface(string fileName)
		{
			mFileName = fileName;

			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;

			if (mDevice == null)
			{
				throw new Exception("Error: It appears that AgateLib has not been initialized yet.  Have you created a DisplayWindow?");
			}

			LoadFromFile();

			//mDevice.Device.DeviceReset += new EventHandler(mDevice_DeviceReset);

			InitVerts();
		}
		public SDX_Surface(Stream stream)
		{
			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;

			if (mDevice == null)
			{
				throw new Exception("Error: It appears that AgateLib has not been initialized yet.  Have you created a DisplayWindow?");
			}

			LoadFromStream(stream);

			InitVerts();
		}
		public SDX_Surface(Size size)
		{
			mSrcRect = new Rectangle(new Point(0, 0), size);

			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;
			
			mTexture = new Ref<Texture>(new Texture(mDevice.Device, size.Width, size.Height, 1, Usage.None,
				Format.A8R8G8B8, Pool.Managed));

			RenderToSurface render = new RenderToSurface(mDevice.Device, size.Width, size.Height,
				Format.A8R8G8B8, Format.D16);

			Viewport v = new Viewport(0, 0, size.Width, size.Height);

			render.BeginScene(mTexture.Value.GetSurfaceLevel(0), v);
			mDevice.Clear(ClearFlags.Target, Color.FromArgb(0, 0, 0, 0).ToArgb(), 1.0f, 0);
			render.EndScene(Filter.None);

			render.Dispose();
			render = null;

			mTextureSize = mSrcRect.Size;

			InitVerts();
		}
		public SDX_Surface(Ref<Texture> texture, Rectangle sourceRect)
		{
			mSrcRect = sourceRect;

			mDisplay = Display.Impl as SDX_Display;
			mDevice = mDisplay.D3D_Device;

			mTexture = new Ref<Texture>(texture);

			mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
				mTexture.Value.GetSurfaceLevel(0).Description.Height);

			InitVerts();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mTexture.IsDisposed == false)
				{
					mTexture.Dispose();
				}
			}
		}


		private void InitVerts()
		{
			SetVertsTextureCoordinates(mVerts, 0, mSrcRect);
			SetVertsColor(new Gradient(Color.White), mVerts, 0, 4);
		}
		public void LoadFromStream(Stream st)
		{
			Drawing.Bitmap bitmap = new Drawing.Bitmap(st);

			mSrcRect = new Rectangle(Point.Empty, Interop.Convert(bitmap.Size));
			
			// this is the speed issue fix in the debugger found on the net (thezbuffer.com has it documented)
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

			bitmap.Dispose();

			stream.Position = 0;

			//mTexture = Texture(mDevice, bitmap, Usage.None, Pool.Managed);

			Format format;


			switch (mDisplay.DisplayMode.Format)
			{
				case Format.X8R8G8B8:
					format = Format.A8R8G8B8;
					break;

				case Format.X8B8G8R8:
					format = Format.A8B8G8R8;
					break;

				default:
					System.Diagnostics.Debug.Assert(false);
					throw new Exception("What format do I use?");

			}

			mTexture = new Ref<Texture>(Texture.FromStream(mDevice.Device,
				stream, 0, 0, 1, Usage.None,
				format, Pool.Managed, Filter.None, Filter.None, 0x00000000));
			
			mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
				mTexture.Value.GetSurfaceLevel(0).Description.Height);

			stream.Dispose();
		}
		public void LoadFromFile()
		{
			if (string.IsNullOrEmpty(mFileName))
				return;

			string path = mFileName;
			Drawing.Bitmap bitmap = new Drawing.Bitmap(path);

			mSrcRect = new Rectangle(Point.Empty, Interop.Convert(bitmap.Size));
			/*
			// this is the speed issue fix in the debugger found on the net (thezbuffer.com has it documented)
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

			stream.Position = 0;
            
			mTexture = Texture.FromStream(mDevice, stream, Usage.None, Pool.Managed);
			 * */
			//mTexture = new Texture(mDevice, bitmap, Usage.None, Pool.Managed);
			//Format format;

			throw new NotImplementedException();

			//switch (mDevice.Device.DisplayMode.Format)
			//{
			//    case Format.X8R8G8B8:
			//        format = Format.A8R8G8B8;
			//        break;

			//    case Format.X8B8G8R8:
			//        format = Format.A8B8G8R8;
			//        break;

			//    default:
			//        System.Diagnostics.Debug.Assert(false);
			//        throw new Exception("What format do I use?");

			//}

			//mTexture = new Ref<Texture>(TextureLoader.FromFile(mDevice.Device, path, 0, 0, 1, Usage.None,
			//     format, Pool.Managed, Filter.None, Filter.None, 0x00000000));

			//mTextureSize = new Size(mTexture.Value.GetSurfaceLevel(0).Description.Width,
			//    mTexture.Value.GetSurfaceLevel(0).Description.Height);

			//bitmap.Dispose();
		}

		#endregion
		#region --- Events and event handlers ---

		public void mDevice_DeviceReset(object sender, EventArgs e)
		{
			LoadFromFile();
		}

		#endregion

		#region --- Drawing to screen functions ---

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
			bool alphaBlend = true;
			float mRotationCos = (float)Math.Cos(state.RotationAngle);
			float mRotationSin = (float)Math.Sin(state.RotationAngle);

			srcRect.X += mSrcRect.Left;
			srcRect.Y += mSrcRect.Top;

			if (displaySize.Width < 0)
			{
				destX -= displaySize.Width;
				rotationCenter.X += displaySize.Width;
			}

			if (displaySize.Height < 0)
			{
				destY -= displaySize.Height;
				rotationCenter.Y += displaySize.Height;
			}

			mDevice.Interpolation = InterpolationHint;

			SetVertsTextureCoordinates(mVerts, 0, srcRect);
			SetVertsColor(state.ColorGradient, mVerts, 0, 4);
			SetVertsPosition(mVerts, 0,
				new RectangleF(destX, destY,
							   srcRect.Width * (float)state.ScaleWidth,
							   srcRect.Height * (float)state.ScaleHeight),
							   rotationCenter.X, rotationCenter.Y,
							   state.DisplayAlignment, mRotationCos, mRotationSin);

			mDevice.DrawBuffer.CacheDrawIndexedTriangles(mVerts, mIndices, mTexture.Value, alphaBlend);
		}

		private void SetVertsTextureCoordinates(PositionTextureColor[] verts, int startIndex,
			Rectangle srcRect)
		{
			TextureCoordinates texCoords = GetTextureCoordinates(srcRect);

			SetVertsTextureCoordinates(verts, startIndex, texCoords);
		}

		private void SetVertsTextureCoordinates(PositionTextureColor[] verts, int startIndex,
			TextureCoordinates texCoords)
		{
			verts[startIndex].TexCoord = new Vector2(texCoords.Left, texCoords.Top);
			verts[startIndex + 1].TexCoord = new Vector2(texCoords.Right, texCoords.Top);
			verts[startIndex + 2].TexCoord = new Vector2(texCoords.Left, texCoords.Bottom);
			verts[startIndex + 3].TexCoord = new Vector2(texCoords.Right, texCoords.Bottom);
		}

		private TextureCoordinates GetTextureCoordinates(Rectangle srcRect)
		{
			return GetTextureCoordinates(new RectangleF(
				srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height));
		}
		private TextureCoordinates GetTextureCoordinates(RectangleF srcRect)
		{
			// if you change these, besure to uncomment the divisions below.
			const float leftBias = 0.0f;
			const float topBias = 0.0f;
			const float rightBias = 0.0f;
			const float bottomBias = 0.0f;
			/*
			leftBias /= DisplayWidth;
			rightBias /= DisplayWidth;
			topBias /= DisplayHeight;
			bottomBias /= DisplayHeight;
			*/
			float uLeft = srcRect.Left / (float)mTextureSize.Width + leftBias;
			float vTop = srcRect.Top / (float)mTextureSize.Height + topBias;
			float uRight = srcRect.Right / (float)mTextureSize.Width + rightBias;
			float vBottom = srcRect.Bottom / (float)mTextureSize.Height + bottomBias;

			TextureCoordinates texCoords = new TextureCoordinates(uLeft, vTop, uRight, vBottom);
			return texCoords;
		}

		private void SetVertsColor(Gradient ColorGradient, PositionTextureColor[] verts, int startIndex, int count)
		{
			verts[startIndex].Color = ColorGradient.TopLeft.ToArgb();
			verts[startIndex + 1].Color = ColorGradient.TopRight.ToArgb();
			verts[startIndex + 2].Color = ColorGradient.BottomLeft.ToArgb();
			verts[startIndex + 3].Color = ColorGradient.BottomRight.ToArgb();
		}
		private void SetVertsColor(Gradient ColorGradient, PositionTextureColor[] verts, int startIndex, int count,
			double x, double y, double width, double height)
		{
			verts[startIndex].Color = ColorGradient.Interpolate(x, y).ToArgb();
			verts[startIndex + 1].Color = ColorGradient.Interpolate(x + width, y).ToArgb();
			verts[startIndex + 2].Color = ColorGradient.Interpolate(x, y + height).ToArgb();
			verts[startIndex + 3].Color = ColorGradient.Interpolate(x + width, y + height).ToArgb();
		}

		private void SetVertsPosition(PositionTextureColor[] verts, int index,
			RectangleF dest, float rotationCenterX, float rotationCenterY,
			OriginAlignment DisplayAlignment,
			float mRotationCos, float mRotationSin)
		{
			float destX = dest.X - 0.5f;
			float destY = dest.Y - 0.5f;
			float destWidth = dest.Width;
			float destHeight = dest.Height;

			mCenterPoint = Origin.CalcF(DisplayAlignment, dest.Size);

			destX += rotationCenterX - mCenterPoint.X;
			destY += rotationCenterY - mCenterPoint.Y;

			// Point at (0, 0) local coordinates
			verts[index].X = mRotationCos * (-rotationCenterX) +
						 mRotationSin * (-rotationCenterY) + destX;

			verts[index].Y = -mRotationSin * (-rotationCenterX) +
						  mRotationCos * (-rotationCenterY) + destY;

			// Point at (DisplayWidth, 0) local coordinates
			verts[index + 1].X = mRotationCos * (-rotationCenterX + destWidth) +
						 mRotationSin * (-rotationCenterY) + destX;

			verts[index + 1].Y = -mRotationSin * (-rotationCenterX + destWidth) +
						  mRotationCos * (-rotationCenterY) + destY;

			// Point at (0, DisplayHeight) local coordinates
			verts[index + 2].X = mRotationCos * (-rotationCenterX) +
						 mRotationSin * (-rotationCenterY + destHeight) + destX;

			verts[index + 2].Y = (-mRotationSin * (-rotationCenterX) +
						   mRotationCos * (-rotationCenterY + destHeight)) + destY;

			// Point at (DisplayWidth, DisplayHeight) local coordinates
			verts[index + 3].X = mRotationCos * (-rotationCenterX + destWidth) +
						 mRotationSin * (-rotationCenterY + destHeight) + destX;

			verts[index + 3].Y = -mRotationSin * (-rotationCenterX + destWidth) +
						  mRotationCos * (-rotationCenterY + destHeight) + destY;

		}


		#endregion

		#region --- Overriden public properties ---

		public override Size SurfaceSize
		{
			get { return mSrcRect.Size; }
		}
		#endregion

		#region --- Surface saving ---

		public override void SaveTo(string filename, ImageFileFormat format)
		{
			Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
			bool disposeSurfWhenDone = false;

			if (surf.Description.Pool == Pool.Default)
			{
				surf = CopyRenderTargetSurfaceToSysmem(surf);

				disposeSurfWhenDone = true;
			}

			//Direct3D.ImageFileFormat d3dformat = SlimDX.Direct3D9.ImageFileFormat.Png;

			//switch (format)
			//{
			//    case ImageFileFormat.Bmp: d3dformat = Direct3D.ImageFileFormat.Bmp; break;
			//    case ImageFileFormat.Png: d3dformat = Direct3D.ImageFileFormat.Png; break;
			//    case ImageFileFormat.Jpg: d3dformat = Direct3D.ImageFileFormat.Jpg; break;
			//    case ImageFileFormat.Tga: d3dformat = Direct3D.ImageFileFormat.Tga; break;
			//}

			DataRectangle rect = surf.LockRectangle(LockFlags.ReadOnly);

			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(surf.Description.Width, surf.Description.Height);
			var target = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
				System.Drawing.Imaging.ImageLockMode.WriteOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			byte[] buffer = new byte[rect.Data.Length];
			rect.Data.Read(buffer, 0, (int)rect.Data.Length);

			Marshal.Copy(buffer, 0, target.Scan0, buffer.Length);

			bmp.UnlockBits(target);
			surf.UnlockRectangle();

			System.Drawing.Imaging.ImageFormat bmpFormat = System.Drawing.Imaging.ImageFormat.Png;

			switch (format)
			{
				case ImageFileFormat.Bmp: bmpFormat = System.Drawing.Imaging.ImageFormat.Bmp; break;
				case ImageFileFormat.Jpg: bmpFormat = System.Drawing.Imaging.ImageFormat.Jpeg; break;
			}

			bmp.Save(filename, bmpFormat);

			if (disposeSurfWhenDone)
				surf.Dispose();
		}

		private Direct3D.Surface CopyRenderTargetSurfaceToSysmem(Direct3D.Surface surf)
		{
			Direct3D.Surface newSurf = Direct3D.Surface.CreateOffscreenPlain(
							mDevice.Device, surf.Description.Width, surf.Description.Height,
							surf.Description.Format, Pool.SystemMemory);

			mDevice.Device.GetRenderTargetData(surf, newSurf);

			return newSurf;
		}

		#endregion

		#region --- SubSurface stuff ---

		public override SurfaceImpl CarveSubSurface(Rectangle srcRect)
		{
			Rectangle newSrcRect = new Rectangle(
				mSrcRect.Left + srcRect.Left,
				mSrcRect.Top + srcRect.Top,
				srcRect.Width,
				srcRect.Height);

			return new SDX_Surface(mTexture, newSrcRect);
		}
		public override void SetSourceSurface(SurfaceImpl surf, Rectangle srcRect)
		{
			mTexture.Dispose();
			mTexture = new Ref<Texture>((surf as SDX_Surface).mTexture);

			mSrcRect = srcRect;

			Direct3D.Surface d3dsurf = mTexture.Value.GetSurfaceLevel(0);

			mTextureSize = new Size(d3dsurf.Description.Width, d3dsurf.Description.Height);

			SetVertsTextureCoordinates(mVerts, 0, mSrcRect);
		}

		#endregion


		public override PixelBuffer ReadPixels(PixelFormat format, Rectangle rect)
		{
			Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
			bool disposeSurfWhenDone = false;

			if (surf.Description.Pool == Pool.Default)
			{
				surf = CopyRenderTargetSurfaceToSysmem(surf);
				disposeSurfWhenDone = true;
			}

			rect.X += mSrcRect.X;
			rect.Y += mSrcRect.Y;

			int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);

			PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

			if (format == PixelFormat.Any)
				format = pixelFormat;

			DataRectangle stm = surf.LockRectangle(
				new Drawing.Rectangle(0, 0, mTextureSize.Width, mTextureSize.Height),
				LockFlags.ReadOnly);

			byte[] array = new byte[SurfaceWidth * SurfaceHeight * pixelPitch];
			int length = SurfaceWidth * pixelPitch;
			int index = 0;

			unsafe
			{
				byte* ptr = (byte*)stm.Data.DataPointer;

				for (int i = rect.Top; i < rect.Bottom; i++)
				{
					// hack if the size requested is too large.
					if (i >= mTextureSize.Height)
						break;

					//IntPtr ptr = (IntPtr)((int)stm.InternalData + i * stride + rect.Left * pixelPitch);
					IntPtr mptr = (IntPtr)(ptr + i * stm.Pitch + rect.Left * pixelPitch);

					Marshal.Copy(mptr, array, index, length);

					index += length;
				}
			}

			surf.UnlockRectangle();

			if (disposeSurfWhenDone)
				surf.Dispose();

			return new PixelBuffer(format, rect.Size, array, pixelFormat);

		}

		public override void WritePixels(PixelBuffer buffer)
		{
			Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
			
			if (surf.Description.Pool == Pool.Default)
			{
				throw new AgateLib.AgateException(
					"Cannot write to FrameBuffer surface in Direct3D.");
			}

			int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);
			PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

			surf.Dispose();

			DataRectangle stm = mTexture.Value.LockRectangle(0, 0);

			if (buffer.PixelFormat != pixelFormat)
				buffer = buffer.ConvertTo(pixelFormat);

			unsafe
			{
				for (int i = 0; i < SurfaceHeight; i++)
				{
					int startIndex = buffer.GetPixelIndex(0, i);
					int rowStride = buffer.RowStride;
					IntPtr dest = (IntPtr)((byte*)stm.Data.DataPointer + i * stm.Pitch);

					Marshal.Copy(buffer.Data, startIndex, dest, rowStride);
				}
			}

			mTexture.Value.UnlockRectangle(0);

		}
		// TODO: Test this method:
		public override void WritePixels(PixelBuffer buffer, Point startPoint)
		{
			Direct3D.Surface surf = mTexture.Value.GetSurfaceLevel(0);
			Rectangle updateRect = new Rectangle(startPoint, buffer.Size);

			int pixelPitch = mDisplay.GetPixelPitch(surf.Description.Format);
			PixelFormat pixelFormat = mDisplay.GetPixelFormat(surf.Description.Format);

			surf.Dispose();

			// This should probably only lock the region of the surface we intend to update.
			// However, as is usually the case with DirectX, doing so gives weird errors
			// with no real explanation as to what is wrong.
			DataRectangle stm = mTexture.Value.LockRectangle
				(0, LockFlags.None);

			if (buffer.PixelFormat != pixelFormat)
				buffer = buffer.ConvertTo(pixelFormat);

			unsafe
			{
				for (int i = 0; i < buffer.Height; i++)
				{
					int startIndex = buffer.GetPixelIndex(0, i);
					int rowStride = buffer.RowStride;
					IntPtr dest = (IntPtr)
						((byte*)stm.Data.DataPointer + (i + updateRect.Top) * stm.Pitch 
													 + updateRect.Left * pixelPitch);

					Marshal.Copy(buffer.Data, startIndex, dest, rowStride);
				}
			}

			mTexture.Value.UnlockRectangle(0);
		}


		internal static Size NextPowerOfTwo(Size size)
		{
			return new Size(NextPowerOfTwo(size.Width), NextPowerOfTwo(size.Height));
		}

		private static int NextPowerOfTwo(int value)
		{
			double log = Math.Log(value) / Math.Log(2);

			double dval = Math.Pow(2, Math.Ceiling(log));
			int retval = (int)dval;

			return retval;
		}
	}

}
