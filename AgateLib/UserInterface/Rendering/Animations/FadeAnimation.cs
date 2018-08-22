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
using AgateLib.Mathematics;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Rendering.Animations
{

    public class FadeAnimation : IWidgetAnimation
    {
        float transitionTime = 0.35f;

        public FadeAnimation(IReadOnlyList<string> args)
        {
            if ((args?.Count ?? 0) > 0)
            {
                if (!float.TryParse(args[0], out transitionTime))
                {
                    Log.Warn($"Failed to parse transition time for fade animation.");
                }
            }
        }

        public bool IsDoubleBuffered => true;

        public void ContentRectUpdated(RenderElementDisplay display)
        {
        }

        public void Initialize(RenderElementDisplay display)
        {
            display.Animation.Alpha = 0;
        }

        public bool Update(RenderElementDisplay display, IUserInterfaceRenderContext renderContext)
        {
            var animation = display.Animation;
            animation.IsVisible = true;

            if (animation.State == AnimationState.TransitionIn)
            {
                return AnimateEntry(display, renderContext, animation);
            }
            else if (animation.State == AnimationState.TransitionOut)
            {
                return AnimateExit(display, renderContext, animation);
            }

            return true;
        }

        private bool AnimateEntry(RenderElementDisplay display, IUserInterfaceRenderContext renderContext, RenderElementAnimator animation)
        {
            animation.Alpha +=
                (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds
                / transitionTime;

            if (animation.Alpha >= 1)
            {
                animation.Alpha = 1;
                animation.AnimatedBorderRect = display.BorderRect;

                return true;
            }

            const float shrinkMax = 0.1f;
            float shrink = shrinkMax * MathF.Pow(1 - animation.Alpha, 3);

            float leftRightMargin = shrink * display.BorderRect.Width;
            float topBottomMargin = shrink * display.BorderRect.Height;

            animation.AnimatedBorderRect = new Rectangle(
                display.BorderRect.X + (int)leftRightMargin,
                display.BorderRect.Y + (int)leftRightMargin,
                display.BorderRect.Width - (int)(2 * leftRightMargin),
                display.BorderRect.Height - (int)(2 * leftRightMargin));

            Log.Debug($"Alpha: {animation.Alpha} BorderRect: {display.BorderRect} AnimatedBorderRect: {animation.AnimatedBorderRect}");

            return false;
        }

        private bool AnimateExit(RenderElementDisplay display, IUserInterfaceRenderContext renderContext, RenderElementAnimator animation)
        {
            animation.Alpha -=
                                (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds
                                / transitionTime;

            if (animation.Alpha <= 0)
            {
                animation.Alpha = 0;
                animation.IsVisible = false;
                return true;
            }

            const float shrinkMax = 0.1f;
            float shrink = shrinkMax * MathF.Pow(1 - animation.Alpha, 0.8f);

            float leftRightMargin = shrink * display.BorderRect.Width;
            float topBottomMargin = shrink * display.BorderRect.Height;

            animation.AnimatedBorderRect = new Rectangle(
                display.BorderRect.X + (int)leftRightMargin,
                display.BorderRect.Y + (int)leftRightMargin,
                display.BorderRect.Width - (int)(2 * leftRightMargin),
                display.BorderRect.Height - (int)(2 * leftRightMargin));

            return false;
        }
    }
}