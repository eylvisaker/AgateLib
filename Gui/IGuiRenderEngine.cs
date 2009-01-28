using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
    public interface IGuiThemeEngine
    {
        /// <summary>
        /// Draws the specified widget to the screen.
        /// </summary>
        /// <param name="widget"></param>
        void DrawWidget(Widget widget);

        /// <summary>
        /// Calculates and returns the minimum size for the widget.
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        Size CalcMinSize(Widget widget);

        /// <summary>
        /// Returns the area for the client space in 
        /// the widget, given its size.
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        Rectangle GetClientArea(Container widget);
        /// <summary>
        /// Returns the size the container widget should be to 
        /// have the specified client area.
        /// </summary>
        /// <param name="widget"></param>
        /// <param name="clientSize"></param>
        /// <returns></returns>
        Size RequestClientAreaSize(Container widget, Size clientSize);
    }
}
