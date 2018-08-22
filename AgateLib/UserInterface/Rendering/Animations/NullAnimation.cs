using AgateLib.UserInterface;
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
            display.Animation.AnimatedContentRect = display.ContentRect;
            display.Animation.AnimatedBorderRect = display.BorderRect;
        }

        public void Initialize(RenderElementDisplay display)
        {
        }

        public bool Update(RenderElementDisplay display, IUserInterfaceRenderContext renderContext)
        {
            var animator = display.Animation;

            animator.IsVisible = display.IsVisible;
            animator.AnimatedContentRect = display.ContentRect;
            animator.AnimatedBorderRect = display.BorderRect;
            animator.Alpha = 1;

            return true;
        }
    }
}
