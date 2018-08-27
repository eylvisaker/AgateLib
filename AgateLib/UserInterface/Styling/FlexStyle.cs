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
using AgateLib.UserInterface;

namespace AgateLib.UserInterface
{
    public class FlexStyle
    {
        public static bool Equals(FlexStyle a, FlexStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.AlignItems != b.AlignItems) return false;
            if (a.Direction != b.Direction) return false;
            if (a.JustifyContent != b.JustifyContent) return false;

            return true;
        }

        /// <summary>
        /// Gets or sets how items should be aligned on the cross axis.
        /// </summary>
        public AlignItems AlignItems { get; set; }

        /// <summary>
        /// Gets or sets the direction of the flex axis
        /// </summary>
        public FlexDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets how items should be aligned on the main axis.
        /// </summary>
        public JustifyContent JustifyContent { get; set; }
    }

    /// <summary>
    /// Enum for values which indicate how items should be aligned on the cross axis.
    /// </summary>
    public enum AlignItems
    {
        /// <summary>
        /// Items are stretched to fill the entire area available to them on the cross axis.
        /// This is the default value.
        /// </summary>
        Stretch,

        /// <summary>
        /// Items are aligned to the start of the cross axis. For a column layout, this aligns
        /// items on the left, and for a row layout this aligns items across the top.
        /// </summary>
        Start,

        /// <summary>
        /// Items are aligned to the end of the cross axis. For a column layout, this
        /// aligns items on the right, for a row layout this aligns items to the bottom.
        /// </summary>
        End,

        /// <summary>
        /// Items are centered along the cross axis.
        /// </summary>
        Center,

        /// <summary>
        /// Default value is Stretch.
        /// </summary>
        Default = Stretch,
    }

    /// <summary>
    /// Enum for values which indicate how items should be aligned on the main axis.
    /// </summary>
    public enum JustifyContent
    {
        /// <summary>
        /// Items are packed from the start of the axis. This is the default.
        /// </summary>
        Start,

        /// <summary>
        /// Items are packed from the end of the axis.
        /// </summary>
        End,

        /// <summary>
        /// Items are packed near the center of the axis.
        /// </summary>
        Center,

        /// <summary>
        /// Additional space is distributed between items.
        /// </summary>
        SpaceBetween,

        /// <summary>
        /// Additional space is distributed around items.
        /// </summary>
        SpaceAround,

        /// <summary>
        /// Additional space is divided up so items have the same amount of space between them and the edges of the
        /// container.
        /// </summary>
        SpaceEvenly,

        /// <summary>
        /// Default value is Start.
        /// </summary>
        Default = Start,
    }
}
