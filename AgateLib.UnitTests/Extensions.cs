using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;

namespace AgateLib
{
    public static class Extensions
    {
        public static void WaitForAnimations(this Desktop desktop)
        {
            var renderContext = CommonMocks.RenderContext();
            renderContext.Setup(x => x.GameTime).Returns(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            desktop.Update(renderContext.Object);
            desktop.Update(renderContext.Object);
        }

        /// <summary>
        /// Clears animations for all widgets in all workspaces.
        /// </summary>
        public static void ClearAnimations(this Desktop desktop)
        {
            desktop.Explore((parent, widget) => 
            {
                if (widget.Display.Animation.State == AnimationState.TransitionOut)
                    widget.Display.IsVisible = false;

                widget.Display.Animation.State = AnimationState.Static;
                widget.Display.Animation.IsVisible = widget.Display.IsVisible;
            });
        }
    }
}
