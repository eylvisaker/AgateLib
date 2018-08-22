using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Collections.Generic;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class PointerIndicator : IFocusIndicator
    {
        private Texture2D texture;
        
        public PointerIndicator(Texture2D texture)
        {
            this.texture = texture;
        }

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        /// <summary>
        /// Draws the focus indicator.
        /// </summary>
        /// <param name="renderContext">Render Context (is this really necessary?)</param>
        /// <param name="focusElement">The element that has focus.</param>
        /// <param name="focusContentRect">The screen coordinates of the content area of the focus element.</param>
        public void DrawFocus(SpriteBatch spriteBatch, IRenderElement focusElement, Rectangle focusContentRect)
        {
            const int overlap = 1;

            Rectangle pointerDest = new Rectangle(
                focusContentRect.Left - texture.Width + overlap,
                (focusContentRect.Top + focusContentRect.Bottom - texture.Height) / 2,
                texture.Width,
                texture.Height);

            DrawPointer(spriteBatch, pointerDest);
        }
        
        protected virtual void DrawPointer(SpriteBatch spriteBatch, Rectangle pointerDest)
        {
            spriteBatch.Draw(texture, pointerDest, Color.White);
        }
    }
}