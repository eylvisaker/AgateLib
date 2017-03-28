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

using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	public static class PixelBufferExtensions
	{
		/// <summary>
		/// Converts an AgateLib.DisplayLib.PixelBuffer object into a System.Drawing.Bitmap object.
		/// </summary>
		/// <param name="buffer">The PixelBuffer object containing the pixel data.</param>
		/// <returns></returns>
		public static System.Drawing.Bitmap ToBitmap(this PixelBuffer buffer)
		{
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(buffer.Width, buffer.Height);

			System.Drawing.Imaging.BitmapData data = bmp.LockBits(
				new System.Drawing.Rectangle(System.Drawing.Point.Empty, buffer.Size.ToDrawing()),
				System.Drawing.Imaging.ImageLockMode.WriteOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			if (buffer.PixelFormat != PixelFormat.BGRA8888)
			{
				buffer = buffer.ConvertTo(PixelFormat.BGRA8888);
			}

			System.Runtime.InteropServices.Marshal.Copy(
				buffer.Data, 0, data.Scan0, buffer.Data.Length);

			bmp.UnlockBits(data);

			return bmp;
		}

		/// <summary>
		/// Copies the data from the unmanaged memory pointer passed in into the internal pixel 
		/// buffer array. Automatic conversion is performed if the format the data 
		/// is in (indicated by format parameter) differs from the format the
		/// pixel buffer is in.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="srcFormat"></param>
		/// <param name="srcRowStride"></param>
		public static void SetData(
			this PixelBuffer buffer, IntPtr data, 
			PixelFormat srcFormat, int srcRowStride)
		{
			int sourceStride = PixelBuffer.GetPixelStride(srcFormat);
			var mData = buffer.Data;

			unsafe
			{
				if (srcFormat == buffer.PixelFormat)
				{
					// optimized copy for same type of data
					int startIndex = 0;
					int rowLength = sourceStride * buffer.Width;

					IntPtr rowPtr = data;
					byte* dataPtr = (byte*)data;

					for (int y = 0; y < buffer.Height; y++)
					{
						Marshal.Copy(rowPtr, mData, startIndex, rowLength);

						startIndex += buffer.RowStride;
						dataPtr += srcRowStride;
						rowPtr = (IntPtr)dataPtr;
					}
				}
				else
				{
					byte[] srcPixel = new byte[srcRowStride];
					int destIndex = 0;
					IntPtr rowPtr = data;
					byte* dataPtr = (byte*)data;
					int width = buffer.Width;
					int destPixelStride = buffer.PixelStride;

					for (int y = 0; y < buffer.Height; y++)
					{
						Marshal.Copy(rowPtr, srcPixel, 0, srcRowStride);

						// check for common Win32 - OpenGL conversion case
						if ( buffer.PixelFormat == PixelFormat.RGBA8888 &&
							srcFormat == PixelFormat.BGRA8888)
						{
							// this setup here is OPTIMIZED.
							// Calling ConvertPixel for each pixel when loading a large
							// image adds about 35% more CPU time to do the conversion.
							// By eliminating the function call and processing this special
							// case here we save some time on image loading.
							int srcIndex = 0;

							for (int x = 0; x < width; x++)
							{
								mData[destIndex] = srcPixel[srcIndex + 2];
								mData[destIndex + 1] = srcPixel[srcIndex + 1];
								mData[destIndex + 2] = srcPixel[srcIndex];
								mData[destIndex + 3] = srcPixel[srcIndex + 3];

								destIndex += destPixelStride;
								srcIndex += sourceStride;
							}
						}
						else
						{
							for (int x = 0; x < width; x++)
							{
								PixelBuffer.ConvertPixel(mData, destIndex, buffer.PixelFormat, srcPixel, x * sourceStride, srcFormat);

								destIndex += destPixelStride;
							}
						}

						dataPtr += srcRowStride;
						rowPtr = (IntPtr)dataPtr;
					}
				}
			}
		}

	}
}
