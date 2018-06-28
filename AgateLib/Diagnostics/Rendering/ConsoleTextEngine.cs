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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Display;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework.Content;

namespace AgateLib.Diagnostics.Rendering
{
    public interface IConsoleTextEngine
    {
        Font Font { get; }

        IContentLayout LayoutContent(string text, int maxWidth);
    }

    [Singleton]
    public class ConsoleTextEngine : IConsoleTextEngine
    {
        private class ConsoleFontProvider : IFontProvider
        {
            public ConsoleFontProvider(IServiceProvider serviceProvider)
            {
                var contentManager = new ContentManager(serviceProvider, "Content/AgateLib");
                var contentProvider = new ContentProvider(contentManager);

                Default = Font.Load(contentProvider, "AgateMono");
            }

            public ConsoleFontProvider(Font font)
            {
                Default = font;
            }

            public Font this[string fontFace] => Default;

            public Font Default { get; }

            public IEnumerator<Font> GetEnumerator()
            {
                return new List<Font> { Default }.GetEnumerator();
            }

            public Font GetOrDefault(string fontFace) => Default;

            public bool HasFont(string fontFace)
            {
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly IFontProvider consoleFontProvider;
        private readonly IContentLayoutEngine contentLayoutEngine;

        public ConsoleTextEngine(IFontProvider fontProvider)
        {
            consoleFontProvider = fontProvider;
            contentLayoutEngine = new ContentLayoutEngine(consoleFontProvider);
        }

        public IContentLayout LayoutContent(string text, int maxWidth)
        {
            return contentLayoutEngine.LayoutContent(text, maxWidth);
        }

        public Font Font => consoleFontProvider.Default;

    }
}
