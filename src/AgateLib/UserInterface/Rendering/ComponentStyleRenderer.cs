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

using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AgateLib.UserInterface.Rendering
{
    public interface IComponentStyleRenderer : IDisposable
    {
        void DrawBackground(ICanvas canvas, BackgroundStyle background, Rectangle backgroundRect);

        void DrawFrame(ICanvas canvas, BorderStyle border, Rectangle borderRect);

        void DrawFrame(ICanvas canvas, Rectangle destOuterRect, Texture2D frameTexture,
            Rectangle frameSourceInner, Rectangle frameSourceOuter,
            ImageScale borderScale);
    }

    [Transient]
    public class ComponentStyleRenderer : IComponentStyleRenderer
    {
        private Texture2D blankSurface;

        public ComponentStyleRenderer(GraphicsDevice graphicsDevice,
                                      ITheme theme)
        {
            Theme = theme;

            blankSurface = new Texture2D(graphicsDevice, 10, 10);
            Color[] data = new Color[10 * 10];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }

            blankSurface.SetData(data);
        }

        public ITheme Theme { get; set; }

        public void Dispose()
        {
            blankSurface.Dispose();
        }

        public void DrawBackground(ICanvas canvas, BackgroundStyle background, Rectangle backgroundRect)
        {
            if (background == null)
            {
                return;
            }

            if (background.Id != null)
            {
                DrawBackgroundFromModel(canvas, Theme.Model.Backgrounds[background.Id], backgroundRect);
            }
            else
            {
                DrawBackgroundFromModel(canvas, background, backgroundRect);
            }
        }

        public void DrawBackgroundFromModel(ICanvas canvas, BackgroundModel background, Rectangle backgroundRect)
        {
            if (background.Color.A > 0)
            {
                canvas.Draw(
                    blankSurface,
                    backgroundRect,
                    background.Color);
            }

            if (string.IsNullOrEmpty(background.Image?.File) == false)
            {
                Texture2D backgroundImage = Theme.LoadContent<Texture2D>(ThemePathTypes.Images, background.Image.File);
                Point origin = backgroundRect.Location;
                Point backgroundPosition = background.Position;
                Rectangle srcRect = background.Image.SourceRect ?? new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);

                switch (background.ImageScale)
                {
                    case ImageScale.Tile:
                        origin.X += backgroundPosition.X;
                        origin.Y += backgroundPosition.Y;

                        switch (background.Repeat)
                        {
                            case BackgroundRepeat.None:
                                canvas.DrawClipped(backgroundImage, origin, backgroundRect, srcRect);
                                break;

                            case BackgroundRepeat.Repeat:
                                canvas.DrawRepeatedClipped(backgroundImage, origin, backgroundRect, true, true, srcRect);
                                break;

                            case BackgroundRepeat.Repeat_X:
                                canvas.DrawRepeatedClipped(backgroundImage, origin, backgroundRect, true, false, srcRect);
                                break;

                            case BackgroundRepeat.Repeat_Y:
                                canvas.DrawRepeatedClipped(backgroundImage, origin, backgroundRect, false, true, srcRect);
                                break;
                        }
                        break;

                    case ImageScale.Stretch:
                        canvas.Draw(backgroundImage, backgroundRect, srcRect, Color.White);
                        break;
                }
            }
        }

        public void DrawFrame(ICanvas canvas, BorderStyle border, Rectangle borderRect)
        {
            if (border == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(border.Image?.File))
            {
                DrawOrdinaryFrame(canvas, border, borderRect);
            }
            else
            {
                DrawImageFrame(canvas, border, borderRect, border.Image.SourceRect);
            }
        }

        private void DrawImageFrame(ICanvas canvas, BorderStyle border, Rectangle borderRect, Rectangle? maybeSrcRect)
        {
            var image = Theme.LoadContent<Texture2D>(ThemePathTypes.Images, border.Image.File);

            var slice = border.ImageSlice;
            Rectangle outerRect = maybeSrcRect ?? new Rectangle(0, 0, image.Width, image.Height);
            Rectangle innerRect = RectangleX.FromLTRB(
                outerRect.Left + slice.Left,
                outerRect.Top + slice.Top,
                outerRect.Right - slice.Right,
                outerRect.Bottom - slice.Bottom);

            DrawFrame(canvas, borderRect, image, innerRect, outerRect, border.ImageScale);
        }

        public void DrawFrame(ICanvas canvas, Rectangle destOuterRect, Texture2D frameTexture,
            Rectangle frameSourceInner, Rectangle frameSourceOuter,
            ImageScale borderScale)
        {
            Rectangle destInnerRect = destOuterRect;
            Size topLeftDelta = new Size(frameSourceInner.X - frameSourceOuter.X, frameSourceInner.Y - frameSourceOuter.Y);
            Size bottomRightDelta = new Size(frameSourceOuter.Right - frameSourceInner.Right, frameSourceOuter.Bottom - frameSourceInner.Bottom);

            destInnerRect.X += topLeftDelta.Width;
            destInnerRect.Y += topLeftDelta.Height;
            destInnerRect.Width -= topLeftDelta.Width + bottomRightDelta.Width;
            destInnerRect.Height -= topLeftDelta.Height + bottomRightDelta.Height;

            Rectangle src, dest;
            Rectangle outer = frameSourceOuter, inner = frameSourceInner;

            // top left
            src = RectangleX.FromLTRB(outer.Left, outer.Top, inner.Left, inner.Top);
            dest = RectangleX.FromLTRB(destOuterRect.Left, destOuterRect.Top, destInnerRect.Left, destInnerRect.Top);

            canvas.Draw(frameTexture, dest, src, Color.White);

            // top
            src = RectangleX.FromLTRB(inner.Left, outer.Top, inner.Right, inner.Top);
            dest = RectangleX.FromLTRB(destInnerRect.Left, destOuterRect.Top, destInnerRect.Right, destInnerRect.Top);

            ScaleSurface(canvas, frameTexture, src, dest, borderScale);

            // top right
            src = RectangleX.FromLTRB(inner.Right, outer.Top, outer.Right, inner.Top);
            dest = RectangleX.FromLTRB(destInnerRect.Right, destOuterRect.Top, destOuterRect.Right, destInnerRect.Top);

            canvas.Draw(frameTexture, dest, src, Color.White);

            // left
            src = RectangleX.FromLTRB(outer.Left, inner.Top, inner.Left, inner.Bottom);
            dest = RectangleX.FromLTRB(destOuterRect.Left, destInnerRect.Top, destInnerRect.Left, destInnerRect.Bottom);

            ScaleSurface(canvas, frameTexture, src, dest, borderScale);

            // right
            src = RectangleX.FromLTRB(inner.Right, inner.Top, outer.Right, inner.Bottom);
            dest = RectangleX.FromLTRB(destInnerRect.Right, destInnerRect.Top, destOuterRect.Right, destInnerRect.Bottom);

            ScaleSurface(canvas, frameTexture, src, dest, borderScale);

            // bottom left
            src = RectangleX.FromLTRB(outer.Left, inner.Bottom, inner.Left, outer.Bottom);
            dest = RectangleX.FromLTRB(destOuterRect.Left, destInnerRect.Bottom, destInnerRect.Left, destOuterRect.Bottom);

            canvas.Draw(frameTexture, dest, src, Color.White);

            // bottom
            src = RectangleX.FromLTRB(inner.Left, inner.Bottom, inner.Right, outer.Bottom);
            dest = RectangleX.FromLTRB(destInnerRect.Left, destInnerRect.Bottom, destInnerRect.Right, destOuterRect.Bottom);

            ScaleSurface(canvas, frameTexture, src, dest, borderScale);

            // bottom right
            src = RectangleX.FromLTRB(inner.Right, inner.Bottom, outer.Right, outer.Bottom);
            dest = RectangleX.FromLTRB(destInnerRect.Right, destInnerRect.Bottom, destOuterRect.Right, destOuterRect.Bottom);

            canvas.Draw(frameTexture, dest, src, Color.White);
        }

        private void DrawOrdinaryFrame(ICanvas canvas, BorderStyle border, Rectangle borderRect)
        {
            if (border.Top.Color.A == 0 &&
                border.Left.Color.A == 0 &&
                border.Right.Color.A == 0 &&
                border.Bottom.Color.A == 0)
            {
                return;
            }

            // draw top
            Rectangle rect = new Rectangle(borderRect.X, borderRect.Y, borderRect.Width, border.Top.Width);

            canvas.Draw(blankSurface, rect, border.Top.Color);

            // draw bottom
            rect = new Rectangle(borderRect.X, borderRect.Bottom - border.Bottom.Width, borderRect.Width, border.Bottom.Width);

            canvas.Draw(blankSurface, rect, border.Bottom.Color);

            // draw left
            rect = new Rectangle(borderRect.X, borderRect.Y, border.Left.Width, borderRect.Height);

            canvas.Draw(blankSurface, rect, border.Left.Color);

            // draw right
            rect = new Rectangle(borderRect.Right - border.Right.Width, borderRect.Y, border.Right.Width, borderRect.Height);

            canvas.Draw(blankSurface, rect, border.Right.Color);
        }

        private void ScaleSurface(ICanvas canvas, Texture2D texture, Rectangle src, Rectangle dest, ImageScale scale)
        {
            if (scale == ImageScale.Tile)
            {
                TileImage(canvas, texture, src, dest);
            }
            else
            {
                canvas.Draw(texture, dest, src, Color.White);
            }
        }

        private void TileImage(ICanvas canvas, Texture2D frameTexture, Rectangle src, Rectangle dest)
        {
            canvas.DrawRepeatedClipped(frameTexture, dest.Location, dest, true, true, src);
        }

    }

    public static class CanvasExtensions
    {
        public static void DrawClipped(this ICanvas canvas, Texture2D image, Point dest, Rectangle clipRect, Rectangle srcRect)
        {
            Rectangle destRect = new Rectangle(dest.X, dest.Y, srcRect.Width, srcRect.Height);

            if (clipRect.Contains(destRect) == false)
            {
                int lc = 0, tc = 0, rc = 0, bc = 0;

                if (destRect.Left < clipRect.Left)
                {
                    lc = clipRect.Left - destRect.Left;
                }

                if (destRect.Top < clipRect.Top)
                {
                    tc = clipRect.Top - destRect.Top;
                }

                if (destRect.Right > clipRect.Right)
                {
                    rc = clipRect.Right - destRect.Right;
                }

                if (destRect.Bottom > clipRect.Bottom)
                {
                    bc = clipRect.Bottom - destRect.Bottom;
                }

                destRect = RectangleX.FromLTRB(destRect.Left + lc, destRect.Top + tc, destRect.Right + rc, destRect.Bottom + bc);
                srcRect = RectangleX.FromLTRB(srcRect.Left + lc, srcRect.Top + tc, srcRect.Right + rc, srcRect.Bottom + bc);

                if (destRect.Width == 0 || destRect.Height == 0)
                {
                    return;
                }
            }

            canvas.Draw(image, destRect, srcRect, Color.White);
        }

        public static void DrawRepeatedClipped(this ICanvas canvas, Texture2D image, Point startPt, Rectangle clipRect, bool repeatX, bool repeatY, Rectangle srcRect)
        {
            int countX = (int)Math.Ceiling(clipRect.Width / (double)srcRect.Width);
            int countY = (int)Math.Ceiling(clipRect.Height / (double)srcRect.Height);

            if (repeatX && startPt.X != clipRect.X)
            {
                startPt.X -= image.Width;
            }

            if (repeatY && startPt.Y != clipRect.Y)
            {
                startPt.Y -= image.Height;
            }

            if (repeatX)
            {
                // TODO: Optimize this with the power of MATH!.
                while (startPt.X + image.Width < clipRect.X)
                {
                    startPt.X += image.Width;
                }
            }

            if (repeatY)
            {
                while (startPt.Y + image.Height < clipRect.Y)
                {
                    startPt.Y += image.Height;
                }
            }

            if (startPt.X + countX * image.Width < clipRect.Right)
            {
                countX++;
            }

            if (startPt.Y + countY * image.Height < clipRect.Bottom)
            {
                countY++;
            }

            if (repeatX == false)
            {
                countX = 1;
            }

            if (repeatY == false)
            {
                countY = 1;
            }

            for (int j = 0; j < countY; j++)
            {
                Point destPt = new Point(startPt.X, startPt.Y + j * srcRect.Height);

                for (int i = 0; i < countX; i++)
                {
                    DrawClipped(canvas, image, destPt, clipRect, srcRect);

                    destPt.X += srcRect.Width;
                }
            }
        }
    }
}
