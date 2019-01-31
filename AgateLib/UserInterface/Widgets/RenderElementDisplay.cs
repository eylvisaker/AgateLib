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

using AgateLib.Display;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// An object which contains all information that the rendering system
    /// needs to render a widget.
    /// </summary>
    public class RenderElementDisplay
    {
        private RenderElementStyle style;
        private Point scrollPosition;

        public RenderElementDisplay(RenderElementProps props)
        {
            style = new RenderElementStyle(this, props);
            Region = new RenderElementRegion(Style);

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
        /// Gets or sets the current scroll position.
        /// Values are always positive.
        /// </summary>
        public Point ScrollPosition
        {
            get => scrollPosition;
            set => scrollPosition = value;
        }

        /// <summary>
        /// Scrolls to the specified render element.
        /// </summary>
        /// <param name="display"></param>
        public void ScrollTo(RenderElementDisplay display)
        {
            var showRect = display.BorderRect;

            int offRight = showRect.Right - scrollPosition.X - MarginRect.Width;
            int offBottom = showRect.Bottom - scrollPosition.Y - MarginRect.Height;
            int offTop = scrollPosition.Y - showRect.Top;
            int offLeft = scrollPosition.X - showRect.Left;

            if (offLeft > 0)
            {
                scrollPosition.X = showRect.Left;
            }
            else if (offRight > 0)
            {
                scrollPosition.X += offRight;
            }

            if (offTop > 0)
            {
                scrollPosition.Y = showRect.Top;
            }
            else if (offBottom > 0)
            {
                scrollPosition.Y += offBottom;
            }
        }

        /// <summary>
        /// Owned by the rendering engine.
        /// </summary>
        public RenderElementAnimator Animation { get; }

        /// <summary>
        /// Gets the layout state.
        /// </summary>
        public RenderElementRegion Region { get; }

        /// <summary>
        /// Gets the active widget style.
        /// </summary>
        public IRenderElementStyle Style => style;

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
        public PseudoClassCollection PseudoClasses { get; } = new PseudoClassCollection();

        public HasOverflow HasOverflow { get; internal set; }

        public bool IsDoubleBuffered => Animation.IsDoubleBuffered
                                     || (HasOverflow != HasOverflow.None
                                         && Style.Overflow != Overflow.Visible);

        public override string ToString() => $"Margin: {MarginRect}, Content: {ContentRect}";
        
    }
}
