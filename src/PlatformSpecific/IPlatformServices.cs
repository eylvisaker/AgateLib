using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.PlatformSpecific
{
    /// <summary>
    /// Public interface which provides information and methods for a specific platform.
    /// </summary>
    public interface IPlatformServices
    {
        /// <summary>
        /// Returns the type of platform.
        /// </summary>
        PlatformType PlatformType { get; }
    }
}
