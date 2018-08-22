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
using System.Text;
using AgateLib.Input;
using AgateLib.Scenes;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.InputMap;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.UserInterface
{
    public interface IUserInterfaceScene : IScene
    {
        Desktop Desktop { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this scene should automatically
        /// exit when there are no workspaces left in the desktop.
        /// </summary>
        bool ExitWhenEmpty { get; set; }
    }

    [Transient]
    public class UserInterfaceScene : Scene, IUserInterfaceScene
    {
        private readonly UserInterfaceRenderContext renderContext;
        private readonly UserInterfaceSceneDriver driver;

        public UserInterfaceScene(GraphicsDevice graphicsDevice,
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
                audio);

            driver.ScreenArea = new Rectangle(Point.Zero, 
                GraphicsDeviceRenderTargetSize);

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

        private Size GraphicsDeviceRenderTargetSize
        {
            get
            {
                var renderTargets = GraphicsDevice.GetRenderTargets();

                if (renderTargets.Length == 0)
                {
                    return new Size(
                        GraphicsDevice.PresentationParameters.BackBufferWidth,
                        GraphicsDevice.PresentationParameters.BackBufferHeight);
                }
                else
                {
                    var renderTarget = (Texture2D)GraphicsDevice.GetRenderTargets()[0].RenderTarget;

                    return new Size(renderTarget.Width, renderTarget.Height);
                }
            }
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
        public void Exit()
        {
            Desktop.ExitUserInterface();
        }

        public void PushWorkspace(Workspace workspace)
        {
            Desktop.PushWorkspace(workspace);
        }
    }
}
