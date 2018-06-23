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
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private readonly WidgetRenderContext renderContext;
        private readonly UserInterfaceSceneDriver driver;

        public UserInterfaceScene(GraphicsDevice graphicsDevice,
            IUserInterfaceRenderer userInterfaceRenderer,
            ILocalizedContentLayoutEngine contentLayoutEngine,
            IStyleConfigurator styleConfigurator,
            IDoubleBuffer doubleBuffer = null,
            RenderTarget2D renderTarget = null)
        {
            DrawBelow = true;
            UpdateBelow = false;

            renderContext = new WidgetRenderContext(
                graphicsDevice,
                contentLayoutEngine,
                userInterfaceRenderer,
                styleConfigurator,
                renderTarget,
                null,
                doubleBuffer);

            driver = new UserInterfaceSceneDriver(renderContext);

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
        /// Gets or sets a value indicating whether this scene should automatically
        /// exit when there are no workspaces left in the desktop.
        /// </summary>
        public bool ExitWhenEmpty { get; set; }

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

        public Desktop Desktop => driver.Desktop;

        public BlendState BlendState { get; set; }

        public WidgetRenderContext RenderContext => renderContext;

        public IMenuIndicatorRenderer Indicator
        {
            get => renderContext.Indicator;
            set => renderContext.Indicator = value;
        }

        /// <summary>
        /// Gets or sets the default theme for widgets in the scene.
        /// </summary>
        public string Theme
        {
            get => Desktop.DefaultTheme;
            set => Desktop.DefaultTheme = value;
        }

        public void Initialize()
        {
            driver.Initialize();
            IsFinished = false;
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
    }
}
