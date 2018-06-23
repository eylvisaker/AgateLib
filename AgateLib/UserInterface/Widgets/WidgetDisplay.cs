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
using AgateLib.Display;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// An object which contains all information that the rendering system
    /// needs to render a widget.
    /// </summary>
    public class WidgetDisplay
    {
        public WidgetDisplay()
        {
            Region = new WidgetRegion(Styles.ActiveStyle);

            Styles.ActiveStyleChanged += (sender, e) =>
            {
                StyleUpdated();
            };

        }

        /// <summary>
        /// Owned by the rendering engine.
        /// </summary>
        public WidgetAnimationState Animation { get; } = new WidgetAnimationState();

        /// <summary>
        /// Gets the layout state.
        /// </summary>
        public WidgetRegion Region { get; }

        /// <summary>
        /// Gets the active widget style.
        /// </summary>
        public WidgetStyle Style => Styles.ActiveStyle;

        /// <summary>
        /// Gets the collection of styles that can be swapped out based on the widget's state.
        /// </summary>
        public WidgetStyleCollection Styles { get; } = new WidgetStyleCollection();

        /// <summary>
        /// Gets the font for this widget.
        /// </summary>
        public Font Font => Style.Font;

        /// <summary>
        /// Gets or sets whether the widget should be displayed and participate in layout.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        public IInstructions Instructions { get; set; }

        /// <summary>
        /// Order of this window for drawing. Higher values mean the window is drawn 
        /// in front of windows with lower values.
        /// </summary>
        public int StackOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the theme the styling engine should 
        /// use to style the owning widget.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Set by parent widgets to indicate whether the owning widget has focus.
        /// </summary>
        public bool HasFocus { get; set; }
        public IndicatorType IndicatorType { get; internal set; }

        internal void StyleUpdated()
        {
            Region.Style = Styles.ActiveStyle;
        }
    }
}
