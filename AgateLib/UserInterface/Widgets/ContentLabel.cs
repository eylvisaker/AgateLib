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
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using AgateLib.UserInterface.Content;

namespace AgateLib.UserInterface.Widgets
{
    public class ContentLabel : Widget
    {
        private readonly IContentLayoutEngine layoutEngine;
        private readonly ITextRepository textRepo;

        private string text;
        private IContentLayout content;
        private bool textDirty;
        private int lastContentMaxWidth;
        private bool readSlowly;

        public ContentLabel(IContentLayoutEngine layoutEngine, ITextRepository textRepo)
        {
            this.layoutEngine = layoutEngine;
            this.textRepo = textRepo;
        }

        public event Action AnimationComplete;

        public ContentLayoutOptions ContentLayoutOptions { get; set; }
            = new ContentLayoutOptions();

        public ContentRenderOptions ContentRenderOptions => content?.Options;

        /// <summary>
        /// Gets or sets the text for the label.
        /// If the text starts with a slash character "/" then the text repository will
        /// be used to lookup the localized text value.
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                textDirty = true;
            }
        }

        public bool ReadSlowly
        {
            get => readSlowly;
            set
            {
                readSlowly = value;

                if (content != null)
                    content.Options.ReadSlowly = value;
            }
        }

        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            RefreshContent(maxSize.Width);

            return content?.Size ?? Size.Empty;
        }

        public override void Draw(IWidgetRenderContext renderContext, Point offset)
        {
            content?.Draw(offset.ToVector2(), renderContext.SpriteBatch);
        }

        public override void Update(IWidgetRenderContext renderContext)
        {
            RefreshContent(Display.Region.ContentRect.Width);

            content?.Update(renderContext.GameTime);
        }

        private void RefreshContent(int maxWidth)
        {
            if (content != null && textDirty == false)
            {
                if (maxWidth > content.Size.Width)
                    return;
            }

            if (textDirty || lastContentMaxWidth != maxWidth)
            {
                if (text == null)
                {
                    content = null;
                }
                else
                {
                    ContentLayoutOptions.Font = Display.Font;

                    var displayText = text;

                    if (displayText.StartsWith("/"))
                    {
                        displayText = textRepo[displayText.Substring(1)];
                    }

                    ContentLayoutOptions.MaxWidth = maxWidth;

                    content = layoutEngine.LayoutContent(displayText, ContentLayoutOptions);
                    content.Options.ReadSlowly = ReadSlowly;

                    content.AnimationComplete += () => AnimationComplete?.Invoke();
                }

                lastContentMaxWidth = maxWidth;
                textDirty = false;
            }
        }
    }
}
