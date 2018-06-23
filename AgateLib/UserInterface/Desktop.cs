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
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface
{
    public class Desktop
    {
        private readonly List<Workspace> workspaces = new List<Workspace>();

        private readonly WidgetEventArgs inputEventArgs = new WidgetEventArgs();
        private readonly IWidgetEventArgsInitialize inputEventArgsInitialize;
        private readonly WorkspaceExitEventArgs workspaceExitEventArgs = new WorkspaceExitEventArgs();

        private Size size;

        private IInstructions instructions = new Instructions();

        private bool inDraw;

        public Desktop()
        {
            inputEventArgsInitialize = inputEventArgs;
        }

        /// <summary>
        /// Explores all the widgets on the desktop using a depth-first search.
        /// </summary>
        /// <param name="explorer"></param>
        public void Explore(Action<IWidget, IWidget> explorer)
        {
            foreach(var workspace in workspaces)
            {
                workspace.Explore(explorer);
            }
        }

        /// <summary>
        /// Event raised when the desktop is empty.
        /// </summary>
        public event Action Empty;
        public event EventHandler<WidgetEventArgs> UnhandledEvent;

        public string DefaultTheme { get; set; } = "default";

        public IInstructions Instructions
        {
            get => instructions;
            set
            {
                Require.ArgumentNotNull(value, nameof(Instructions));

                instructions = value;

                foreach (var workspace in workspaces)
                    workspace.Instructions = instructions;
            }
        }

        /// <summary>
        /// Pushes a new workspace onto the desktop. Optionally activates a window
        /// in that workspace.
        /// </summary>
        /// <param name="workspace">The new active workspace.</param>
        /// <param name="activateWindow">Optional; the name of the window to activate in the workspace. If this parameter is blank no window will be activated by this method.</param>
        /// <param name="style">Optional; specify an IStyleConfigurator object to apply styling to the window.</param>
        public void PushWorkspace(Workspace workspace, string activateWindow = "")
        {
            Require.Not(inDraw, "Cannot add workspace while drawing.");
            Require.Not(workspaces.Contains(workspace), "Cannot add same workspace twice.", CommonException.ArgumentException);

            workspaces.Add(workspace);

            workspace.Instructions = Instructions;

            // TODO: find a better way to do this.
            if (string.IsNullOrWhiteSpace(workspace.DefaultTheme))
                workspace.DefaultTheme = DefaultTheme;

            if (!string.IsNullOrWhiteSpace(activateWindow))
            {
                workspace.ActivateWindow(activateWindow);
            }

            workspace.TransitionIn();
        }

        public void BringWorkspaceToFront(Workspace workspace)
        {
            Require.That(workspaces.Contains(workspace), "Workspace must be contained in the desktop to bring it to front.");

            workspaces.Remove(workspace);
            workspaces.Add(workspace);
        }

        public void PopWorkspace()
        {
            Require.Not(inDraw, "Cannot modify workspaces while drawing.");
            Require.That(workspaces.Count > 0, "Cannot pop workspace when there are none.");

            var workspace = workspaces[workspaces.Count - 1];

            workspace.TransitionOut();
        }

        /// <summary>
        /// Gets or sets the size of the desktop. Should match the size of the
        /// render area of the graphics device.
        /// </summary>
        public Size Size
        {
            get => size;
            set
            {
                size = value;

                foreach (var w in workspaces)
                    w.Size = size;
            }
        }

        public Workspace ActiveWorkspace => workspaces.LastOrDefault();

        public IReadOnlyList<Workspace> Workspaces => workspaces;

        public void ButtonUp(MenuInputButton button)
        {
            inputEventArgsInitialize.InitializeButtonUp(button);

            ActiveWorkspace.HandleInputEvent(inputEventArgs);

            if (!inputEventArgs.Handled)
                UnhandledEvent?.Invoke(this, inputEventArgs);
        }

        public void ButtonDown(MenuInputButton button)
        {
            inputEventArgsInitialize.InitializeButtonDown(button);

            ActiveWorkspace.HandleInputEvent(inputEventArgs);

            if (!inputEventArgs.Handled)
                UnhandledEvent?.Invoke(this, inputEventArgs);
        }


        protected virtual void OnEmpty()
        {
            Empty?.Invoke();
        }

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

        public void Initialize()
        {
            foreach (var w in workspaces)
                w.Initialize();
        }

        public void Update(IWidgetRenderContext renderContext)
        {
            foreach (var workspace in workspaces)
                workspace.IsActive = workspace == ActiveWorkspace;

            if (ActiveWorkspace?.AnimationState == AnimationState.Dead)
            {
                workspaces.Remove(ActiveWorkspace);
            }

            if (workspaces.Count == 0)
            {
                OnEmpty();
                return;
            }

            Workspace activeWorkspace;

            do
            {
                // Do this to detect whether a new workspace was pushed
                // while updating the active workspace. If so, the new
                // workspace will also need to be updated.
                activeWorkspace = ActiveWorkspace;

                ActiveWorkspace?.Update(renderContext);

            } while (activeWorkspace != ActiveWorkspace);
        }

        public void Draw(IWidgetRenderContext renderContext)
        {
            try
            {
                inDraw = true;

                foreach (var w in workspaces)
                {
                    w.Draw(renderContext);
                }
            }
            finally
            {
                inDraw = false;
            }

            Instructions.Draw(renderContext);
        }

        public void ActivateWindowInWorkspace(string workspace, string window,
            WindowActivationBehaviors behavior = WindowActivationBehaviors.Default)
        {
            var w = workspaces.First(x => x.Name.Equals(workspace, StringComparison.OrdinalIgnoreCase));

            w.ActivateWindow(window, behavior);
        }

    }
}

