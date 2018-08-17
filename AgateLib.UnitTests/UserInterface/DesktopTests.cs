using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;
using Moq;
using AgateLib.UserInterface;
using AgateLib.Tests.Fakes;

namespace AgateLib.Tests.UserInterface
{
    public class DesktopTests
    {
        [Fact]
        public void Desktop_InputIsSentToActiveWorkspaceOnly()
        {
            Desktop desktop = new Desktop(CommonMocks.FontProvider().Object, CommonMocks.StyleConfigurator().Object);

            (var w1, var e1) = CommonMocks.Widget("w1", elementCanHaveFocus: true);
            (var w2, var e2) = CommonMocks.Widget("w2", elementCanHaveFocus: true);

            var workspace1 = new Workspace("a", w1.Object);
            var workspace2 = new Workspace("b", w2.Object);

            desktop.PushWorkspace(workspace2);
            desktop.PushWorkspace(workspace1);

            desktop.ClearAnimations();
            int goodCalls = 0;
            int badCalls = 0;

            e1.Setup(x => x.OnUserInterfaceAction(It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<UserInterfaceActionEventArgs>(e => ++goodCalls);

            e2.Setup(x => x.OnUserInterfaceAction(It.IsAny<UserInterfaceActionEventArgs>()))
                .Callback<UserInterfaceActionEventArgs>(e => ++badCalls);

            desktop.OnUserInterfaceAction(new UserInterfaceActionEventArgs().Reset(UserInterfaceAction.Down));

            goodCalls.Should().Be(1);
            badCalls.Should().Be(0);
        }

        [Fact]
        public void Desktop_MultipleWorkspaces()
        {
            Desktop desktop = new Desktop(CommonMocks.FontProvider().Object, CommonMocks.StyleConfigurator().Object);

            bool exited = false;

            desktop.Empty += () => exited = true;

            (var window1, var element1) = CommonMocks.Widget("window");
            (var window2, var element2) = CommonMocks.Widget("window");

            var workspace1 = CreateWorkspace("default", window1.Object);
            var workspace2 = CreateWorkspace("other", window2.Object);

            desktop.PushWorkspace(workspace1);
            desktop.PushWorkspace(workspace2); 

            desktop.ActiveWorkspace.Should().BeSameAs(workspace2);
            desktop.WaitForAnimations();
            desktop.ActiveWorkspace.Should().BeSameAs(workspace2);

            desktop.PopWorkspace();

            desktop.ActiveWorkspace.Should().BeSameAs(workspace2, "Active workspace should still be the old one, until the transition out animation is complete.");
            desktop.WaitForAnimations();
            desktop.ActiveWorkspace.Should().BeSameAs(workspace1, "Transition out animation is complete, so the active workspace should be the one underneath.");

            desktop.PopWorkspace();
            desktop.WaitForAnimations();

            exited.Should().BeTrue();
        }

        private Workspace CreateWorkspace(string workspaceName, params IWidget[] contents)
        {
            var result = new Workspace(workspaceName);

            foreach (var w in contents)
                result.Add(w);

            return result;
        }

        [Fact]
        public void Desktop_WidgetGetsInstructions()
        {
            var renderContext = new FakeRenderContext();

            Desktop desktop = new Desktop(CommonMocks.FontProvider().Object, CommonMocks.StyleConfigurator().Object);
            Workspace workspace = new Workspace("");

            (var widget, var element) = CommonMocks.Widget("happy");
            workspace.Add(widget.Object);

            desktop.PushWorkspace(workspace);

            desktop.Update(renderContext);

            element.Object.Display.System.Instructions.Should().BeSameAs(desktop.Instructions);
        }
    }
}
