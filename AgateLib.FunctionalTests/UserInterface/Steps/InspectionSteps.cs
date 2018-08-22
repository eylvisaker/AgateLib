using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Tests.UserInterface.Support;
using AgateLib.Tests.UserInterface.Support.Systems;
using AgateLib.UserInterface;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AgateLib.Tests.UserInterface.Steps
{
    [Binding]
    public class InspectionSteps
    {
        private readonly UIContext context;

        public InspectionSteps(UIContext context)
        {
            this.context = context;
        }

        public ITestSystem TestSystem => context.TestSystem;

        [Then(@"the active window is (.*)/(.*)")]
        public void ThenASpecificWorkspaceAndWindowAreActive(string workspace, string window)
        {
            context.ActiveWorkspace.Name.ToLowerInvariant().Should().Be(workspace.ToLowerInvariant());

            context.ActiveWindow.Name.Should().NotBeNull($"workspace {workspace} was found to be the " +
                $"active workspace, but the active window has no name");            

            context.ActiveWindow.Name.ToLowerInvariant().Should().Be(window.ToLowerInvariant());
        }

        [Then(@"(.*) is equipped on (.*) in the (.*) slot")]
        public void ThenAnItemIsEquipped(string itemName, string pcName, string slot)
        {
            TestSystem.VerifyItemIsEquipped(itemName, pcName, slot);
        }

        [Then(@"(.*) is in the inventory")]
        public void ThenAnItemIsInTheInventory(string itemName)
        {
            TestSystem.VerifyItemIsInInventory(itemName);
        }

        [Then(@"the items are arranged")]
        public void ThenTheItemsAreArranged()
        {
            TestSystem.VerifyItemsAreArranged();
        }

        [Then(@"(.*) is in slot (.*) in the inventory")]
        public void ThenItemIsInSlotXInTheInventory(string itemName, int slot)
        {
            TestSystem.VerifyItemIsInSlotXInTheInventory(itemName, slot);
        }

        [Then(@"(.*) is the active menu item")]
        public void ThenXIsTheActiveMenuItem(string menuItemName)
        {
            var focus = context.ActiveWorkspace.Focus as ButtonElement;

            focus.Props.Text.Should().BeEquivalentTo(menuItemName);
            //var menu = context.ActiveWindow as Menu;

            //throw new NotImplementedException();
            ////menu.Layout.Focus.Name.Should().BeEquivalentTo(menuItemName);
        }

        [Then(@"(.*) is healed")]
        public void ThenPCIsHealed(string pcName)
        {
            TestSystem.VerifyPCIsHealed(pcName);
        }

    }
}