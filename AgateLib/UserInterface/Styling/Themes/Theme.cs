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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AgateLib.Display;
using AgateLib.Quality;
using AgateLib.UserInterface.Styling.Themes.Model;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface ITheme
    {
        IFontProvider Fonts { get; set; }

        void ApplyTo(IWidget widget, IWidgetStackState state);
    }

    public interface IWidgetStackState
    {
        IWidget Parent { get; }
    }

    /// <summary>
    /// Represents a theme.
    /// </summary>
    public class Theme : ITheme
    {
        /// <summary>
        /// Creates the default theme which requires no assets.
        /// </summary>
        /// <returns></returns>
        public static Theme CreateDefaultTheme()
        {
            var data = new ThemeData
            {
                new ThemeStyle
                {
                    Pattern = "*",
                    Background = new ThemeWidgetBackground { Color = Color.Black, },
                    Font = new FontStyleProperties { Color = Color.White, },
                    Animation = new ThemeWidgetAnimation
                    {
                        TransitionIn = "fade-in",
                        TransitionOut = "fade-out"
                    }
                },

                new ThemeStyle
                {
                    Pattern = "menu.*",
                    Padding = LayoutBox.SameAllAround(8),                    
                },

                new ThemeStyle
                {
                    Pattern = "menu.*",
                    WidgetState = "selected",
                    Padding = LayoutBox.SameAllAround(8),
                    Background = new ThemeWidgetBackground { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black }
                }
            };

            return new Theme { Data = data };
        }

        public static Theme DefaultTheme { get; set; } = CreateDefaultTheme();

        private ThemeData data;
        private List<ThemeStyle> themeMatches = new List<ThemeStyle>();

        /// <summary>
        /// Initializes a Theme object with an empty ThemeData value.
        /// </summary>
        public Theme() : this(new ThemeData()) { }

        /// <summary>
        /// Initializes a Theme object.
        /// </summary>
        /// <param name="data"></param>
        public Theme(ThemeData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets or sets the data for the theme. This cannot be null.
        /// </summary>
        public ThemeData Data
        {
            get => data;
            set
            {
                data = value ?? throw new ArgumentNullException(nameof(Data));
                InitializeSelectors();
            }
        }

        public IFontProvider Fonts { get; set; }

        public void ApplyTo(IWidget widget, IWidgetStackState state)
        {
            themeMatches.Clear();
            themeMatches.AddRange(data.Where(x => x.Selector.Matches(widget, state)));

            if (themeMatches.Count == 0)
            {
                ApplyDefaultStyling(widget.Display.Style);
                return;
            }

            // TODO: sort matches
            var defaultTheme = themeMatches.FirstOrDefault(x => string.IsNullOrEmpty(x.WidgetState))
                ?? themeMatches.First();

            foreach (var theme in themeMatches)
            {
                var style = widget.Display.Styles.GetOrCreate(theme.WidgetState);

                ApplyDefaultStyling(style);

                if (theme != defaultTheme)
                {
                    ApplyStyle(style, defaultTheme);
                }

                ApplyStyle(style, theme);

                widget.Display.StyleUpdated();
            }
        }

        private void ApplyDefaultStyling(WidgetStyle style)
        {
            style.Font = new Font(Fonts.Default);
        }

        private void ApplyStyle(WidgetStyle style, ThemeStyle theme)
        {
            ApplyAnimation(style.Animation, theme.Animation);
            ApplyBackground(style.Background, theme.Background);
            ApplyBorder(style.Border, theme.Border);
            ApplyFont(style, theme.Font);
            ApplyPadding(style, theme.Padding);
            ApplyMargin(style, theme.Margin);
            ApplySize(style.Size, theme.Size);
        }

        private void ApplySize(ThemeWidgetSize style, ThemeWidgetSize theme)
        {
            if (theme == null)
                return;

            style.Min = theme.Min;
            style.Max = theme.Max;
        }

        private void ApplyAnimation(AnimationStyle style, ThemeWidgetAnimation theme)
        {
            if (theme == null)
                return;

            style.TransitionIn = theme.TransitionIn;
            style.TransitionOut = theme.TransitionOut;

            style.TransitionInTime = theme.TransitionInTime ?? 0.35f;
            style.TransitionOutTime = theme.TransitionOutTime ?? 0.35f;
        }

        private void ApplyBackground(BackgroundStyle style, ThemeWidgetBackground theme)
        {
            if (theme == null)
                return;

            style.Image = theme?.Image ?? style.Image;
            style.Color = theme?.Color ?? style.Color;
            style.Repeat = theme?.Repeat ?? style.Repeat;
            style.Clip = theme?.Clip ?? style.Clip;
            style.Position = theme?.Position ?? style.Position;
        }

        private void ApplyBorder(BorderStyle style, ThemeWidgetBorder theme)
        {
            if (theme == null)
                return;

            style.Image = theme.Image ?? style.Image;
            style.ImageSlice = theme.ImageSlice ?? style.ImageSlice;
            style.ImageScale = theme.ImageScale ?? style.ImageScale;

            style.Left = theme.Left ?? style.Left;
            style.Right = theme.Right ?? style.Right;
            style.Top = theme.Top ?? style.Top;
            style.Bottom = theme.Bottom ?? style.Bottom;
        }

        private void ApplyFont(WidgetStyle style, FontStyleProperties theme)
        {
            if (theme == null)
                return;

            Font font = style.Font;

            if (!string.IsNullOrWhiteSpace(theme.Family))
            {
                font = Fonts[theme.Family];
            }

            font = font ?? Fonts.Default;
            var currentColor = font.Color;

            style.Font = new Font(font, theme.Size ?? font.Size, theme.Style ?? font.Style)
            {
                Color = theme.Color ?? currentColor
            };
        }

        private void ApplyMargin(WidgetStyle style, LayoutBox? margin)
        {
            style.Margin = margin ?? style.Margin;
        }

        private void ApplyPadding(WidgetStyle style, LayoutBox? padding)
        {
            style.Padding = padding ?? style.Padding;
        }


        private void InitializeSelectors()
        {
            foreach (var item in data)
            {
                item.Selector = new PatternWidgetSelector(item.Pattern);
            }
        }
    }
}
