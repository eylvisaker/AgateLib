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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which encapsulates raw pixel data.  This can be used to 
	/// construct or modify surface data programmatically.
	/// </summary>
	public sealed class PixelBuffer
	{
		#region --- Private Data ---

		PixelFormat mFormat;
		byte[] mData;
		Size mSize;
		int mRowStride;
		int mPixelStride;

		#endregion

		#region --- Constructors ---
		/// <summary>
		/// static constructor to test pixel formats.
		/// </summary>
		static void TestPixelFormatStrides()
		{
			foreach (PixelFormat format in Enum.GetValues(typeof(PixelFormat)))
			{
				if (format == PixelFormat.Any)
					continue;

				int test = GetPixelStride(format);
			}
		}

		/// <summary>
		/// Constructs a PixelBuffer object. 
		/// </summary>
		/// <param name="size">The size of the image data in pixels.</param>
		/// <param name="format">The raw data format of the pixels to be contained
		/// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
		public PixelBuffer(PixelFormat format, Size size)
			: this(format, size, new byte[size.Width * size.Height * GetPixelStride(format)])
		{ }
		/// <summary>
		/// Constructs a PixelBuffer object. 
		/// Data passed is not copied; it is referenced.
		/// </summary>
		/// <param name="size">The size of the image data in pixels.</param>
		/// <param name="format">The raw data format of the pixels to be contained
		/// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
		/// <param name="data">Raw pixel data.  It must be the correct size
		/// for the format passed.  This data will not be copied; it will be
		/// referenced.</param>
		public PixelBuffer(PixelFormat format, Size size, byte[] data)
			: this(format, size, data, false)
		{ }
		/// <summary>
		/// Constructs a PixelBuffer object. 
		/// This overload performs automatic conversion of the data
		/// passed to match the format specified for the pixel buffer.
		/// The data is always copied in memory, even if it is of the
		/// same type as the format parameter.
		/// </summary>
		/// <param name="size">The size of the image data in pixels.</param>
		/// <param name="format">The raw data format of the pixels to be contained
		/// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
		/// <param name="data">Raw pixel data.  It must be the correct size
		/// for the format passed.</param>
		/// <param name="dataFormat">Format of the raw pixel data.  This data will be 
		/// copied into the PixelBuffer.</param>
		public PixelBuffer(PixelFormat format, Size size, byte[] data, PixelFormat dataFormat)
			: this(format, size)
		{
			Condition.Requires<ArgumentException>(format != PixelFormat.Any,
				"A specific pixel format and must be specified. PixelFormat.Any is not valid.");

			mFormat = format;
			mSize = size;

			SetStrides();

			SetData(data, dataFormat);

		}

		/// <summary>
		/// Constructs a PixelBuffer object. 
		/// This overload allows you to specify whether or not the 
		/// data parameter should be copied.
		/// </summary>
		/// <param name="size">The size of the image data in pixels.</param>
		/// <param name="format">The raw data format of the pixels to be contained
		/// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
		/// <param name="data">Raw pixel data.  It must be the correct size
		/// for the format passed.</param>
		/// <param name="copyData">True if the data should be copied into the pixel buffer.</param>
		public PixelBuffer(PixelFormat format, Size size, byte[] data, bool copyData)
		{
			Condition.Requires<ArgumentException>(format != PixelFormat.Any,
				"A specific pixel format and must be specified. PixelFormat.Any is not valid.");

			TestPixelFormatStrides();

			mFormat = format;
			mSize = size;

			SetStrides();

			if (copyData == false)
				Data = data;
			else
			{
				byte[] newData = new byte[data.Length];
				data.CopyTo(newData, 0);

				Data = newData;
			}
		}

		/// <summary>
		/// Constructs a PixelBuffer object, taking image data from a preexisting pixel buffer.
		/// </summary>
		/// <param name="buffer">The PixelBuffer object to copy image data from.</param>
		/// <param name="srcRect">The source rectangle in the buffer to copy image data from.</param>
		public PixelBuffer(PixelBuffer buffer, Rectangle srcRect)
			: this(buffer.PixelFormat, srcRect.Size)
		{
			this.CopyFrom(buffer, srcRect, new Point(0, 0), false);
		}
		/// <summary>
		/// Returns a deep copy of the PixelBuffer object.
		/// </summary>
		/// <returns></returns>
		public PixelBuffer Clone()
		{
			return new PixelBuffer(this, new Rectangle(0, 0, Width, Height));
		}

		private void SetStrides()
		{
			mPixelStride = GetPixelStride(mFormat);
			mRowStride = mSize.Width * mPixelStride;
		}

		/// <summary>
		/// Loads image data from a file and returns a PixelBuffer.
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static PixelBuffer FromFile(string file)
		{
			using (Surface surf = new Surface(file))
			{
				return surf.ReadPixels();
			}
		}
		#endregion

		#region --- Public Properties ---

		/// <summary>
		/// Gets the format of the pixel data.
		/// </summary>
		public PixelFormat PixelFormat
		{
			get { return mFormat; }
		}

		/// <summary>
		/// Gets or sets the raw pixel data, in the format indicated by PixelFormat. 
		/// An exception is thrown when setting Data if the length of the array passed is 
		/// not Width * Height * PixelStride.
		/// </summary>
		/// <remarks>
		/// The data is not copied when set, it is only referenced, so changes to the array that
		/// is passed in will affect the data in the pixel buffer.  It is assumed that the data
		/// passed in is of the same format as the pixel buffer.  If you wish to copy data use
		/// the <see cref="SetData(byte[], PixelFormat)"/> method.
		/// </remarks>
		public byte[] Data
		{
			get { return mData; }
			set
			{
				int correctLen = Width * Height * PixelStride;

				if (correctLen != value.Length)
					throw new ArgumentException("PixelBuffer Data must be set with the correct length."
						+ "\r\nLength expected: " + correctLen.ToString()
						+ "\r\nLength of data passed: " + value.Length);

				mData = value;
			}
		}

		/// <summary>
		/// Gets how many bytes each pixel takes up in memory.
		/// </summary>
		public int PixelStride
		{
			get { return mPixelStride; }
		}

		/// <summary>
		/// Returns the height in pixels of the buffer.
		/// </summary>
		public int Height
		{
			get { return mSize.Height; }
		}
		/// <summary>
		/// Returns the width in pixels of the buffer.
		/// </summary>
		public int Width
		{
			get { return mSize.Width; }
		}
		/// <summary>
		/// Returns the size (width, height) in pixels of the buffer.
		/// </summary>
		public Size Size
		{
			get { return mSize; }
		}

		/// <summary>
		/// Returns how many bytes a single row takes up. This value can be 
		/// used to increase an index to go from one line of pixels to the next.
		/// </summary>
		public int RowStride
		{
			get { return mRowStride; }
		}

		/// <summary>
		/// Checks to see if this PixelBuffer contains only transparent pixels.
		/// Pixels with an alpha value of less than Display.AlphaThreshold are considered
		/// transparent.
		/// </summary>
		/// <returns></returns>
		public bool IsBlank()
		{
			return IsBlank(Display.AlphaThreshold);
		}
		/// <summary>
		/// Checks to see if this PixelBuffer contains only transparent pixels.
		/// This overload allows the alpha tolerance to be specified explicitly.
		/// </summary>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsBlank(double alphaTolerance)
		{
			for (int i = 0; i < Height; i++)
			{
				if (IsRowBlank(i, alphaTolerance) == false)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Checks to see if this region of the pixelbuffer only contains transparent pixels.
		/// Pixels with an alpha value of less than Display.AlphaThreshold are considered
		/// transparent.
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public bool IsRegionBlank(Rectangle rectangle)
		{
			return IsRegionBlank(rectangle, Display.AlphaThreshold);
		}
		/// <summary>
		/// Returns true if all pixels within the passed rectangle are below the passed alpha tolerance value.
		/// </summary>
		/// <param name="rectangle"></param>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsRegionBlank(Rectangle rectangle, double alphaTolerance)
		{
			var tol = alphaTolerance * 255.0;

			for (int y = 0; y < rectangle.Height; y++)
			{
				for (int x = 0; x < rectangle.Width; x++)
				{
					Point pt = new Point(x + rectangle.X, y + rectangle.Y);

					if (GetPixel(pt.X, pt.Y).A > tol)
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// Pixels with an alpha value of less than Display.AlphaThreshold are considered
		/// transparent.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public bool IsRowBlank(int row)
		{
			return IsRowBlank(row, Display.AlphaThreshold);
		}
		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// This overload allows the alpha tolerance to be specified explicitly.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsRowBlank(int row, double alphaTolerance)
		{
			return IsRowBlank(row, 0, Width, alphaTolerance);
		}
		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="left"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		public bool IsRowBlank(int row, int left, int width)
		{
			return IsRowBlank(row, 0, Width, Display.AlphaThreshold);
		}
		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// This overload allows the alpha tolerance to be specified explicitly.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="left"></param>
		/// <param name="width"></param>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsRowBlank(int row, int left, int width, double alphaTolerance)
		{
			if (FormatHasAlpha(PixelFormat) == false)
				return false;

			for (int i = 0; i < width; i++)
			{
				Color clr = GetPixel(i + left, row);

				if (clr.A / 255.0 > alphaTolerance)
					return false;
			}

			return true;
		}
		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// Pixels with an alpha value of less than Display.AlphaThreshold are considered
		/// transparent.
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		public bool IsColumnBlank(int col)
		{
			return IsColumnBlank(col, Display.AlphaThreshold);
		}
		/// <summary>
		/// Checks to see if the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// This overload allows the alpha tolerance to be specified explicitly.
		/// </summary>
		/// <param name="col"></param>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsColumnBlank(int col, double alphaTolerance)
		{
			return IsColumnBlank(col, 0, Height, alphaTolerance);
		}
		/// <summary>
		/// Checks to see if a portion of the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// </summary>
		/// <param name="col"></param>
		/// <param name="top"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public bool IsColumnBlank(int col, int top, int height)
		{
			return IsColumnBlank(col, top, height, Display.AlphaThreshold);
		}
		/// <summary>
		/// Checks to see if a portion of the selected row of this PixelBuffer contains only
		/// transparent pixels.
		/// </summary>
		/// <param name="col"></param>
		/// <param name="top"></param>
		/// <param name="height"></param>
		/// <param name="alphaTolerance"></param>
		/// <returns></returns>
		public bool IsColumnBlank(int col, int top, int height, double alphaTolerance)
		{
			if (FormatHasAlpha(PixelFormat) == false)
				return false;

			for (int i = 0; i < Height; i++)
			{
				Color clr = GetPixel(col, i);

				if (clr.A / 255.0 > alphaTolerance)
					return false;
			}

			return true;
		}
		#endregion
		#region --- Public Methods ---

		/// <summary>
		/// Copies pixel data from the specified PixelBuffer.
		/// </summary>
		/// <param name="buffer">The pixel buffer to copy from.</param>
		/// <param name="srcRect"></param>
		/// <param name="destPt"></param>
		/// <param name="clip">If true, the copied region will automatically
		/// be clipped.  If false, this method will throw an exception if the area
		/// being copied to is out of range.</param>
		/// <param name="skipTransparent">Pass true to avoid copying transparent pixels
		/// for pixel formats that contain an alpha channel. This will only avoid copying
		/// a pixel if it has an alpha of exactly 0. Passing true can reduce performance
		/// significantly.</param>
		public void CopyFrom(PixelBuffer buffer, Rectangle srcRect, Point destPt, bool clip, bool skipTransparent = false)
		{
			Require.ArgumentNotNull(buffer, nameof(buffer));
			Require.ArgumentInRange(srcRect.X >= 0 && srcRect.Y >= 0 && srcRect.Right <= buffer.Width && srcRect.Bottom <= buffer.Height, nameof(srcRect),
				"Source rectangle but be entirely contained within the pixel buffer.");
			Require.ArgumentInRange(destPt.X >= 0 && destPt.Y >= 0, nameof(destPt),
				"Destination point X and Y must be non-negative.");
			Require.True<ArgumentException>(clip || (srcRect.Width + destPt.X <= Width), 
				"Either clip must be true or the destination copy area must be entirely available in the destination pixel buffer.");
			Require.True<ArgumentException>(clip || (srcRect.Height + destPt.Y <= Height), "Either clip must be true or the destination copy area must be entirely available in the destination pixel buffer.");

			if (skipTransparent)
			{
				for (int y = 0; y < srcRect.Height; y++)
				{
					for (int x = 0; x < srcRect.Width; x++)
					{
						Color pixel = buffer.GetPixel(x + srcRect.X, y + srcRect.Y);

						if (pixel.A == 0)
							continue;

						SetPixel(x + destPt.X, y + destPt.Y, pixel);
					}
				}
			}
			else
			{
				if (buffer.RowStride == RowStride && buffer.PixelFormat == PixelFormat && destPt.X == 0)
				{
					int destIndex = GetPixelIndex(destPt.X, destPt.Y);
					int srcIndex = buffer.GetPixelIndex(srcRect.X, srcRect.Y);

					int size = buffer.RowStride * srcRect.Height;

					Array.Copy(buffer.Data, srcIndex, Data, destIndex, size);
				}
				for (int y = 0; y < srcRect.Height; y++)
				{
					if (buffer.PixelFormat == PixelFormat)
					{
						int destIndex = GetPixelIndex(destPt.X, y + destPt.Y);
						int srcIndex = buffer.GetPixelIndex(srcRect.X, y + srcRect.Y);

						Array.Copy(buffer.Data, srcIndex,
								   Data, destIndex, PixelStride * srcRect.Width);
					}
					else
					{
						for (int x = 0; x < srcRect.Width; x++)
						{
							Color pixel = buffer.GetPixel(x + srcRect.X, y + srcRect.Y);
							SetPixel(x + destPt.X, y + destPt.Y, pixel);
						}
					}
				}
			}
		}
		/// <summary>
		/// Gets the index of the first byte in the pixel in the Data array
		/// at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int GetPixelIndex(int x, int y)
		{
			if (IsPointValid(x, y) == false)
				throw new IndexOutOfRangeException(string.Format(
					"The point {0}{1},{2}{3} does not exist in the pixel buffer which has size {4},{5}",
					"{", x, y, "}", Width, Height));

			return y * RowStride + x * PixelStride;
		}

		/// <summary>
		/// Returns true if the specified point is within the pixel buffer.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsPointValid(int x, int y)
		{
			if (x < 0) return false;
			if (y < 0) return false;
			if (x >= Width) return false;
			if (y >= Height) return false;

			return true;
		}

		/// <summary>
		/// Returns true if the specified point is within the pixel buffer.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool IsPointValid(Point pt)
		{
			return IsPointValid(pt.X, pt.Y);
		}

		/// <summary>
		/// Copies pixel data from the specified location to a Color structure.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Color GetPixel(int x, int y)
		{
			int index = GetPixelIndex(x, y);

			switch (PixelFormat)
			{
				case PixelFormat.ARGB8888:
					return Color.FromArgb(Data[index], Data[index + 1], Data[index + 2], Data[index + 3]);
				case PixelFormat.ABGR8888:
					return Color.FromArgb(Data[index], Data[index + 3], Data[index + 2], Data[index + 1]);
				case PixelFormat.BGRA8888:
					return Color.FromArgb(Data[index + 3], Data[index + 2], Data[index + 1], Data[index]);
				case PixelFormat.RGBA8888:
					return Color.FromArgb(Data[index + 3], Data[index], Data[index + 1], Data[index + 2]);
				default:
					throw new NotSupportedException(string.Format("Pixel format {0} not supported by GetPixel.", PixelFormat));
			}
		}

		/// <summary>
		/// Sets the color at a particular pixel.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="clr"></param>
		public void SetPixel(int x, int y, Color clr)
		{
			int index = GetPixelIndex(x, y);

			if (this.PixelStride == 4)
			{
				int a = clr.A;
				int r = clr.R;
				int g = clr.G;
				int b = clr.B;

				switch (PixelFormat)
				{
					case PixelFormat.ARGB8888:
						SetARGB8(a, r, g, b, Data,
							index, index + 1, index + 2, index + 3);
						break;

					case PixelFormat.ABGR8888:
						SetARGB8(a, r, g, b, Data,
							index, index + 3, index + 2, index + 1);
						break;

					case PixelFormat.BGRA8888:
						SetARGB8(a, r, g, b, Data,
							index + 3, index + 2, index + 1, index);
						break;

					case PixelFormat.RGBA8888:
						SetARGB8(a, r, g, b, Data,
							index + 3, index, index + 1, index + 2);
						break;

					default:
						throw new NotSupportedException(string.Format("Pixel format {0} not supported by SetPixel.", PixelFormat));

				}
			}
			else
				throw new NotSupportedException(string.Format("Pixel format {0} not supported by SetPixel.", PixelFormat));


		}

		/// <summary>
		/// Copies the data from the array passed in into the internal pixel 
		/// buffer array. Automatic conversion is performed if the format the data 
		/// is in (indicated by format parameter) differs from the format the
		/// pixel buffer is in.
		/// </summary>
		/// <param name="srcData"></param>
		/// <param name="srcFormat"></param>
		public void SetData(byte[] srcData, PixelFormat srcFormat)
		{
			int sourceStride = GetPixelStride(srcFormat);
			int sourceIndex = 0;
			int destIndex = 0;

			Condition.Requires<ArgumentException>(Width * Height * sourceStride == srcData.Length,
				"Source data does not have the right amount of data for the format specified.");

			if (srcFormat == this.PixelFormat)
			{
				srcData.CopyTo(mData, 0);
				return;
			}

			for (int i = 0; i < Width * Height; i++)
			{
				ConvertPixel(mData, destIndex, this.PixelFormat, srcData, sourceIndex, srcFormat);

				sourceIndex += sourceStride;
				destIndex += this.PixelStride;
			}
		}


		/// <summary>
		/// Creates a new PixelBuffer and copies the data in this PixelBuffer,
		/// performing automatic conversion.
		/// </summary>
		/// <param name="pixelFormat">PixelFormat that the newly created PixelBuffer should have.</param>
		/// <returns></returns>
		public PixelBuffer ConvertTo(PixelFormat pixelFormat)
		{
			return new PixelBuffer(pixelFormat, Size, this.Data, this.PixelFormat);
		}

		/// <summary>
		/// Creates a new PixelBuffer of the specified size, with the
		/// data in this PixelBuffer copied to the upper left corner.
		/// </summary>
		/// <param name="pixelFormat">PixelFormat that the newly created PixelBuffer should have.</param>
		/// <param name="textureSize"></param>
		/// <returns></returns>
		public PixelBuffer ConvertTo(PixelFormat pixelFormat, Size textureSize)
		{
			return ConvertTo(pixelFormat, textureSize, Point.Zero);
		}

		/// <summary>
		/// Creates a new PixelBuffer of the specified size, with the
		/// data in this PixelBuffer copied so that the upper left corner
		/// is specified by point.
		/// </summary>
		/// <param name="pixelFormat">PixelFormat that the newly created PixelBuffer should have.</param>
		/// <param name="textureSize"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public PixelBuffer ConvertTo(PixelFormat pixelFormat, Size textureSize, Point point)
		{
			var result = new PixelBuffer(pixelFormat, textureSize);

			for (int y = 0; y < Height; y++)
			{
				int srcIndex = RowStride * y;
				int destIndex = result.RowStride * y + point.X;

				// same format copy, no conversion necessary.
				if (pixelFormat == PixelFormat)
				{

					//for (int x = 0; x < Width * PixelStride; x++)
					//{
					//    result.Data[destIndex + x] = Data[srcIndex + x];
					//}
					Array.Copy(Data, srcIndex,
							   result.Data, destIndex, Width * PixelStride);
				}
				else
				{
					// different formats must convert.
					for (int x = 0; x < Width; x++)
					{
						result.SetPixel(x, y, GetPixel(x, y));
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Saves the data in the PixelBuffer for to an image file.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		public void SaveTo(string filename, ImageFileFormat format)
		{
			Display.SavePixelBuffer(this, filename, format);
		}

		/// <summary>
		/// Replaces all instances of the specified color.
		/// </summary>
		/// <param name="searchColor">The color to replace.</param>
		/// <param name="newColor">The new color to overwrite searchColor.</param>
		public void ReplaceColor(Color searchColor, Color newColor)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (searchColor == GetPixel(x, y))
					{
						SetPixel(x, y, newColor);
					}
				}
			}
		}

		#endregion

		#region --- Public Static Methods ---

		/// <summary>
		/// Converts a single pixel in the specified format at the specified location 
		/// from the source array and writes it to the specified location in the 
		/// destination array.
		/// </summary>
		/// <param name="dest">Destination array to write to.</param>
		/// <param name="destIndex">Index in destination array to begin writing.</param>
		/// <param name="destFormat">Pixel format to use when writing to destination array.</param>
		/// <param name="src">Source array to read pixel data from</param>
		/// <param name="srcIndex">Index in source array where pixel data should be read from.</param>
		/// <param name="srcFormat">The format of the pixel data in the source array.</param>
		public static void ConvertPixel(byte[] dest, int destIndex, PixelFormat destFormat, byte[] src, int srcIndex, PixelFormat srcFormat)
		{
			// check for trivial case.
			// this is commented because it should be checked for above this method.
			//if (destFormat == srcFormat)
			//{
			//    for (int i = 0; i < GetPixelStride(destFormat); i++)
			//        dest[destIndex + i] = src[srcIndex + i];

			//    return;
			//}

			// check for common Win32 - OpenGL conversion case
			if (destFormat == PixelFormat.RGBA8888 &&
				srcFormat == PixelFormat.BGRA8888)
			{
				dest[destIndex] = src[srcIndex + 2];
				dest[destIndex + 1] = src[srcIndex + 1];
				dest[destIndex + 2] = src[srcIndex];
				dest[destIndex + 3] = src[srcIndex + 3];

				return;
			}

			// This approach is very slow partly because it uses floating point
			// arithmetic. It is especially slow if you have a profiler attached
			// to your process and are loading large images.
			double a, r, g, b;

			GetSourcePixelAttributes(src, srcIndex, srcFormat, out a, out r, out g, out b);

			SetDestPixelAttributes(dest, destIndex, destFormat, a, r, g, b);
		}

		/// <summary>
		/// Returns the number of bytes in memory used by a single pixel in the
		/// specified format.
		/// </summary>
		/// <param name="format">Which format to look up.</param>
		/// <returns>
		/// The number of bytes used by the format.  This is always
		/// either 2 for 15 or 16 bit formats, 3 for 24 bit formats, and 4 for
		/// 32 bit formats.
		/// </returns>
		public static int GetPixelStride(PixelFormat format)
		{
			switch (format)
			{
				case PixelFormat.ABGR8888:
				case PixelFormat.ARGB8888:
				case PixelFormat.BGRA8888:
				case PixelFormat.RGBA8888:
				case PixelFormat.XRGB8888:
				case PixelFormat.XBGR8888:
					return 4;

				case PixelFormat.RGB888:
				case PixelFormat.BGR888:
					return 3;

				case PixelFormat.BGR565:
				case PixelFormat.XBGR1555:
				case PixelFormat.XRGB1555:
				case PixelFormat.RGB565:
					return 2;

				default:
					throw new ArgumentException("Unrecognized pixel format.");
			}

		}

		/// <summary>
		/// Returns true if the specified PixelFormat contains an
		/// alpha channel.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public static bool FormatHasAlpha(PixelFormat format)
		{
			switch (format)
			{
				case PixelFormat.ABGR8888:
				case PixelFormat.ARGB8888:
				case PixelFormat.BGRA8888:
				case PixelFormat.RGBA8888:
					return true;

				case PixelFormat.XRGB8888:
				case PixelFormat.XBGR8888:
					return false;

				case PixelFormat.RGB888:
				case PixelFormat.BGR888:
					return false;

				case PixelFormat.BGR565:
				case PixelFormat.XBGR1555:
				case PixelFormat.XRGB1555:
				case PixelFormat.RGB565:
					return false;

				default:
					throw new ArgumentOutOfRangeException("FormatHasAlpha",
						"FormatHasAlpha parameter format unrecognized.");

			}
		}

		#endregion

		#region --- Private pixel conversion helpers ---

		private static void SetDestPixelAttributes(byte[] dest, int destIndex, PixelFormat destFormat,
			double A, double R, double G, double B)
		{
			switch (destFormat)
			{
				case PixelFormat.ARGB8888:
					SetARGB8(A, R, G, B, dest, destIndex, destIndex + 1, destIndex + 2, destIndex + 3);
					break;
				case PixelFormat.ABGR8888:
					SetARGB8(A, R, G, B, dest, destIndex, destIndex + 3, destIndex + 2, destIndex + 1);
					break;

				case PixelFormat.RGBA8888:
					SetARGB8(A, R, G, B, dest, destIndex + 3, destIndex, destIndex + 1, destIndex + 2);
					break;
				case PixelFormat.BGRA8888:
					SetARGB8(A, R, G, B, dest, destIndex + 3, destIndex + 2, destIndex + 1, destIndex);
					break;

				default:
					throw new NotSupportedException(string.Format("Conversions to PixelFormat {0} not currently supported.",
						destFormat));
			}
		}


		private static void GetSourcePixelAttributes(byte[] src, int srcIndex, PixelFormat srcFormat,
			out double a, out double r, out double g, out double b)
		{
			switch (srcFormat)
			{
				case PixelFormat.ARGB8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex, srcIndex + 1, srcIndex + 2, srcIndex + 3);
					break;
				case PixelFormat.ABGR8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex, srcIndex + 3, srcIndex + 2, srcIndex + 1);
					break;

				case PixelFormat.RGBA8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex + 3, srcIndex, srcIndex + 1, srcIndex + 2);
					break;

				case PixelFormat.BGRA8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex + 3, srcIndex + 2, srcIndex + 1, srcIndex);
					break;

				case PixelFormat.XRGB8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex, srcIndex + 1, srcIndex + 2, srcIndex + 3);
					a = 1.0;
					break;

				case PixelFormat.XBGR8888:
					GetARGB8(out a, out r, out g, out b, src, srcIndex, srcIndex + 3, srcIndex + 2, srcIndex + 1);
					a = 1.0;
					break;


				default:
					throw new NotSupportedException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
						"Conversions from PixelFormat {0} not currently supported.",
						srcFormat));
			}

		}

		private static void SetARGB8(int a, int r, int g, int b,
			byte[] dest, int aindex, int rindex, int gindex, int bindex)
		{
			dest[aindex] = (byte)a;
			dest[rindex] = (byte)r;
			dest[gindex] = (byte)g;
			dest[bindex] = (byte)b;
		}

		private static void SetARGB8(double a, double r, double g, double b,
			byte[] dest, int aindex, int rindex, int gindex, int bindex)
		{
			dest[aindex] = (byte)(a * 255.0 + 0.5);
			dest[rindex] = (byte)(r * 255.0 + 0.5);
			dest[gindex] = (byte)(g * 255.0 + 0.5);
			dest[bindex] = (byte)(b * 255.0 + 0.5);
		}

		private static void GetARGB8(out double a, out double r, out double g, out double b,
			byte[] src, int aindex, int rindex, int gindex, int bindex)
		{
			a = src[aindex] / 255.0;
			r = src[rindex] / 255.0;
			g = src[gindex] / 255.0;
			b = src[bindex] / 255.0;
		}

		#endregion

		/// <summary>
		/// Sets every pixel in the PixelBuffer to the specified color.
		/// </summary>
		/// <param name="color"></param>
		public void Clear(Color color)
		{
			FillRectangle(color, new Rectangle(Point.Zero, Size));
		}

		/// <summary>
		/// Fills an elipse within the specified rectangle.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void FillEllipse(Color color, int x, int y, int width, int height)
		{
			double centerx = x + width * 0.5;
			double centery = y + width * 0.5;

			double _height = height * 0.5;
			double _width = width * 0.5;

			for (int j = y; j < y + height; j++)
			{
				for (int i = x; i < x + width; i++)
				{
					if (Math.Pow(i - centerx, 2) * _height * _height + Math.Pow(j - centery, 2) * _width * _width
						<= _height * _height * _width * _width)
					{
						SetPixel(i, j, color);
					}
				}
			}
		}
		/// <summary>
		/// Fills the specified rectangle with the passed color.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="inside"></param>
		public void FillRectangle(Color color, Rectangle inside)
		{
			for (int j = inside.Y; j < inside.Bottom; j++)
			{
				for (int i = inside.X; i < inside.Right; i++)
				{
					SetPixel(i, j, color);
				}
			}
		}

		/// <summary>
		/// Compares the pixels within the two pixel buffers to see if they represent
		/// equivalent colors.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool PixelsEqual(PixelBuffer a, PixelBuffer b)
		{
			if (object.ReferenceEquals(a, b))
				return true;

			if (a.Width != b.Width)
				return false;
			if (a.Height != b.Height)
				return false;

			if (a.PixelFormat == b.PixelFormat)
			{
				if (a.Data.SequenceEqual(b.Data))
					return true;
				else
					return false;
			}

			for (int y = 0; y < a.Height; y++)
			{
				for (int x = 0; x < a.Width; x++)
				{
					if (a.GetPixel(x, y) != b.GetPixel(x, y))
						return false;
				}
			}

			return true;
		}
	}
}
