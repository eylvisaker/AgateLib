
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Mathematics;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Rendering.Transitions
{

    public class WidgetFadeInTransition : IWidgetTransition
    {
        public bool IsDoubleBuffered => true;

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

            Log.Debug($"Alpha: {animation.Alpha} Rect: {animation.AnimatedBorderRect}");

            return false;
        }
    }

    public class WidgetFadeOutTransition : IWidgetTransition
    {
        public bool IsDoubleBuffered => true;

        public void ContentRectUpdated(RenderElementDisplay display)
        {
        }

        public void Initialize(RenderElementDisplay display)
        {
        }

        public bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext)
        {
            var animation = display.Animator;

            animation.AnimatedBorderRect = display.BorderRect;

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
