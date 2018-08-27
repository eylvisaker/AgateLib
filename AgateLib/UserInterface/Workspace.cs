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
#define __DEBUG_WORKSPACE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;
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

            public Desktop Desktop { get; set; }

            public IFontProvider Fonts { get; set; }

            public IRenderElement Focus
            {
                get => workspace.Focus;
            }

            public void SetFocus(IRenderElement newFocus)
            {
                workspace.Focus = newFocus;
            }

            public IInstructions Instructions { get; set; }

            public IUserInterfaceAudio Audio { get; set; }

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

            public void PushWorkspace(Workspace newWorkspace)
            {
                Desktop.PushWorkspace(newWorkspace);
            }

            public void PopWorkspace()
            {
                Desktop.PopWorkspace();
            }

            public void PlaySound(object originator, UserInterfaceSound sound)
            {
                Audio?.PlaySound(originator, sound);
            }
        }

        private readonly VisualTree visualTree = new VisualTree();
        private readonly WorkspaceDisplaySystem displaySystem;
        private readonly IRenderable app;

        /// <summary>
        /// Initializes a workspace object.
        /// </summary>
        /// <param name="name">Name of the workspace.</param>
        public Workspace(string name, IRenderable root)
        {
            Name = name;

            displaySystem = new WorkspaceDisplaySystem(this);
            visualTree.DisplaySystem = displaySystem;

            this.app = root;
        }

        public IRenderElement Focus
        {
            get => visualTree.Focus;
            set => visualTree.Focus = value;
        }

        public event Action<UserInterfaceActionEventArgs> UnhandledEvent;
        public event Action BeforeTransitionOut;

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

        public IUserInterfaceAudio Audio
        {
            get => displaySystem.Audio;
            set => displaySystem.Audio = value;
        }

        /// <summary>
        /// The area of the screen area where the UI controls will be layed out.
        /// </summary>
        public Rectangle ScreenArea { get; set; }

        public string Name { get; private set; }

        /// <summary>
        /// Gets whether the workspace is the active workspace.
        /// </summary>
        public bool IsActive { get; internal set; }

        public string DefaultTheme
        {
            get => visualTree.DefaultTheme;
            set => visualTree.DefaultTheme = value;
        }

        public void HandleUIAction(UserInterfaceActionEventArgs args)
        {
            DebugMsg($"Handling UI action {args.Action}...");

            Focus?.OnUserInterfaceAction(args);

            if (!args.Handled)
                UnhandledEvent?.Invoke(args);
        }

        public void Update(IUserInterfaceRenderContext renderContext)
        {
            visualTree.Update(renderContext);
        }

        public void Render()
        {
            if (displaySystem.Fonts == null)
                return;

            visualTree.Render(app);

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

        public void Draw(IUserInterfaceRenderContext renderContext)
        {
            visualTree.Draw(renderContext, ScreenArea);
        }

        /// <summary>
        /// Explores the entire UI tree via a depth-first search.
        /// </summary>
        /// <param name="explorer"></param>
        public void Explore(Action<IRenderElement> explorer)
        {
            visualTree.Walk(element => { explorer(element); return true; });
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

        internal Desktop Desktop
        {
            get => this.displaySystem.Desktop;
            set => this.displaySystem.Desktop = value;
        }

        internal void TransitionOut()
        {
            BeforeTransitionOut?.Invoke();

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

        [Conditional("__DEBUG_WORKSPACE")]
        private void DebugMsg(string message)
        {
            Log.Debug(message);
        }
    }
}
