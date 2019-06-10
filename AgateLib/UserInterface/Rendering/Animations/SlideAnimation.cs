using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AgateLib.UserInterface.Rendering.Animations
{
    public class SlideAnimation : IRenderElementAnimation
    {
        private enum SlideFromLocation
        {
            Auto,
            Left,
            Top,
            Right,
            Bottom,
        }

        private SlideFromLocation slideFrom;
        private AnimationState lastState = (AnimationState)(-1);
        private float wiggle = 0;
        private float animationTimeLength_s = 0.5f;
        private Vector2 offScreenPos;
        private Vector2 onScreenPos;
        private float currentTime;
        private int timeSign = 1;
        private bool initialized = false;

        public SlideAnimation(IReadOnlyList<string> args)
        {
            if (args.Count > 0)
            {
                Enum.TryParse(args[0], true, out slideFrom);
            }
        }

        public bool IsDoubleBuffered => true;

        public void ContentRectUpdated(RenderElementDisplay display)
        {
        }

        public void Initialize(RenderElementDisplay display)
        {
            InitializeEndPoints(display);
        }

        private void InitializeEndPoints(RenderElementDisplay display)
        {
            lastState = display.Animation.State;

            offScreenPos = OffscreenPosition(display);
            onScreenPos = display.BorderRect.Location.ToVector2();

            if (lastState == AnimationState.TransitionIn)
            {
                timeSign = 1;
                currentTime = 0;
            }
            else
            {
                timeSign = -1;
                currentTime = animationTimeLength_s;
            }
        }

        private Vector2 OffscreenPosition(RenderElementDisplay display)
        {
            SlideFromLocation target = slideFrom;

            if (target == SlideFromLocation.Auto)
            {
                int deltaTop = display.BorderRect.Top;
                int deltaLeft = display.BorderRect.Left;
                int deltaRight = display.System.ScreenArea.Width - display.BorderRect.Right;
                int deltaBottom = display.System.ScreenArea.Height - display.BorderRect.Bottom;

                int deltaTarget = deltaTop;
                target = SlideFromLocation.Top;

                if (deltaRight <= deltaTarget) { target = SlideFromLocation.Right; deltaTarget = deltaRight; }
                if (deltaLeft <= deltaTarget) { target = SlideFromLocation.Left; deltaTarget = deltaLeft; }
                if (deltaBottom < deltaTarget) { target = SlideFromLocation.Bottom; deltaTarget = deltaBottom; }
            }

            return OffscreenPosition(display, target);
        }

        private Vector2 OffscreenPosition(RenderElementDisplay display, SlideFromLocation slideOffPoint)
        {
            const int offscreenMargin = 30;
            Size screenSize = display.System.ScreenArea.Size;

            switch (slideOffPoint)
            {
                case SlideFromLocation.Left:
                    return new Vector2(-offscreenMargin - display.BorderRect.Width, display.BorderRect.Top);
                case SlideFromLocation.Top:
                    return new Vector2(display.BorderRect.Left, -offscreenMargin - display.BorderRect.Height);
                case SlideFromLocation.Right:
                    return new Vector2(screenSize.Width + offscreenMargin, display.BorderRect.Top);
                case SlideFromLocation.Bottom:
                    return new Vector2(display.BorderRect.Left, screenSize.Height + offscreenMargin);

                default:
                    throw new ArgumentException(
                        $"{nameof(slideOffPoint)} must be top, left, right or bottom.");
            }
        }

        public bool Update(RenderElementDisplay display, IUserInterfaceRenderContext renderContext)
        {
            var animation = display.Animation;
            animation.IsVisible = true;

            if (animation.State != lastState)
            {
                InitializeEndPoints(display);
            }

            currentTime += timeSign * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            // Using equation x = 1/2*a*t^2 + b*t
            // x = 0 is off-screen position
            // x = 1 is on-screen position
            float a = -2 * (1 + wiggle);
            float b = 2 - wiggle;

            float t = currentTime / animationTimeLength_s;
            float x = 0.5f * a * t * t + b * t;
            float _x = x;

            if (x >= 1)
                x = 1;
            if (x <= 0)
                x = 0;

            Vector2 position = offScreenPos + (onScreenPos - offScreenPos) * x;

            display.Animation.AnimatedBorderRect = new Rectangle(
                position.ToPoint(), display.BorderRect.Size);

            if (t <= 0 && timeSign < 0)
                return true;
            if (t >= 1 && timeSign > 0)
                return true;

            return false;
        }
    }
}
