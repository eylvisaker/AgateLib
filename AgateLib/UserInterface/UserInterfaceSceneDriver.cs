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
using AgateLib.Input;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.UserInterface
{
    public class UserInterfaceSceneDriver
    {
        private IWidgetRenderContext renderContext;
        private Desktop desktop;

        private UserInterfaceInputEvents uiInput = new UserInterfaceInputEvents();

        public UserInterfaceSceneDriver(IWidgetRenderContext renderContext)
        {
            this.renderContext = renderContext;

            this.renderContext.BeforeUpdate +=
                widget => widget.Display.Instructions = Desktop.Instructions;

            desktop = new Desktop();

            Desktop.UnhandledEvent += Desktop_UnhandledEvent;

            uiInput.ButtonDown += desktop.ButtonDown;
            uiInput.ButtonUp += desktop.ButtonUp;
        }

        public event Action ExitPressed;

        public IWidgetRenderContext RenderContext
        {
            get => renderContext;
            set => renderContext = value;
        }

        public Desktop Desktop => desktop;

        public IUserInterfaceInputMap InputMap
        {
            get => uiInput.InputMap;
            set => uiInput.InputMap = value;
        }

        /// <summary>
        /// If true, the Exit event will be raised when the user
        /// presses the back button on the controller.
        /// </summary>
        public bool ExitOnExitButton { get; set; }

        public void Initialize()
        {
            uiInput.ClearPressedButtons();

            desktop.Initialize();
        }

        public void UpdateInput(IInputState input)
        {
            uiInput.UpdateState(input);
        }

        public void Update(GameTime time)
        {
            uiInput.TriggerEvents();

            renderContext.InitializeUpdate(time);

            desktop.Size = renderContext.GraphicsDeviceRenderTargetSize;

            desktop.Update(renderContext);

            if (desktop.Workspaces.Count == 0)
            {
                ExitPressed?.Invoke();
            }
        }

        public void Draw(GameTime time, SpriteBatch spriteBatch, RenderTarget2D renderTarget = null)
        {
            renderContext.BeginDraw(time, spriteBatch, renderTarget);

            desktop.Draw(renderContext);
        }

        private void Desktop_UnhandledEvent(object sender, WidgetEventArgs e)
        {
            if (e.EventType == WidgetEventType.ButtonUp
                && e.Button == MenuInputButton.Exit)
            {
                if (ExitOnExitButton)
                {
                    desktop.Clear();
                }

                ExitPressed?.Invoke();
            }
        }
    }
}
