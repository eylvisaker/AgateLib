using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.Support;
using AgateLib.UserInterface;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AgateLib.Tests.UserInterface.Steps
{
    [Binding]
    public class UserInterfaceSteps
    {
        private readonly UIContext context;
        private readonly Navigator navigator;
        private readonly Instructor instructor;

        public UserInterfaceSteps(UIContext context, Navigator navigator, Instructor instructor)
        {
            this.context = context;
            this.navigator = navigator;
            this.instructor = instructor;
        }

        [When(@"I select (.*)")]
        public void WhenISelectAMenuItem(string menuItem)
        {
            var items = menuItem.Split(',').Select(x => x.Trim());

            foreach (var item in items)
            {
                navigator.GoTo(item);
                instructor.SendButtonPress(UserInterfaceAction.Accept);
            }
        }

        [When(@"I press the (.*) button")]
        public void WhenIPressAButton(UserInterfaceAction input)
        {
            instructor.SendButtonPress(input);
        }

        [When(@"I press the (.*) button ([0-9]+) times")]
        public void WhenIPressAButton(UserInterfaceAction input, int times)
        {
            for (int i = 0; i < times; i++)
            {
                WhenIPressAButton(input);
            }
        }

        [Then(@"The game starts")]
        public void ThenTheGameStarts()
        {
            context.VerifyEvent("Game Start");
        }

        [Then(@"The game loads")]
        public void ThenTheGameLoads()
        {
            context.VerifyEvent("Game Load");
        }

        [Then(@"the (.*) workspace is active")]
        public void VerifyWorkspaceIsActive(string workspace)
        {
            context.Desktop.ActiveWorkspace.Should().NotBeNull();
            context.Desktop.ActiveWorkspace.Name
                .Should().BeEquivalentTo(workspace);
        }

        [Then(@"the exit event is triggered")]
        public void ThenTheExitEventIsTriggered()
        {
            context.ExitTriggered.Should().BeTrue();
        }

    }
}
