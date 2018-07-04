using System;
using System.Collections.Generic;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.Tests;

namespace AgateLib.Tests.UserInterface.Support
{
    public class UIContext
    {
        private readonly UserInterfaceSceneDriver scene;
        private readonly List<string> events = new List<string>();
        private readonly IWidgetRenderContext renderContext;

        public UIContext()
        {
            renderContext = CommonMocks.RenderContext().Object;

            scene = new UserInterfaceSceneDriver(
                renderContext,
                CommonMocks.StyleConfigurator().Object,
                CommonMocks.FontProvider().Object);
        }

        public UserInterfaceSceneDriver Scene => scene;

        [Obsolete("Use Scene instead.")]
        public Desktop Desktop => scene.Desktop;

        public IWidget ActiveWindow => Desktop.ActiveWorkspace.ActiveWindow;

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