using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public class FakeCanvas : ICanvas
    {
        public FakeCanvas(Size? size = null)
        {
            var actualSize = size ?? new Size(1920, 1080);

            Coordinates = new Rectangle(Point.Zero, actualSize);
            Size = actualSize;
        }
        public SpriteBatch SpriteBatch => null;

        public Rectangle Coordinates { get; }

        public Size Size { get; }

        public int Width => Size.Width;

        public int Height => Size.Height;

        public BlendState BlendState => new BlendState();

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
        }

        public void Draw(IContentLayout view, Vector2 position)
        {
        }

        public void Draw(IContentLayout view, Vector2 position, OriginAlignment destAlignment)
        {
        }

        public void DrawRect(Color borderColor, Rectangle rectangle)
        {
        }

        public void DrawText(Font font, Vector2 position, string text)
        {
            font.DrawText(null, position, text);
        }

        public void End()
        {
        }

        public void FillRect(Rectangle destRect, Color color)
        {
        }

        public void PopState()
        {
        }

        public void PushState(bool copyExisting = true, SpriteSortMode? sortMode = null, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
        }
    }
}
