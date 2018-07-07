﻿//
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
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AgateLib.UserInterface
{
    public class Workspace
    {
        class WorkspaceDisplaySystem : IDisplaySystem
        {
            private readonly Workspace workspace;

            public WorkspaceDisplaySystem(Workspace workspace)
            {
                this.workspace = workspace;
            }

            public IFontProvider Fonts { get; set; }

            public IRenderElement Focus
            {
                get => workspace.Focus;
                set => workspace.Focus = value;
            }

            public IInstructions Instructions { get; set; }

            public IRenderElement ParentOf(IRenderElement element)
            {
                IRenderElement parent = null;

                workspace.visualTree.Walk(e =>
                {
                    if (e.Children == null)
                        return true;

                    if (e.Children.Contains(element))
                    {
                        parent = e;
                    }

                    return parent == null;
                });

                return parent;
            }
        }

        private readonly VisualTree visualTree = new VisualTree();

        private readonly WorkspaceDisplaySystem displaySystem;

        private List<IWidget> legacyChildren = new List<IWidget>();
        private App legacyApp;

        private IRenderable app;
        private IWidget activeWindow;

        [Obsolete("Use overload which supplies root element instead.")]
        public Workspace(string name)
        {
            Name = name;
            displaySystem = new WorkspaceDisplaySystem(this);

            visualTree.DisplaySystem = displaySystem;
        }

        /// <summary>
        /// Initializes a workspace object.
        /// </summary>
        /// <param name="name">Name of the workspace.</param>
        public Workspace(string name, IRenderable root)
        {
            Name = name;
            displaySystem = new WorkspaceDisplaySystem(this);
            this.app = root;

            visualTree.DisplaySystem = displaySystem;
        }

        public IRenderElement Focus
        {
            get => visualTree.Focus;
            set => visualTree.Focus = value;
        }

        public event EventHandler<InputEventArgs> UnhandledEvent;


        public IStyleConfigurator Style
        {
            get => visualTree.Style;
            set => visualTree.Style = value ?? throw new ArgumentNullException(nameof(Style));
        }

        public IFontProvider Fonts
        {
            get => displaySystem.Fonts;
            set => displaySystem.Fonts = value ?? throw new ArgumentNullException(nameof(Fonts));
        }

        public IInstructions Instructions
        {
            get => displaySystem.Instructions;
            set => displaySystem.Instructions = value ?? throw new ArgumentNullException(nameof(Instructions));
        }

        [Obsolete("Use overload which sets root render element .")]
        public void Add(IWidget child)
        {
            legacyChildren.Add(child);
            legacyApp = new App(new AppProps { Children = legacyChildren.ToList<IRenderable>() });
            Render();
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

        [Obsolete]
        public IWidget ActiveWindow
        {
            get => activeWindow;
            set => throw new NotImplementedException();
        }

        public string DefaultTheme
        {
            get => visualTree.DefaultTheme;
            set => visualTree.DefaultTheme = value;
        }

        public void Clear()
        {
            legacyChildren.Clear();
        }

        public void HandleInputEvent(InputEventArgs args)
        {
            Focus?.OnInputEvent(args);

            if (!args.Handled)
                UnhandledEvent?.Invoke(this, args);
        }

        public void Update(IWidgetRenderContext renderContext)
        {
            foreach (var item in legacyChildren)
                item.Update(renderContext);

            visualTree.Update(renderContext);
        }

        public void Render()
        {
            if (displaySystem.Fonts == null)
                return;

            visualTree.Render(app ?? legacyApp);

            if (Focus == null)
            {
                visualTree.Walk(element =>
                {
                    if (element.CanHaveFocus)
                    {
                        Focus = element;
                        return false;
                    }

                    return true;
                });
            }
        }

        public void Draw(IWidgetRenderContext renderContext)
        {
            visualTree.Draw(renderContext, new Rectangle(Point.Zero, Size));
        }

        /// <summary>
        /// Explores the entire UI tree via a depth-first search.
        /// </summary>
        /// <param name="explorer"></param>
        public void Explore(Action<IRenderElement> explorer)
        {
            visualTree.Walk(element => { explorer(element); return true; });
        }

        public void Initialize()
        {
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
            ActiveWindow = window;

            //Layout.Focus = window;

            //if (behavior.HasFlag(WindowActivationBehaviors.BringToFront))
            //{
            //    window.Display.StackOrder = Layout.Items.Max(x => x.Display.StackOrder) + 1;
            //}
        }

        /// <summary>
        /// Finds a window that matches a user specified criteria.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IWidget FindWindow(Func<IWidget, bool> selector)
        {
            return legacyChildren.SingleOrDefault(selector);
        }

        public override string ToString()
        {
            return $"Workspace: {Name}";
        }

        public AnimationState AnimationState
        {
            get
            {
                if (visualTree.TreeRoot.Children.Any(x => x.Display.Animation.State == AnimationState.TransitionIn))
                    return AnimationState.TransitionIn;

                if (visualTree.TreeRoot.Children.Any(x => x.Display.Animation.State == AnimationState.TransitionOut))
                    return AnimationState.TransitionOut;

                if (visualTree.TreeRoot.Children.All(x => x.Display.Animation.State == AnimationState.Dead))
                    return AnimationState.Dead;

                return AnimationState.Static;
            }
        }

        internal VisualTree VisualTree { get => visualTree; }

        internal void TransitionOut()
        {
            foreach (var window in visualTree.TreeRoot.Children)
            {
                window.Display.Animation.State = AnimationState.TransitionOut;
            }
        }

        internal void TransitionIn()
        {
            foreach (var window in visualTree.TreeRoot.Children)
            {
                window.Display.Animation.State = AnimationState.TransitionIn;
            }
        }
    }
}
