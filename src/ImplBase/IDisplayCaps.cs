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
        /// Indicates whether or not full screen windows can be created.
        /// </summary>
        bool SupportsFullScreen { get; }
        /// <summary>
        /// Indicates whether or not the screen resolution can be changed.
        /// If the Display driver supports full screen but not mode switching,
        /// then a DisplayWindow which is created with as a full screen window
        /// cannot change resolutions after it is initially set.
        /// </summary>
        bool SupportsFullScreenModeSwitching { get; }
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
        /// Indicates whether Surface gradients are supported.  If not, then setting Surface.ColorGradient
        /// color of a surface is the same as setting the Surface.Color with the average of the
        /// gradient colors.
        /// </summary>
        bool SupportsGradient { get; }
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

        /// <summary>
        /// Indicates whether the driver can create a bitmap font from an operatig
        /// system font.
        /// </summary>
        bool CanCreateBitmapFont { get; }
    }
}