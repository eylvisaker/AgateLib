//     ``The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
