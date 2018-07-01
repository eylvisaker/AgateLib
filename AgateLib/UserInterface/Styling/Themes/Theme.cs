﻿//
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
using AgateLib.UserInterface.Styling.Themes.Model;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Styling.Themes
{
    public interface ITheme
    {
        IFontProvider Fonts { get; set; }

        void Apply(IRenderElement widget, RenderElementStack parentStack);
    }

    [Obsolete]
    public interface IWidgetStackState
    {
        IRenderElement Parent { get; }
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
                    Selector = "workspace",
                    Padding = LayoutBox.SameAllAround(30),
                    Flex = new FlexStyle
                    {
                        AlignItems = AlignItems.Center,
                    }
                },

                new ThemeStyle
                {
                    Selector = "workspace > *",
                    Background = new BackgroundStyle { Color = Color.Black, },
                    Font = new FontStyleProperties { Color = Color.White, },
                    Animation = new AnimationStyle
                    {
                        TransitionIn = "fade-in",
                        TransitionOut = "fade-out"
                    }
                },

                new ThemeStyle
                {
                    Selector = "menu",
                    Flex = new FlexStyle { AlignItems = AlignItems.Stretch },
                },

                new ThemeStyle
                {
                    Selector = "menuitem",
                    Padding = LayoutBox.SameAllAround(8),
                },

                new ThemeStyle
                {
                    Selector = "menuitem:selected",
                    Background = new BackgroundStyle { Color = Color.White },
                    Font = new FontStyleProperties { Color = Color.Black },
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

        public void Apply(IRenderElement element, RenderElementStack stack)
        {
            element.Display.ElementStyles.Clear();

            element.Display.ElementStyles.AddRange(
                data.Where(x => x.MatchExecutor.Matches(element, stack))
                    .Select(x => x.ToElementStyle()));
        }

        private void InitializeSelectors()
        {
            foreach (var item in data)
            {
                item.MatchExecutor = new CssWidgetSelector(item.Selector);
            }
        }
    }
}
