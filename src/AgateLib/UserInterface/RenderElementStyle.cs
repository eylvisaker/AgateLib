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
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface
{
    public interface IRenderElementStyle
    {
        event Action FontChanged;

        Font Font { get; }

        BorderStyle Border { get; }

        BackgroundStyle Background { get; }

        AnimationStyle Animation { get; }

        ISizeConstraints Size { get; }

        FlexStyle Flex { get; }

        FlexItemStyle FlexItem { get; }

        LayoutStyle Layout { get; }

        LayoutBox Padding { get; }

        LayoutBox Margin { get; }

        TextAlign TextAlign { get; }

        Overflow Overflow { get; }

        /// <summary>
        /// Called by the rendering engine before each frame to ensure that the style
        /// is updated based on the widget's current state.
        /// </summary>
        void Update(float scaling);
    }

    public class RenderElementStyle : IRenderElementStyle
    {
        private RenderElementDisplay display;
        private RenderElementProps props;

        private RenderElementStyleProperties Inline { get; set; }
        private RenderElementStyleProperties DefaultStyle { get; set; }

        private List<RenderElementStyleProperties> activeProperties = new List<RenderElementStyleProperties>();
        private List<RenderElementStyleProperties> testProperties = new List<RenderElementStyleProperties>();

        private FontStyleProperties fontProperties = new FontStyleProperties();
        private FontStyleProperties compareFont = new FontStyleProperties();
        private FontStyleProperties parentFontProperties = new FontStyleProperties();
        private FontStyleProperties parentCompareFont = new FontStyleProperties();

        private Font font;

        public RenderElementStyle(RenderElementDisplay display, RenderElementProps props)
        {
            this.display = display;

            SetProps(props);
        }

        public event Action FontChanged;

        public void SetProps(RenderElementProps props)
        {
            this.props = props;

            if (props.Style != null)
            {
                Inline = new RenderElementStyleProperties(props.Style);
            }
            else
            {
                Inline = null;
            }

            if (props.DefaultStyle != null)
            {
                DefaultStyle = new RenderElementStyleProperties(props.DefaultStyle);
            }
            else
            {
                DefaultStyle = null;
            }
        }

        public void Update(float scaling)
        {
            if (!FindActiveProperties(scaling))
            {
                if (ParentFontChanged() && AggregateFont())
                {
                    FontChanged?.Invoke();
                }

                return;
            }

            bool fontChanged = AggregateFont();

            Background = Aggregate(p => p.Background);
            Border = Aggregate(p => p.Border);
            Margin = Aggregate(p => p.Margin) ?? default(LayoutBox);
            Padding = Aggregate(p => p.Padding) ?? default(LayoutBox);
            TextAlign = Aggregate(p => p.TextAlign) ?? TextAlign.Left;
            Animation = Aggregate(p => p.Animation);
            Flex = Aggregate(p => p.Flex);
            FlexItem = Aggregate(p => p.FlexItem);
            Layout = Aggregate(p => p.Layout);
            Size = Aggregate(p => p.Size);
            Overflow = Aggregate(p => p.Overflow) ?? default(Overflow);

            if (fontChanged)
            {
                FontChanged?.Invoke();
            }
        }

        private bool ParentFontChanged()
        {
            parentCompareFont.CopyFrom(display.ParentFont);

            return !parentCompareFont.Equals(parentFontProperties);
        }

        private bool AggregateFont()
        {
            compareFont.Family = Aggregate(p => p.FontFace);
            compareFont.Color = Aggregate(p => p.TextColor);
            compareFont.Style = Aggregate(p => p.FontStyle);
            compareFont.Size = Aggregate(p => p.FontSize);

            if (compareFont.IsEmpty)
            {
                return SetFont(display.ParentFont);
            }
            else if (!compareFont.Equals(fontProperties) || ParentFontChanged())
            {
                Swap(ref fontProperties, ref compareFont);

                if (string.IsNullOrWhiteSpace(fontProperties.Family))
                {
                    SetFont(display.ParentFont);
                }
                else if (display.Fonts.HasFont(fontProperties.Family)
                         && !fontProperties.Family.Equals(Font?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    SetFont(display.Fonts[fontProperties.Family]);
                }
                else if (font == null)
                {
                    SetFont(display.ParentFont);
                }

                font.Color = fontProperties.Color ?? display.ParentFont?.Color ?? Font.Color;
                font.Style = fontProperties.Style ?? display.ParentFont?.Style ?? Font.Style;
                font.Size = fontProperties.Size ?? display.ParentFont?.Size ?? Font.Size;

                parentFontProperties.CopyFrom(display.ParentFont);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the font for the render element style. This will compare font objects
        /// and only create a new font object if they are different.
        /// Returns true if the font changed.
        /// </summary>
        /// <param name="parentFont"></param>
        private bool SetFont(Font parentFont)
        {
            Require.ArgumentNotNull(parentFont, nameof(parentFont));
            bool result = false;

            if (font?.Core != parentFont.Core
             || font?.Name != parentFont.Name)
            {
                font = new Font(display.ParentFont);
                result = true;
            }

            if (font.Color != parentFont.Color)
            {
                result = true;
            }

            if (font.Style != parentFont.Style)
            {
                result = true;
            }

            if (font.Size != parentFont.Size)
            {
                result = true;
            }

            font.Color = parentFont.Color;
            font.Style = parentFont.Style;
            font.Size = parentFont.Size;

            return result;
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        private T Aggregate<T>(Func<IRenderElementStyleProperties, T> property)
            where T : class
        {
            T result = null;

            foreach (IRenderElementStyleProperties prop in activeProperties)
            {
                T thisProp = property(prop);

                if (thisProp == null)
                {
                    continue;
                }

                result = thisProp;
            }

            return result;
        }

        private T? Aggregate<T>(Func<IRenderElementStyleProperties, T?> property)
            where T : struct
        {
            T? result = null;

            foreach (IRenderElementStyleProperties prop in activeProperties)
            {
                var thisProp = property(prop);

                if (thisProp == null)
                {
                    continue;
                }

                result = thisProp;
            }

            return result;
        }

        /// <summary>
        /// Updates the list of active properties.
        /// Returns true if a change was made, false if no changes were made.
        /// </summary>
        /// <returns></returns>
        private bool FindActiveProperties(float scaling)
        {
            testProperties.Clear();

            if (DefaultStyle != null)
            {
                testProperties.Add(DefaultStyle);
            }

            // TODO: Implement filtering based on current state and pseudoclasses.
            foreach (var property in display.ElementStyles)
            {
                if (PropertyApplies(property))
                {
                    testProperties.Add(property);
                }
            }

            if (Inline != null)
            {
                testProperties.Add(Inline);
            }

            SortProperties(testProperties);

            bool sameProps = true;

            if (testProperties.Count != activeProperties.Count)
            {
                sameProps = false;
            }

            if (sameProps)
            {
                for (int i = 0; i < testProperties.Count; i++)
                {
                    if (testProperties[i] != activeProperties[i] ||
                        testProperties[i].Scaling != scaling)
                    {
                        sameProps = false;
                        break;
                    }
                }
            }

            if (sameProps)
            {
                return false;
            }

            activeProperties.Clear();
            activeProperties.AddRange(testProperties);

            foreach (RenderElementStyleProperties prop in activeProperties)
            {
                prop.Scaling = scaling;
            }

            return true;
        }

        private void SortProperties(List<RenderElementStyleProperties> props)
        {
            props.Sort((x, y) => x.Specificity.CompareTo(y.Specificity));
        }

        private bool PropertyApplies(RenderElementStyleProperties property)
        {
            if (!property.PseudoClasses.Any())
            {
                return true;
            }

            foreach (var propertyPseudoClass in property.PseudoClasses)
            {
                if (!display.PseudoClasses.Contains(propertyPseudoClass))
                {
                    return false;
                }
            }

            return true;
        }

        public Font Font
        {
            get
            {
                return font ?? display.ParentFont;
            }
        }

        public BorderStyle Border { get; private set; }

        public BackgroundStyle Background { get; private set; }

        public AnimationStyle Animation { get; private set; }

        public ISizeConstraints Size { get; private set; }

        public LayoutBox Padding { get; private set; }

        public LayoutBox Margin { get; private set; }

        public TextAlign TextAlign { get; private set; }

        public Overflow Overflow { get; private set; }

        public FlexStyle Flex { get; private set; }

        public FlexItemStyle FlexItem { get; private set; }

        public LayoutStyle Layout { get; private set; }

    }
}
