using AgateLib.Display;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class ThemeRenderElementStyle : IRenderElementStyleProperties
    {
        private ThemeStyle themeStyle;
        private float scaling = 1;
        private readonly SelectorMatch match;

        public ThemeRenderElementStyle(ThemeStyle themeStyle, SelectorMatch match)
        {
            this.themeStyle = themeStyle;
            this.match = match;
        }

        public string FontFace => themeStyle.Font?.Family;

        public Color? TextColor => themeStyle.Font?.Color;

        public int? FontSize
        {
            get
            {
                int? result = themeStyle.Font?.Size;

                if (result != null)
                {
                    result = (int)(result.Value * Scaling);
                }

                return result;
            }
        }

        public FontStyles? FontStyle => themeStyle.Font?.Style;

        public BackgroundStyle Background => themeStyle.Background;

        public BorderStyle Border => themeStyle.Border;

        public AnimationStyle Animation => themeStyle.Animation;

        public FlexStyle Flex => themeStyle.Flex;

        public FlexItemStyle FlexItem => themeStyle.FlexItem;

        public SizeConstraints Size => themeStyle.Size;
        ISizeConstraints IRenderElementStyleProperties.Size => Size;

        public LayoutBox? Margin => themeStyle.Margin;

        public LayoutBox? Padding => themeStyle.Padding;

        public LayoutStyle Layout => themeStyle.Layout;

        public TextAlign? TextAlign => themeStyle.TextAlign;

        public Overflow? Overflow => themeStyle.Overflow;

        public int Specificity => match.Specificity;

        public IReadOnlyCollection<string> PseudoClasses => match.PseudoClasses;

        public float Scaling
        {
            get => scaling;
            set
            {
                scaling = value;
            }
        }

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
