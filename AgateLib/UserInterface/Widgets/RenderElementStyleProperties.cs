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

        /// <summary>
        /// The pseudoclasses this property set applies to.
        /// </summary>
        IReadOnlyCollection<string> PseudoClasses { get; }

        int Specificity { get; }
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

        public IReadOnlyCollection<string> PseudoClasses => themeStyle.PseudoClasses;

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

        public int Specificity => themeStyle.Specificity;
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

        public int Specificity => 1000;

        IReadOnlyCollection<string> IRenderElementStyleProperties.PseudoClasses => null;
    }

}
