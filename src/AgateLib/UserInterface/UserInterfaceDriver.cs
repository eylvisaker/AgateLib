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

using AgateLib.Display;
using AgateLib.Input;
using AgateLib.Quality;
using AgateLib.UserInterface.InputMap;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AgateLib.UserInterface
{
    public class UserInterfaceDriver
    {
        private IUserInterfaceRenderContext renderContext;
        private Desktop desktop;

        private UserInterfaceInputEvents uiInput = new UserInterfaceInputEvents();

        private bool setIgnoreInput;
        private List<UserInterfaceAction> ignoreInputsFor = new List<UserInterfaceAction>();

        public UserInterfaceDriver(UserInterfaceConfig config,
                                   IUserInterfaceRenderContext renderContext,
                                   IStyleConfigurator styles,
                                   IFontProvider fonts,
                                   IAnimationFactory animationFactory,
                                   IUserInterfaceAudio audio = null,
                                   ICursor cursor = null)
        {
            Config = config;
            this.renderContext = renderContext;

            Require.That(config.ScreenArea.Width > 0, "Screen area width must be positive.");
            Require.That(config.ScreenArea.Height > 0, "Screen area height must be positive.");

            desktop = new Desktop(config, renderContext, fonts, styles, animationFactory, audio);

            desktop.FocusChanged += Desktop_FocusChanged;
            Desktop.UnhandledEvent += Desktop_UnhandledEvent;

            uiInput.UIAction += desktop.OnUserInterfaceAction;

            uiInput.ButtonDown += desktop.OnButtonDown;
            uiInput.ButtonUp += desktop.OnButtonUp;

            Cursor = cursor ?? new ThemedCursor(styles);
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
        public UserInterfaceConfig Config { get; }

        /// <summary>
        /// Gets the object which handles rendering of the indicator.
        /// </summary>
        public ICursor Cursor { get; set; }

        /// <summary>
        /// If true, the Exit event will be raised when the user
        /// presses the back button on the controller.
        /// </summary>
        public bool ExitOnExitButton { get; set; }

        /// <summary>
        /// Gets or sets the screen area the user interface will draw to.
        /// </summary>
        public Rectangle ScreenArea
        {
            get => Config.ScreenArea;
            set => Config.ScreenArea = value;
        }

        public void Initialize()
        {
            setIgnoreInput = true;

            uiInput.ClearPressedButtons();
        }

        private void Desktop_FocusChanged()
        {
            ignoreInputsFor.Add(UserInterfaceAction.Accept);
            ignoreInputsFor.Add(UserInterfaceAction.Cancel);
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

        public void UpdateInput(GameTime time, IInputState input)
        {
            if (setIgnoreInput)
            {
                uiInput.IgnoreCurrentInput(input);
                setIgnoreInput = false;
            }
            else if (ignoreInputsFor.Count > 0)
            {
                foreach (var action in ignoreInputsFor)
                {
                    uiInput.IgnoreCurrentInput(input, action);
                }

                ignoreInputsFor.Clear();
            }

            uiInput.UpdateState(input);
        }

        public void Update(GameTime time)
        {
            uiInput.TriggerEvents(time);

            renderContext.InitializeUpdate(time);

            desktop.Update(renderContext);

            Cursor.UserInterfaceRenderer = renderContext.UserInterfaceRenderer;
            Cursor.Update(time);

            if (desktop.Workspaces.Count == 0)
            {
                ExitPressed?.Invoke();
            }
        }

        public void Draw(GameTime time, ICanvas canvas, RenderTarget2D renderTarget = null)
        {
            renderContext.PrepDraw(time, canvas, renderTarget);

            desktop.Draw(renderContext);

            if (Cursor != null)
            {
                Cursor.UserInterfaceRenderer = renderContext.UserInterfaceRenderer;

                if (desktop.ActiveWorkspace?.Focus != null)
                {
                    Rectangle animatedScreenArea = ScreenAreaOf(desktop.ActiveWorkspace.Focus);

                    Cursor.MoveToFocus(desktop.ActiveWorkspace,
                                        desktop.ActiveWorkspace.Focus,
                                        animatedScreenArea,
                                        animatedScreenArea);
                }
            }

            Cursor?.Draw(time, renderContext.Canvas, desktop.VisualScaling);
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
