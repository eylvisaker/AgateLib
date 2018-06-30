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
