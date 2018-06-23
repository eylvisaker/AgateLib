//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Layout
{
    /// <summary>
    /// Interface for performing layout of items.
    /// </summary>
    public interface IWidgetLayout : ICollection<IWidget>, IWidgetChildren
    {
        /// <summary>
        /// Event raised when a widget is added to the collection.
        /// </summary>
        event WidgetEventHandler WidgetAdded;

        /// <summary>
        /// Gets the collection of items in the layout.
        /// </summary>
        IEnumerable<IWidget> Items { get; }

        /// <summary>
        /// Processes an input event.
        /// </summary>
        /// <param name="input"></param>
        void InputEvent(WidgetEventArgs input);
        
        /// <summary>
        /// Computes the ideal size of the layout.
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="renderContext"></param>
        /// <returns></returns>
        Size ComputeIdealSize(Size maxSize, IWidgetRenderContext renderContext);

        /// <summary>
        /// Applies the layout for the specified size.
        /// </summary>
        /// <param name="size">The size of the layout in pixels. Should be close to
        /// the return value from ComputeIdealSize if possible.</param>
        void ApplyLayout(Size size, IWidgetRenderContext renderContext);
    }

    public delegate void WidgetEventHandler(IWidget widget);
}
