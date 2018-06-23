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
using System.Text;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Styling.Themes
{
    [Singleton]
    public class ThemeStyler : IStyleConfigurator
    {
        private readonly StyleConfiguratorState state;

        public ThemeStyler(IFontProvider fontProvider, IThemeCollection themes)
        {
            state = new StyleConfiguratorState(themes) { Fonts = fontProvider };
        }

        public void ApplyStyle(IWidget widget, string defaultTheme)
        {
            state.DefaultTheme = defaultTheme;

            ApplyStyleCore(widget);
        }

        private void ApplyStyleCore(IWidget widget)
        { 
            try
            {
                state.PushTheme(widget.Display.Theme);

                state.CurrentTheme.ApplyTo(widget, state);

                if (widget.Children != null)
                {
                    try
                    {
                        state.PushParent(widget);

                        foreach (var child in widget.Children)
                        {
                            ApplyStyleCore(child);
                        }
                    }
                    finally
                    {
                        state.PopParent();
                    }
                }
            }
            finally
            {
                state.PopTheme();
            }
        }
    }

    public class StyleConfiguratorState : IWidgetStackState
    {
        private IThemeCollection themes;

        private Stack<string> widgetThemes = new Stack<string>();
        private List<IWidget> parents = new List<IWidget>();

        private IFontProvider fonts;

        // hacky..
        private int emptyThemes = 0;

        public StyleConfiguratorState(IThemeCollection themes)
        {
            this.themes = themes;
        }

        public IFontProvider Fonts
        {
            get => fonts;
            set
            {
                fonts = value;

                foreach (var theme in themes.Values)
                {
                    theme.Fonts = value;
                }
            }
        }

        public string CurrentThemeKey
        {
            get
            {

                if (widgetThemes.Count == 0)
                    return DefaultTheme ?? "default";

                return widgetThemes.Peek();
            }
        }

        public ITheme CurrentTheme => themes[CurrentThemeKey];

        public void PushTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                emptyThemes++;
                return;
            }
            if (!themes.ContainsKey(theme))
            {
                emptyThemes++;
                return;
            }

            widgetThemes.Push(theme);
        }

        public void PopTheme()
        {
            if (emptyThemes > 0)
            {
                emptyThemes--;
                return;
            }

            widgetThemes.Pop();
        }


        public IWidget Parent => parents.Count > 0 ? parents[parents.Count - 1] : null;

        public string DefaultTheme { get; set; }

        public void PushParent(IWidget widget)
        {
            parents.Add(widget);
        }

        public void PopParent()
        {
            parents.RemoveAt(parents.Count - 1);
        }

    }
}
