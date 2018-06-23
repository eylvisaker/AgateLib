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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface.Content
{
    public class ContentRenderContext
    {
        float charsToDisplay;

        public ContentRenderOptions Options { get; } = new ContentRenderOptions();

        /// <summary>
        /// The number of items already drawn.
        /// </summary>
		public int ItemsDisplayed { get; set; }

        /// <summary>
        /// The maximum number of items to draw in this draw frame.
        /// </summary>
		public int MaxItemsToDisplay { get; set; } = int.MaxValue;

        public int RemainingItemsToDisplay => MaxItemsToDisplay - ItemsDisplayed;

        public bool Complete => ItemsDisplayed >= MaxItemsToDisplay;

        public SpriteBatch SpriteBatch { get; set; }

        public void Reset()
        {
            ItemsDisplayed = 0;

            if (Options.ReadSlowly)
            {
                MaxItemsToDisplay = (int)charsToDisplay;
            }
            else
            {
                MaxItemsToDisplay = int.MaxValue;
            }
        }

        internal void Update(GameTime time)
        {
            charsToDisplay += (float)time.ElapsedGameTime.TotalSeconds * Options.SlowReadRate;
        }

        public void Draw(Texture2D image, Rectangle destRect, Color color)
        {
            SpriteBatch.Draw(image, destRect, color);
        }

        public void DrawText(Font font, Vector2 destPt, string text)
        {
            font.DrawText(SpriteBatch, destPt, text);
        }
    }
}