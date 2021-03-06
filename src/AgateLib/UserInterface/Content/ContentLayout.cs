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
using AgateLib.UserInterface.Content.LayoutItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Content
{
    public interface IContentLayout
    {
        /// <summary>
        /// Event raised when the animation is completed.
        /// </summary>
        event Action AnimationComplete;

        /// <summary>
        /// Gets or sets the maximum width of the content layout.
        /// </summary>
        int? MaxWidth { get; set; }

        /// <summary>
        /// Gets the default height of a single line of text for the font used
        /// in the content layout.
        /// </summary>
        int LineHeight { get; }

        /// <summary>
        /// Gets or sets the alignment of the text when drawn.
        /// </summary>
        TextAlign TextAlign { get; set; }

        bool AnimationCompleted { get; }

        /// <summary>
        /// Gets or sets the render options.
        /// </summary>
        ContentRenderOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the render context.
        /// </summary>
        ContentRenderContext RenderContext { get; set; }

        /// <summary>
        /// Gets the size of the content given the current maximum size.
        /// </summary>
        Size Size { get; }

        void Update(GameTime time);

        void Draw(Vector2 dest, SpriteBatch spriteBatch = null);

        /// <summary>
        /// Restarts the animation.
        /// </summary>
        void RestartAnimation();

        void DoLayout();
    }

    public class ContentLayout : IContentLayout
    {
        private readonly List<IContentLayoutItem> items;
        private readonly int defaultLineHeight;

        private ContentRenderContext renderContext = new ContentRenderContext();
        private bool animationCompleted;

        private int? maxWidth;
        private bool layoutDirty = true;

        private TextAlign textAlign;

        public ContentLayout(IEnumerable<IContentLayoutItem> items, int lineHeight)
        {
            this.items = items.ToList();
            this.defaultLineHeight = lineHeight;
        }

        public event Action AnimationComplete;

        public int LineHeight => defaultLineHeight;

        public ContentRenderContext RenderContext
        {
            get => renderContext;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(RenderContext));
                }

                renderContext = value;
            }
        }

        public ContentRenderOptions Options
        {
            get => renderContext.Options;
            set => renderContext.Options = value;
        }

        public bool AnimationCompleted
        {
            get => animationCompleted;
            private set
            {
                if (animationCompleted == value)
                {
                    return;
                }

                animationCompleted = value;

                if (value)
                {
                    AnimationComplete?.Invoke();
                }
            }
        }

        public IReadOnlyList<IContentLayoutItem> Items => items;

        public int? MaxWidth
        {
            get => maxWidth;
            set
            {
                if (value == maxWidth)
                {
                    return;
                }

                maxWidth = value;

                layoutDirty = true;
                DoLayout();
            }
        }

        /// <summary>
        /// How to align the text when laid out.
        /// </summary>
        public TextAlign TextAlign
        {
            get => textAlign;
            set
            {
                if (value == textAlign)
                {
                    return;
                }

                textAlign = value;

                layoutDirty = true;
                DoLayout();
            }
        }

        public Size Size { get; private set; }

        public void Draw(Vector2 dest, SpriteBatch spriteBatch = null)
        {
            if (layoutDirty)
            {
                DoLayout();
            }

            renderContext.Reset();
            renderContext.SpriteBatch = spriteBatch;

            foreach (var item in items)
            {
                if (renderContext.Complete)
                {
                    return;
                }

                item.Draw(dest, renderContext);
            }
        }

        public void RestartAnimation()
        {
            renderContext.RestartAnimation();
            AnimationCompleted = false;
        }

        public void Update(GameTime time)
        {
            renderContext.Update(time);

            foreach (var item in items)
            {
                item.Update(time);
            }

            AnimationCompleted = renderContext.ItemsDisplayed >= items.Sum(x => x.Count);
        }

        public void DoLayout()
        {
            Point pt = Point.Zero;
            Size resultSize = new Size();
            int lineHeight = defaultLineHeight;
            int lineStartIndex = 0;

            if (items.Count == 0)
            {
                Size = resultSize;
                layoutDirty = false;
                return;
            }

            int myMaxWidth = CalcMaxWidth();

            void ApplyAlignment(int start, int end)
            {
                if (end < start)
                {
                    return;
                }

                IEnumerable<IContentLayoutItem> affectedItems
                    = items.Skip(start).Take(end - start + 1);

                int width = items[end].Bounds.Right
                          - items[start].Position.X;

                int height = affectedItems.Max(x => x.Size.Height);

                int shiftRight = 0;

                if (TextAlign == TextAlign.Right)
                {
                    shiftRight = myMaxWidth - width;
                }
                else if (TextAlign == TextAlign.Center)
                {
                    shiftRight = (myMaxWidth - width) / 2;
                }

                if (shiftRight > 0)
                {
                    foreach (var item in affectedItems)
                    {
                        item.Position = new Point(item.Position.X + shiftRight,
                                                  item.Position.Y);
                    }
                }
            }

            void NewLine()
            {
                pt.X = 0;
                pt.Y += lineHeight;
                lineHeight = defaultLineHeight;
            }

            for (int index = 0; index < items.Count; index++)
            {
                IContentLayoutItem item = items[index];

                if (pt.X > 0 && pt.X + item.Size.Width > maxWidth)
                {
                    NewLine();
                    ApplyAlignment(lineStartIndex, index - 1);
                    lineStartIndex = index;
                }

                item.Position = pt;
                lineHeight = Math.Max(lineHeight, item.Size.Height);

                resultSize.Width = Math.Max(resultSize.Width,
                                            item.Position.X + item.Size.Width);

                resultSize.Height = Math.Max(resultSize.Height,
                                             item.Position.Y + item.Size.Height);

                if (item.NewLinesAfter > 0)
                {
                    NewLine();
                    ApplyAlignment(lineStartIndex, index);
                    lineStartIndex = index + 1;

                    if (item.NewLinesAfter > 1)
                    {
                        pt.Y += (item.NewLinesAfter - 1) * defaultLineHeight;
                    }
                }
                else
                {
                    pt.X += item.Size.Width + item.ExtraWhiteSpace;
                }
            }

            ApplyAlignment(lineStartIndex, items.Count - 1);

            Size = resultSize;
            layoutDirty = false;
        }

        private int CalcMaxWidth()
        {
            if (MaxWidth.HasValue)
            {
                return MaxWidth.Value;
            }

            int currentLineWidth = 0;
            int maxLineWidth = 0;

            foreach (IContentLayoutItem item in items)
            {
                currentLineWidth += item.Bounds.Width;

                if (item.NewLinesAfter > 0)
                {
                    maxLineWidth = Math.Max(maxLineWidth, currentLineWidth);
                    currentLineWidth = 0;
                }
                else
                {
                    currentLineWidth += item.ExtraWhiteSpace;
                }
            }

            maxLineWidth = Math.Max(maxLineWidth, currentLineWidth);

            return maxLineWidth;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            foreach (var item in items)
            {
                b.Append(item.ToString());
                b.Append(" ");
            }

            return b.ToString().Trim();
        }
    }
}
