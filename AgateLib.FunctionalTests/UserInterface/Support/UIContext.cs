using AgateLib.Tests.UserInterface.FF6;
using AgateLib.Tests.UserInterface.Support.Systems;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering.Animations;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.Tests.UserInterface.Support
{
    public class UIContext
    {
        private readonly UserInterfaceSceneDriver scene;
        private readonly List<string> events = new List<string>();
        private readonly IUserInterfaceRenderContext renderContext;

        public UIContext()
        {
            renderContext = CommonMocks.RenderContext().Object;

            scene = new UserInterfaceSceneDriver(
                renderContext,
                CommonMocks.StyleConfigurator().Object,
                CommonMocks.FontProvider().Object,
                new AnimationFactory());

            scene.ScreenArea = new Rectangle(0, 0, 1280, 720);
        }

        public ITestSystem TestSystem { get; set; }

        public UserInterfaceSceneDriver Scene => scene;

        public Desktop Desktop => scene.Desktop;

        public IRenderElement ActiveWindow
        {
            get
            {
                var workspace = Desktop.ActiveWorkspace;

                var item = workspace.Focus;

                while (item.Parent?.Parent != null)
                    item = item.Parent;

                return item;
            }
        }

        public Workspace ActiveWorkspace => Desktop.ActiveWorkspace;

        public bool ExitTriggered { get; private set; }

        public FF6Model Model { get; set; }

        public void RecordEvent(string name)
        {
            events.Add(name);
        }

        public void VerifyEvent(string name)
        {
            events.Should().Contain(name);
        }

        public void WaitForAnimations()
        {
            Desktop.WaitForAnimations();
        }
    }
}