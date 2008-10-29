using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.PlatformSpecific
{
    /// <summary>
    /// Public interface which provides information and methods for a specific platform.
    /// </summary>
    public interface IPlatformServices
    {
        /// <summary>
        /// Returns the type of platform.
        /// </summary>
        Core.PlatformType PlatformType { get; }
    }
}
