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

using AgateLib.Input;
using AgateLib.Mathematics.Geometry;
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
    public interface IUserInterfaceScene : ISceneEnhanced
    {
        /// <summary>
        /// Gets the desktop object for the UI scene.
        /// </summary>
        Desktop Desktop { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this scene should automatically
        /// exit when there are no workspaces left in the desktop.
        /// </summary>
        bool ExitWhenEmpty { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the scene should automatically
        /// exit when the user pushes a key or button mapped to the exit command.
        /// </summary>
        bool ExitOnExitButton { get; set; }

        /// <summary>
        /// Gets the instructions object.
        /// </summary>
        IInstructions Instructions { get; }

        /// <summary>
        /// Gets the button mapping. 
        /// </summary>
        Dictionary<Buttons, UserInterfaceAction> ButtonMap { get; }

        /// <summary>
        /// Gets or sets the focus indicator.
        /// </summary>
        IFocusIndicator Indicator { get; set; }

        /// <summary>
        /// Creates a workspace for the specified widget or render element and
        /// adds it to the scene.
        /// </summary>
        /// <param name="root"></param>
        void Add(IRenderable root);

        /// <summary>
        /// Tells the UI to transition out, and when the animation is complete
        /// calls the callback function.
        /// </summary>
        /// <param name="p"></param>
        [Obsolete("Use Exit().ContinueWith or await Exit() instead.", true)]
        void ExitThen(Action callback);

        /// <summary>
        /// Exits the user interface by informing all workspaces to begin their exit transition animation. Returns a task that resolves when the exit transition 
        /// is complete.
        /// </summary>
        Task Exit();
    }

    [Transient]
    public class UserInterfaceScene : Scene, IUserInterfaceScene
    {
        private readonly UserInterfaceRenderContext renderContext;
        private readonly UserInterfaceSceneDriver driver;

        private TaskCompletionSource<bool> exitTask;

        public UserInterfaceScene(Rectangle screenArea,
                                  GraphicsDevice graphicsDevice,
                                  IUserInterfaceRenderer userInterfaceRenderer,
                                  IContentLayoutEngine contentLayoutEngine,
                                  IFontProvider fontProvider,
                                  IStyleConfigurator styleConfigurator,
                                  IAnimationFactory animationFactory = null,
                                  IUserInterfaceAudio audio = null,
                                  IDoubleBuffer doubleBuffer = null,
                                  RenderTarget2D renderTarget = null)
        {
            DrawBelow = true;
            UpdateBelow = false;

            Animations = animationFactory ?? new AnimationFactory();
            GraphicsDevice = graphicsDevice;

            renderContext = new UserInterfaceRenderContext(
                screenArea,
                graphicsDevice,
                contentLayoutEngine,
                userInterfaceRenderer,
                styleConfigurator,
                fontProvider,
                Animations,
                renderTarget,
                null,
                doubleBuffer);

            driver = new UserInterfaceSceneDriver(
                renderContext,
                styleConfigurator,
                fontProvider,
                Animations,
                audio);

            driver.ScreenArea = screenArea;

            driver.Desktop.Empty += () =>
            {
                if (ExitWhenEmpty)
                    IsFinished = true;
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

        public IFocusIndicator Indicator
        {
            get => driver.Indicator;
            set => driver.Indicator = value;
        }

        /// <summary>
        /// Gets or sets the default theme for widgets in the scene.
        /// </summary>
        public string Theme
        {
            get => Desktop.DefaultTheme;
            set => Desktop.DefaultTheme = value;
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

        protected override void OnSceneStart()
        {
            base.OnSceneStart();

            driver.Initialize();
        }

        protected override void OnUpdateInput(IInputState input)
        {
            driver.UpdateInput(input);
            base.OnUpdateInput(input);
        }

        protected override void OnUpdate(GameTime time)
        {
            driver.Update(time);
            base.OnUpdate(time);
        }

        protected override void DrawScene(GameTime time)
        {
            renderContext.GameTime = time;

            renderContext.SpriteBatch.Begin(blendState: BlendState);

            driver.Draw(time, renderContext.SpriteBatch, renderContext.RenderTarget);

            renderContext.SpriteBatch.End();
        }

        public void FlushSpriteBatch()
        {
            renderContext.SpriteBatch.End();
            renderContext.SpriteBatch.Begin(blendState: BlendState);
        }

        public void IgnoreCurrentInput()
        {
            driver.IgnoreCurrentInput();
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

            next = (sender, e) => { nextAction(); SceneEnd -= next; };

            SceneEnd += next;
        }

        /// <summary>
        /// Exits the user interface by informing all workspaces to begin their exit transition animation.
        /// </summary>
        public Task Exit()
        {
            Desktop.ExitUserInterface();

            exitTask = new TaskCompletionSource<bool>();
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

        protected override void OnSceneEnd()
        {
            base.OnSceneEnd();

            exitTask?.SetResult(true);
        }
    }
}
