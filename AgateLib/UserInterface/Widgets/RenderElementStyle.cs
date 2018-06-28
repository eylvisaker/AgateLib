using AgateLib.Display;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public interface IRenderElementStyle
    {
        IFont Font { get; }

        /// <summary>
        /// Called by the rendering engine before each frame to ensure that the style
        /// is updated based on the widget's current state.
        /// </summary>
        void Update();
    }

    public class RenderElementStyle : IRenderElementStyle
    {
        private WidgetDisplay display;
        private readonly IRenderElementStyleProperties inline;

        List<IRenderElementStyleProperties> validProperties
            = new List<IRenderElementStyleProperties>();

        FontStyleProperties fontProperties = new FontStyleProperties();
        FontStyleProperties compareFont = new FontStyleProperties();

        public RenderElementStyle(WidgetDisplay display, IRenderElementStyleProperties inline)
        {
            this.display = display;
            this.inline = inline;
        }

        public void Update()
        {
            FindValidProperties();

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

            foreach (var prop in validProperties)
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

            foreach (var prop in validProperties)
            {
                var thisProp = property(prop);

                if (thisProp == null)
                    continue;

                result = thisProp;
            }

            return result;
        }

        private void FindValidProperties()
        {
            validProperties.Clear();

            // TODO: Implement filtering based on current state and pseudoclasses.
            validProperties.AddRange(display.ElementStyles);

            if (inline != null)
            {
                validProperties.Add(inline);
            }
        }

        public IFont Font { get; private set; }
    }

    public interface IxRenderElementStyle
    {
        //Overflow Overflow { get; }
        //IFont Font { get; }
    }

    public enum Overflow
    {
        Visible,
        Hidden,
        Scroll,
    }

    public interface IRenderElementStyleProperties
    {
        string FontFace { get; }
        Color? TextColor { get; }
        int? FontSize { get; }
        FontStyles? FontStyle { get; }
    }

    public static class RenderElementStyleExtensions
    {
        public static IRenderElementStyleProperties ToElementStyle(this ThemeStyle themeStyle)
        {
            return new ThemeRenderElementStyle(themeStyle);
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

    }

    public class InlineElementStyle : IRenderElementStyleProperties
    {
        public string FontFace { get; set; }

        public Color? TextColor { get; set; }

        public int? FontSize { get; set; }

        public FontStyles? FontStyle { get; set; }
    }
}
