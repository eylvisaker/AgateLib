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
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// Structure for holding information about the widget's layout within its parent's content area.
    /// </summary>
    public class WidgetRegion
    {
        WidgetStyle style;
        Rectangle contentRect;

        /// <summary>
        /// Constructs a WidgetLayout structure.
        /// </summary>
        /// <param name="activeStyle"></param>
        /// <remarks>
        /// The dependency on WidgetStyle is something I'm calling a "perfect" dependency.
        /// WidgetLayout needs a WidgetStyle object to function and avoid the null reference checks everywhere for situations which would be unrecoverable anyway if it was null.
        /// WidgetLayout interacts with WidgetStyle in a completely read-only manner. Nothing is written to WidgetStyle.
        /// WidgetLayout caches nothing from WidgetStyle.
        /// The WidgetStyle can be switched to any other WidgetStyle at-will without any deterimental effect to the behavior of the WidgetLayout.
        /// Because of these facts, we can freely change the style for a widget at will.
        /// </remarks>
        public WidgetRegion(WidgetStyle activeStyle)
        {
            this.Style = activeStyle;
        }

        /// <summary>
        /// Gets a LayoutBox for the border to content distance.
        /// </summary>
        public LayoutBox BorderToContentOffset
        {
            get => new LayoutBox
            {
                Left = Style.Border.Left.Width + Style.Padding.Left,
                Top = Style.Border.Top.Width + Style.Padding.Top,
                Right = Style.Border.Right.Width + Style.Padding.Right,
                Bottom = Style.Border.Bottom.Width + Style.Padding.Bottom,
            };
        }

        /// <summary>
        /// Gets a LayoutBox for the margin to content distance.
        /// </summary>
        public LayoutBox MarginToContentOffset
        {
            get => new LayoutBox
            {
                Left = Style.Border.Left.Width + Style.Padding.Left + Style.Margin.Left,
                Top = Style.Border.Top.Width + Style.Padding.Top + Style.Margin.Top,
                Right = Style.Border.Right.Width + Style.Padding.Right + Style.Margin.Right,
                Bottom = Style.Border.Bottom.Width + Style.Padding.Bottom + Style.Margin.Bottom,
            };
        }

        /// <summary>
        /// Gets the margin rectangle for this widget. This includes
        /// margin + border + padding.
        /// Position is in parent-client coordinates.
        /// </summary>
        public Rectangle MarginRect
        {
            get
            {
                var offset = MarginToContentOffset;

                return new Rectangle(
                    contentRect.X - offset.Left,
                    contentRect.Y - offset.Right,
                    contentRect.Width + offset.Left + offset.Right,
                    contentRect.Height + offset.Top + offset.Bottom);
            }
        }

        /// <summary>
        /// Gets or sets the position of the content for this widget.
        /// This is in the parent's client space.
        /// </summary>
        public Point Position
        {
            get => contentRect.Location;
            set => contentRect.Location = value;
        }

        /// <summary>
        /// Gets or sets the size of the client area for this widget.
        /// </summary>
        public Size ContentSize
        {
            get => new Size(contentRect.Size.X, contentRect.Size.Y);
            set => contentRect.Size = new Point(value.Width, value.Height);
        }

        /// <summary>
        /// Gets the border rectangle for this widget.
        /// This includes border + padding.
        /// Position is in parent-client coordinates.
        /// </summary>
        public Rectangle BorderRect
        {
            get
            {
                var offset = BorderToContentOffset;

                return new Rectangle(
                    contentRect.X - offset.Left,
                    contentRect.Y - offset.Top,
                    contentRect.Width + offset.Left + offset.Right,
                    contentRect.Height + offset.Top + offset.Bottom);
            }
        }

        /// <summary>
        /// Gets the content rectangle in client coordinates of this
        /// widget. 
        /// </summary>
        public Rectangle ContentRect
        {
            get => contentRect;
            set => contentRect = value;
        }

        /// <summary>
        /// A reference to the active style for this widget.
        /// </summary>
        public WidgetStyle Style
        {
            get => style;
            set
            {
                style = value ?? throw new ArgumentNullException(nameof(Style));

                Size.MinimumSize = style.Size.Min;
                Size.MaximumSize = style.Size.Max;
            }
        }

        /// <summary>
        /// Gets the SizeMetrics object for this widget.
        /// </summary>
        public SizeMetrics Size { get; } = new SizeMetrics();
    }
}
