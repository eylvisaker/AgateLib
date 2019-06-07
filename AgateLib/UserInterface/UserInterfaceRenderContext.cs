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
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Interface for a render context which provides functions needed to 
    /// render an entire GUI.
    /// </summary>
    public interface IUserInterfaceRenderContext
    {
        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }


        /// <summary>
        /// Gets the sprite batch object being used.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Gets the render target this widget should be drawing to.
        /// </summary>
        RenderTarget2D RenderTarget { get; }

        /// <summary>
        /// Gets the GameTime object.
        /// </summary>
        GameTime GameTime { get; }

        /// <summary>
        /// Font provider
        /// </summary>
        IFontProvider Fonts { get; }

        /// <summary>
        /// Gets the style renderer for the UI.
        /// </summary>
        IUserInterfaceRenderer UserInterfaceRenderer { get; }

        /// <summary>
        /// Gets a rectangle corresponding to the "in-bounds" render area.
        /// </summary>
        Rectangle ScreenArea { get; }

        /// <summary>
        /// Gets the pixel size of the graphics device's render target area.
        /// </summary>
        Size GraphicsDeviceRenderTargetSize { get; }

        /// <summary>
        /// Creates an IContentLayout object for the specified text.
        /// </summary>
        /// <param name="text">The text to layout. If localizeText is true, this text will be used
        /// as a key in the lookup table for the current language.</param>
        /// <param name="contentLayoutOptions">Options for content layout.</param>
        /// <param name="performLocalization">If true, the text will be localized. Defaults to true.</param>
        /// <returns></returns>
        IContentLayout CreateContentLayout(
            string text,
            ContentLayoutOptions contentLayoutOptions,
            bool performLocalization = true);

        /// <summary>
        /// Draws a child widget of the current widget.
        /// </summary>
        /// <param name="parentContentArea"></param>
        /// <param name="child"></param>
        void DrawChild(Rectangle parentContentArea, IRenderElement child);

        /// <summary>
        /// Draws a collection of child widgets of the current widget.
        /// </summary>
        /// <param name="contentArea"></param>
        /// <param name="children"></param>
        void DrawChildren(Rectangle contentArea, IEnumerable<IRenderElement> children);

        /// <summary>
        /// Prepares the render context for an update.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="instructions"></param>
        void InitializeUpdate(GameTime time);

        /// <summary>
        /// Prepares the render context for drawing.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="renderTarget"></param>
        void PrepDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget);

        /// <summary>
        /// Updates the animation state for the specified element.
        /// </summary>
        /// <param name="element"></param>
        void UpdateAnimation(IRenderElement element);

        void Draw(Texture2D image, Rectangle destRect, Color color);

        void Draw(Texture2D image, Rectangle destRect, Rectangle sourceRect, Color color);

        void Draw(IContentLayout content, Vector2 destination);

        void Draw(IContentLayout content, Rectangle destinationArea);

        void DrawText(Font font, Vector2 destination, string text);
    }

    public class UserInterfaceRenderContext : IUserInterfaceRenderContext
    {
        private readonly IContentLayoutEngine contentLayoutEngine;
        private readonly IFontProvider fonts;
        private readonly IAnimationFactory animationFactory;

        private UserInterfaceRenderContext parentRenderContext;
        private bool workspaceIsActive;

        public UserInterfaceRenderContext(Rectangle screenArea,
            GraphicsDevice graphicsDevice,
            IContentLayoutEngine contentLayoutEngine,
            IUserInterfaceRenderer uiRenderer,
            IStyleConfigurator styleConfigurator,
            IFontProvider fonts,
            IAnimationFactory animation,
            RenderTarget2D renderTarget = null,
            SpriteBatch spriteBatch = null,
            IDoubleBuffer doubleBuffer = null)
        {
            this.GraphicsDevice = graphicsDevice;
            this.contentLayoutEngine = contentLayoutEngine;
            this.UserInterfaceRenderer = uiRenderer;
            this.fonts = fonts;
            this.animationFactory = animation;
            this.RenderTarget = renderTarget;
            this.SpriteBatch = spriteBatch ?? new SpriteBatch(graphicsDevice);
            this.DoubleBuffer = doubleBuffer ?? new DoubleBuffer();
        }

        public UserInterfaceRenderContext(UserInterfaceRenderContext parent,
            SpriteBatch spriteBatch,
            RenderTarget2D renderTarget)
        {
            this.ScreenArea = parent.ScreenArea;
            this.GraphicsDevice = parent.GraphicsDevice;
            this.SpriteBatch = spriteBatch;
            this.RenderTarget = renderTarget;

            this.contentLayoutEngine = parent.contentLayoutEngine;
            this.GameTime = parent.GameTime;
            this.UserInterfaceRenderer = parent.UserInterfaceRenderer;
            this.DoubleBuffer = parent.DoubleBuffer;

            this.WorkspaceIsActive = parent.WorkspaceIsActive;

            this.parentRenderContext = parent;
        }

        public GraphicsDevice GraphicsDevice { get; }

        public SpriteBatch SpriteBatch { get; set; }

        public RenderTarget2D RenderTarget { get; set; }

        public IFontProvider Fonts => fonts;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public IDoubleBuffer DoubleBuffer { get; set; }

        public GameTime GameTime { get; set; }

        public Rectangle ScreenArea { get; set; }

        public Size GraphicsDeviceRenderTargetSize => ScreenArea.Size;

        public bool WorkspaceIsActive
        {
            get
            {
                if (parentRenderContext != null)
                    return parentRenderContext.WorkspaceIsActive;

                return workspaceIsActive;
            }
            set => workspaceIsActive = value;
        }

        public void UpdateAnimation(IRenderElement element)
        {
            UserInterfaceRenderer.UpdateAnimation(this, element);

            if (element.Display.IsDoubleBuffered)
            {
                IUserInterfaceRenderContext newContext 
                    = DoubleBuffer.PrepRenderState(element, this);

                Rectangle rtClientDest 
                    = element.Display.Animation.Buffer.ContentDestination;

                UserInterfaceRenderer.DrawBackground(newContext, element.Display, rtClientDest);
                UserInterfaceRenderer.DrawFrame(newContext, element.Display, rtClientDest);

                rtClientDest.X -= element.Display.ScrollPosition.X;
                rtClientDest.Y -= element.Display.ScrollPosition.Y;

                bool clip = false;
                Rectangle oldClipRect = GraphicsDevice.ScissorRectangle;

                if (element.Display.HasOverflow != HasOverflow.None
                    && element.Style.Overflow != Overflow.Visible)
                {
                    DoubleBuffer.Flush(newContext);

                    Rectangle clipRect = new Rectangle(Point.Zero,
                                                       element.Display.ContentRect.Size);

                    clipRect.X = element.Display.Region.MarginToContentOffset.Left;
                    clipRect.Y = element.Display.Region.MarginToContentOffset.Top;

                    clip = true;

                    GraphicsDevice.ScissorRectangle = clipRect;
                }

                element.Events.BeforeDraw?.Invoke(newContext, rtClientDest);
                element.Draw(newContext, rtClientDest);
                element.Events.AfterDraw?.Invoke(newContext, rtClientDest);

                DoubleBuffer.CompleteRendering(this, element);
            }
        }

        public void DrawChild(Rectangle parentContentDest, IRenderElement element)
        {
            var animation = element.Display.Animation;

            if (element.Display.IsDoubleBuffered)
            {
                if (animation.RenderTarget != null)
                {
                    var screenRect = animation.AnimatedMarginRect;
                    screenRect.X += parentContentDest.X;
                    screenRect.Y += parentContentDest.Y;

                    SpriteBatch.Draw(
                        animation.RenderTarget,
                        screenRect,
                        null,
                        animation.Color,
                        0,
                        Vector2.Zero,
                        SpriteEffects.None,
                        animation.LayerDepth);
                }
            }
            else
            {
                Rectangle dest = new Rectangle(
                    parentContentDest.X + element.Display.Animation.AnimatedContentRect.X,
                    parentContentDest.Y + element.Display.Animation.AnimatedContentRect.Y,
                    element.Display.Animation.AnimatedContentRect.Width,
                    element.Display.Animation.AnimatedContentRect.Height);

                Rectangle oldScissor = GraphicsDevice.ScissorRectangle;

                UserInterfaceRenderer.DrawBackground(this, element.Display, dest);
                UserInterfaceRenderer.DrawFrame(this, element.Display, dest);

                element.Events.BeforeDraw?.Invoke(this, dest);
                element.Draw(this, dest);
                element.Events.AfterDraw?.Invoke(this, dest);
            }
        }

        public void DrawChildren(Rectangle contentDest, IEnumerable<IRenderElement> items)
        {
            foreach (var child in items.Where(x => x.Display.IsVisible))
            {
                DrawChild(contentDest, child);
            }
        }

        public IContentLayout CreateContentLayout(
            string text,
            ContentLayoutOptions contentLayoutOptions,
            bool performLocalization = true)
        {
            return contentLayoutEngine.LayoutContent(text, contentLayoutOptions, performLocalization);
        }

        public void InitializeUpdate(GameTime time)
        {
            GameTime = time;
        }

        public void PrepDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            GameTime = time;
            SpriteBatch = spriteBatch;
            RenderTarget = renderTarget;
        }

        public void Draw(Texture2D image, Rectangle destRect, Color color)
        {
            SpriteBatch.Draw(image, destRect, color);
        }

        public void Draw(Texture2D image, Rectangle destRect, Rectangle sourceRect, Color color)
        {
            SpriteBatch.Draw(image, destRect, sourceRect, color);
        }

        public void DrawText(Font font, Vector2 destination, string text)
        {
            font.DrawText(SpriteBatch, destination, text);
        }

        public void Draw(IContentLayout content, Vector2 destination)
        {
            content.Draw(destination, SpriteBatch);
        }

        public void Draw(IContentLayout content, Rectangle destinationArea)
        {
            content.Draw(destinationArea.Location.ToVector2(), SpriteBatch);
        }
    }
}
