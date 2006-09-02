using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Defines the interface used for render targets.
    /// SurfaceImpl and DisplayWindowImpl implement this interface.
    /// </summary>
    public interface IRenderTargetImpl
    {
        /// <summary>
        /// Utility functions that can be called by the rendering system
        /// when rendering starts and ends.
        /// </summary>
        void BeginRender();
        /// <summary>
        /// Utility functions that can be called by the rendering system
        /// when rendering starts and ends.
        /// </summary>
        void EndRender(bool waitVSync);
        /// <summary>
        /// Gets the Size of the render target, in pixels.
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// Gets the Height of the render target, in pixels.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Gets the Width of the render target, in pixels.
        /// </summary>
        int Height { get; }

    }
}
