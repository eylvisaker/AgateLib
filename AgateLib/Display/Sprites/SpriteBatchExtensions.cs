using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Display.Sprites
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, 
                                ISprite sprite, 
                                Vector2 position, 
                                int layerDepth)
        {
            sprite.Draw(spriteBatch, position, layerDepth);
        }
    }
}
