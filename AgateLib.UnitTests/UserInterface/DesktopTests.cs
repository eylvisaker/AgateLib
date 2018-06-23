using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Fakes;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Xunit;
using Moq;

namespace AgateLib.UserInterface
{
    public class DesktopTests
    {
        [Fact]
        public void Desktop_InputIsSentToActiveWorkspaceOnly()
        {
            Desktop desktop = new Desktop();

            var w1 = CommonMocks.Widget("w1");
            var w2 = CommonMocks.Widget("w2");

            var workspace1 = new Workspace("a");
            var workspace2 = new Workspace("b");

            workspace1.Add(w1.Object);
            workspace2.Add(w2.Object);

            desktop.PushWorkspace(workspace2);
            desktop.PushWorkspace(workspace1);

            desktop.ClearAnimations();
            int goodCalls = 0;
            int badCalls = 0;

            w1.Setup(x => x.ProcessEvent(It.IsAny<WidgetEventArgs>()))
                .Callback<WidgetEventArgs>(e => ++goodCalls);

            w2.Setup(x => x.ProcessEvent(It.IsAny<WidgetEventArgs>()))
                .Callback<WidgetEventArgs>(e => ++badCalls);

            desktop.ButtonDown(MenuInputButton.Down);
            desktop.ButtonUp(MenuInputButton.Down);

            goodCalls.Should().Be(2);
            badCalls.Should().Be(0);
        }

        [Fact]
        public void Desktop_MultipleWorkspaces()
        {
            Desktop desktop = new Desktop();

            bool exited = false;

            desktop.Empty += () => exited = true;

            var workspace1 = CreateWorkspace("default", CommonMocks.Widget("window").Object);
            var workspace2 = CreateWorkspace("other", CommonMocks.Widget("window").Object);

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

        private Workspace CreateWorkspace(string workspaceName, params Widget[] contents)
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

            Desktop desktop = new Desktop();
            Workspace workspace = new Workspace("");

            var widget = CommonMocks.Widget("happy");
            workspace.Add(widget.Object);

            desktop.PushWorkspace(workspace);

            desktop.Update(renderContext);

            widget.Object.Display.Instructions.Should().BeSameAs(desktop.Instructions);
        }
    }
}
