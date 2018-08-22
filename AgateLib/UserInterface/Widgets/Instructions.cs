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
using AgateLib.Display;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.UserInterface
{
    public interface IInstructions
    {
        void Draw(IUserInterfaceRenderContext renderContext);

        void Set(Buttons button, string text);

        void Clear(Buttons button);

        Font Font { get; set; }
    }

    public class Instructions : IInstructions
    {
        class InstructionItem
        {
            public bool Visible { get; set; } = true;

            public string Text { get; set; }

            public IContentLayout Content { get; set; }

            public IContentLayout ButtonContent { get; set; }
        }

        private readonly Dictionary<Buttons, InstructionItem> items 
            = new Dictionary<Buttons, InstructionItem>();

        private readonly ContentLayoutOptions contentLayoutOptions 
            = new ContentLayoutOptions();

        public Font Font { get; set; }

        public int Margin { get; set; } = 12;

        public void Clear(Buttons button)
        {
            if (items.TryGetValue(button, out var item))
            {
                item.Visible = false;
            }
        }

        public void Draw(IUserInterfaceRenderContext renderContext)
        {
            contentLayoutOptions.Font = Font;
            contentLayoutOptions.MaxWidth = renderContext.Area.Width;

            var validItems = items.Where(kvp => kvp.Value.Visible
                                             && !string.IsNullOrWhiteSpace(kvp.Value.Text));

            int totalWidth = 0;
            int count = 0;

            foreach (var itemKvp in validItems)
            {
                var item = itemKvp.Value;

                InitializeContent(renderContext, itemKvp.Key, item);

                totalWidth += item.Content.Size.Width + item.ButtonContent.Size.Width;
                count++;
            }

            totalWidth += (count - 1) * Margin;

            int x = (renderContext.Area.Width - totalWidth) / 2;

            foreach (var itemKvp in validItems)
            {
                DrawContent(renderContext, itemKvp.Value.ButtonContent, ref x);
                DrawContent(renderContext, itemKvp.Value.Content, ref x);

                x += Margin;
            }
        }

        private void InitializeContent(IUserInterfaceRenderContext renderContext, Buttons key, InstructionItem item)
        {
            if (item.ButtonContent == null)
            {
                item.ButtonContent = renderContext.CreateContentLayout(
                    "{Button " + key + "} ", contentLayoutOptions, performLocalization: false);
            }

            if (item.Content == null)
            {
                item.Content = renderContext.CreateContentLayout(item.Text, contentLayoutOptions);
            }
        }
        
        private static void DrawContent(IUserInterfaceRenderContext renderContext, IContentLayout content, ref int x)
        {
            content.Draw(
                new Vector2(x, renderContext.Area.Height - content.Size.Height),
                renderContext.SpriteBatch);

            x += content.Size.Width;
        }

        public void Set(Buttons button, string text)
        {
            if (!items.TryGetValue(button, out var item))
            {
                item = new InstructionItem();
                items[button] = item;
            }

            item.Text = text;
            item.Content = null;
            item.Visible = true;
        }
    }
}
