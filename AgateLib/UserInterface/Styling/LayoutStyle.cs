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

namespace AgateLib.UserInterface
{
    public class LayoutStyle
    {
        public static bool Equals(LayoutStyle a, LayoutStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.PositionType != b.PositionType) return false;

            return true;
        }

        /// <summary>
        /// Gets or sets how the item is positioned.
        /// </summary>
        public PositionType PositionType { get; set; }
    }

    public enum PositionType
    {
        /// <summary>
        /// Static - the item is always positioned in the normal flow.
        /// </summary>
        Static,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Relative,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Fixed,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Absolute,

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        Sticky, 

        Default = Static,
    }
}
