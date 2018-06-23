using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Collections.Generic;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManualTests.AgateLib.UserInterface.FF6
{
    public class PointerIndicator : MenuIndicatorRenderer
    {
        private Texture2D texture;
        
        public PointerIndicator(Texture2D texture)
        {
            this.texture = texture;
        }
        
        public override void TheDrawFocus(IWidgetRenderContext renderContext, IWidget widget, Rectangle destRect)
        {
            const int overlap = 1;

            Rectangle pointerDest = new Rectangle(
                destRect.Left - texture.Width + overlap,
                (destRect.Top + destRect.Bottom - texture.Height) / 2,
                texture.Width,
                texture.Height);

            DrawPointer(renderContext, pointerDest);
        }
        
        protected virtual void DrawPointer(IWidgetRenderContext renderContext, Rectangle pointerDest)
        {
            renderContext.SpriteBatch.Draw(texture, pointerDest, Color.White);
        }
    }
}