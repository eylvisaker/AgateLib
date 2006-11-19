using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Interface which allows querying of Display capabilities.
    /// </summary>
    public interface IDisplayCaps
    {
        /// <summary>
        /// - Indicates whether setting Surface.SetScale has any visible effect.
        /// </summary>
        bool SupportsScaling { get; }
        /// <summary>
        ///  Indicates whether setting Surface.RotationAngle has any visible effect.
        /// </summary>
        bool SupportsRotation { get; }
        /// <summary>
        /// Indicates whether setting Surface.Color has any visible effect.
        /// </summary>
        bool SupportsColor { get; }
        /// <summary>
        /// Indicates whether setting Surface.Alpha has any visible effect.
        /// </summary>
        bool SupportsSurfaceAlpha { get; }
        /// <summary>
        /// Indicates whether the alpha channel in surface pixels is used.
        /// </summary>
        bool SupportsPixelAlpha { get; }
        /// <summary>
        /// Indicates whether or not lighting is supported.
        /// </summary>
        bool SupportsLighting { get; }
        /// <summary>
        /// Indicates the maximum number of lights which can be used.
        /// </summary>
        int MaxLights { get; }
        /// <summary>
        /// Indicates whether there is hardware acceleration available for 2D and 3D drawing.
        /// </summary>
        bool IsHardwareAccelerated { get; }
        /// <summary>
        /// Indicates whether or not 3D drawing is supported.
        /// </summary>
        bool Supports3D { get; }
    }
}