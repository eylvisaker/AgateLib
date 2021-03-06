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

using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Content.LayoutItems
{
    public interface IContentLayoutItem
    {
        /// <summary>
        /// Extra white space after the item in pixels.
        /// This white space may be cut off by the layout engine if 
        /// it inserts a new line after this item.
        /// </summary>
        int ExtraWhiteSpace { get; set; }

        /// <summary>
        /// The number of new lines that immediately follow this item.
        /// </summary>
        int NewLinesAfter { get; set; }

        Point Position { get; set; }

        Size Size { get; }

        Rectangle Bounds { get; }

        /// <summary>
        /// The number of items to be drawn by this item.
        /// For text, this is the number of characters.
        /// For an image, this is usually 1.
        /// </summary>
        int Count { get; }

        void Update(GameTime time);

        void Draw(Vector2 origin, ContentRenderContext renderContext);
    }

    public abstract class ContentLayoutItem : IContentLayoutItem
    {
        public int NewLinesAfter { get; set; }

        public int ExtraWhiteSpace { get; set; }

        public Point Position { get; set; }

        public Rectangle Bounds => new Rectangle(Position, Size);

        public abstract Size Size { get; }

        public abstract int Count { get; }

        public abstract void Draw(Vector2 origin, ContentRenderContext renderContext);

        public virtual void Update(GameTime time)
        { }
    }
}