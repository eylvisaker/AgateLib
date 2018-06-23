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
using AgateLib.UserInterface.Rendering.Transitions;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface.Rendering
{
    /// <summary>
    /// Interface for doing basic animation and background/border rendering of widgets.
    /// </summary>
    public interface IUserInterfaceRenderer
    {
        /// <summary>
        /// Updates the animation state of the widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="widget"></param>
        void UpdateAnimation(IWidgetRenderContext renderContext, IWidget widget);

        /// <summary>
        /// Draws the background of a widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="display"></param>
        /// <param name="dest"></param>
        void DrawBackground(IWidgetRenderContext renderContext, WidgetDisplay display, Point dest);

        /// <summary>
        /// Draws the border of a widget.
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="display"></param>
        /// <param name="dest"></param>
        void DrawFrame(IWidgetRenderContext renderContext, WidgetDisplay display, Point dest);


        void DrawBackground(SpriteBatch spriteBatch, BackgroundStyle background, Rectangle backgroundRect);

        void DrawFrame(SpriteBatch spriteBatch, BorderStyle border, Rectangle borderRect);

        void DrawFrame(SpriteBatch spriteBatch, Rectangle destOuterRect, Texture2D frameTexture,
            Rectangle frameSourceInner, Rectangle frameSourceOuter,
            ImageScale borderScale);
    }

    [Transient]
    public class UserInterfaceRenderer : IUserInterfaceRenderer
    {
        private readonly IComponentStyleRenderer styleRenderer;

        Dictionary<string, IWidgetTransition> transitions = new Dictionary<string, IWidgetTransition>();

        private BlendState blendState = new BlendState
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
        };

        /// <summary>
        /// Constructs a UserInterfaceRenderer object.
        /// </summary>
        /// <param name="styleRenderer"></param>
		public UserInterfaceRenderer(IComponentStyleRenderer styleRenderer)
        {
            this.styleRenderer = styleRenderer ?? throw new ArgumentNullException(nameof(styleRenderer));

            transitions["sudden"] = new WidgetSuddenTransition();
            transitions["fade-in"] = new WidgetFadeInTransition();
            transitions["fade-out"] = new WidgetFadeOutTransition();
        }

        public void Dispose()
        {
        }

        public void DrawBackground(IWidgetRenderContext renderContext, WidgetDisplay display, Point dest)
        {
            // This draws the background behind the border - a problem for borders which have transparency on their outer edges
            // but important for borders which have transparency on their inner edges. Oh what to do...
            //Rectangle backgroundRect = new Rectangle(
            //                                new Point(dest.X - display.Region.BorderToContentOffset.Left,
            //                                    dest.Y - display.Region.BorderToContentOffset.Top),
            //                                display.Region.BorderRect.Size);

            // Here we make a compromise and set the background rectangle to be slightly smaller than
            // the border rectangle
            Rectangle backgroundRect = new Rectangle(
                dest.X - display.Region.BorderToContentOffset.Left + display.Region.Style.Border.Left.Width / 2, 
                dest.Y - display.Region.BorderToContentOffset.Top + display.Region.Style.Border.Top.Width / 2,
                display.Region.BorderRect.Width - display.Region.Style.Border.Width / 2,
                display.Region.BorderRect.Height - display.Region.Style.Border.Height / 2);

            styleRenderer.DrawBackground(
                renderContext.SpriteBatch,
                display.Style.Background,
                backgroundRect);
        }

        public void DrawBackground(SpriteBatch spriteBatch, BackgroundStyle background, Rectangle backgroundRect)
        {
            styleRenderer.DrawBackground(spriteBatch, background, backgroundRect);
        }

        public void DrawFrame(IWidgetRenderContext renderContext, WidgetDisplay display, Point dest)
        {
            styleRenderer.DrawFrame(
                renderContext.SpriteBatch,
                display.Style.Border,
                new Rectangle(
                    new Point(dest.X - display.Region.BorderToContentOffset.Left,
                        dest.Y - display.Region.BorderToContentOffset.Top),
                    display.Region.BorderRect.Size));
        }

        public void DrawFrame(SpriteBatch spriteBatch, BorderStyle border, Rectangle borderRect)
        {
            styleRenderer.DrawFrame(spriteBatch, border, borderRect);
        }

        public void DrawFrame(SpriteBatch spriteBatch, Rectangle destOuterRect, Texture2D frameTexture, Rectangle frameSourceInner, Rectangle frameSourceOuter, ImageScale borderScale)
        {
            styleRenderer.DrawFrame(
                spriteBatch,
                destOuterRect,
                frameTexture,
                frameSourceInner,
                frameSourceOuter,
                borderScale);
        }

        public void UpdateAnimation(IWidgetRenderContext renderContext, IWidget widget)
        {
            var animator = widget.Display.Animation;

            SetTransition(widget);

            bool finished = animator.Transition?.Update(widget, renderContext) ?? false;

            if (finished)
            {
                AdvanceTransitionState(widget);
            }
        }

        private void AdvanceTransitionState(IWidget widget)
        {
            var animator = widget.Display.Animation;

            switch (animator.State)
            {
                case AnimationState.TransitionIn:
                    animator.State = AnimationState.Static;
                    break;

                case AnimationState.TransitionOut:
                    animator.State = AnimationState.Dead;
                    break;

                case AnimationState.Dead:
                    animator.State = AnimationState.TransitionIn;
                    break;
            }

            SetTransition(widget);
        }

        private void SetTransition(IWidget widget)
        {
            var animator = widget.Display.Animation;
            var style = widget.Display.Style.Animation;

            string transitionName = "sudden";

            switch (widget.Display.Animation.State)
            {
                case AnimationState.TransitionIn:
                    transitionName = style.TransitionIn ?? transitionName;
                    break;

                case AnimationState.TransitionOut:
                    transitionName = style.TransitionOut ?? transitionName;
                    break;

                default:
                    transitionName = "sudden";
                    break;

            }

            if (transitions.TryGetValue(transitionName, out IWidgetTransition newTransition))
            {
                if (animator.Transition != newTransition)
                {
                    animator.Transition = newTransition;
                    animator.Transition?.Initialize(widget);
                }
            }
        }
    }
}
