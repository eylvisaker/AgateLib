using AgateLib.Display;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class ThemeRenderElementStyle : IRenderElementStyleProperties
    {
        private ThemeStyle themeStyle;
        private readonly SelectorMatch match;

        public ThemeRenderElementStyle(ThemeStyle themeStyle, SelectorMatch match)
        {
            this.themeStyle = themeStyle;
            this.match = match;
        }

        public string FontFace => themeStyle.Font?.Family;

        public Color? TextColor => themeStyle.Font?.Color;

        public int? FontSize => themeStyle.Font?.Size;

        public FontStyles? FontStyle => themeStyle.Font?.Style;

        public BackgroundStyle Background => themeStyle.Background;

        public BorderStyle Border => themeStyle.Border;

        public AnimationStyle Animation => themeStyle.Animation;

        public FlexStyle Flex => themeStyle.Flex;

        public FlexItemStyle FlexItem => themeStyle.FlexItem;

        public SizeConstraints Size => themeStyle.Size;

        public LayoutBox? Margin => themeStyle.Margin;

        public LayoutBox? Padding => themeStyle.Padding;

        public LayoutStyle Layout => themeStyle.Layout;

        public int Specificity => match.Specificity;

        public IReadOnlyCollection<string> PseudoClasses => match.PseudoClasses;

        public override string ToString()
        {
            return $"Theme Style: {BuildDescription()}";
        }

        private string BuildDescription()
        {
            return match.ToString();
        }
    }
}
