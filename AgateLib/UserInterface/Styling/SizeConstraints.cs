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
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    public class SizeConstraints
    {
        public static bool Equals(SizeConstraints a, SizeConstraints b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            // TODO: Implement these as percentages of parent size.
            //if (a.Width != b.Width) return false;
            //if (a.Height != b.Height) return false;

            if (a.MinWidth != b.MinWidth) return false;
            if (a.MaxWidth != b.MaxWidth) return false;

            if (a.MinHeight != b.MinHeight) return false;
            if (a.MaxHeight != b.MaxHeight) return false;

            return true;
        }

        //public int? Width { get; set; }
        //public int? Height { get; set; }

        public int MinWidth { get; set; }
        public int? MaxWidth { get; set; }

        public int MinHeight { get; set; }
        public int? MaxHeight { get; set; }

        public Size ToMinSize()
        {
            return new Size(MinWidth, MinHeight);
        }
    }
}
