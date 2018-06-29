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

using AgateLib.Mathematics;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Rendering.Transitions
{

    public interface IWidgetTransition
    {
        bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext);

        void Initialize(RenderElementDisplay display);

        void ContentRectUpdated(RenderElementDisplay display);
    }

    public class WidgetSuddenTransition : IWidgetTransition
    {
        public void ContentRectUpdated(RenderElementDisplay display)
        {
            display.Animator.AnimatedContentRect = display.Region.ContentRect;
            display.Animator.AnimatedBorderRect = display.Region.BorderRect;
        }

        public void Initialize(RenderElementDisplay display)
        {
        }

        public bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext)
        {
            var animator = display.Animator;

            animator.IsVisible = display.IsVisible;
            animator.AnimatedContentRect = display.Region.ContentRect;
            animator.AnimatedBorderRect = display.Region.BorderRect;
            animator.Alpha = 1;

            return true;
        }
    }

    public class WidgetFadeInTransition : IWidgetTransition
    {
        public void ContentRectUpdated(RenderElementDisplay display)
        {
        }

        public void Initialize(RenderElementDisplay display)
        {
            display.Animator.Alpha = 0;
        }

        public bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext)
        {
            var animation = display.Animator;
            animation.IsVisible = true;

            animation.Alpha +=
                (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds
                / display.Style.Animation.TransitionInTime;

            if (animation.Alpha >= 1)
            {
                animation.Alpha = 1;
                animation.AnimatedBorderRect = display.Region.BorderRect;

                return true;
            }

            const float shrinkMax = 0.1f;
            float shrink = shrinkMax * MathF.Pow(1 - animation.Alpha, 3);

            float leftRightMargin = shrink * display.Region.BorderRect.Width;
            float topBottomMargin = shrink * display.Region.BorderRect.Height;

            animation.AnimatedBorderRect = new Rectangle(
                display.Region.BorderRect.X + (int)leftRightMargin,
                display.Region.BorderRect.Y + (int)leftRightMargin,
                display.Region.BorderRect.Width - (int)(2 * leftRightMargin),
                display.Region.BorderRect.Height - (int)(2 * leftRightMargin));

            return false;
        }
    }

    public class WidgetFadeOutTransition : IWidgetTransition
    {
        public void ContentRectUpdated(RenderElementDisplay display)
        {
        }

        public void Initialize(RenderElementDisplay display)
        {
        }

        public bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext)
        {
            var animation = display.Animator;

            animation.AnimatedBorderRect = display.Region.BorderRect;

            animation.Alpha -=
                (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds
                / display.Style.Animation.TransitionOutTime;

            if (animation.Alpha <= 0)
            {
                animation.Alpha = 0;
                animation.IsVisible = false;
                return true;
            }

            const float shrinkMax = 0.1f;
            float shrink = shrinkMax * MathF.Pow(1 - animation.Alpha, 0.8f);

            float leftRightMargin = shrink * display.Region.BorderRect.Width;
            float topBottomMargin = shrink * display.Region.BorderRect.Height;

            animation.AnimatedBorderRect = new Rectangle(
                display.Region.BorderRect.X + (int)leftRightMargin,
                display.Region.BorderRect.Y + (int)leftRightMargin,
                display.Region.BorderRect.Width - (int)(2 * leftRightMargin),
                display.Region.BorderRect.Height - (int)(2 * leftRightMargin));

            return false;
        }
    }
}
