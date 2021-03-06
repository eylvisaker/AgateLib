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

using AgateLib.Display;
using AgateLib.Quality;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.UserInterface
{
    public class Desktop
    {
        private readonly IUserInterfaceLayoutContext layoutContext;
        private readonly IAnimationFactory animationFactory;

        private readonly List<Workspace> workspaces = new List<Workspace>();

        private readonly WorkspaceExitEventArgs workspaceExitEventArgs = new WorkspaceExitEventArgs();
        private readonly UserInterfaceAppContext appContext = new UserInterfaceAppContext();
        private IInstructions instructions = new Instructions();

        private bool inDraw;

        private FadeColor inactiveWorkspaceFadeColor = new FadeColor();

        public Desktop(UserInterfaceConfig config,
                       IUserInterfaceLayoutContext layoutContext,
                       IFontProvider fonts,
                       IStyleConfigurator styles,
                       IAnimationFactory animationFactory, 
                       IUserInterfaceAudio audio)
        {
            this.Config = config;
            this.animationFactory = animationFactory;
            this.layoutContext = layoutContext;
            this.Audio = audio;

            appContext.Config = config;

            Styles = styles;
            Fonts = fonts;

            inactiveWorkspaceFadeColor.FadeIn();

            if (Audio != null)
            {
                Audio.ActiveDesktop = this;
            }
        }

        /// <summary>
        /// Event raised when the user makes an input action.
        /// </summary>
        public event Action<UserInterfaceActionEventArgs> UserInterfaceAction;

        /// <summary>
        /// Occurs when a button is pressed.
        /// </summary>
        public event Action<ButtonStateEventArgs> ButtonDown;

        /// <summary>
        /// Occurs when a button is released.
        /// </summary>
        public event Action<ButtonStateEventArgs> ButtonUp;

        /// <summary>
        /// Occurs when the focus control changes.
        /// </summary>
        public event Action FocusChanged;

        public UserInterfaceConfig Config { get; }

        public IStyleConfigurator Styles { get; }

        public IFontProvider Fonts { get; }

        public IUserInterfaceAudio Audio { get; set; }

        public float VisualScaling => Config.VisualScaling;

        public UserInterfaceAppContext AppContext => appContext;

        /// <summary>
        /// Explores all the elements in the visual tree on the desktop using a depth-first search.
        /// </summary>
        /// <param name="explorer"></param>
        public void Explore(Action<IRenderElement> explorer)
        {
            foreach (var workspace in workspaces)
            {
                workspace.Explore(explorer);
            }
        }

        /// <summary>
        /// Event raised when the desktop is empty.
        /// </summary>
        public event Action Empty;
        public event Action<UserInterfaceActionEventArgs> UnhandledEvent;

        public IInstructions Instructions
        {
            get => instructions;
            set
            {
                Require.ArgumentNotNull(value, nameof(Instructions));

                instructions = value;

                foreach (var workspace in workspaces)
                {
                    workspace.Instructions = instructions;
                }
            }
        }

        /// <summary>
        /// Pushes a new workspace onto the desktop. Optionally activates a window
        /// in that workspace.
        /// </summary>
        /// <param name="workspace">The new active workspace.</param>
        /// <param name="activateWindow">Optional; the name of the window to activate in the workspace. If this parameter is blank no window will be activated by this method.</param>
        public void PushWorkspace(Workspace workspace, string activateWindow = "")
        {
            Require.Not(inDraw, "Cannot add workspace while drawing.");
            Require.Not(workspaces.Contains(workspace), "Cannot add same workspace twice.", CommonException.ArgumentException);

            workspace.Config = Config;

            workspace.InitializeVisualTree(animationFactory);

            workspaces.Add(workspace);

            workspace.Theme = workspace.Theme ?? Theme ?? Config.DefaultTheme;
            workspace.Style = Styles;
            workspace.Fonts = Fonts;
            workspace.Instructions = Instructions;
            workspace.Audio = Audio;
            workspace.AppContext = AppContext;
            workspace.FocusChanged += Workspace_FocusChanged;

            workspace.Desktop = this;
            workspace.TransitionIn(layoutContext);

            FocusChanged?.Invoke();
        }

        public void BringWorkspaceToFront(Workspace workspace)
        {
            Require.That(workspaces.Contains(workspace), "Workspace must be contained in the desktop to bring it to front.");

            workspaces.Remove(workspace);
            workspaces.Add(workspace);
        }

        /// <summary>
        /// Tells the top workspace on the stack to transition out.
        /// </summary>
        public void PopWorkspace()
        {
            Require.Not(inDraw, "Cannot modify workspaces while drawing.");
            Require.That(workspaces.Count > 0, "Cannot pop workspace when there are none.");

            var workspace = workspaces[workspaces.Count - 1];

            workspace.FocusChanged -= Workspace_FocusChanged;
            workspace.TransitionOut();

            if (workspaces.Count == 1)
            {
                inactiveWorkspaceFadeColor.FadeOut();
            }

            FocusChanged?.Invoke();
        }

        /// <summary>
        /// Tells all workspaces to transition out.
        /// </summary>
        public void ExitUserInterface()
        {
            foreach (var workspace in workspaces)
            {
                workspace.TransitionOut();
            }

            inactiveWorkspaceFadeColor.FadeOut();
        }

        public Workspace ActiveWorkspace => workspaces.LastOrDefault();

        public IReadOnlyList<Workspace> Workspaces => workspaces;

        public Color InactiveWorkspaceFade
        {
            get => inactiveWorkspaceFadeColor.ActiveColor;
            set => inactiveWorkspaceFadeColor.ActiveColor = value;
        }

        /// <summary>
        /// Gets or sets the default theme for the desktop. New workspaces added to this desktop
        /// will inherit this theme, unless they have their theme overriden.
        /// </summary>
        public string Theme { get; set; }

        #region --- Handling Input ---

        public void OnUserInterfaceAction(UserInterfaceActionEventArgs args)
        {
            UserInterfaceAction?.Invoke(args);

            if (args.Handled)
            {
                return;
            }

            if (ActiveWorkspace?.AnimationState == AnimationState.Static)
            {
                ActiveWorkspace?.HandleUIAction(args);
            }

            if (!args.Handled)
            {
                UnhandledEvent?.Invoke(args);
            }
        }

        public void OnButtonDown(ButtonStateEventArgs args)
        {
            ButtonDown?.Invoke(args);

            if (args.Handled)
            {
                return;
            }

            if (ActiveWorkspace?.AnimationState == AnimationState.Static)
            {
                ActiveWorkspace?.HandleButtonDown(args);
            }
        }

        public void OnButtonUp(ButtonStateEventArgs args)
        {
            ButtonUp?.Invoke(args);

            if (args.Handled)
            {
                return;
            }

            if (ActiveWorkspace?.AnimationState == AnimationState.Static)
            {
                ActiveWorkspace?.HandleButtonUp(args);
            }
        }

        protected virtual void OnEmpty()
        {
            Empty?.Invoke();
        }

        #endregion

        /// <summary>
        /// Instructs all workspaces to transition out.
        /// </summary>
        public void Clear()
        {
            foreach (var workspace in workspaces)
            {
                workspace.TransitionOut();
            }
        }

        public void Update(IUserInterfaceRenderContext renderContext)
        {
            inactiveWorkspaceFadeColor.Update(renderContext.GameTime);

            foreach (var workspace in workspaces)
            {
                workspace.IsActive = workspace == ActiveWorkspace;
            }

            if (workspaces.Count == 0)
            {
                OnEmpty();
                return;
            }

            for (int i = 0; i < workspaces.Count; i++)
            {
                workspaces[i].Update(renderContext);
            }

            Workspace activeWorkspace;

            do
            {
                // Do this to detect whether a new workspace was pushed
                // while updating the active workspace. If so, the new
                // workspace will also need to be updated.
                activeWorkspace = ActiveWorkspace;

                if (ActiveWorkspace?.AnimationState == AnimationState.Dead)
                {
                    workspaces.Remove(ActiveWorkspace);
                }

            } while (activeWorkspace != ActiveWorkspace);
        }

        public void Draw(IUserInterfaceRenderContext renderContext)
        {
            try
            {
                inDraw = true;

                foreach (var w in workspaces)
                {
                    if (w == ActiveWorkspace && inactiveWorkspaceFadeColor.CurrentColor.A > 0)
                    {
                        renderContext.Canvas.FillRect(Config.ScreenArea, inactiveWorkspaceFadeColor.CurrentColor);
                    }

                    w.Draw(renderContext);
                }
            }
            finally
            {
                inDraw = false;
            }

            Instructions.Draw(renderContext);
        }

        private void Workspace_FocusChanged()
        {
            FocusChanged?.Invoke();
        }
    }
}

