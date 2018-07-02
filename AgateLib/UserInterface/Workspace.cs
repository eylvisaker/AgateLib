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
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
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
        }

        [Obsolete("Is this needed?")]
        private readonly SizeMetrics screenMetrics = new SizeMetrics();

        [Obsolete("Is this needed?")]
        private readonly WidgetRegion region
            = new WidgetRegion(new RenderElementStyle(new RenderElementDisplay(), new InlineElementStyle()));

        private readonly VisualTree visualTree = new VisualTree();

        private List<IWidget> children = new List<IWidget>();

        private IWidget activeWindow;

        /// <summary>
        /// Initializes a workspace object.
        /// </summary>
        /// <param name="name">Name of the workspace.</param>
        public Workspace(string name)
        {
            Name = name;

            displaySystem = new WorkspaceDisplaySystem(this);

            visualTree.DisplaySystem = displaySystem;
        }

        public IRenderElement Focus
        {
            get => visualTree.Focus;
            set => visualTree.Focus = value;
        }

        public event EventHandler<InputEventArgs> UnhandledEvent;

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

        public void Add(IWidget child)
        {
            children.Add(child);
        }

        /// <summary>
        /// The size of the screen area where the UI controls will be layed out.
        /// </summary>
        public Size Size { get; set; }

        public string Name { get; private set; }

        private readonly WorkspaceDisplaySystem displaySystem;

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
            children.Clear();
        }

        public bool Contains(IWidget window)
        {
            return children.Contains(window);
        }

        public void HandleInputEvent(InputEventArgs args)
        {
            Focus.OnInputEvent(args);

            if (!args.Handled)
                UnhandledEvent?.Invoke(this, args);
        }

        public void Update(IWidgetRenderContext renderContext)
        {
            foreach (var item in children)
                item.Update(renderContext);

            visualTree.Update(renderContext);
        }

        public void Render()
        {
            visualTree.Render(new FlexBox(new FlexBoxProps
            {
                StyleId = Name,
                StyleTypeId = "workspace",
                Children = children.ToList<IRenderable>()
            }));

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
        /// <param name="explorer">A function which takes two parameters: the parent
        /// widget and the child widget. Parent widget will be null for top level
        /// widgets.</param>
        public void Explore(Action<IRenderElement, IRenderElement> explorer)
        {
            void ExploreInner(IRenderElement parent)
            {
                if (parent.Children == null)
                    return;

                foreach (var item in parent.Children)
                {
                    explorer(parent, item);

                    ExploreInner(item);
                }
            }

            //foreach(var item in Layout.Items)
            //{
            //    explorer(null, item);

            //    ExploreInner(item);
            //}
            throw new NotImplementedException();
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
            return children.SingleOrDefault(selector);
        }

        public override string ToString()
        {
            return $"Workspace: {Name}";
        }

        public AnimationState AnimationState
        {
            get
            {
                if (visualTree.TreeRoot.Children.Any(x => x.Display.Animator.State == AnimationState.TransitionIn))
                    return AnimationState.TransitionIn;

                if (visualTree.TreeRoot.Children.Any(x => x.Display.Animator.State == AnimationState.TransitionOut))
                    return AnimationState.TransitionOut;

                if (visualTree.TreeRoot.Children.All(x => x.Display.Animator.State == AnimationState.Dead))
                    return AnimationState.Dead;

                return AnimationState.Static;
            }
        }

        public IStyleConfigurator Style
        {
            get => visualTree.Style;
            set => visualTree.Style = value;
        }

        internal void TransitionOut()
        {
            foreach (var window in visualTree.TreeRoot.Children)
            {
                window.Display.Animator.State = AnimationState.TransitionOut;
            }
        }

        internal void TransitionIn()
        {
            foreach (var window in visualTree.TreeRoot.Children)
            {
                window.Display.Animator.State = AnimationState.TransitionIn;
            }
        }
    }
}
