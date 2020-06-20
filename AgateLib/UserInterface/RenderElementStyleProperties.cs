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
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.UserInterface
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

        FlexItemStyle FlexItem { get; }

        LayoutStyle Layout { get; }

        ISizeConstraints Size { get; }

        TextAlign? TextAlign { get; }

        Overflow? Overflow { get; }

        /// <summary>
        /// The pseudoclasses this property set applies to.
        /// </summary>
        IReadOnlyCollection<string> PseudoClasses { get; }

        int Specificity { get; }
    }

    public class RenderElementStyleProperties : IRenderElementStyleProperties
    {
        private readonly IRenderElementStyleProperties core;
        private float scaling = 1;

        private ScaledSizeConstraints size;

        public RenderElementStyleProperties(IRenderElementStyleProperties core)
        {
            Require.ArgumentNotNull(core, nameof(core));

            this.core = core;

            if (core.Size != null)
            {
                size = new ScaledSizeConstraints(core.Size);
            }

            Padding = core.Padding;
            Margin = core.Margin;
        }

        public float Scaling
        {
            get => scaling;
            set
            {
                scaling = value;

                if (size != null)
                {
                    size.Scaling = value;
                }

                if (core.Padding != null)
                {
                    Padding = core.Padding.Value.Scale(scaling);
                }

                if (core.Margin != null)
                {
                    Margin = core.Margin.Value.Scale(scaling);
                }
            }
        }

        public string FontFace => core.FontFace;

        public Color? TextColor => core.TextColor;

        public int? FontSize
        {
            get
            {
                if (core.FontSize != null)
                {
                    return (int)(core.FontSize.Value * Scaling);
                }

                return core.FontSize;
            }
        }

        public FontStyles? FontStyle => core.FontStyle;

        public AnimationStyle Animation => core.Animation;

        public BackgroundStyle Background => core.Background;

        public BorderStyle Border => core.Border;

        public LayoutBox? Padding { get; private set; }

        public LayoutBox? Margin { get; private set; }

        public FlexStyle Flex => core.Flex;

        public FlexItemStyle FlexItem => core.FlexItem;

        public LayoutStyle Layout => core.Layout;

        public ISizeConstraints Size => size;

        public TextAlign? TextAlign => core.TextAlign;

        public Overflow? Overflow => core.Overflow;

        public IReadOnlyCollection<string> PseudoClasses => core.PseudoClasses;

        public int Specificity => core.Specificity;

    }

    public static class RenderElementStyleExtensions
    {
        public static LayoutBox ToLayoutBox(this BorderStyle borderStyle)
        {
            if (borderStyle == null)
            {
                return default;
            }

            return new LayoutBox(borderStyle.Left.Width,
                                 borderStyle.Top.Width,
                                 borderStyle.Right.Width,
                                 borderStyle.Bottom.Width);
        }
    }
}
