using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderable
    {
        /// <summary>
        /// Renders the item. If this item is already an IRenderElement object,
        /// it should just return itself.
        /// </summary>
        /// <returns></returns>
        IRenderElement Render();
    }
}
