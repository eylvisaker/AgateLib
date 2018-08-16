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

using System;
using AgateLib.Input;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
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

        public UserInterfaceSceneDriver(
            IWidgetRenderContext renderContext, 
            IStyleConfigurator styles, 
            IFontProvider fonts)
        {
            this.renderContext = renderContext;

            desktop = new Desktop(fonts, styles);

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
        /// Gets the object which handles rendering of the indicator.
        /// </summary>
        public IFocusIndicator Indicator { get; set; }

        /// <summary>
        /// If true, the Exit event will be raised when the user
        /// presses the back button on the controller.
        /// </summary>
        public bool ExitOnExitButton { get; set; }

        /// <summary>
        /// Gets or sets the screen area the user interface will draw to.
        /// </summary>
        public Rectangle ScreenArea { get; set; }

        public void Initialize()
        {
            uiInput.ClearPressedButtons();
        }

        public void UpdateInput(IInputState input)
        {
            uiInput.UpdateState(input);

            uiInput.TriggerEvents();
        }

        public void Update(GameTime time)
        {
            renderContext.InitializeUpdate(time);

            desktop.ScreenArea = ScreenArea;

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

            if (desktop.ActiveWorkspace?.Focus != null && Indicator != null)
            {
                Indicator.UserInterfaceRenderer = renderContext.UserInterfaceRenderer;

                Indicator.DrawFocus(renderContext.SpriteBatch, 
                                    desktop.ActiveWorkspace.Focus,
                                    ScreenAreaOf(desktop.ActiveWorkspace.Focus));
            }
        }

        private Rectangle ScreenAreaOf(IRenderElement focus)
        {
            Rectangle result = new Rectangle(Point.Zero, focus.Display.Animation.AnimatedContentRect.Size);

            IRenderElement current = focus;

            while(current != null)
            {
                var currentLoc = result.Location;

                currentLoc.X += current.Display.Animation.AnimatedContentRect.X;
                currentLoc.Y += current.Display.Animation.AnimatedContentRect.Y;

                result.Location = currentLoc;

                current = current.Parent;
            }

            return result;
        }

        private void Desktop_UnhandledEvent(object sender, InputEventArgs e)
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
