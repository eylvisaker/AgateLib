using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

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
