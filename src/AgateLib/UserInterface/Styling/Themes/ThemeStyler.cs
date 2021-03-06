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

using System.Collections.Generic;

namespace AgateLib.UserInterface.Styling.Themes
{
    [Singleton]
    public class ThemeStyler : IStyleConfigurator
    {
        private readonly IThemeCollection themes;

        public ThemeStyler(IThemeCollection themes)
        {
            this.themes = themes;
        }

        public string DefaultThemeKey
        {
            get => themes.DefaultThemeKey;
            set => themes.DefaultThemeKey = value;
        }

        public IEnumerable<string> AvailableThemes => themes.Keys;

        /// <summary>
        /// Applys the styling to the whole visual tree starting from this element.
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="theme">Optional override of the theme</param>
        public void Apply(IRenderElement rootElement, string themeOverride = null)
        {
            string startTheme = themeOverride ?? DefaultThemeKey;

            RenderElementStack parentStack = new RenderElementStack();

            void ApplyRecurse(IRenderElement element, ITheme theme)
            {
                theme = ThemeOf(element, theme);

                theme.Apply(element, parentStack);

                if (element.Children == null)
                {
                    return;
                }

                try
                {
                    parentStack.PushParent(element);

                    foreach (var child in element.Children)
                    {
                        ApplyRecurse(child, theme);
                    }
                }
                finally
                {
                    parentStack.PopParent();
                }
            }

            var initialTheme = ThemeOf(rootElement, themes[startTheme]);

            ApplyRecurse(rootElement, initialTheme);
        }

        public ITheme Theme(string themeKey)
        {
            return themes[themeKey];
        }

        /// <summary>
        /// Returns `theme` or the theme specified for that element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        private ITheme ThemeOf(IRenderElement element, ITheme theme)
        {
            if (!string.IsNullOrWhiteSpace(element.Props.Theme)
                && themes.ContainsKey(element.Props.Theme))
            {
                return themes[element.Props.Theme];
            }

            return theme;
        }
    }

    public class RenderElementStack
    {
        private List<IRenderElement> parentStack = new List<IRenderElement>();

        public IReadOnlyList<IRenderElement> ParentStack => parentStack;

        public int Count => parentStack.Count;

        public void PushParent(IRenderElement parent)
        {
            parentStack.Add(parent);
        }

        public IRenderElement PopParent()
        {
            var item = parentStack[parentStack.Count - 1];
            parentStack.RemoveAt(parentStack.Count - 1);

            return item;
        }
    }
}
