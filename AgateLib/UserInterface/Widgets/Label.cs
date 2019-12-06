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
        public Label(string text) : base(new LabelProps { Text = text }) { }

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
                ResetReading = Props.ResetReading,
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

        /// <summary>
        /// If true, the slow reading counter will be reset if the text changes.
        /// </summary>
        public bool ResetReading { get; set; }
    }

    public class LabelElement : RenderElement<LabelElementProps, LabelElementState>
    {
        class LabelAnimationState : ILabelAnimationState
        {
            private LabelElement label;

            public LabelAnimationState(LabelElement labelElement)
            {
                this.label = labelElement;
            }

            public float SlowReadRate
            {
                get => label.content?.Options.SlowReadRate ?? 0f;
                set
                {
                    if (label.content == null)
                        return;

                    label.content.Options.SlowReadRate = value;
                }
            }

            public bool IsComplete => label.content.AnimationCompleted;

            public void Restart()
            {
                label.content.RestartAnimation();
            }
        }

        private UserInterfaceEvent evt = new UserInterfaceEvent();
        private IContentLayout content;
        private string lastText;

        public LabelElement(LabelElementProps props) : base(props)
        {
            SetState(new LabelElementState());

            Style.FontChanged += () => State.Dirty = true;

            AnimationState = new LabelAnimationState(this);
        }

        public ILabelAnimationState AnimationState { get; }

        private bool Dirty
        {
            get => State.Dirty;
            set => State.Dirty = value;
        }

        private ContentLayoutOptions LayoutOptions => State.LayoutOptions;


        protected override void OnReceiveProps()
        {
            base.OnReceiveProps();

            Dirty = true;

            if (Props.ResetReading && Props.Text != lastText)
            {
                content.RestartAnimation();
            }

            lastText = Props.Text;
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
        {
            RefreshContent(layoutContext);

            content.MaxWidth = size.Width;
        }

        public override string StyleTypeId => "label";

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext,
                                                  Size maxSize)
        {
            RefreshContent(layoutContext);

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
                renderContext.Canvas.Draw(content, clientArea.Location.ToVector2());
            }
        }

        public override void Update(IUserInterfaceRenderContext renderContext)
        {
            RefreshContent(renderContext);

            content.TextAlign = Style.TextAlign;
            content.Options.ReadSlowly = Props.ReadSlowly;

            content?.Update(renderContext.GameTime);

            base.Update(renderContext);
        }

        public override string ToString()
        {
            return $"label: {Props.Text?.Substring(0, Math.Min(200, Props.Text.Length)) ?? "null"}";
        }

        private void RefreshContent(IUserInterfaceLayoutContext layoutContext)
        {
            if (Style.Font == null)
                return;

            bool needsRefresh = Dirty;

            needsRefresh |= content == null;
            needsRefresh |= LayoutOptions.Font?.Name != Style.Font.Name;
            needsRefresh |= LayoutOptions.Font?.Size != Style.Font.Size;
            needsRefresh |= LayoutOptions.Font?.Color != Style.Font.Color;
            needsRefresh |= LayoutOptions.Font?.Style != Style.Font.Style;

            if (!needsRefresh)
                return;

            LayoutOptions.Font = new Font(Style.Font);

            content = layoutContext.CreateContentLayout(Props.Text, LayoutOptions, Props.PerformLocalization);

            content.AnimationComplete += () => Props.AnimationComplete?.Invoke(evt.Reset(this));

            if (State.RenderOptions != null)
            {
                content.Options = State.RenderOptions;
                content.RenderContext = State.RenderContext;
            }
            else
            {
                State.RenderOptions = content.Options;
                State.RenderContext = content.RenderContext;
            }

            Dirty = false;
        }
    }

    public class LabelElementProps : RenderElementProps
    {
        public string Text { get; set; }

        public bool PerformLocalization { get; set; }

        public bool ReadSlowly { get; set; }

        public bool ResetReading { get; set; }

        public UserInterfaceEventHandler AnimationComplete { get; set; }
    }

    public class LabelElementState : RenderElementState
    {
        public bool Dirty { get; set; }

        public ContentLayoutOptions LayoutOptions { get; set; } = new ContentLayoutOptions();

        public ContentRenderOptions RenderOptions { get; set; }

        public ContentRenderContext RenderContext { get; set; }
    }

    public interface ILabelAnimationState
    {
        float SlowReadRate { get; set; }

        bool IsComplete { get; }

        void Restart();
    }
}
