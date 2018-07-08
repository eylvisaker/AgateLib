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
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// An object which contains all information that the rendering system
    /// needs to render a widget.
    /// </summary>
    public class RenderElementDisplay
    {
        RenderElementStyle style;

        public RenderElementDisplay(RenderElementProps props)
        {
            style = new RenderElementStyle(this, props);
            Region = new WidgetRegion(Style);

            Animation = new RenderElementAnimator(this);
        }

        /// <summary>
        /// Called only by RenderElement&lt;T&gt; to pass along a props update.
        /// </summary>
        /// <param name="props"></param>
        internal void SetProps(RenderElementProps props)
        {
            style.SetProps(props);
        }

        /// <summary>
        /// Owned by the rendering engine.
        /// </summary>
        public RenderElementAnimator Animation { get; }

        /// <summary>
        /// Gets the layout state.
        /// </summary>
        public WidgetRegion Region { get; }

        /// <summary>
        /// Gets the active widget style.
        /// </summary>
        public IRenderElementStyle Style => style;

        /// <summary>
        /// Gets the collection of styles that can be swapped out based on the widget's state.
        /// </summary>
        [Obsolete]
        public WidgetStyleCollection Styles { get; } = new WidgetStyleCollection();

        /// <summary>
        /// Gets the font for this widget.
        /// </summary>
        [Obsolete("Elements should use Style.Font instead.")]
        public Font Font => (Font)Style.Font;

        /// <summary>
        /// Gets a value indicating whether this widget is participating in the layout flow.
        /// </summary>
        public bool IsInLayout
        {
            get
            {
                if (!IsVisible) return false;

                var positionType = Style.Layout?.PositionType ?? PositionType.Default;

                if (positionType == PositionType.Absolute || positionType == PositionType.Fixed)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets or sets whether the widget should be displayed and participate in layout.
        /// </summary>
        public bool IsVisible { get; set; } = true;


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

        public IndicatorType IndicatorType { get; internal set; }

        public List<IRenderElementStyleProperties> ElementStyles { get; }
            = new List<IRenderElementStyleProperties>();

        public Font ParentFont { get; set; }

        public IFontProvider Fonts => System.Fonts;

        /// <summary>
        /// Gets the content rectangle, or sets all three rectangles based on the box model
        /// and the specified content rectangle.
        /// </summary>
        public Rectangle ContentRect
        {
            get => Region.ContentRect;
            set
            {
                Region.SetContentRect(value);
                Animation.ContentRectUpdated();
            }
        }

        /// <summary>
        /// Gets the margin rectangle, or sets all three rectangles based on the box model
        /// and the specified margin rectangle.
        /// </summary>
        public Rectangle MarginRect
        {
            get => Region.MarginRect;
            set
            {
                Rectangle contentRect = new Rectangle(
                    value.X + Region.MarginToContentOffset.Left,
                    value.Y + Region.MarginToContentOffset.Top,
                    value.Width - Region.MarginToContentOffset.Width,
                    value.Height - Region.MarginToContentOffset.Height);

                Region.SetContentRect(contentRect);
            }
        }

        /// <summary>
        /// Gets the border rectangle, or sets all three rectangles based on the box model
        /// and the specified border rectangle.
        /// </summary>
        public Rectangle BorderRect
        {
            get => Region.BorderRect;
            set
            {
                Rectangle contentRect = new Rectangle(
                    value.X + Region.BorderToContentOffset.Left,
                    value.Y + Region.BorderToContentOffset.Top,
                    value.Width - Region.BorderToContentOffset.Width,
                    value.Height - Region.BorderToContentOffset.Height);

                Region.SetContentRect(contentRect);
            }
        }

        public IDisplaySystem System { get; internal set; }

        /// <summary>
        /// Gets the collection of pseudoclases that apply to this element.
        /// </summary>
        public HashSet<string> PseudoClasses { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}
