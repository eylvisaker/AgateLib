using AgateLib.Display;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Styling.Themes.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElementStyle
    {
        IFont Font { get; }

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

        public RenderElementStyle(RenderElementDisplay display, IRenderElementStyleProperties inline)
        {
            this.display = display;
            this.inline = inline;
        }

        public void Update()
        {
            if (!FindActiveProperties() && Font != null)
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

            if (Font == null || !compareFont.Equals(fontProperties))
            {
                Swap(ref fontProperties, ref compareFont);

                if (!string.IsNullOrWhiteSpace(fontProperties.Family)
                    && display.Fonts.HasFont(fontProperties.Family)
                    && !fontProperties.Family.Equals(Font?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Font = new Font(display.Fonts[fontProperties.Family]);
                }
                else if (Font == null)
                {
                    Font = new Font(display.Fonts.GetOrDefault(fontProperties.Family));
                }

                Font.Color = fontProperties.Color ?? display.ParentFont?.Color ?? Font.Color;
                Font.Style = fontProperties.Style ?? display.ParentFont?.Style ?? Font.Style;
                Font.Size = fontProperties.Size ?? display.ParentFont?.Size ?? Font.Size;
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
            testProperties.AddRange(display.ElementStyles);

            if (inline != null)
            {
                testProperties.Add(inline);
            }

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

        public IFont Font { get; private set; }

        public BorderStyle Border { get; private set; }

        public BackgroundStyle Background { get; private set; }

        public AnimationStyle Animation { get; private set; }

        public ThemeWidgetSize Size { get; } = new ThemeWidgetSize();

        public LayoutBox Padding { get; private set; }

        public LayoutBox Margin { get; private set; }

        public FlexStyle Flex { get; private set; }
    }

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
    }

    public static class RenderElementStyleExtensions
    {
        public static IRenderElementStyleProperties ToElementStyle(this ThemeStyle themeStyle)
        {
            return new ThemeRenderElementStyle(themeStyle);
        }

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

    public class ThemeRenderElementStyle : IRenderElementStyleProperties
    {
        private ThemeStyle themeStyle;

        public ThemeRenderElementStyle(ThemeStyle themeStyle)
        {
            this.themeStyle = themeStyle;
        }

        public string FontFace => themeStyle.Font?.Family;
        public Color? TextColor => themeStyle.Font?.Color;
        public int? FontSize => themeStyle.Font?.Size;
        public FontStyles? FontStyle => themeStyle.Font?.Style;

        public BackgroundStyle Background => themeStyle.Background;

        public BorderStyle Border => themeStyle.Border;


        public AnimationStyle Animation => themeStyle.Animation;

        public FlexStyle Flex => themeStyle.Flex;

        public LayoutBox? Margin => themeStyle.Margin;
        public LayoutBox? Padding => themeStyle.Padding;
    }

    public class InlineElementStyle : IRenderElementStyleProperties
    {
        public string FontFace { get; set; }

        public Color? TextColor { get; set; }

        public int? FontSize { get; set; }

        public FontStyles? FontStyle { get; set; }

        public BackgroundStyle Background { get; set; }

        public BorderStyle Border { get; set; }

        public AnimationStyle Animation { get; set; }

        public FlexStyle Flex { get; set; }

        public LayoutBox? Padding { get; set; }

        public LayoutBox? Margin { get; set; }
    }

}
