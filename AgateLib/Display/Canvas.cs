using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Display
{
    public interface ICanvas
    {
        [Obsolete("Do not use this.")]
        SpriteBatch SpriteBatch { get; }

        Rectangle Coordinates { get; }

        Size Size { get; }

        int Width { get; }

        int Height { get; }

        void FillRect(Color color, Rectangle destRect);
        
        void Draw(Texture2D texture, Rectangle destinationRectangle, Color color);
        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color);
        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);
        void Draw(Texture2D texture, Vector2 position, Color color);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

        void Draw(IContentLayout view, Vector2 position);
        void Draw(IContentLayout view, Vector2 position, OriginAlignment destAlignment);

        void DrawRect(Color borderColor, Rectangle rectangle);

        void DrawText(Font font, Vector2 position, string text);
    }

    public class Canvas : ICanvas
    {
        private readonly GraphicsDevice graphics;
        private readonly SpriteBatch spriteBatch;
        private Rectangle coordinates;
        private Texture2D whiteTexture;

        public Canvas(GraphicsDevice graphics)
        {
            this.graphics = graphics;

            spriteBatch = new SpriteBatch(graphics);

            coordinates = new Rectangle(0, 0,
                                        graphics.PresentationParameters.BackBufferWidth,
                                        graphics.PresentationParameters.BackBufferHeight);

            whiteTexture = new TextureBuilder(graphics).SolidColor(1, 1, Color.White);
        }

        [Obsolete("Avoid using this.")]
        public SpriteBatch SpriteBatch => spriteBatch;

        public Rectangle Coordinates => coordinates;

        public Size Size => coordinates.Size;

        public int Width => Size.Width;

        public int Height => Size.Height;

        public void Draw(IContentLayout view, Vector2 position)
        {
            view.Draw(position, spriteBatch);
        }

        public void Draw(IContentLayout view, Vector2 position, OriginAlignment destAlignment)
        {
            Vector2 offset = Origin.Calc(destAlignment, view.Size).ToVector2();

            view.Draw(position - offset, spriteBatch);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }
        
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void DrawRect(Color borderColor, Rectangle rectangle) => throw new NotImplementedException();

        public void DrawText(Font font, Vector2 position, string text)
        {
            font.DrawText(spriteBatch, position, text);
        }

        public void FillRect(Color color, Rectangle destRect)
        {
            Draw(whiteTexture, destRect, color);
        }

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, 
                          BlendState blendState = null, 
                          SamplerState samplerState = null, 
                          DepthStencilState depthStencilState = null, 
                          RasterizerState rasterizerState = null, 
                          Effect effect = null, 
                          Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(sortMode, 
                              blendState, 
                              samplerState, 
                              depthStencilState, 
                              rasterizerState, 
                              effect, 
                              transformMatrix);
        }

        public void End()
        {
            spriteBatch.End();
        }
    }
}
