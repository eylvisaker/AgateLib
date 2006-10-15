using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    /// <summary>
    /// Enum which describes different pixel formats.
    /// Order of the characters in the constant name specifies the
    /// ordering of the bytes for the pixel data, from most to least significant.
    /// See remarks for more information.
    /// </summary>
    /// <remarks>
    /// Order of the characters in the constant name specifies the
    /// ordering of the bytes for the pixel data, from most to least significant.
    /// 
    /// For example, ARGB8888 indicates that the blue channel is the least significant,
    /// the alpha channel is most significant, and each channel is eight bits long.
    /// Bytes are stored in little endian order, so for ARGB8888, the blue channel for
    /// a pixel is stored in the lowest memory address, followed by green, red, and finally
    /// alpha.
    /// </remarks>
    public enum PixelFormat
    {
        /// <summary>
        /// Format specifying the Agate should choose what pixel format 
        /// to use, where appropriate.
        /// </summary>
        Any,

        #region --- 32 bit formats ---

        /// <summary>
        /// 
        /// </summary>
        ARGB8888,
        /// <summary>
        /// 
        /// </summary>
        ABGR8888,
        /// <summary>
        /// 
        /// </summary>
        BGRA8888,
        /// <summary>
        /// 
        /// </summary>
        RGBA8888,

        /// <summary>
        /// 
        /// </summary>
        XRGB8888,

        #endregion

        #region --- 24 bit formats ---

        /// <summary>
        /// 
        /// </summary>
        RGB888,
        /// <summary>
        /// 
        /// </summary>
        BGR888,

        #endregion

        #region --- 16 bit formats ---

        /// <summary>
        /// 
        /// </summary>
        RGB565,
        /// <summary>
        /// 
        /// </summary>
        XRGB1555,
        /// <summary>
        /// 
        /// </summary>
        XBGR1555,
        /// <summary>
        /// 
        /// </summary>
        BGR565,

        #endregion

    }
    /// <summary>
    /// Class which encapsulates raw pixel data.  This can be used to 
    /// construct or modify surface data programmatically.
    /// </summary>
    [Serializable]
    public class PixelBuffer
    {
        #region --- Private Data ---

        PixelFormat mFormat;
        byte[] mData;
        Size mSize;

        #endregion

        #region --- Constructors ---

        /// <summary>
        /// static constructor to test pixel formats.
        /// </summary>
        static PixelBuffer()
        {
            foreach (PixelFormat format in Enum.GetValues(typeof(PixelFormat)))
            {
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
            : this(format, size, new byte[size.Width * size.Height])
        { }
        /// <summary>
        /// Constructs a PixelBuffer object. 
        /// </summary>
        /// <param name="size">The size of the image data in pixels.</param>
        /// <param name="format">The raw data format of the pixels to be contained
        /// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
        /// <param name="data">Raw pixel data.  It must be the correct size
        /// for the format passed.</param>
        public PixelBuffer(PixelFormat format, Size size, byte[] data)
        {
            if (format == PixelFormat.Any)
                throw new Exception("A specific pixel format and must be specified."
                    + "PixelFormat.Any is not valid.");

            mFormat = format;
            mSize = size;

            Data = data;
        }
        /// <summary>
        /// Constructs a PixelBuffer object. 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="width">The width of the image data in pixels.</param>
        /// <param name="height">The height of the image data in pixels.</param>
        public PixelBuffer(PixelFormat format, int width, int height)
            : this(format, new Size(width, height))
        { }
        /// <summary>
        /// Constructs a PixelBuffer object. 
        /// </summary>
        /// <param name="width">The width of the image data in pixels.</param>
        /// <param name="height">The height of the image data in pixels.</param>
        /// <param name="format">The raw data format of the pixels to be contained
        /// in the pixel buffer.  PixelFormat.Any is not a valid parameter.</param>
        /// <param name="data">Raw pixel data.  It must be the correct size
        /// for the format passed.</param>
        public PixelBuffer(PixelFormat format, int width, int height, byte[] data)
            : this(format, new Size(width, height), data)
        { }

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
        public byte[] Data
        {
            get { return mData; }
            set
            {
                int correctLen = Width * Height * PixelStride;

                if (correctLen != value.Length)
                    throw new ArgumentException("PixelBuffer Data must be set with the correct length."
                        + "Length expected: " + correctLen.ToString()
                        + "Length of data passed: " + value.Length);

                mData = value;
            }
        }

        /// <summary>
        /// Gets how many bytes each pixel takes up in memory.
        /// </summary>
        public int PixelStride
        {
            get { return GetPixelStride(mFormat); }
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
            get { return Width * PixelStride; }
        }

        #endregion

        #region --- Public Methods ---

        /// <summary>
        /// Copies the data from the array passed in into the internal pixel 
        /// buffer array. Automatic conversion is performed if the format the data 
        /// is in (indicated by format parameter) differs from the format the
        /// pixel buffer is in.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="srcFormat"></param>
        public void SetData(byte[] data, PixelFormat srcFormat)
        {
            int sourceStride = GetPixelStride(srcFormat);
            int sourceIndex = 0;
            int destIndex = 0;

            if (Width * Height *  sourceStride != data.Length)
                throw new ArgumentException("Source data does not have the right amount of data" +
                    " for the format specified.");

            for (int i = 0; i < Width * Height; i++)
            {
                ConvertPixel(mData, destIndex, this.PixelFormat, data, sourceIndex, srcFormat);

                sourceIndex += sourceStride;
                destIndex += this.PixelStride;
            }
        }

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
            throw new Exception(" Method not implemented.");
        }

        /// <summary>
        /// Returns the number of bytes in memory used by a single pixel in the
        /// specified format.
        /// </summary>
        /// <param name="format">Which format to look up.</param>
        /// <returns>The number of bytes used by the format.  This is always
        /// either 2 for 15 or 16 bit formats, 3 for 24 bit formats, and 4 for
        /// 32 bit formats.</returns>
        public static int GetPixelStride(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.ABGR8888:
                case PixelFormat.ARGB8888:
                case PixelFormat.BGRA8888:
                case PixelFormat.RGBA8888:
                case PixelFormat.XRGB8888:
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
                    throw new Exception("Unrecognized pixel format.");
            }

        }
        #endregion

    }
}