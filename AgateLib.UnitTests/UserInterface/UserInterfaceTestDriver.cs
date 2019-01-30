﻿using AgateLib.Input;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.UserInterface
{
    public class UserInterfaceTestDriver
    {
        private readonly UserInterfaceSceneDriver uiDriver;
        private readonly Workspace defaultWorkspace;
        private readonly ManualInputState input;

        public UserInterfaceTestDriver(IRenderable app,
                            IStyleConfigurator styleConfigurator = null,
                            IFontProvider fontProvider = null,
                            IContentLayoutEngine contentLayoutEngine = null)
        {
            var renderContext = CommonMocks.RenderContext(contentLayoutEngine).Object;

            uiDriver = new UserInterfaceSceneDriver(
                renderContext,
                styleConfigurator ?? new ThemeStyler(new ThemeCollection { ["default"] = Theme.CreateDefaultTheme() }),
                fontProvider ?? CommonMocks.FontProvider().Object);

            defaultWorkspace = new Workspace("default", app);

            input = new ManualInputState();

            uiDriver.Desktop.PushWorkspace(defaultWorkspace);

            DoLayout();
        }

        private void WaitForAnimations()
        {
            while (uiDriver.Desktop.ActiveWorkspace.AnimationState == AnimationState.TransitionIn ||
                   uiDriver.Desktop.ActiveWorkspace.AnimationState == AnimationState.TransitionOut)
            {
                uiDriver.Update(new GameTime(TimeSpan.FromSeconds(10),
                                             TimeSpan.FromSeconds(10)));
            }

            uiDriver.Update(new GameTime());
        }

        public Size Size { get; set; } = new Size(1280, 720);

        public Desktop Desktop => uiDriver.Desktop;

        public IRenderElement Focus => defaultWorkspace.Focus;

        public void DoLayout()
        {
            uiDriver.ScreenArea = new Rectangle(Point.Zero, Size);

            WaitForAnimations();

            foreach (var workspace in Desktop.Workspaces)
            {
                workspace.VisualTree.DoLayout(uiDriver.RenderContext, uiDriver.ScreenArea);
            }
        }

        public void Press(Buttons buttons, PlayerIndex playerIndex = PlayerIndex.One)
        {
            // Button down
            ButtonDown(buttons, playerIndex);

            // Button up
            ButtonUp();

            WaitForAnimations();
        }

        public void ButtonUp()
        {
            input.Reset();

            uiDriver.UpdateInput(input);
            uiDriver.Update(new GameTime());
        }

        private void ButtonDown(Buttons buttons, PlayerIndex playerIndex)
        {
            input.Reset();

            input.GamePadState[playerIndex] = new GamePadState(
                Vector2.Zero,
                Vector2.Zero,
                0,
                0,
                buttons);

            uiDriver.UpdateInput(input);
            uiDriver.Update(new GameTime());
        }
    }
}