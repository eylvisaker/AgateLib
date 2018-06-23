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
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface.Content
{
    public interface IContentLayout
    {
        Size Size { get; }

        /// <summary>
        /// Event raised when the animation is completed.
        /// </summary>
        event Action AnimationComplete;

        bool AnimationCompleted { get; }

        ContentRenderOptions Options { get; }

        void Update(GameTime time);

        void Draw(Vector2 dest, SpriteBatch spriteBatch = null);

        void Reset();
    }

    public class ContentLayout : IContentLayout
    {
        private readonly List<IContentLayoutItem> items;

        private ContentRenderContext renderContext = new ContentRenderContext();
        private bool animationCompleted;

        public ContentLayout(IEnumerable<IContentLayoutItem> items)
        {
            this.items = items.ToList();

            var width = (int)this.items.Select(x => x.Location.X + x.Size.Width).Max();
            var height = (int)this.items.Select(x => x.Location.Y + x.Size.Height).Max();

            Size = new Size(width, height);
        }

        public event Action AnimationComplete;

        public ContentRenderOptions Options => renderContext.Options;

        public bool AnimationCompleted
        {
            get => animationCompleted;
            private set
            {
                if (animationCompleted == value)
                    return;

                animationCompleted = value;

                if (value)
                    AnimationComplete?.Invoke();
            }
        }

        public IReadOnlyList<IContentLayoutItem> Items => items;

        public Size Size { get; }

        public void Draw(Vector2 dest, SpriteBatch spriteBatch = null)
        {
            renderContext.Reset();
            renderContext.SpriteBatch = spriteBatch;

            foreach (var item in items)
            {
                if (renderContext.Complete)
                    return;

                item.Draw(dest, renderContext);
            }
        }

        public void Reset()
        {
            renderContext.Reset();
        }

        public void Update(GameTime time)
        {
            renderContext.Update(time);

            foreach (var item in items)
                item.Update(time);

            AnimationCompleted = renderContext.ItemsDisplayed >= items.Sum(x => x.Count);
        }
    }
}
