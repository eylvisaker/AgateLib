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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.WinForms;
using Geometry = AgateLib.Geometry;
using PixelFormat = AgateLib.DisplayLib.PixelFormat;
using InterpolationMode = AgateLib.DisplayLib.InterpolationMode;

namespace AgateDrawing
{
	class Drawing_Surface : SurfaceImpl
	{
		#region --- Private variables ---

		Drawing_Display mDisplay;
		Bitmap mImage;

		#endregion

		#region --- Creation / Destruction ---

		public Drawing_Surface(string fileName)
		{
			mDisplay = Display.Impl as Drawing_Display;

			mImage = (Bitmap)Image.FromFile(fileName);
			ConvertImage();

			System.Diagnostics.Debug.Assert(mImage != null);
		}
		public Drawing_Surface(Stream st)
		{
			mDisplay = Display.Impl as Drawing_Display;

			mImage = (Bitmap)Image.FromStream(st);
			ConvertImage();

			System.Diagnostics.Debug.Assert(mImage != null);
		}
		/// <summary>
		/// Creates a Drawing_Surface object which wraps the specified image.
		/// </summary>
		/// <param name="image"></param>
		public Drawing_Surface(Bitmap image)
		{
			mDisplay = Display.Impl as Drawing_Display;

			mImage = image;

			System.Diagnostics.Debug.Assert(mImage != null);
		}
		/// <summary>
		/// Creates a Drawing_Surface object and copies pixels from the specified image.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="sourceRect"></param>
		public Drawing_Surface(Bitmap image, Rectangle sourceRect)
		{
			mDisplay = Display.Impl as Drawing_Display;

			// copy the pixels from the srcRect
			mImage = new Bitmap(sourceRect.Width, sourceRect.Height);

			Graphics g = Graphics.FromImage(mImage);

			g.DrawImage(image,
				new Rectangle(0, 0, sourceRect.Width, sourceRect.Height),
				(Rectangle)sourceRect, GraphicsUnit.Pixel);

			g.Dispose();
			

			System.Diagnostics.Debug.Assert(mImage != null);
		}
		public Drawing_Surface(Geometry.Size sz)
		{
			mDisplay = Display.Impl as Drawing_Display;

			mImage = new Bitmap(sz.Width, sz.Height);

			System.Diagnostics.Debug.Assert(mImage != null);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mImage != null)
				{
					mImage.Dispose();
				}

				mImage = null;
			}
		}

		private void ConvertImage()
		{
			var newImage = new Bitmap(mImage.Width, mImage.Height);

			Graphics g = Graphics.FromImage(newImage);
			g.DrawImage(mImage, new Rectangle(0, 0, mImage.Width, mImage.Height));

			g.Dispose();
			mImage.Dispose();

			mImage = newImage;
		}

		#endregion
		#region --- Protected Drawing helper methods ---

		protected Geometry.Rectangle SrcRect
		{
			get
			{
				return new Geometry.Rectangle(Geometry.Point.Empty,
					Interop.Convert(mImage.Size));
			}
		}
		protected Geometry.Rectangle DestRect(int dest_x, int dest_y, Geometry.Rectangle srcRect, double ScaleWidth, double ScaleHeight)
		{
			return new Geometry.Rectangle(dest_x, dest_y,
				(int)(srcRect.Width * ScaleWidth),
				(int)(srcRect.Height * ScaleHeight));
		}

		#endregion
		#region --- Draw to Screen Methods ---

		public override void Draw(SurfaceState state)
		{
			for (int i = 0; i < state.DrawInstances.Count; i++)
			{
				Draw(state, state.DrawInstances[i]);
			}
		}

		private void Draw(SurfaceState s, SurfaceDrawInstance inst)
		{
			mDisplay.CheckInFrame("Surface.Draw");
			System.Diagnostics.Debug.Assert(mImage != null);

			Geometry.SizeF displaySize = s.GetDisplaySize(SurfaceSize);
			Geometry.PointF rotationCenter = s.GetRotationCenter(displaySize);

			Drawing_Display disp = Display.Impl as Drawing_Display;
			Graphics g = disp.FrameGraphics;
			GraphicsState state = g.Save();
			Geometry.PointF translatePoint = Origin.CalcF(s.DisplayAlignment, displaySize);

			if (displaySize.Width < 0)
			{
				translatePoint.X += displaySize.Width;
				rotationCenter.X += displaySize.Width;
			}
			if (displaySize.Height < 0)
			{
				translatePoint.Y += displaySize.Height;
				rotationCenter.Y += displaySize.Height;
			}

			if (s.RotationAngle != 0)
			{
				// translate to rotation point, rotate, and translate back.
				// System.Drawing rotates Clockwise!  So we must reverse the
				// rotation angle.
				g.TranslateTransform(-rotationCenter.X, -rotationCenter.Y, MatrixOrder.Append);
				g.RotateTransform(-(float)s.RotationAngleDegrees, MatrixOrder.Append);
				g.TranslateTransform(rotationCenter.X, rotationCenter.Y, MatrixOrder.Append);
			}

			g.TranslateTransform(inst.DestLocation.X - translatePoint.X,
								 inst.DestLocation.Y - translatePoint.Y, MatrixOrder.Append);

			SetInterpolation(g);

			Geometry.Rectangle srcRect = inst.GetSourceRect(SurfaceSize);

			if (s.Color != Geometry.Color.White)
			{
				ImageAttributes imageAttributes = new ImageAttributes();

				ColorMatrix colorMatrix = new ColorMatrix(new float[][]{
                   new float[] { s.Color.R / 255.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, s.Color.G / 255.0f, 0.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, 0.0f, s.Color.B / 255.0f, 0.0f, 0.0f },
                   new float[] { 0.0f, 0.0f, 0.0f, (float)s.Alpha, 0.0f },
                   new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f} });

				imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				g.DrawImage(mImage, Interop.Convert(DestRect(0, 0, srcRect, s.ScaleWidth, s.ScaleHeight)),
					srcRect.X,
					srcRect.Y,
					srcRect.Width,
					srcRect.Height,
					GraphicsUnit.Pixel,
					imageAttributes);

			}
			else
			{
				g.DrawImage(mImage, Interop.Convert(DestRect(0, 0, srcRect, s.ScaleWidth, s.ScaleHeight)),
					srcRect.X,
					srcRect.Y,
					srcRect.Width,
					srcRect.Height,
					GraphicsUnit.Pixel);
			}

			g.Restore(state);
		}

		private void SetInterpolation(Graphics g)
		{
			switch (InterpolationHint)
			{
				case InterpolationMode.Default:
				case InterpolationMode.Fastest:
					g.CompositingQuality = CompositingQuality.HighSpeed;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

					break;

				case InterpolationMode.Nicest:
					g.CompositingQuality = CompositingQuality.HighQuality;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					break;
			}
		}

		#endregion
		#region --- Public overriden properties ---

		public override Geometry.Size SurfaceSize
		{
			get { return Interop.Convert(mImage.Size); }
		}

		#endregion

		#region --- Surface Data Manipulations ---

		public override SurfaceImpl CarveSubSurface(Geometry.Rectangle srcRect)
		{
			return new Drawing_Surface(mImage, Interop.Convert(srcRect));
		}

		public override void SaveTo(string filename, ImageFileFormat format)
		{
			ImageFormat drawformat = ImageFormat.Png;

			switch (format)
			{
				case ImageFileFormat.Png:
					drawformat = ImageFormat.Png;
					break;

				case ImageFileFormat.Bmp:
					drawformat = ImageFormat.Bmp;
					break;

				case ImageFileFormat.Jpg:
					drawformat = ImageFormat.Jpeg;
					break;

			}

			mImage.Save(filename, drawformat);
		}
		public override void SetSourceSurface(SurfaceImpl surf, Geometry.Rectangle srcRect)
		{
			mImage.Dispose();

			mImage = new Bitmap(srcRect.Width, srcRect.Height);
			Graphics g = Graphics.FromImage(mImage);

			g.DrawImage((surf as Drawing_Surface).mImage,
				new Rectangle(Point.Empty, Interop.Convert(srcRect.Size)),
				Interop.Convert(srcRect), GraphicsUnit.Pixel);

			g.Dispose();

			System.Diagnostics.Debug.Assert(mImage != null);
		}

		public override PixelBuffer ReadPixels(PixelFormat format)
		{
			return ReadPixels(format, new Geometry.Rectangle(Geometry.Point.Empty, SurfaceSize));
		}
		public override PixelBuffer ReadPixels(PixelFormat format, Geometry.Rectangle rect)
		{
			BitmapData data = mImage.LockBits(Interop.Convert(rect), ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			if (format == PixelFormat.Any)
				format = PixelFormat.BGRA8888;

			PixelBuffer buffer = new PixelBuffer(format, rect.Size);
			byte[] bytes = new byte[4 * rect.Width * rect.Height];

			Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

			mImage.UnlockBits(data);

			buffer.SetData(bytes, PixelFormat.BGRA8888);

			return buffer;
		}
		public override void WritePixels(PixelBuffer buffer)
		{
			BitmapData data = mImage.LockBits(new Rectangle(Point.Empty, Interop.Convert(SurfaceSize)),
				ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			if (buffer.PixelFormat != PixelFormat.BGRA8888)
			{
				buffer = buffer.ConvertTo(PixelFormat.BGRA8888);
			}

			Marshal.Copy(buffer.Data, 0, data.Scan0, buffer.Data.Length);

			mImage.UnlockBits(data);
		}

		#endregion
	}
}