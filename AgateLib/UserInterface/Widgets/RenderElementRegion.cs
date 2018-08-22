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

using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Structure for holding information about the widget's layout within its parent's content area.
    /// </summary>
    public class RenderElementRegion
    {
        private Rectangle contentRect;

        /// <summary>
        /// Constructs a WidgetLayout structure.
        /// </summary>
        /// <param name="activeStyle"></param>
        public RenderElementRegion(IRenderElementStyle activeStyle)
        {
            this.Style = activeStyle;
        }

        /// <summary>
        /// A reference to the active style for this widget.
        /// </summary>
        public IRenderElementStyle Style { get; private set; }

        /// <summary>
        /// Gets a LayoutBox for the border to content distance.
        /// </summary>
        public LayoutBox BorderToContentOffset
        {
            get
            {
                var borderLayout = Style.Border.ToLayoutBox();

                return new LayoutBox
                {
                    Left = borderLayout.Left + Style.Padding.Left,
                    Top = borderLayout.Top + Style.Padding.Top,
                    Right = borderLayout.Right + Style.Padding.Right,
                    Bottom = borderLayout.Bottom + Style.Padding.Bottom,
                };
            }
        }

        /// <summary>
        /// Gets a LayoutBox for the margin to content distance.
        /// </summary>
        public LayoutBox MarginToContentOffset
        {
            get
            {
                var borderLayout = Style.Border.ToLayoutBox();

                return new LayoutBox
                {
                    Left = borderLayout.Left + Style.Padding.Left + Style.Margin.Left,
                    Top = borderLayout.Top + Style.Padding.Top + Style.Margin.Top,
                    Right = borderLayout.Right + Style.Padding.Right + Style.Margin.Right,
                    Bottom = borderLayout.Bottom + Style.Padding.Bottom + Style.Margin.Bottom,
                };
            }
        }

        /// <summary>
        /// Gets the margin rectangle for this widget. This includes
        /// margin + border + padding.
        /// Position is in parent-client coordinates.
        /// </summary>
        public Rectangle MarginRect
        {
            get => MarginToContentOffset.Expand(contentRect);
        }

        /// <summary>
        /// Gets the border rectangle for this widget.
        /// This includes border + padding.
        /// Position is in parent-client coordinates.
        /// </summary>
        public Rectangle BorderRect => BorderToContentOffset.Expand(contentRect);

        /// <summary>
        /// Gets the content rectangle in client coordinates of this
        /// widget. 
        /// </summary>
        public Rectangle ContentRect
        {
            get => contentRect;
        }

        /// <summary>
        /// Gets the size of the margin rectangle.
        /// </summary>
        public Size MarginSize => MarginRect.Size;

        public void SetContentRect(Rectangle newContentRect) => contentRect = newContentRect;

        public void SetContentSize(Size newContentSize) => contentRect.Size = newContentSize;

        public void SetMarginPosition(Point position)
        {
            var contentPos = new Point(position.X + MarginToContentOffset.Left,
                                       position.Y + MarginToContentOffset.Top);

            contentRect.Location = contentPos;
        }

        public void SetMarginSize(Size newMarginSize)
        {
            contentRect.Size = new Size(newMarginSize.Width - MarginToContentOffset.Width,
                                        newMarginSize.Height - MarginToContentOffset.Height);
        }

        /// <summary>
        /// Gets or sets the ideal content size of this item given its size constraints.
        /// </summary>
        public Size IdealContentSize { get; set; }

        /// <summary>
        /// Gets the ideal margin size by combining MarginToContentOffset with IdealContentSize.
        /// </summary>
        public Size IdealMarginSize => MarginToContentOffset.Expand(IdealContentSize);

        public Point ContentSize => contentRect.Size;

        /// <summary>
        /// Computes the constrained content size of the widget, given the constraints and its ideal 
        /// content size.
        /// </summary>
        /// <returns></returns>
        public Size CalcConstrainedContentSize(Size parentMaxSize)
        {
            Size result = new Size(
                /*Style.Size?.Width ?? */IdealContentSize.Width,
                /*Style.Size?.Height ??*/ IdealContentSize.Height);

            if (Style.Size != null)
            {
                result.Width = Maybe(Math.Max, result.Width, Style.Size.MinWidth);
                result.Width = Maybe(Math.Min, result.Width, Style.Size.MaxWidth);

                result.Height = Maybe(Math.Max, result.Height, Style.Size.MinHeight);
                result.Height = Maybe(Math.Min, result.Height, Style.Size.MaxHeight);
            }

            result.Width = Math.Min(result.Width, parentMaxSize.Width);
            result.Height = Math.Min(result.Height, parentMaxSize.Height);

            return result;
        }

        private int Maybe(Func<int, int, int> func, int a, int? b)
        {
            if (b == null)
                return a;

            return func(a, b.Value);
        }
    }
}
