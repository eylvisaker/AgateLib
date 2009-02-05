using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
    /// <summary>
    /// Enum which describes different pixel formats.
    /// Order of the characters in the constant name specifies the
    /// ordering of the bytes for the pixel data, from least to most significant.
    /// See remarks for more information.
    /// </summary>
    /// <remarks>
    /// Order of the characters in the constant name specifies the
    /// ordering of the bytes for the pixel data, from least to most significant on 
    /// a little-endian architecture.  In other words, the first character indicates
    /// the meaning of the first byte or bits in memory.
    /// 
    /// For example, ARGB8888 indicates that the alpha channel is the least significant,
    /// the blue channel is most significant, and each channel is eight bits long.
    /// The alpha channel is stored first in memory, followed by red, green and blue.
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

        /// <summary>
        /// 
        /// </summary>
        XBGR8888,

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
}
