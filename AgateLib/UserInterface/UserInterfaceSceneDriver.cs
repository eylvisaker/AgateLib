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
using AgateLib.UserInterface.InputMap;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.UserInterface.Rendering.Animations;

namespace AgateLib.UserInterface
{
    public class UserInterfaceSceneDriver
    {
        private IUserInterfaceRenderContext renderContext;
        private Desktop desktop;

        private UserInterfaceInputEvents uiInput = new UserInterfaceInputEvents();

        private bool setIgnoreInput;

        public UserInterfaceSceneDriver(
            IUserInterfaceRenderContext renderContext, 
            IStyleConfigurator styles, 
            IFontProvider fonts,
            IAnimationFactory animationFactory,
            IUserInterfaceAudio audio = null)
        {
            this.renderContext = renderContext;

            desktop = new Desktop(fonts, styles, animationFactory);

            desktop.Audio = audio;

            Desktop.UnhandledEvent += Desktop_UnhandledEvent;

            uiInput.UIAction += desktop.OnUserInterfaceAction;

            uiInput.ButtonDown += desktop.OnButtonDown;
            uiInput.ButtonUp += desktop.OnButtonUp;
        }

        public event Action ExitPressed;

        public IUserInterfaceRenderContext RenderContext
        {
            get => renderContext;
            set => renderContext = value;
        }

        public Desktop Desktop => desktop;

        public UserInterfaceInputMap InputMap
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
            setIgnoreInput = true;

            uiInput.ClearPressedButtons();
        }

        /// <summary>
        /// Ignores the current set of inputs, so that any buttons pressed
        /// will have to be released and pressed again before they will trigger
        /// any button press events.
        /// </summary>
        public void IgnoreCurrentInput()
        {
            setIgnoreInput = true;
        }

        public void UpdateInput(IInputState input)
        {
            if (setIgnoreInput)
            {
                uiInput.IgnoreCurrentInput(input);
                setIgnoreInput = false;
            }

            uiInput.UpdateState(input);
        }

        public void Update(GameTime time)
        {
            uiInput.TriggerEvents(time);

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
            renderContext.PrepDraw(time, spriteBatch, renderTarget);

            desktop.Draw(renderContext);

            if (desktop.ActiveWorkspace?.Focus != null && Indicator != null)
            {
                Indicator.UserInterfaceRenderer = renderContext.UserInterfaceRenderer;

                Indicator.DrawFocus(renderContext.SpriteBatch, 
                                    desktop.ActiveWorkspace.Focus,
                                    desktop.ActiveWorkspace,
                                    ScreenAreaOf(desktop.ActiveWorkspace.Focus));
            }
        }

        private Rectangle ScreenAreaOf(IRenderElement focus)
        {
            Rectangle result = new Rectangle(Point.Zero, focus.Display.Animation.AnimatedContentRect.Size);

            IRenderElement current = focus;

            Point currentLoc = result.Location;

            while (current != null)
            {
                currentLoc.X += current.Display.Animation.AnimatedContentRect.X;
                currentLoc.Y += current.Display.Animation.AnimatedContentRect.Y;

                current = current.Parent;

                if (current != null)
                {
                    currentLoc.X -= current.Display.ScrollPosition.X;
                    currentLoc.Y -= current.Display.ScrollPosition.Y;
                }
            }

            result.Location = currentLoc;

            return result;
        }

        private void Desktop_UnhandledEvent(UserInterfaceActionEventArgs args)
        {
            if (args.Action == UserInterfaceAction.Exit)
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
