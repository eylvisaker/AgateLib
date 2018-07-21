using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Tests.UserInterface.Support;
using AgateLib.Tests.UserInterface.Support.Systems;
using AgateLib.UserInterface.Widgets;
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
            var pc = context.Model.Party.Characters.FirstOrDefault(x =>
                x.Name.Equals(pcName, StringComparison.OrdinalIgnoreCase))
                ?? throw new ArgumentException($"Could not find character named {pcName}");

            var item = pc.Equipment[slot];

            if (itemName == "nothing")
            {
                item.Should().BeNull();
            }
            else
            {
                item.Should().NotBeNull($"{itemName} should be equipped at {slot}");
                item.Name.Should().Be(itemName);
            }
        }

        [Then(@"(.*) is in the inventory")]
        public void ThenAnItemIsInTheInventory(string itemName)
        {
            var item = context.Model.FindInInventory(itemName);
            item.Should().NotBeNull();
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
            var focus = context.ActiveWorkspace.Focus as MenuItemElement;

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