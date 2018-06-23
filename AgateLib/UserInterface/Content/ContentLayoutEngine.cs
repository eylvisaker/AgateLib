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
using Microsoft.Xna.Framework.Graphics;
using AgateLib.Display;
using AgateLib.UserInterface.Content.Commands;
using AgateLib.UserInterface.Styling.Themes.Model;

namespace AgateLib.UserInterface.Content
{
    public interface IContentLayoutEngine
    {
        IContentLayout LayoutContent(string text, ContentLayoutOptions layoutOptions);

        void AddCommand(string name, IContentCommand command);
    }

    [Singleton]
    public class ContentLayoutEngine : IContentLayoutEngine
    {
        private readonly IFontProvider fonts;
        private readonly Tokenizer tokenizer = new Tokenizer();

        Dictionary<string, IContentCommand> commands
            = new Dictionary<string, IContentCommand>(StringComparer.OrdinalIgnoreCase);

        public ContentLayoutEngine(IFontProvider fonts)
        {
            this.fonts = fonts;

            commands.Add("color", new SetTextColor());
        }

        public IContentLayout LayoutContent(string text, ContentLayoutOptions layoutOptions)
        {
            layoutOptions.Font = layoutOptions.Font ?? LookupFont(layoutOptions.FontLookup);

            var tokens = tokenizer.Tokenize(text);
            var context = new LayoutContext(tokens, layoutOptions);

            while (context.AnyTokensLeft)
            {
                ProcessToken(context, context.ReadNextToken());
            }

            return context.Layout;
        }

        private Font LookupFont(FontStyleProperties fontStyle)
        {
            if (fontStyle == null)
                return new Font(fonts.Default);

            var font = fonts[fontStyle.Family];

            font = new Font(font ?? fonts.Default);

            font.Size = fontStyle.Size ?? font.Size;
            font.Style = fontStyle.Style ?? font.Style;

            return font;
        }

        private void ProcessToken(LayoutContext context, string token)
        {
            if (IsCommandToken(token))
            {
                ProcessCommand(context, StripCommandChars(token));
            }
            else
            {
                context.AppendText(token);
            }
        }

        private void ProcessCommand(LayoutContext context, string token)
        {
            (string command, string arg) = SplitCommand(token);

            if (commands.TryGetValue(command, out IContentCommand contentCommand))
            {
                contentCommand.Execute(context, arg);
            }
        }

        (string command, string arg) SplitCommand(string token)
        {
            int space = token.IndexOf(' ');
            if (space == -1)
                space = token.Length;

            var command = token.Substring(0, space);
            var arg = token.Substring(space).Trim();

            return (command, arg);
        }

        private string StripCommandChars(string token)
        {
            return token.Substring(1, token.Length - 2);
        }

        private bool IsCommandToken(string token)
        {
            if (token == null)
                return false;

            return token.StartsWith("{") && token.EndsWith("}");
        }

        public void AddCommand(string name, IContentCommand command)
        {
            commands.Add(name, command);
        }
    }


    public static class ContentLayoutEngineExtensions
    {
        public static IContentLayout LayoutContent(this IContentLayoutEngine engine, string text, int maxWidth = int.MaxValue, FontStyleProperties font = null)
        {
            return engine.LayoutContent(text, new ContentLayoutOptions
            {
                MaxWidth = maxWidth,
                FontLookup = font,
            });
        }
    }
}
