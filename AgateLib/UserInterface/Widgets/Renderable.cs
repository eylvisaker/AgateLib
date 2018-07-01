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

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderable
    {
        /// <summary>
        /// Renders the item. This can return either an IWidget or an IRenderElement object.
        /// </summary>
        /// <remarks>
        /// If this returns an IWidget, then the rendering engine will call Render on the object 
        /// until an IRenderElement is returned.
        /// If this item is an IRenderElement object, it should just return itself.
        ///</remarks>
        /// <returns></returns>
        IRenderable Render();
    }

    public static class RenderableExtension
    {
        /// <summary>
        /// Calls Render repeatedly on the return values of renderable.Render() until an IRenderElement is returned.
        /// </summary>
        /// <param name="renderable"></param>
        /// <returns></returns>
        public static IRenderElement Finalize(this IRenderable renderable)
        {
            IRenderable result = renderable;
            int count = 0;
            const int max = 200;

            do
            {
                if (result == null)
                    throw new ArgumentNullException("Renderable is null.");
                if (count > max)
                    throw new InvalidOperationException($"After {max} iterations, {renderable} failed to return a render element.");

                result = result.Render();

                count++;
            } while (!(result is IRenderElement));

            return (IRenderElement)result;
        }
    }
}
