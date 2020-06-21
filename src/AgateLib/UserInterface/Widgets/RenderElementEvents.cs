using Microsoft.Xna.Framework;
using System;

namespace AgateLib.UserInterface
{
    public class RenderElementEvents
    {
        /// <summary>
        /// Before Draw action. Arguments are renderContext, contentDest
        /// </summary>
        public Action<IUserInterfaceRenderContext, Rectangle> BeforeDraw;

        /// <summary>
        /// After Draw action. Arguments are renderContext, contentDest
        /// </summary>
        public Action<IUserInterfaceRenderContext, Rectangle> AfterDraw;
    }
}
