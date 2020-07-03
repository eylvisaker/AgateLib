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

using AgateLib.Input;
using AgateLib.Scenes;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.InputMap;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
    [Transient]
    public class UserInterfaceBufferedScene : BufferedScene, IUserInterfaceScene
    {
        private readonly UserInterfaceRenderContext renderContext;
        private readonly UserInterfaceDriver driver;
        private readonly List<Action> actionsToInvoke = new List<Action>();

        private TaskCompletionSource<bool> exitTask;

        public UserInterfaceBufferedScene(UserInterfaceConfig config,
                                          GraphicsDevice graphicsDevice,
                                          IUserInterfaceRenderer userInterfaceRenderer,
                                          IContentLayoutEngine contentLayoutEngine,
                                          IFontProvider fontProvider,
                                          IStyleConfigurator styleConfigurator,
                                          IAnimationFactory animationFactory = null,
                                          IUserInterfaceAudio audio = null,
                                          IDoubleBuffer doubleBuffer = null,
                                          RenderTarget2D renderTarget = null)
            : base(graphicsDevice, config.ScreenSize)
        {
            DrawBelow = true;
            UpdateBelow = false;

            Animations = animationFactory ?? new AnimationFactory();
            GraphicsDevice = graphicsDevice;

            renderContext = new UserInterfaceRenderContext(graphicsDevice,
                                                           contentLayoutEngine,
                                                           userInterfaceRenderer,
                                                           actionsToInvoke,
                                                           fontProvider,
                                                           Animations,
                                                           renderTarget,
                                                           null,
                                                           doubleBuffer);

            driver = new UserInterfaceDriver(config,
                                             renderContext,
                                             styleConfigurator,
                                             fontProvider,
                                             Animations,
                                             audio);

            driver.Desktop.Empty += () =>
            {
                if (ExitWhenEmpty)
                {
                    IsFinished = true;
                }
            };

            BlendState = new BlendState
            {
                ColorSourceBlend = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.InverseSourceAlpha,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
            };
        }

        /// <summary>
        /// Gets or sets the input map object. 
        /// </summary>
        public UserInterfaceInputMap InputMap
        {
            get => driver.InputMap;
            set => driver.InputMap = value;
        }

        /// <summary>
        /// Gets or sets the button mapping. This is equivalent to InputMap.ButtonMap.
        /// </summary>
        public Dictionary<Buttons, UserInterfaceAction> ButtonMap
        {
            get => InputMap.ButtonMap;
            set => InputMap.ButtonMap = value;
        }

        /// <summary>
        /// Gets or sets the keyboard mapping. This is equivalent to InputMap.KeyMap.
        /// </summary>
        public Dictionary<Keys, UserInterfaceAction> KeyMap
        {
            get => InputMap.KeyMap;
            set => InputMap.KeyMap = value;
        }

        /// <summary>
        /// Gets the animation factory for this user interface scene.
        /// </summary>
        public IAnimationFactory Animations { get; }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this scene should automatically
        /// exit when there are no workspaces left in the desktop.
        /// Defaults to true.
        /// </summary>
        public bool ExitWhenEmpty { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the scene should automatically
        /// exit when the user pushes a key or button mapped to the exit command.
        /// </summary>
        public bool ExitOnExitButton
        {
            get => driver.ExitOnExitButton;
            set => driver.ExitOnExitButton = value;
        }

        /// <summary>
        /// Event raised when the user requests to exit the UI.
        /// </summary>
        public event Action ExitPressed
        {
            add => driver.ExitPressed += value;
            remove => driver.ExitPressed -= value;
        }

        /// <summary>
        /// Gets the desktop object.
        /// </summary>
        public Desktop Desktop => driver.Desktop;

        /// <summary>
        /// Gets or sets the blend state used for the sprite batch object.
        /// </summary>
        public BlendState BlendState { get; set; }

        /// <summary>
        /// Gets the boundary of the viewport.
        /// </summary>
        public Rectangle GraphicsDeviceViewportBounds => GraphicsDevice.Viewport.Bounds;

        public ICursor Pointer
        {
            get => driver.Cursor;
            set => driver.Cursor = value;
        }

        /// <summary>
        /// Gets or sets the area of the screen that is available for the UI to render into.
        /// </summary>
        public Rectangle ScreenArea
        {
            get => driver.ScreenArea;
            set => driver.ScreenArea = value;
        }

        public IInstructions Instructions => Desktop.Instructions;

        public Color InactiveWorkspaceFade
        {
            get => Desktop.InactiveWorkspaceFade;
            set => Desktop.InactiveWorkspaceFade = value;
        }

        protected override void OnSceneStart()
        {
            base.OnSceneStart();

            driver.Initialize();
        }

        protected override void OnUpdateInput(GameTime time, IInputState input)
        {
            driver.UpdateInput(time, input);
            base.OnUpdateInput(time, input);
        }

        protected override void OnUpdate(GameTime time)
        {
            driver.Update(time);
            base.OnUpdate(time);
        }

        protected override void DrawScene(GameTime time)
        {
            renderContext.GameTime = time;

            renderContext.Canvas.Begin(blendState: BlendState);

            driver.Draw(time, renderContext.Canvas, renderContext.RenderTarget);

            renderContext.Canvas.End();

            InvokeActions();
        }

        private void InvokeActions()
        {
            foreach (var action in actionsToInvoke)
            {
                action.Invoke();
            }

            actionsToInvoke.Clear();
        }

        public void FlushSpriteBatch()
        {
            renderContext.Canvas.End();
            renderContext.Canvas.Begin(blendState: BlendState);
        }

        /// <summary>
        /// Exits the user interface, then executes an action once the animation is complete and
        /// this scene is about to be removed from the scene stack.
        /// </summary>
        /// <param name="nextAction"></param>
        public void ExitThen(Action nextAction)
        {
            Exit();

            EventHandler next = null;

            next = (sender, e) => { nextAction(); End -= next; };

            End += next;
        }

        /// <summary>
        /// Exits the user interface by informing all workspaces to begin their exit transition animation.
        /// </summary>
        public Task Exit()
        {
            Desktop.ExitUserInterface();

            if (exitTask == null)
            {
                exitTask = new TaskCompletionSource<bool>();
            }

            return exitTask.Task;
        }

        public void PushWorkspace(Workspace workspace)
        {
            Desktop.PushWorkspace(workspace);
        }

        public void Add(IRenderable root)
        {
            bool validName = false;

            for (int i = 0; !validName; i++)
            {
                var name = $"workspace-{i:00}";

                if (!Desktop.Workspaces.Any(x => x.Name == name))
                {
                    validName = true;

                    PushWorkspace(new Workspace(name, root));
                }
            }
        }

        public void AddContext<T>(T context)
        {
            Desktop.AppContext.Add(context);
        }

        protected override void OnSceneEnd()
        {
            base.OnSceneEnd();

            var task = exitTask;
            exitTask = null;

            task?.SetResult(true);
        }
    }
}
