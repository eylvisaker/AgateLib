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

using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface.Content.LayoutItems
{
    /// <summary>
    /// Draws an image as part of the content.
    /// </summary>
	public class TextureLayoutItem : ContentLayoutItem
    {
        private readonly Texture2D image;

        public TextureLayoutItem(Texture2D image, Rectangle sourceRect)
        {
            this.image = image;

            SourceRect = sourceRect;
        }

        public override int Count => 1;

        public override Size Size => SourceRect.Size;

        public Rectangle SourceRect { get; }

        public override void Draw(Vector2 origin, ContentRenderContext renderContext)
        {
            Vector2 drawPt = origin + Position.ToVector2();

            var destRect = new Rectangle(drawPt.ToPoint(), Size);

            renderContext.Draw(image, destRect, SourceRect, Color.White);

            renderContext.ItemsDisplayed++;
        }

        public override string ToString() => "{Image}";
    }
}