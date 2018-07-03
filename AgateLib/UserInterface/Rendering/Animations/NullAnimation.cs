using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Rendering.Animations
{

    public class NullAnimation : IWidgetAnimation
    {
        public bool IsDoubleBuffered => false;

        public void ContentRectUpdated(RenderElementDisplay display)
        {
            display.Animation.AnimatedContentRect = display.Region.ContentRect;
            display.Animation.AnimatedBorderRect = display.Region.BorderRect;
        }

        public void Initialize(RenderElementDisplay display)
        {
        }

        public bool Update(RenderElementDisplay display, IWidgetRenderContext renderContext)
        {
            var animator = display.Animation;

            animator.IsVisible = display.IsVisible;
            animator.AnimatedContentRect = display.Region.ContentRect;
            animator.AnimatedBorderRect = display.Region.BorderRect;
            animator.Alpha = 1;

            return true;
        }
    }
}
