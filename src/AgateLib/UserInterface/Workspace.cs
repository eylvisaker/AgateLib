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
#define __DEBUG_WORKSPACE

using AgateLib.Display;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace AgateLib.UserInterface
{
    public class Workspace
    {
        private class WorkspaceDisplaySystem : IDisplaySystem
        {
            private readonly Workspace workspace;

            public WorkspaceDisplaySystem(Workspace workspace)
            {
                this.workspace = workspace;
            }

            public Desktop Desktop { get; set; }

            public string DefaultTheme
            {
                get => Desktop.DefaultTheme;
                set => Desktop.DefaultTheme = value;
            }

            public float VisualScaling
            {
                get => Desktop.VisualScaling;
                set => Desktop.VisualScaling = value;
            }

            public Font DefaultFont => Desktop.DefaultFont;

            public IFontProvider Fonts { get; set; }

            public IRenderElement Focus
            {
                get => workspace.Focus;
            }

            public IInstructions Instructions { get; set; }

            public IUserInterfaceAudio Audio { get; set; }

            public Rectangle ScreenArea => Desktop.ScreenArea;

            public bool SetFocus(IRenderElement newFocus)
            {
                return workspace.SetFocus(newFocus);
            }

            public IRenderElement ParentOf(IRenderElement element)
            {
                IRenderElement parent = null;

                workspace.visualTree.Walk(e =>
                {
                    if (e.Children == null)
                    {
                        return true;
                    }

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

        private readonly IRenderable app;
        private readonly Logger log;

        private VisualTree visualTree;
        private WorkspaceDisplaySystem displaySystem;

        private string defaultTheme;

        /// <summary>
        /// Initializes a workspace object.
        /// </summary>
        /// <param name="name">Name of the workspace.</param>
        public Workspace(string name, IRenderable root)
        {
            Name = name;

            this.log = LogManager.GetLogger($"Workspace.{name}");
            this.app = root;
        }

        public event Action<UserInterfaceActionEventArgs> UnhandledEvent;
        public event Action BeforeTransitionOut;
        public event Action FocusChanged;

        public float VisualScaling
        {
            get => visualTree.VisualScaling;
            set
            {
                visualTree.VisualScaling = value;

                if (Desktop != null)
                {
                    Render();
                }
            }
        }

        public IRenderElement Focus => visualTree.Focus;

        public bool SetFocus(IRenderElement newFocus)
        {
            if (visualTree.Focus == newFocus)
            {
                return false;
            }

            bool result = visualTree.SetFocus(newFocus);

            if (result)
            {
                FocusChanged?.Invoke();
            }

            return result;
        }

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
        public Rectangle ScreenArea { get; internal set; }

        public string Name { get; private set; }


        /// <summary>
        /// Gets whether the workspace is the active workspace.
        /// </summary>
        public bool IsActive { get; internal set; }

        public string DefaultTheme
        {
            get => defaultTheme;
            set
            {
                defaultTheme = value;

                if (visualTree != null)
                {
                    visualTree.DefaultTheme = value;

                    if (Desktop != null)
                    {
                        Render();
                    }
                }
            }
        }

        internal Desktop Desktop
        {
            get => this.displaySystem.Desktop;
            set => this.displaySystem.Desktop = value;
        }

        public void HandleUIAction(UserInterfaceActionEventArgs args)
        {
            DebugMsg($"Handling UI action {args.Action}...");

            var target = Focus ?? visualTree.TreeRoot;

            target?.OnUserInterfaceAction(args);

            if (!args.Handled)
            {
                UnhandledEvent?.Invoke(args);
            }
        }

        public void HandleButtonDown(ButtonStateEventArgs args)
        {
            DebugMsg($"Handling button down {args.ActionButton}...");

            var target = Focus ?? visualTree.TreeRoot;

            target?.OnButtonDown(args);
        }

        public void HandleButtonUp(ButtonStateEventArgs args)
        {
            DebugMsg($"Handling button up {args.ActionButton}...");

            var target = Focus ?? visualTree.TreeRoot;

            target?.OnButtonUp(args);
        }

        public void Update(IUserInterfaceRenderContext renderContext)
        {
            visualTree.Update(renderContext, ScreenArea);

            EnsureFocus();
        }

        public void Render()
        {
            if (displaySystem.Fonts == null)
            {
                return;
            }

            visualTree.Render(app);
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
                {
                    return AnimationState.TransitionIn;
                }

                if (visualTree.TreeRoot.Children.Any(x => x.Display.Animation.State == AnimationState.TransitionOut))
                {
                    return AnimationState.TransitionOut;
                }

                if (visualTree.TreeRoot.Children.All(x => x.Display.Animation.State == AnimationState.Dead))
                {
                    return AnimationState.Dead;
                }

                return AnimationState.Static;
            }
        }

        public VisualTree VisualTree => visualTree;

        public UserInterfaceAppContext AppContext
        {
            get => visualTree.AppContext;
            internal set => visualTree.AppContext = value;
        }

        public void TransitionOut()
        {
            BeforeTransitionOut?.Invoke();

            displaySystem.Audio?.PlaySound(this, UserInterfaceSound.WorkspaceRemoved);

            BeginTopLevelTransition(AnimationState.TransitionOut,
                                    visualTree.TreeRoot);
        }

        public void TransitionIn()
        {
            displaySystem.Audio?.PlaySound(this, UserInterfaceSound.WorkspaceAdded);

            BeginTopLevelTransition(AnimationState.TransitionIn,
                                    visualTree.TreeRoot);
        }

        internal void InitializeVisualTree(IAnimationFactory animationFactory)
        {
            displaySystem = new WorkspaceDisplaySystem(this);

            visualTree = new VisualTree(animationFactory)
            {
                DisplaySystem = displaySystem,
                DefaultTheme = defaultTheme,
            };
        }

        private void BeginTopLevelTransition(AnimationState state,
                                             IRenderElement element)
        {
            element.Display.Animation.State = state;
            element.Display.Animation.InitializeTransition();

            if (state == AnimationState.TransitionOut)
            {
                if (element.Style.Animation?.Exit == null)
                {
                    BeginTopLevelTransition(state, element.Children);
                }
            }
            else if (state == AnimationState.TransitionIn)
            {
                if (element.Style.Animation?.Entry == null)
                {
                    BeginTopLevelTransition(state, element.Children);
                }
            }
        }

        private void BeginTopLevelTransition(AnimationState state,
                                             IEnumerable<IRenderElement> children)
        {
            if (children == null)
            {
                return;
            }

            foreach (IRenderElement element in children)
            {
                BeginTopLevelTransition(state, element);
            }
        }

        /// <summary>
        /// Makes sure that something in the workspace has focus.
        /// </summary>
        public void EnsureFocus()
        {
            if (Focus == null)
            {
                visualTree.Walk(element =>
                {
                    if (element.CanHaveFocus)
                    {
                        SetFocus(element);
                        return false;
                    }

                    return true;
                });
            }
        }

        [Conditional("__DEBUG_WORKSPACE")]
        private void DebugMsg(string message)
        {
            log.Trace($"Workspace {Name}: {message}");
        }
    }
}