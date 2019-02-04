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
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib.UserInterface
{
    public class Label : Widget<LabelProps>
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
            RefreshContent(renderContext);

            content.MaxWidth = size.Width;
        }

        public override string StyleTypeId => "label";

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext,
                                                  Size maxSize)
        {
            RefreshContent(renderContext);

            if (content != null)
            {
                content.MaxWidth = maxSize.Width;

                return content.Size;
            }

            return Size.Empty;
        }

        public override Size CalcMinContentSize(int? widthConstraint, int? heightConstraint)
        {
            if (content == null)
                return base.CalcMinContentSize(widthConstraint, heightConstraint);

            if (widthConstraint != null)
            {
                content.MaxWidth = widthConstraint.Value;

                content.DoLayout();

                return content.Size;
            }

            return new Size(1, Style.Font.FontHeight);
        }

        public override void Draw(IUserInterfaceRenderContext renderContext,
                                  Rectangle clientArea)
        {
            if (content != null)
            {
                renderContext.Draw(content, clientArea);
            }
        }

        public override void Update(IUserInterfaceRenderContext renderContext)
        {
            RefreshContent(renderContext);

            content.TextAlign = Style.TextAlign;
            content.Options.ReadSlowly = Props.ReadSlowly;

            content?.Update(renderContext.GameTime);
        }

        public override string ToString()
        {
            return $"label: {Props.Text?.Substring(0, Math.Min(200, Props.Text.Length)) ?? "null"}";
        }

        private void RefreshContent(IUserInterfaceRenderContext renderContext)
        {
            if (Style.Font == null)
                return;

            bool needsRefresh = dirty;

            needsRefresh |= content == null;
            needsRefresh |= layoutOptions.Font?.Name != Style.Font.Name;
            needsRefresh |= layoutOptions.Font?.Size != Style.Font.Size;
            needsRefresh |= layoutOptions.Font?.Color != Style.Font.Color;
            needsRefresh |= layoutOptions.Font?.Style != Style.Font.Style;

            if (!needsRefresh)
                return;

            layoutOptions.Font = new Font(Style.Font);

            content = renderContext.CreateContentLayout(Props.Text, layoutOptions, Props.PerformLocalization);

            content.AnimationComplete += () => Props.AnimationComplete?.Invoke(evt.Reset(this));

            dirty = false;
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
