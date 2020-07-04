using AgateLib.Demo.UserInterface.MultipleWorkspaces;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace AgateLib.UserInterface.MultipleWorkspaces
{
    public class MultiWorkspaceUnitTests
    {
        [Fact]
        public void NewWorkspaceIsOpened()
        {
            var app = new MultiWorkspaceApp(new MultiWorkspaceAppProps
            {

            });

            var driver = new UserInterfaceTestDriver(app);

            driver.Desktop.Workspaces.Count.Should().Be(1);

            driver.Press(Buttons.A);

            driver.Desktop.Workspaces.Count.Should().Be(2);
        }

        [Fact]
        public void StatusLabelIsUpdatedWithinWorkspace()
        {
            var statusLabelRef = new ElementReference();

            var app = new MultiWorkspaceApp(new MultiWorkspaceAppProps
            {
                StatusLabelRef = statusLabelRef,
            });

            var driver = new UserInterfaceTestDriver(app);

            var labelProps = statusLabelRef.Current.Props as LabelElementProps;
            labelProps.Text.Should().Contain("first workspace");

            driver.Press(Buttons.DPadDown);

            labelProps = statusLabelRef.Current.Props as LabelElementProps;
            labelProps.Text.Should().Contain("second workspace");
        }

        [Fact]
        public void StatusLabelIsUpdatedBetweenWorkspaces()
        {
            var statusLabelRef = new ElementReference();

            var app = new MultiWorkspaceApp(new MultiWorkspaceAppProps
            {
                StatusLabelRef = statusLabelRef,
            });

            var driver = new UserInterfaceTestDriver(app);

            var labelProps = statusLabelRef.Current.Props as LabelElementProps;
            labelProps.Text.Should().Contain("first workspace");

            driver.Press(Buttons.A);

            labelProps = statusLabelRef.Current.Props as LabelElementProps;
            labelProps.Text.Should().Contain("pancakes");
        }
    }
}
