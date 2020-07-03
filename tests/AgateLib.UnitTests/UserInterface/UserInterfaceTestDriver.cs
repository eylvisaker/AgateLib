using AgateLib.Input;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using System;

namespace AgateLib.UserInterface
{
    public class UserInterfaceTestDriver
    {
        private readonly UserInterfaceDriver uiDriver;
        private readonly Workspace defaultWorkspace;
        private readonly ManualInputState input;

        public UserInterfaceTestDriver(IRenderable app,
                                       Size? screenSize = null,
                                       IStyleConfigurator styleConfigurator = null,
                                       IFontProvider fontProvider = null,
                                       IContentLayoutEngine contentLayoutEngine = null,
                                       IAnimationFactory animationFactory = null)
        {
            var renderContext = CommonMocks.RenderContext(contentLayoutEngine).Object;
            Config = new UserInterfaceConfig { ScreenArea = new Rectangle(Point.Zero, screenSize ?? new Size(1280, 720)) };

            uiDriver = new UserInterfaceDriver(
                Config,
                renderContext,
                styleConfigurator ?? new ThemeStyler(new ThemeCollection(Config) { ["default"] = Theme.CreateDefaultTheme(Mock.Of<IContentProvider>()) }),
                fontProvider ?? CommonMocks.FontProvider().Object,
                animationFactory ?? new AnimationFactory());

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

            uiDriver.UpdateInput(new GameTime(), input);
            uiDriver.Update(new GameTime());
        }

        public Size Size => Config.ScreenArea.Size;

        public UserInterfaceConfig Config { get; }

        public Desktop Desktop => uiDriver.Desktop;

        public IRenderElement Focus => defaultWorkspace.Focus;

        public IRenderElement RootElement => defaultWorkspace.VisualTree.TreeRoot;

        public void DoLayout()
        {
            uiDriver.ScreenArea = new Rectangle(Point.Zero, Size);

            WaitForAnimations();

            foreach (var workspace in Desktop.Workspaces)
            {
                workspace.VisualTree.DoLayout(uiDriver.RenderContext);
            }
        }

        public void Press(Buttons buttons, PlayerIndex playerIndex = PlayerIndex.One, int repeatCount = 1)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                // Button down
                ButtonDown(buttons, playerIndex);

                // Button up
                ButtonUp();

                WaitForAnimations();
            }
        }

        public void ButtonUp()
        {
            input.Reset();

            uiDriver.UpdateInput(new GameTime(), input);
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

            uiDriver.UpdateInput(new GameTime(), input);
            uiDriver.Update(new GameTime());
        }
    }
}
