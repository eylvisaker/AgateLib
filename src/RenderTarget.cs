using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Defines the interface used for render targets.
    /// Implemented by DisplayWindow and Surface
    /// </summary>
    public interface IRenderTarget
    {
        /// <summary>
        /// Gets the library implementation of the render target.
        /// </summary>
        IRenderTargetImpl Impl { get; }

        /// <summary>
        /// Gets the width of the render target in pixels.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Gets the height of the render target in pixels.
        /// </summary>
        int Height { get; }
        /// <summary>
        /// Gets the size of the render target in pixels.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Event that is fired when the render target is resized.
        /// </summary>
        event EventHandler Resize;
    }
}
