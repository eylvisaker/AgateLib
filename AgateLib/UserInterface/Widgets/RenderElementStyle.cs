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
using AgateLib.UserInterface.Styling;
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

        SizeConstraints Size { get; }

        FlexStyle Flex { get; }

        FlexItemStyle FlexItem { get; }

        LayoutStyle Layout { get; }

        LayoutBox Padding { get; }

        LayoutBox Margin { get; }


        /// <summary>
        /// Called by the rendering engine before each frame to ensure that the style
        /// is updated based on the widget's current state.
        /// </summary>
        void Update();
    }

    public class RenderElementStyle : IRenderElementStyle
    {
        private RenderElementDisplay display;
        private RenderElementProps props;

        private IRenderElementStyleProperties Inline => props.Style;
        private IRenderElementStyleProperties DefaultStyle => props.DefaultStyle;

        private List<IRenderElementStyleProperties> activeProperties = new List<IRenderElementStyleProperties>();
        private List<IRenderElementStyleProperties> testProperties = new List<IRenderElementStyleProperties>();

        private FontStyleProperties fontProperties = new FontStyleProperties();
        private FontStyleProperties compareFont = new FontStyleProperties();
        private FontStyleProperties parentFontProperties = new FontStyleProperties();
        private FontStyleProperties parentCompareFont = new FontStyleProperties();

        private Font font;

        public RenderElementStyle(RenderElementDisplay display, RenderElementProps props)
        {
            this.display = display;
            this.props = props;
        }

        public event Action FontChanged;

        public void SetProps(RenderElementProps props)
        {
            this.props = props;
        }

        public void Update()
        {
            if (!FindActiveProperties())
            {
                if (ParentFontChanged())
                    AggregateFont();

                return;
            }

            AggregateFont();

            Background = Aggregate(p => p.Background);
            Border = Aggregate(p => p.Border);
            Margin = Aggregate(p => p.Margin) ?? default(LayoutBox);
            Padding = Aggregate(p => p.Padding) ?? default(LayoutBox);
            Animation = Aggregate(p => p.Animation);
            Flex = Aggregate(p => p.Flex);
            FlexItem = Aggregate(p => p.FlexItem);
            Layout = Aggregate(p => p.Layout);
            Size = Aggregate(p => p.Size);
        }

        private bool ParentFontChanged()
        {
            parentCompareFont.CopyFrom(display.ParentFont);

            return !parentCompareFont.Equals(parentFontProperties);
        }

        private void AggregateFont()
        {
            compareFont.Family = Aggregate(p => p.FontFace);
            compareFont.Color = Aggregate(p => p.TextColor);
            compareFont.Style = Aggregate(p => p.FontStyle);
            compareFont.Size = Aggregate(p => p.FontSize);

            if (compareFont.IsEmpty)
            {
                font = new Font(display.ParentFont);
            }
            else if (!compareFont.Equals(fontProperties) || ParentFontChanged())
            {
                Swap(ref fontProperties, ref compareFont);

                if (string.IsNullOrWhiteSpace(fontProperties.Family))
                {
                    font = new Font(display.ParentFont);
                }
                else if (display.Fonts.HasFont(fontProperties.Family)
                         && !fontProperties.Family.Equals(Font?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    font = new Font(display.Fonts[fontProperties.Family]);
                }
                else if (Font == null)
                {
                    font = new Font(display.ParentFont);
                }

                font.Color = fontProperties.Color ?? display.ParentFont?.Color ?? Font.Color;
                font.Style = fontProperties.Style ?? display.ParentFont?.Style ?? Font.Style;
                font.Size = fontProperties.Size ?? display.ParentFont?.Size ?? Font.Size;
            }

            parentFontProperties.CopyFrom(display.ParentFont);

            FontChanged?.Invoke();
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

            foreach (var prop in activeProperties)
            {
                T thisProp = property(prop);

                if (thisProp == null)
                    continue;

                result = thisProp;
            }

            return result;
        }

        private T? Aggregate<T>(Func<IRenderElementStyleProperties, T?> property)
            where T : struct
        {
            T? result = null;

            foreach (var prop in activeProperties)
            {
                var thisProp = property(prop);

                if (thisProp == null)
                    continue;

                result = thisProp;
            }

            return result;
        }

        /// <summary>
        /// Updates the list of active properties.
        /// Returns true if a change was made, false if no changes were made.
        /// </summary>
        /// <returns></returns>
        private bool FindActiveProperties()
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
                    testProperties.Add(property);
            }

            if (Inline != null)
            {
                testProperties.Add(Inline);
            }

            SortProperties(testProperties);

            bool sameProps = true;

            if (testProperties.Count != activeProperties.Count)
                sameProps = false;

            if (sameProps)
            {
                for (int i = 0; i < testProperties.Count; i++)
                {
                    if (testProperties[i] != activeProperties[i])
                    {
                        sameProps = false;
                        break;
                    }
                }
            }

            if (sameProps)
                return false;

            activeProperties.Clear();
            activeProperties.AddRange(testProperties);

            return true;
        }

        private void SortProperties(List<IRenderElementStyleProperties> props)
        {
            props.Sort((x, y) => x.Specificity.CompareTo(y.Specificity));
        }

        private bool PropertyApplies(IRenderElementStyleProperties property)
        {
            if (!property.PseudoClasses.Any())
                return true;

            foreach (var propertyPseudoClass in property.PseudoClasses)
            {
                if (!display.PseudoClasses.Contains(propertyPseudoClass))
                    return false;
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

        public SizeConstraints Size { get; private set; }

        public LayoutBox Padding { get; private set; }

        public LayoutBox Margin { get; private set; }

        public FlexStyle Flex { get; private set; }

        public FlexItemStyle FlexItem { get; private set; }

        public LayoutStyle Layout { get; private set; }

    }
}
