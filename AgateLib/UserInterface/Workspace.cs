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
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface
{
    public class Workspace
    {
        private class WorkspaceRenderContext : IWidgetRenderContext
        {
            private IWidgetRenderContext rootRenderContext;

            public WorkspaceRenderContext(Workspace workspace)
            {
                Workspace = workspace;
            }

            public Workspace Workspace { get; }

            public GraphicsDevice GraphicsDevice => rootRenderContext.GraphicsDevice;

            public SpriteBatch SpriteBatch => rootRenderContext.SpriteBatch;

            public RenderTarget2D RenderTarget => rootRenderContext.RenderTarget;

            public GameTime GameTime => rootRenderContext.GameTime;

            public IUserInterfaceRenderer UserInterfaceRenderer => rootRenderContext.UserInterfaceRenderer;

            public IMenuIndicatorRenderer Indicator => rootRenderContext.Indicator;

            public Rectangle Area => rootRenderContext.Area;

            public Size GraphicsDeviceRenderTargetSize => rootRenderContext.GraphicsDeviceRenderTargetSize;

            public IWidgetRenderContext RootRenderContext
            {
                get => rootRenderContext;
                set => rootRenderContext = value ?? throw new ArgumentNullException();
            }

            public event Action<IWidget> BeforeUpdate
            {
                add
                {
                    rootRenderContext.BeforeUpdate += value;
                }
                remove
                {
                    rootRenderContext.BeforeUpdate -= value;
                }
            }

            public void ApplyStyles(IEnumerable<IWidget> items, string defaultTheme)
            {
                rootRenderContext.ApplyStyles(items, defaultTheme);
            }

            public IContentLayout CreateContentLayout(string text, ContentLayoutOptions contentLayoutOptions, bool localizeText = true)
            {
                return rootRenderContext.CreateContentLayout(text, contentLayoutOptions, localizeText);
            }

            public void DrawChild(Point contentDest, IWidget child)
            {
                rootRenderContext.DrawChild(contentDest, child);
            }

            public void DrawChildren(Point contentDest, IEnumerable<IWidget> children)
            {
                rootRenderContext.DrawChildren(contentDest, children);
            }
            
            public void BeginDraw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
            {
                rootRenderContext.BeginDraw(time, spriteBatch, renderTarget);
            }

            public void InitializeUpdate(GameTime time)
            {
                rootRenderContext.InitializeUpdate(time);
            }

            public void Update(IWidget widget)
            {
                widget.Display.Instructions = Workspace.Instructions;
                
                UserInterfaceRenderer?.UpdateAnimation(this, widget);

                widget.Update(this);
            }

            public void Update(IEnumerable<IWidget> items)
            {
                foreach (var item in items)
                {
                    Update(item);
                }
            }

            public void DrawWorkspace(Workspace workspace, IEnumerable<IWidget> items)
            {
                rootRenderContext.DrawWorkspace(workspace, items);
            }
        }

        private readonly SizeMetrics screenMetrics = new SizeMetrics();
        private readonly WidgetRegion region = new WidgetRegion(new WidgetStyle());

        private IWidgetLayout layout;
        private IInstructions instructions;

        private WorkspaceRenderContext workspaceRenderContext;

        /// <summary>
        /// Initializes the workspace object.
        /// </summary>
        /// <param name="name">Name of the workspace.</param>
        /// <param name="anchor">Specifies the alighment for the default layout, which is
        /// an AnchoredLayout object.</param>
        public Workspace(string name, OriginAlignment anchor = OriginAlignment.Center)
        {
            workspaceRenderContext = new WorkspaceRenderContext(this);

            Name = name;

            Layout = new AnchoredLayout { Anchor = anchor };
        }

        public event EventHandler<WidgetEventArgs> UnhandledEvent;

        public IInstructions Instructions
        {
            get => instructions;
            set => instructions = value ?? throw new ArgumentNullException(nameof(Instructions));
        }

        public IWidgetLayout Layout
        {
            get => layout;
            set
            {
                if (layout != null)
                    layout.WidgetAdded -= Layout_WidgetAdded;

                layout = value;

                layout.WidgetAdded += Layout_WidgetAdded;

                foreach (var window in layout)
                    Layout_WidgetAdded(window);
            }
        }

        private void Layout_WidgetAdded(IWidget widget)
        {
            widget.Display.Animation.IsDoubleBuffered = true;
            
            // TODO: Figure out how to apply a style when a widget is added while
            // the workspace is active.

            // ApplyStyles(widget);
        }

        /// <summary>
        /// The size of the screen area where the UI controls will be layed out.
        /// </summary>
        public Size Size { get; set; }

        public string Name { get; private set; }

        /// <summary>
        /// Gets whether the workspace is the active workspace.
        /// </summary>
        public bool IsActive { get; internal set; }
        
        public IWidget ActiveWindow => Layout.Focus;

        public string DefaultTheme { get; set; }

        public void Clear()
        {
            Layout.Clear();
        }

        public void Add(IWidget window)
        {
            Layout.Add(window);

            UpdateLayout();
        }

        public bool Contains(IWidget window)
        {
            return Layout.Contains(window);
        }

        public void HandleInputEvent(WidgetEventArgs args)
        {
            Layout.InputEvent(args);

            if (!args.Handled)
                UnhandledEvent?.Invoke(this, args);
        }

        public void Draw(IWidgetRenderContext renderContext)
        {
            renderContext.DrawWorkspace(this, Layout.Items);
        }

        public void Update(IWidgetRenderContext renderContext)
        {
            workspaceRenderContext.RootRenderContext = renderContext;

            screenMetrics.ParentMaxSize = Size;
            region.ContentSize = Size;

            renderContext.ApplyStyles(Layout.Items, DefaultTheme);

            UpdateLayout();

            workspaceRenderContext.Update(layout.Items);
        }

        private void UpdateLayout()
        {
            if (workspaceRenderContext.RootRenderContext == null)
                return;

            Layout.ComputeIdealSize(Size, workspaceRenderContext);
            Layout.ApplyLayout(Size, workspaceRenderContext);
        }

        /// <summary>
        /// Explores the entire UI tree via a depth-first search.
        /// </summary>
        /// <param name="explorer">A function which takes two parameters: the parent
        /// widget and the child widget. Parent widget will be null for top level
        /// widgets.</param>
        public void Explore(Action<IWidget, IWidget> explorer)
        {
            void ExploreInner(IWidget parent)
            {
                if (parent.Children == null)
                    return;

                foreach(var item in parent.Children)
                {
                    explorer(parent, item);

                    ExploreInner(item);
                }
            }

            foreach(var item in Layout.Items)
            {
                explorer(null, item);

                ExploreInner(item);
            }
        }

        public void Initialize()
        {
            Explore((parent, child) =>
            {
                child.Display.Instructions = Instructions;
                child.Initialize();
            });
        }

        /// <summary>
        /// Activates the window with the specified name.
        /// Throws an InvalidOperationException if unsuccessful.
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public void ActivateWindow(string windowName, WindowActivationBehaviors behavior = WindowActivationBehaviors.Default)
        {
            IWidget window = FindWindow(w => w.Name.Equals(windowName, StringComparison.OrdinalIgnoreCase));

            if (window == null)
                throw new InvalidOperationException($"Could not find window {windowName}");

            ActivateWindow(window, behavior);
        }

        /// <summary>
        /// Activates the window. 
        /// </summary>
        /// <param name="window"></param>
        public void ActivateWindow(IWidget window, WindowActivationBehaviors behavior = WindowActivationBehaviors.Default)
        {
            Layout.Focus = window;

            if (behavior.HasFlag(WindowActivationBehaviors.BringToFront))
            {
                window.Display.StackOrder = Layout.Items.Max(x => x.Display.StackOrder) + 1;
            }
        }

        /// <summary>
        /// Finds a window that matches a user specified criteria.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IWidget FindWindow(Func<IWidget, bool> selector)
        {
            return Layout.Items.SingleOrDefault(selector);
        }

        /// <summary>
        /// Finds a window that matches a user specified criteria.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public T FindWindow<T>(Func<T, bool> selector) where T : IWidget
        {
            return Layout.Items.OfType<T>().SingleOrDefault(selector);
        }

        public override string ToString()
        {
            return $"Workspace: {Name}";
        }

        public AnimationState AnimationState
        {
            get
            {
                if (Layout.Items.Any(x => x.Display.Animation.State == AnimationState.TransitionIn))
                    return AnimationState.TransitionIn;

                if (Layout.Items.Any(x => x.Display.Animation.State == AnimationState.TransitionOut))
                    return AnimationState.TransitionOut;

                if (Layout.Items.All(x => x.Display.Animation.State == AnimationState.Dead))
                    return AnimationState.Dead;

                return AnimationState.Static;
            }
        }
        internal void TransitionOut()
        {
            foreach (var window in Layout.Items)
            {
                window.Display.Animation.State = AnimationState.TransitionOut;
            }
        }

        internal void TransitionIn()
        {
            foreach(var window in Layout.Items)
            {
                window.Display.Animation.State = AnimationState.TransitionIn;
            }
        }

    }

    public static class WorkspaceExtensions
    {
        public static void ApplyStyle(this Workspace workspace, IWidgetRenderContext renderContext, params IWidget[] widgets)
        {
            renderContext.ApplyStyles(widgets, workspace.DefaultTheme);
        }


    }
}
