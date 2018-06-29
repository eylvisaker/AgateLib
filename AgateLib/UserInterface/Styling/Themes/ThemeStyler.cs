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
using System.Text;
using AgateLib.Display;
using AgateLib.UserInterface.Widgets;

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

        public void Apply(IRenderElement rootWidget, string defaultTheme)
        {
            RenderElementStack parentStack = new RenderElementStack();

            void ApplyRecurse(IRenderElement widget, ITheme theme)
            {
                theme = ThemeOf(widget, theme);

                theme.Apply(widget, parentStack);

                if (widget.Children == null)
                    return;

                try
                {
                    parentStack.PushParent(widget);

                    foreach (var child in widget.Children)
                    {
                        ApplyRecurse(child, theme);
                    }
                }
                finally
                {
                    parentStack.PopParent();
                }
            }

            var initialTheme = ThemeOf(rootWidget, themes[defaultTheme]);

            ApplyRecurse(rootWidget, initialTheme);
        }

        private ITheme ThemeOf(IRenderElement widget, ITheme theme)
        {
            if (!string.IsNullOrWhiteSpace(widget.Display.Theme)
                && themes.ContainsKey(widget.Display.Theme))
            {
                return themes[widget.Display.Theme];
            }

            return theme;
        }
    }

    public class RenderElementStack
    {
        List<IRenderElement> parentStack = new List<IRenderElement>();

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
