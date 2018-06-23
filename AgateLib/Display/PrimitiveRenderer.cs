//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Display
{
    /// <summary>
    /// Interface for the primitive renderer. This can draw lines and shapes.
    /// </summary>
    public interface IPrimitiveRenderer
    {
        /// <summary>
        /// Draws a set of lines. The lineType parameter controls how
        /// lines are connected.
        /// </summary>
        /// <param name="lineType">The type of lines to draw.</param>
        /// <param name="color">The color of lines to draw.</param>
        /// <param name="points">The points that are used to 
        /// build the individual line segments.</param>
        void DrawLines(Effect effect, LineType lineType, Color color, IEnumerable<Vector2> points);

        /// <summary>
        /// Draws a solid color filled convex polygon. 
        /// </summary>
        /// <remarks>The point list passed in is assumed to be
        /// convex - if it is not the polygon won't be drawn correctly.
        /// 
        /// TODO: Make this return a structure that will draw the primitives on command.
        /// </remarks>
        /// <param name="color"></param>
        /// <param name="points"></param>
        void FillConvexPolygon(Effect effect, Color color, IReadOnlyList<Vector2> points);

        /// <summary>
        /// Draws a textured polygon
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="polygon"></param>
        void DrawTexturedPolygon(Effect effect, IReadOnlyList<Vector2> points, IReadOnlyList<Vector2> textureCoordinates);

    }

    [Singleton]
    public class PrimitiveRenderer : IPrimitiveRenderer
    {
        private readonly GraphicsDevice graphics;

        public PrimitiveRenderer(GraphicsDevice graphics)
        {
            this.graphics = graphics;
        }

        public void DrawLines(Effect effect, LineType lineType, Color color, IEnumerable<Vector2> points)
        {
            VertexPositionColor[] vertices = points.Select(x =>
                        new VertexPositionColor(new Vector3(x.X, x.Y, 0), color))
                        .ToArray();

            PrimitiveType primitiveType;
            int primitives;

            switch (lineType)
            {
                case LineType.LineSegments:
                    primitiveType = PrimitiveType.LineList;
                    primitives = vertices.Length / 2;
                    break;

                case LineType.Polygon:
                    primitiveType = PrimitiveType.LineStrip;
                    primitives = vertices.Length;

                    // Add an extra point to connect from the last to the first.
                    var list = vertices.ToList();
                    list.Add(list[0]);
                    vertices = list.ToArray();

                    break;

                case LineType.Path:
                default:
                    primitiveType = PrimitiveType.LineStrip;
                    primitives = vertices.Length - 1;
                    break;
            }


            foreach (EffectPass e in effect.CurrentTechnique.Passes)
            {
                e.Apply();

                graphics.DrawUserPrimitives(primitiveType, vertices, 0, primitives);
            }
        }

        public void DrawTexturedPolygon(Effect effect, IReadOnlyList<Vector2> polygon, IReadOnlyList<Vector2> texCoords)
        {
            int triangles = polygon.Count - 2;

            if (triangles < 1)
            {
                Log.WriteLine(LogLevel.Debug, "DrawTexturedPolygon call with no triangles.");
                return;
            }

            var vertices = new VertexPositionTexture[triangles * 3];

            // XNA doesn't support triangle fan, so we write out the individual triangles.
            for (int i = 0; i < triangles; i++)
            {
                var vindex = i * 3;

                vertices[vindex + 0] = new VertexPositionTexture(new Vector3(polygon[0].X, polygon[0].Y, 0), texCoords[0]);
                vertices[vindex + 1] = new VertexPositionTexture(new Vector3(polygon[i + 2].X, polygon[i + 2].Y, 0), texCoords[i+2]);
                vertices[vindex + 2] = new VertexPositionTexture(new Vector3(polygon[i + 1].X, polygon[i + 1].Y, 0), texCoords[i+1]);
            }

            foreach (EffectPass e in effect.CurrentTechnique.Passes)
            {
                e.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, triangles);
            }
        }

        public void FillConvexPolygon(Effect effect, Color color, IReadOnlyList<Vector2> polygon)
        {
            int triangles = polygon.Count - 2;

            var vertices = new VertexPositionColor[triangles * 3];

            // XNA doesn't support triangle fan, so we write out the individual triangles.
            for (int i = 0; i < triangles; i++)
            {
                var vindex = i * 3;

                vertices[vindex + 0] = new VertexPositionColor(new Vector3(polygon[0].X, polygon[0].Y, 0), color);
                vertices[vindex + 1] = new VertexPositionColor(new Vector3(polygon[i + 2].X, polygon[i + 2].Y, 0), color);
                vertices[vindex + 2] = new VertexPositionColor(new Vector3(polygon[i + 1].X, polygon[i + 1].Y, 0), color);
            }

            //RasterizerState state = new RasterizerState
            //{
            //	CullMode = CullMode.None,
            //	FillMode = FillMode.Solid,
            //	ScissorTestEnable = true,
            //};

            foreach (EffectPass e in effect.CurrentTechnique.Passes)
            {
                e.Apply();

                //graphics.RasterizerState = state;
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, triangles);
            }
        }
    }

    /// <summary>
    /// Extensions for IPrimitiveRenderer
    /// </summary>
    public static class PrimitiveRendererExtensions
    {
        /// <summary>
        /// Draws a set of lines. The lineType parameter controls how
        /// lines are connected.
        /// </summary>
        /// <param name="lineType">The type of lines to draw.</param>
        /// <param name="color">The color of lines to draw.</param>
        /// <param name="points">The points that are used to 
        /// build the individual line segments.</param>
        public static void DrawLines(this IPrimitiveRenderer primitives, Effect effect, LineType lineType, Color color,
            IEnumerable<Vector2> points)
        {
            primitives.DrawLines(effect, lineType, color, points);
        }

        /// <summary>
        /// Draws a filled convex polygon.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="points"></param>
        public static void FillPolygon(this IPrimitiveRenderer primitives, Effect effect, Color color, IReadOnlyList<Vector2> points)
        {
            primitives.FillConvexPolygon(effect, color, points);
        }

        /// <summary>
        ///     Draws a line between the two points specified.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void DrawLine(this IPrimitiveRenderer primitives, Effect effect, Color color, Vector2 a, Vector2 b)
        {
            primitives.DrawLines(effect, LineType.LineSegments, color, new[] { a, b });
        }

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void DrawRect(this IPrimitiveRenderer primitives, Effect effect, Color color, Rectangle rect)
        {
            primitives.DrawRect(effect, color, (RectangleF)rect);
        }

        /// <summary>
        /// Draws the outline of a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void DrawRect(this IPrimitiveRenderer primitives, Effect effect, Color color, RectangleF rect)
        {
            primitives.DrawLines(effect, LineType.Polygon, color,
                new QuadrilateralBuilder().BuildRectangle(rect));
        }

        /// <summary>
        /// Draws the outline of an ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
        public static void DrawEllipse(this IPrimitiveRenderer primitives, Effect effect, Color color, Rectangle boundingRect)
        {
            primitives.DrawEllipse(effect, color, (RectangleF)boundingRect);
        }

        /// <summary>
        /// Draws the outline of an ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect">The rectangle the circle should be inscribed in.</param>
        public static void DrawEllipse(this IPrimitiveRenderer primitives, Effect effect, Color color, RectangleF boundingRect)
        {
            primitives.DrawLines(effect, LineType.Polygon, color,
                new EllipseBuilder().BuildEllipse(boundingRect));
        }

        /// <summary>
        /// Draws an unfilled polygon.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="polygon"></param>
        public static void DrawPolygon(this IPrimitiveRenderer primitives, Effect effect, Color color, Polygon polygon)
        {
            primitives.DrawLines(effect, LineType.Polygon, color, polygon.Points);
        }

        /// <summary>
        /// Draws a filled polygon.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="polygon"></param>
        public static void FillPolygon(this IPrimitiveRenderer primitives, Effect effect, Color color, Polygon polygon)
        {
            if (polygon.IsConvex)
            {
                primitives.FillPolygon(effect, color, polygon.Points);
            }
            else
            {
                foreach (var convexPoly in polygon.ConvexDecomposition)
                {
                    primitives.FillPolygon(effect, color, convexPoly.Points);
                }
            }
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void FillRect(this IPrimitiveRenderer primitives, Effect effect, Color color, Rectangle rect)
        {
            primitives.FillRect(effect, color, (RectangleF)rect);
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public static void FillRect(this IPrimitiveRenderer primitives, Effect effect, Color color, RectangleF rect)
        {
            primitives.FillPolygon(effect, color, new QuadrilateralBuilder().BuildRectangle(rect));
        }

        /// <summary>
        /// Draws a filled ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect"></param>
        public static void FillEllipse(this IPrimitiveRenderer primitives, Effect effect, Color color, Rectangle boundingRect)
        {
            primitives.FillEllipse(effect, color, (RectangleF)boundingRect);
        }

        /// <summary>
        /// Draws a filled ellipse, inscribed inside a rectangle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="boundingRect"></param>
        public static void FillEllipse(this IPrimitiveRenderer primitives, Effect effect, Color color, RectangleF boundingRect)
        {
            primitives.FillPolygon(effect, color, new EllipseBuilder().BuildEllipse(boundingRect).ToArray());
        }

        /// <summary>
        /// Draws a filled ellipse.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="center"></param>
        /// <param name="majorAxisRadius"></param>
        /// <param name="minorAxisRadius"></param>
        /// <param name="rotationAngle"></param>
        public static void FillEllipse(this IPrimitiveRenderer primitives, Effect effect, Color color, Vector2 center,
            double majorAxisRadius, double minorAxisRadius, double rotationAngle)
        {
            primitives.FillPolygon(effect, color, new EllipseBuilder().BuildEllipse(center, majorAxisRadius, minorAxisRadius, rotationAngle).ToArray());
        }

        /// <summary>
        /// Draws a filled circle.
        /// </summary>
        /// <param name="primitives"></param>
        /// <param name="color"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void FillCircle(this IPrimitiveRenderer primitives, Effect effect, Color color, Vector2 center, double radius)
        {
            primitives.FillPolygon(effect, color, new EllipseBuilder().BuildCircle(center, radius).ToArray());
        }
    }
}