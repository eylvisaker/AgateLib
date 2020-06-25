using AgateLib.Tests.Fakes;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using System;

namespace AgateLib
{
    public static class Extensions
    {
        public static void WaitForAnimations(this Desktop desktop)
        {
            var renderContext = new FakeRenderContext
            {
                GameTime = new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2))
            };

            desktop.Update(renderContext);
            desktop.Update(renderContext);
        }

        /// <summary>
        /// Clears animations for all widgets in all workspaces.
        /// </summary>
        public static void ClearAnimations(this Desktop desktop)
        {
            desktop.Explore(element =>
            {
                if (element.Display.Animation.State == AnimationState.TransitionOut)
                {
                    element.Display.IsVisible = false;
                }

                element.Display.Animation.State = AnimationState.Static;
                element.Display.Animation.IsVisible = element.Display.IsVisible;
            });
        }
    }
}
