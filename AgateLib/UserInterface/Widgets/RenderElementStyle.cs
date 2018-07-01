using AgateLib.Display;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElementStyle
    {
        Font Font { get; }

        BorderStyle Border { get; }

        BackgroundStyle Background { get; }

        AnimationStyle Animation { get; }

        ThemeWidgetSize Size { get; }

        FlexStyle Flex { get; }

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
        private readonly IRenderElementStyleProperties inline;

        List<IRenderElementStyleProperties> activeProperties = new List<IRenderElementStyleProperties>();
        List<IRenderElementStyleProperties> testProperties = new List<IRenderElementStyleProperties>();

        FontStyleProperties fontProperties = new FontStyleProperties();
        FontStyleProperties compareFont = new FontStyleProperties();

        Font font;

        public RenderElementStyle(RenderElementDisplay display, IRenderElementStyleProperties inline)
        {
            this.display = display;
            this.inline = inline;
        }

        public void Update()
        {
            if (!FindActiveProperties())
                return;

            AggregateFont();

            Background = Aggregate(p => p.Background);
            Border = Aggregate(p => p.Border);
            Margin = Aggregate(p => p.Margin) ?? default(LayoutBox);
            Padding = Aggregate(p => p.Padding) ?? default(LayoutBox);
            Animation = Aggregate(p => p.Animation);
            Flex = Aggregate(p => p.Flex);
        }

        private void AggregateFont()
        {
            compareFont.Family = Aggregate(p => p.FontFace);
            compareFont.Color = Aggregate(p => p.TextColor);
            compareFont.Style = Aggregate(p => p.FontStyle);
            compareFont.Size = Aggregate(p => p.FontSize);

            if (compareFont.IsEmpty)
            {
                font = null;
            }
            else if (!compareFont.Equals(fontProperties))
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
                var thisProp = property(prop);

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

            // TODO: Implement filtering based on current state and pseudoclasses.
            foreach(var property in display.ElementStyles)
            {
                if (PropertyApplies(property))
                    testProperties.Add(property);
            }

            if (inline != null)
            {
                testProperties.Add(inline);
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

        public ThemeWidgetSize Size { get; } = new ThemeWidgetSize();

        public LayoutBox Padding { get; private set; }

        public LayoutBox Margin { get; private set; }

        public FlexStyle Flex { get; private set; }
    }
}
