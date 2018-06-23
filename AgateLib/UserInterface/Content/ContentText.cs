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

using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Content
{
    public class ContentText : IContentLayoutItem
    {
        Vector2 destination;

        public ContentText(string text, Font font, Vector2 dest)
        {
            Text = text;
            Font = font;
            destination = dest;

            Size = Font.MeasureString(text);
        }

        public override string ToString()
        {
            return $"ContentText:{Text}";
        }

        public int Count => Text.Length;

        public Vector2 Location => destination;

        public string Text { get; }

        public Font Font { get; }

        public Size Size { get; }

        public void Draw(Vector2 origin, ContentRenderContext renderContext)
        {
            Vector2 destPt = destination + origin;

            if (renderContext.RemainingItemsToDisplay > Text.Length)
            {
                renderContext.DrawText(Font, destPt, Text);
                renderContext.ItemsDisplayed += Text.Length;
            }
            else
            {
                int count = renderContext.RemainingItemsToDisplay;

                renderContext.DrawText(Font, destPt, Text.Substring(0, count));

                renderContext.ItemsDisplayed += count;
            }

        }

        public void Update(GameTime time)
        {
        }
    }
}