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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Content;
using System.Linq;
using AgateLib.Mathematics.Geometry;
using System;
using AgateLib.UserInterface.Styling;

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// Interface for a render context which provides functions needed to 
    /// render an entire GUI.
    /// </summary>
    public interface IWidgetRenderContext
    {
        /// <summary>
        /// Event which is raised before each widget is updated.
        /// </summary>
        event Action<IRenderElement> BeforeUpdate;

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
        Rectangle Area { get; }

        /// <summary>
        /// Gets the pixel size of the graphics device's render target area.
        /// </summary>
        Size GraphicsDeviceRenderTargetSize { get; }

        /// <summary>
        /// Gets the object which handles rendering of the menu indicator.
        /// </summary>
        [Obsolete("This shouldn't be public")]
        IMenuIndicatorRenderer Indicator { get; }

        /// <summary>
        /// Creates an IContentLayout object for the specified text.
        /// </summary>
        /// <param name="text">The text to layout. If localizeText is true, this text will be used
        /// as a key in the lookup table for the current language.</param>
        /// <param name="contentLayoutOptions">Options for content layout.</param>
        /// <param name="localizeText">If true, the text will be localized. Defaults to true.</param>
        /// <returns></returns>
        IContentLayout CreateContentLayout(
            string text,
            ContentLayoutOptions contentLayoutOptions,
            bool localizeText = true);
        
        /// <summary>
        /// Draws a child widget of the current widget.
        /// </summary>
        /// <param name="contentArea"></param>
        /// <param name="child"></param>
        void DrawChild(Rectangle contentArea, IRenderElement child);

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
        void BeginDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget);

        void DrawWorkspace(Workspace workspace, IEnumerable<IRenderWidget> items);
        void DrawWorkspace(Workspace workspace, VisualTree visualTree);

        void UpdateAnimation(IRenderElement x);
    }

    public class WidgetRenderContext : IWidgetRenderContext
    {
        private readonly ILocalizedContentLayoutEngine contentLayoutEngine;
        private readonly IFontProvider fonts;
        private IRenderElement lastParentWidget;

        private WidgetRenderContext parentRenderContext;
        bool workspaceIsActive;

        private InputEventArgs eventArgs = new InputEventArgs();

        public WidgetRenderContext(
            GraphicsDevice graphicsDevice,
            ILocalizedContentLayoutEngine contentLayoutEngine,
            IUserInterfaceRenderer uiRenderer,
            IStyleConfigurator styleConfigurator,
            IFontProvider fonts,
            RenderTarget2D renderTarget = null,
            SpriteBatch spriteBatch = null,
            IDoubleBuffer doubleBuffer = null)
        {
            this.GraphicsDevice = graphicsDevice;
            this.contentLayoutEngine = contentLayoutEngine;
            this.UserInterfaceRenderer = uiRenderer;
            this.StyleConfigurator = styleConfigurator;
            this.fonts = fonts;
            this.RenderTarget = renderTarget;
            this.SpriteBatch = spriteBatch ?? new SpriteBatch(graphicsDevice);
            this.DoubleBuffer = doubleBuffer ?? new DoubleBuffer();
        }

        public WidgetRenderContext(WidgetRenderContext parent,
            SpriteBatch spriteBatch,
            RenderTarget2D renderTarget)
        {
            this.GraphicsDevice = parent.GraphicsDevice;
            this.SpriteBatch = spriteBatch;
            this.RenderTarget = renderTarget;

            this.contentLayoutEngine = parent.contentLayoutEngine;
            this.GameTime = parent.GameTime;
            this.UserInterfaceRenderer = parent.UserInterfaceRenderer;
            this.DoubleBuffer = parent.DoubleBuffer;

            this.lastParentWidget = parent.lastParentWidget;
            this.WorkspaceIsActive = parent.WorkspaceIsActive;

            this.parentRenderContext = parent;
        }

        /// <summary>
        /// Event that is raised before a widget is updated.
        /// </summary>
        public event Action<IRenderElement> BeforeUpdate;

        public GraphicsDevice GraphicsDevice { get; }

        public SpriteBatch SpriteBatch { get; set; }

        public RenderTarget2D RenderTarget { get; set; }

        public IFontProvider Fonts => fonts;

        public IUserInterfaceRenderer UserInterfaceRenderer { get; set; }

        public IStyleConfigurator StyleConfigurator { get; }

        public IDoubleBuffer DoubleBuffer { get; set; }

        public GameTime GameTime { get; set; }

        public IMenuIndicatorRenderer Indicator { get; set; }

        public Rectangle Area
        {
            get
            {
                if (RenderTarget != null)
                    return new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height);

                return new Rectangle(0, 0,
                    GraphicsDevice.PresentationParameters.BackBufferWidth,
                    GraphicsDevice.PresentationParameters.BackBufferHeight);
            }
        }

        public Size GraphicsDeviceRenderTargetSize
        {
            get
            {
                var renderTargets = GraphicsDevice.GetRenderTargets();

                if (renderTargets.Length == 0)
                {
                    return new Size(
                        GraphicsDevice.PresentationParameters.BackBufferWidth,
                        GraphicsDevice.PresentationParameters.BackBufferHeight);
                }
                else
                {
                    var renderTarget = (Texture2D)GraphicsDevice.GetRenderTargets()[0].RenderTarget;

                    return new Size(renderTarget.Width, renderTarget.Height);
                }
            }
        }

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

        public void UpdateAnimation(IRenderElement widget)
        {
            UserInterfaceRenderer.UpdateAnimation(this, widget);
        }
        
        public void DrawChild(Rectangle parentContentDest, IRenderElement widget)
        {
            if (widget.Display.Animator.IsDoubleBuffered)
            {
                var newContext = DoubleBuffer.PrepRenderState(widget, this);

                var rtClientDest = widget.Display.Animator.Buffer.ContentDestination;

                UserInterfaceRenderer.DrawBackground(newContext, widget.Display, rtClientDest);
                UserInterfaceRenderer.DrawFrame(newContext, widget.Display, rtClientDest);

                widget.Draw(newContext, rtClientDest);

                DoubleBuffer.CompleteRendering(parentContentDest, this, widget);
            }
            else
            {

                Rectangle dest = new Rectangle(
                    parentContentDest.X + widget.Display.Animator.AnimatedContentRect.X,
                    parentContentDest.Y + widget.Display.Animator.AnimatedContentRect.Y,
                    widget.Display.Animator.AnimatedContentRect.Width,
                    widget.Display.Animator.AnimatedContentRect.Height);

                UserInterfaceRenderer.DrawBackground(this, widget.Display, dest);
                UserInterfaceRenderer.DrawFrame(this, widget.Display, dest);

                widget.Draw(this, dest);
            }

            eventArgs.Initialize(WidgetEventType.DrawComplete);
            eventArgs.Area = parentContentDest;

            widget.OnInputEvent(eventArgs);
        }

        public void DrawWorkspace(Workspace workspace, IEnumerable<IRenderWidget> items)
        {
            WorkspaceIsActive = workspace.IsActive;

            DrawChildren(new Rectangle(Point.Zero, workspace.Size), items);

            EndWorkspace(workspace);
        }

        [Obsolete]
        public void DrawWorkspace(Workspace workspace, VisualTree visualTree)
        {
            WorkspaceIsActive = workspace.IsActive;

            DrawChildren(new Rectangle(), new[] { visualTree.TreeRoot });

            EndWorkspace(workspace);
        }

        public void DrawChildren(Rectangle contentDest, IEnumerable<IRenderWidget> children)
        {
            DrawChildren(contentDest, children.Cast<IRenderElement>());
        }

        public void DrawChildren(Rectangle contentDest, IEnumerable<IRenderElement> items)
        {
            var oldParent = lastParentWidget;

            foreach (var child in items.Where(x => x.Display.IsVisible)
                .OrderBy(x => x.Display.StackOrder))
            {
                lastParentWidget = child;
                DrawChild(contentDest, child);
            }

            lastParentWidget = oldParent;
        }

        public IContentLayout CreateContentLayout(
            string text,
            ContentLayoutOptions contentLayoutOptions,
            bool localizeText = true)
        {
            return contentLayoutEngine.LayoutContent(text, contentLayoutOptions, localizeText);
        }

        public void InitializeUpdate(GameTime time)
        {
            GameTime = time;
        }

        public void BeginDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            GameTime = time;
            SpriteBatch = spriteBatch;
            RenderTarget = renderTarget;
        }
        
        public void EndWorkspace(Workspace workspace)
        {
            if (WorkspaceIsActive)
            {
                Indicator?.DrawFocus(this, workspace);
            }
        }
    }
}
