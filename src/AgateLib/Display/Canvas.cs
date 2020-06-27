using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.UserInterface.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AgateLib.Display
{
    public interface ICanvas
    {
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
        void Draw(Texture2D texture, Vector2 position, OriginAlignment alignment, Color color);
        void Draw(Texture2D texture, Vector2 position, Color color);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);

        void Draw(IContentLayout view, Vector2 position);
        void Draw(IContentLayout view, Vector2 position, OriginAlignment destAlignment);
        void DrawLines(LineType lineType, Color color, IEnumerable<Vector2> points);
        void DrawTexturedPolygon(Texture2D texture, IReadOnlyList<Vector2> polygon, IReadOnlyList<Vector2> texCoords);

        void DrawText(Font font, Vector2 position, string text);

        void FillConvexPolygon(Color color, IReadOnlyList<Vector2> polygon);

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
        private class SpriteBatchBeginArgs
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
        private PrimitiveRenderer primitives;
        private SpriteBatchBeginArgs spriteBatchBeginArgs = new SpriteBatchBeginArgs();
        private Stack<SpriteBatchBeginArgs> beginArgsStack = new Stack<SpriteBatchBeginArgs>();
        private BasicEffect basicEffect;

        public Canvas(GraphicsDevice graphics)
        {
            this.graphics = graphics;

            spriteBatch = new SpriteBatch(graphics);

            coordinates = new Rectangle(0, 0,
                                        graphics.PresentationParameters.BackBufferWidth,
                                        graphics.PresentationParameters.BackBufferHeight);

            whiteTexture = new TextureBuilder(graphics).SolidColor(1, 1, Color.White);

            primitives = new PrimitiveRenderer(graphics);
            basicEffect = new BasicEffect(graphics);
        }

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

        public void Draw(Texture2D texture, Vector2 position, OriginAlignment alignment, Color color)
        {
            var offset = Origin.Calc(alignment, new Size(texture.Width, texture.Height));

            spriteBatch.Draw(texture, position - offset.ToVector2(), color);
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

        public void DrawLines(LineType lineType, Color color, IEnumerable<Vector2> points)
        {
            PushState(true);

            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = false;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(coordinates, -1, 1);

            primitives.DrawLines(basicEffect, lineType, color, points);

            PopState();
        }

        public void DrawTexturedPolygon(Texture2D texture, IReadOnlyList<Vector2> polygon, IReadOnlyList<Vector2> texCoords)
        {
            PushState(true);

            basicEffect.VertexColorEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(coordinates, -1, 1);
            basicEffect.Texture = texture;

            primitives.DrawTexturedPolygon(basicEffect, polygon, texCoords);

            PopState();
        }

        public void FillConvexPolygon(Color color, IReadOnlyList<Vector2> polygon)
        {
            PushState(true);

            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = false;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(coordinates, -1, 1);

            primitives.FillConvexPolygon(basicEffect, color, polygon);

            PopState();
        }
    }

    public static class CanvasExtensions
    {
        public static Matrix TransformMatrix(this ICanvas canvas)
        {
            return Matrix.CreateOrthographicOffCenter(canvas.Coordinates, -1, 1);
        }

        /// <summary>
        /// Draws a set of lines. The lineType parameter controls how
        /// lines are connected.
        /// </summary>
        /// <param name="lineType">The type of lines to draw.</param>
        /// <param name="color">The color of lines to draw.</param>
        /// <param name="points">The points that are used to 
        /// build the individual line segments.</param>
        public static void DrawLines(this ICanvas canvas, LineType lineType, Color color,
            IEnumerable<Vector2> points)
        {
            canvas.DrawLines(lineType, color, points);
        }

        /// <summary>
        /// Draws a filled convex polygon.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="points"></param>
        public static void FillPolygon(this ICanvas canvas, Color color, IReadOnlyList<Vector2> points)
        {
            canvas.FillConvexPolygon(color, points);
        }

        /// <summary>
        ///     Draws a line between the two points specified.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void DrawLine(this ICanvas canvas, Color color, Vector2 a, Vector2 b)
        {
            canvas.DrawLines(LineType.LineSegments, color, new[] { a, b });
        }

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void DrawRect(this ICanvas canvas, Color color, Rectangle rect)
        {
            canvas.DrawRect(color, (RectangleF)rect);
        }

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void DrawRect(this ICanvas canvas, Color color, RectangleF rect)
        {
            canvas.DrawLines(LineType.Polygon, color,
                new QuadrilateralBuilder().BuildRectangle(rect));
        }
        
        /// <summary>
        /// Draws the outline of an ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
        public static void DrawEllipse(this ICanvas canvas, Color color, Rectangle boundingRect)
        {
            canvas.DrawEllipse(color, (RectangleF)boundingRect);
        }

        /// <summary>
        /// Draws the outline of an ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
        public static void DrawEllipse(this ICanvas canvas, Color color, RectangleF boundingRect)
        {
            canvas.DrawLines(LineType.Polygon, color,
                new EllipseBuilder().BuildEllipse(boundingRect).Points);
        }

        /// <summary>
        /// Draws an unfilled polygon.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="polygon"></param>
        public static void DrawPolygon(this ICanvas canvas, Color color, Polygon polygon)
        {
            canvas.DrawLines(LineType.Polygon, color, polygon.Points);
        }

        /// <summary>
        /// Draws a filled polygon.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="polygon"></param>
        public static void FillPolygon(this ICanvas canvas, Color color, Polygon polygon)
        {
            if (polygon.IsConvex)
            {
                canvas.FillPolygon(color, polygon.Points);
            }
            else
            {
                foreach (var convexPoly in polygon.ConvexDecomposition)
                {
                    canvas.FillPolygon(color, convexPoly.Points);
                }
            }
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void FillRect(this ICanvas canvas, Color color, Rectangle rect)
        {
            canvas.FillRect(color, (RectangleF)rect);
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void FillRect(this ICanvas canvas, Color color, RectangleF rect)
        {
            canvas.FillPolygon(color, new QuadrilateralBuilder().BuildRectangle(rect));
        }

        /// <summary>
        /// Draws a filled ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect"></param>
        public static void FillEllipse(this ICanvas canvas, Color color, Rectangle boundingRect)
        {
            canvas.FillEllipse(color, (RectangleF)boundingRect);
        }

        /// <summary>
        /// Draws a filled ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect"></param>
        public static void FillEllipse(this ICanvas canvas, Color color, RectangleF boundingRect)
        {
            canvas.FillPolygon(color, new EllipseBuilder().BuildEllipse(boundingRect).Points);
        }

        /// <summary>
        /// Draws a filled ellipse.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="center"></param>
        /// <param name="majorAxisRadius"></param>
        /// <param name="minorAxisRadius"></param>
        /// <param name="rotationAngle"></param>
        public static void FillEllipse(this ICanvas canvas,
                                       Color color,
                                       Vector2 center,
                                       double majorAxisRadius,
                                       double minorAxisRadius,
                                       double rotationAngle)
        {
            canvas.FillPolygon(
                color,
                new EllipseBuilder().BuildEllipse(center, majorAxisRadius, minorAxisRadius, rotationAngle).Points);
        }

        /// <summary>
        /// Draws a filled circle.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="color"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void FillCircle(this ICanvas canvas, Color color, Vector2 center, double radius)
        {
            canvas.FillPolygon(color, new EllipseBuilder().BuildCircle(center, radius).Points);
        }
    }
}
