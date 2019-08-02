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

        /// <summary>
        /// Gets the current blend state.
        /// </summary>
        BlendState BlendState { get; }

        void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred,
                   BlendState blendState = null,
                   SamplerState samplerState = null,
                   DepthStencilState depthStencilState = null,
                   RasterizerState rasterizerState = null,
                   Effect effect = null,
                   Matrix? transformMatrix = null);

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

        void End();

        /// <summary>
        /// Fills a rectangle with a solid color.
        /// </summary>
        /// <param name="destRect"></param>
        /// <param name="color"></param>
        void FillRect(Rectangle destRect, Color color);

        /// <summary>
        /// Flushes the drawing buffer. Stores the last parameters to Begin.
        /// </summary>
        /// <param name="copyExisting">If true, this will copy the existing state and overwrite only values which were passed in</param>
        void PushState(bool copyExisting = true,
                       SpriteSortMode? sortMode = null,
                       BlendState blendState = null,
                       SamplerState samplerState = null,
                       DepthStencilState depthStencilState = null,
                       RasterizerState rasterizerState = null,
                       Effect effect = null,
                       Matrix? transformMatrix = null);

        /// <summary>
        /// Flushes the drawing buffer. Uses the last parameters to Begin to resume the drawing state.
        /// </summary>
        void PopState();
    }

    public class Canvas : ICanvas
    {
        class SpriteBatchBeginArgs
        {
            public SpriteSortMode sortMode = SpriteSortMode.Deferred;
            public BlendState blendState = null;
            public SamplerState samplerState = null;
            public DepthStencilState depthStencilState = null;
            public RasterizerState rasterizerState = null;
            public Effect effect = null;
            public Matrix? transformMatrix = null;
        };

        private readonly GraphicsDevice graphics;
        private readonly SpriteBatch spriteBatch;
        private Rectangle coordinates;
        private Texture2D whiteTexture;
        private SpriteBatchBeginArgs spriteBatchBeginArgs = new SpriteBatchBeginArgs();
        private Stack<SpriteBatchBeginArgs> beginArgsStack = new Stack<SpriteBatchBeginArgs>();

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

        public BlendState BlendState => spriteBatchBeginArgs.blendState;

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

        public void FillRect(Rectangle destRect, Color color)
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
            spriteBatchBeginArgs.sortMode = sortMode;
            spriteBatchBeginArgs.blendState = blendState;
            spriteBatchBeginArgs.samplerState = samplerState;
            spriteBatchBeginArgs.depthStencilState = depthStencilState;
            spriteBatchBeginArgs.rasterizerState = rasterizerState;
            spriteBatchBeginArgs.effect = effect;
            spriteBatchBeginArgs.transformMatrix = transformMatrix;

            BeginSpriteBatch();
        }


        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Flushes the drawing buffer. Stores the last parameters to Begin.
        /// </summary>
        public void PushState(bool copyExisting,
                              SpriteSortMode? sortMode = null,
                              BlendState blendState = null,
                              SamplerState samplerState = null,
                              DepthStencilState depthStencilState = null,
                              RasterizerState rasterizerState = null,
                              Effect effect = null,
                              Matrix? transformMatrix = null)
        {
            beginArgsStack.Push(spriteBatchBeginArgs);

            if (copyExisting)
            {
                spriteBatchBeginArgs = new SpriteBatchBeginArgs
                {
                    sortMode = sortMode ?? spriteBatchBeginArgs.sortMode,
                    blendState = blendState ?? spriteBatchBeginArgs.blendState,
                    samplerState = samplerState ?? spriteBatchBeginArgs.samplerState,
                    depthStencilState = depthStencilState ?? spriteBatchBeginArgs.depthStencilState,
                    rasterizerState = rasterizerState ?? spriteBatchBeginArgs.rasterizerState,
                    effect = effect ?? spriteBatchBeginArgs.effect,
                    transformMatrix = transformMatrix ?? spriteBatchBeginArgs.transformMatrix
                };
            }
            else
            {
                spriteBatchBeginArgs = new SpriteBatchBeginArgs
                {
                    sortMode = sortMode ?? SpriteSortMode.Deferred,
                    blendState = blendState,
                    samplerState = samplerState,
                    depthStencilState = depthStencilState,
                    rasterizerState = rasterizerState,
                    effect = effect,
                    transformMatrix = transformMatrix
                };
            }

            End();
            BeginSpriteBatch();
        }

        /// <summary>
        /// Flushes the drawing buffer. Uses the last parameters to Begin to resume the drawing state.
        /// </summary>
        public void PopState()
        {
            End();

            spriteBatchBeginArgs = beginArgsStack.Pop();

            BeginSpriteBatch();
        }

        private void BeginSpriteBatch()
        {
            spriteBatch.Begin(spriteBatchBeginArgs.sortMode,
                              spriteBatchBeginArgs.blendState,
                              spriteBatchBeginArgs.samplerState,
                              spriteBatchBeginArgs.depthStencilState,
                              spriteBatchBeginArgs.rasterizerState,
                              spriteBatchBeginArgs.effect,
                              spriteBatchBeginArgs.transformMatrix);
        }
    }
}
