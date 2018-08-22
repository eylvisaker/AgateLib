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
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Rendering.Animations;

namespace AgateLib.UserInterface
{
    public class Desktop
    {
        private readonly List<Workspace> workspaces = new List<Workspace>();

        private readonly WorkspaceExitEventArgs workspaceExitEventArgs = new WorkspaceExitEventArgs();

        private Rectangle screenArea;

        private IInstructions instructions = new Instructions();

        private bool inDraw;

        public Desktop(IFontProvider fonts, IStyleConfigurator styles)
        {
            Styles = styles;
            Fonts = fonts;
        }

        public IStyleConfigurator Styles { get; }

        public IFontProvider Fonts { get; }

        public IUserInterfaceAudio Audio { get; set; }

        /// <summary>
        /// Explores all the elements in the visual tree on the desktop using a depth-first search.
        /// </summary>
        /// <param name="explorer"></param>
        public void Explore(Action<IRenderElement> explorer)
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
        public event Action<UserInterfaceActionEventArgs> UnhandledEvent;

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
        public void PushWorkspace(Workspace workspace, string activateWindow = "")
        {
            Require.Not(inDraw, "Cannot add workspace while drawing.");
            Require.Not(workspaces.Contains(workspace), "Cannot add same workspace twice.", CommonException.ArgumentException);

            workspaces.Add(workspace);

            workspace.Desktop = this;
            workspace.Style = Styles;
            workspace.Fonts = Fonts;
            workspace.Instructions = Instructions;
            workspace.Audio = Audio;

            // TODO: find a better way to do this.
            if (string.IsNullOrWhiteSpace(workspace.DefaultTheme))
                workspace.DefaultTheme = DefaultTheme;

            workspace.Render();

            workspace.TransitionIn();
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

            workspace.TransitionOut();
        }

        /// <summary>
        /// Tells all workspaces to transition out.
        /// </summary>
        public void ExitUserInterface()
        {
            foreach(var workspace in workspaces)
            {
                workspace.TransitionOut();
            }
        }

        /// <summary>
        /// Gets or sets the size of the desktop. Should match the size of the
        /// render area of the graphics device.
        /// </summary>
        public Rectangle ScreenArea
        {
            get => screenArea;
            set
            {
                screenArea = value;

                foreach (var w in workspaces)
                    w.ScreenArea = screenArea;
            }
        }

        public Workspace ActiveWorkspace => workspaces.LastOrDefault();

        public IReadOnlyList<Workspace> Workspaces => workspaces;
        
        public void OnUserInterfaceAction(UserInterfaceActionEventArgs args)
        {
            ActiveWorkspace?.HandleUIAction(args);

            if (!args.Handled)
                UnhandledEvent?.Invoke(args);
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

        public void Update(IUserInterfaceRenderContext renderContext)
        {
            foreach (var workspace in workspaces)
                workspace.IsActive = workspace == ActiveWorkspace;

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
                    w.Draw(renderContext);
                }
            }
            finally
            {
                inDraw = false;
            }

            Instructions.Draw(renderContext);
        }
    }
}

