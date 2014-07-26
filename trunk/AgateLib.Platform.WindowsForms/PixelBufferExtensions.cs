using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms
{
	public static class PixelBufferExtensions
	{
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
