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
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib.UserInterface
{
    public class Label : Widget<LabelProps, WidgetState>
    {
        public Label(LabelProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new LabelElement(new LabelElementProps
            {
                Text = Props.Text,
                PerformLocalization = Props.LocalizeContent,
                ReadSlowly = Props.ReadSlowly,
                AnimationComplete = Props.AnimationComplete,
            }.CopyFromWidgetProps(Props));
        }

        public override string ToString()
        {
            return $"Label: {Props.Text?.Substring(0, Math.Min(20, Props.Text.Length)) ?? "null"}";
        }
    }

    public class LabelProps : WidgetProps
    {
        public string Text { get; set; }

        /// <summary>
        /// Defaults to true. Set to false to bypass content localization.
        /// </summary>
        public bool LocalizeContent { get; set; } = true;

        /// <summary>
        /// Defaults to false. Set to true to have characters printed out one at a time.
        /// </summary>
        public bool ReadSlowly { get; set; }

        /// <summary>
        /// Callback which is executed when the animation completes.
        /// </summary>
        public UserInterfaceEventHandler AnimationComplete { get; set; }
    }

    public class LabelElement : RenderElement<LabelElementProps>
    {
        private IContentLayout content;
        private bool dirty;
        private int lastContentMaxWidth;
        private ContentLayoutOptions layoutOptions = new ContentLayoutOptions();
        private UserInterfaceEvent evt = new UserInterfaceEvent();

        public LabelElement(LabelElementProps props) : base(props)
        {
            Style.FontChanged += () => dirty = true;
        }

        protected override void OnReceiveProps()
        {
            base.OnReceiveProps();

            dirty = true;
        }

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size)
        {
        }

        public override string StyleTypeId => "label";

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
        {
            RefreshContent(renderContext, maxSize.Width);

            return content?.Size ?? Size.Empty;
        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            content?.Draw(clientArea.Location.ToVector2(), renderContext.SpriteBatch);
        }

        public override void Update(IUserInterfaceRenderContext renderContext)
        {
            content?.Update(renderContext.GameTime);
        }

        public override string ToString()
        {
            return $"label: {Props.Text?.Substring(0, Math.Min(200, Props.Text.Length)) ?? "null"}";
        }

        private void RefreshContent(IUserInterfaceRenderContext renderContext, int maxWidth)
        {
            if (!dirty && content != null)
            {
                if (maxWidth > content.Size.Width)
                    return;

                dirty = true;
            }

            if (dirty || lastContentMaxWidth != maxWidth)
            {
                if (Props.Text == null)
                {
                    content = null;
                }
                else
                {
                    layoutOptions.Font = Style.Font;
                    layoutOptions.MaxWidth = maxWidth;

                    content = renderContext.CreateContentLayout(Props.Text, layoutOptions, Props.PerformLocalization);
                    content.Options.ReadSlowly = Props.ReadSlowly;

                    content.AnimationComplete += () => Props.AnimationComplete?.Invoke(evt.Reset(this));
                }

                lastContentMaxWidth = maxWidth;
                dirty = false;
            }
        }
    }

    public class LabelElementProps : RenderElementProps
    {
        public string Text { get; set; }

        public bool PerformLocalization { get; set; }

        public bool ReadSlowly { get; set; }

        public UserInterfaceEventHandler AnimationComplete { get; set; }
    }
}
