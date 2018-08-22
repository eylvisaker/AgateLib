using AgateLib.Input;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class TestUIDriver
    {
        private readonly UserInterfaceSceneDriver uiDriver;
        private readonly Workspace defaultWorkspace;
        private readonly ManualInputState input;

        public TestUIDriver(IRenderable app, 
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

        public Size Size { get; set; } = new Size(1280, 720);

        public Desktop Desktop => uiDriver.Desktop;

        public IRenderElement Focus => defaultWorkspace.Focus;

        public void DoLayout()
        {
            uiDriver.Update(new GameTime());

            foreach (var workspace in Desktop.Workspaces)
            {
                var root = workspace.VisualTree.TreeRoot;

                root.DoLayout(uiDriver.RenderContext, Size);
            }
        }

        public void Press(Buttons buttons, PlayerIndex playerIndex = PlayerIndex.One)
        {
            // Button down
            ButtonDown(buttons, playerIndex);

            // Button up
            ButtonUp();
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
