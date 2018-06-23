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
    public class ContentMenuItem : Widget
    {
        private string text;
        private IContentLayout content;
        private Size maxSize;
        private ButtonPress<MenuInputButton> buttonPress = new ButtonPress<MenuInputButton>();

        public ContentMenuItem()
        {
            Display.IndicatorType = IndicatorType.Standard;
            Display.Styles.ActiveStyleChanged += (sender, e) =>
            {
                content = null;
            };

            buttonPress.Press += button =>
            {
                if (button == MenuInputButton.Accept)
                {
                    PressAccept?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        public override bool CanHaveFocus => true;

        /// <summary>
        /// Event raised when the player presses the accept button on this menu item.
        /// </summary>
        public event EventHandler PressAccept;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                content = null;
            }
        }

        public ContentLayoutOptions ContentLayoutOptions { get; set; } = new ContentLayoutOptions();
        
        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            Display.Region.Size.ParentMaxSize = maxSize;

            RefreshContent(renderContext);

            return content?.Size ?? Size.Empty;
        }

        public override void Draw(IWidgetRenderContext renderContext, Point offset)
        {
            RefreshContent(renderContext);

            content.Draw(offset.ToVector2(), renderContext.SpriteBatch);
        }
        
        public override void ProcessEvent(WidgetEventArgs inputEventArgs)
        {
            if (inputEventArgs.Button == MenuInputButton.Accept)
            {
                if (inputEventArgs.EventType == WidgetEventType.ButtonDown)
                {
                    buttonPress.ButtonDown(inputEventArgs.Button);

                    inputEventArgs.Handled = true;
                }
                else if (inputEventArgs.EventType == WidgetEventType.ButtonUp)
                {
                    if (buttonPress.IsButtonDown(inputEventArgs.Button))
                        inputEventArgs.Handled = true;

                    buttonPress.ButtonUp(inputEventArgs.Button);
                }
            }
        }

        public override void Update(IWidgetRenderContext renderContext)
        {
            RefreshContent(renderContext);
        }

        private void RefreshContent(IWidgetRenderContext renderContext)
        {
            if (text != null && (content == null 
                                 || ContentLayoutOptions.MaxWidth != Display.Region.Size.ParentMaxWidth
                                 || ContentLayoutOptions.Font != Display.Font))
            {
                ContentLayoutOptions.Font = Display.Font;
                ContentLayoutOptions.MaxWidth = Display.Region.Size.ParentMaxSize.Width;

                content = renderContext.CreateContentLayout(text, ContentLayoutOptions);
            }
        }
    }
}
