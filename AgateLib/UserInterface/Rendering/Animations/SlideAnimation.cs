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
        private Rectangle onScreenRect;
        private Rectangle offScreenRect;

        private Vector2 offParentPos;
        private Vector2 onParentPos;
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

            onScreenRect = display.Parent?.ToScreen(display.BorderRect) ?? display.BorderRect;
            offScreenRect = OffscreenRect(display);

            Vector2 delta = offScreenRect.Location.ToVector2() - onScreenRect.Location.ToVector2(); 

            onParentPos = display.BorderRect.Location.ToVector2();
            offParentPos = onParentPos + delta;

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

        private Rectangle OffscreenRect(RenderElementDisplay display)
        {
            SlideFromLocation target = slideFrom;

            if (target == SlideFromLocation.Auto)
            {
                int deltaTop = onScreenRect.Top;
                int deltaLeft = onScreenRect.Left;
                int deltaRight = display.System.ScreenArea.Width - onScreenRect.Right;
                int deltaBottom = display.System.ScreenArea.Height - onScreenRect.Bottom;

                int deltaTarget = deltaTop;
                target = SlideFromLocation.Top;

                if (deltaRight <= deltaTarget) { target = SlideFromLocation.Right; deltaTarget = deltaRight; }
                if (deltaLeft <= deltaTarget) { target = SlideFromLocation.Left; deltaTarget = deltaLeft; }
                if (deltaBottom < deltaTarget) { target = SlideFromLocation.Bottom; deltaTarget = deltaBottom; }
            }

            return new Rectangle(
                OffscreenPosition(display, target),
                onScreenRect.Size);
        }

        private Point OffscreenPosition(RenderElementDisplay display, SlideFromLocation slideOffPoint)
        {
            const int offscreenMargin = 30;
            Size screenSize = display.System.ScreenArea.Size;

            switch (slideOffPoint)
            {
                case SlideFromLocation.Left:
                    return new Point(-offscreenMargin - onScreenRect.Width, onScreenRect.Top);
                case SlideFromLocation.Top:
                    return new Point(onScreenRect.Left, -offscreenMargin - onScreenRect.Height);
                case SlideFromLocation.Right:
                    return new Point(screenSize.Width + offscreenMargin, onScreenRect.Top);
                case SlideFromLocation.Bottom:
                    return new Point(onScreenRect.Left, screenSize.Height + offscreenMargin);

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

            Vector2 position = offParentPos + (onParentPos - offParentPos) * x;

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
