﻿//
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
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{

    public interface IRenderElementStyleProperties
    {
        string FontFace { get; }
        Color? TextColor { get; }
        int? FontSize { get; }
        FontStyles? FontStyle { get; }

        AnimationStyle Animation { get; }

        BackgroundStyle Background { get; }

        BorderStyle Border { get; }

        LayoutBox? Padding { get; }

        LayoutBox? Margin { get; }

        FlexStyle Flex { get; }

        LayoutStyle Layout { get; }

        /// <summary>
        /// The pseudoclasses this property set applies to.
        /// </summary>
        IReadOnlyCollection<string> PseudoClasses { get; }

        int Specificity { get; }
    }

    public static class RenderElementStyleExtensions
    {
        public static LayoutBox ToLayoutBox(this BorderStyle borderStyle)
        {
            if (borderStyle == null)
                return default(LayoutBox);

            return new LayoutBox(borderStyle.Left.Width,
                                 borderStyle.Top.Width,
                                 borderStyle.Right.Width,
                                 borderStyle.Bottom.Width);
        }
    }


    public class InlineElementStyle : IRenderElementStyleProperties
    {
        public static bool Equals(InlineElementStyle a, InlineElementStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.FontFace != b.FontFace) return false;
            if (a.TextColor != b.TextColor) return false;
            if (a.FontSize != b.FontSize) return false;
            if (a.FontStyle != b.FontStyle) return false;
            if (!BackgroundStyle.Equals(a.Background, b.Background)) return false;
            if (!BorderStyle.Equals(a.Border, b.Border)) return false;
            if (!AnimationStyle.Equals(a.Animation, b.Animation)) return false;
            if (!FlexStyle.Equals(a.Flex, b.Flex)) return false;
            if (!Nullable.Equals(a.Padding, b.Padding)) return false;
            if (!Nullable.Equals(a.Margin, b.Margin)) return false;
            if (!LayoutStyle.Equals(a.Layout, b.Layout)) return false;

            return true;
        }

        public string FontFace { get; set; }

        public Color? TextColor { get; set; }

        public int? FontSize { get; set; }

        public FontStyles? FontStyle { get; set; }

        public BackgroundStyle Background { get; set; }

        public BorderStyle Border { get; set; }

        public AnimationStyle Animation { get; set; }

        public FlexStyle Flex { get; set; }

        public LayoutStyle Layout { get; set; }

        public LayoutBox? Padding { get; set; }

        public LayoutBox? Margin { get; set; }

        public int Specificity => 1000;

        IReadOnlyCollection<string> IRenderElementStyleProperties.PseudoClasses => null;

        public override bool Equals(object obj)
        {
            if (!(obj is InlineElementStyle other))
                return false;

            return Equals(this, other);
        }
    }

}
