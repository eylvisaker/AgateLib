using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.FunctionalTests.UserInterface.Support;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AgateLib.FunctionalTests.UserInterface.Steps
{
    [Binding]
    public class InspectionSteps
    {
        private readonly UIContext context;

        public InspectionSteps(UIContext context)
        {
            this.context = context;
        }

        [Then(@"the active window is (.*)/(.*)")]
        public void ThenASpecificWorkspaceAndWindowAreActive(string workspace, string window)
        {
            context.ActiveWorkspace.Name.ToLowerInvariant().Should().Be(workspace.ToLowerInvariant());
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
            var items = context.Model.Inventory.ToList();

            items.Sort((x, y) =>
            {
                int type = x.ItemType.CompareTo(y.ItemType);
                int name = x.Name.CompareTo(y.Name);

                if (type != 0) return type;
                if (name != 0) return name;

                return 0;
            });

            context.Model.Inventory.Should().BeEquivalentTo(items, config => config.WithStrictOrdering());
        }

        [Then(@"(.*) is in slot (.*) in the inventory")]
        public void ThenItemIsInSlotXInTheInventory(string itemName, int slot)
        {
            var existing = context.Model.Inventory[slot];

            existing.Name.Should().BeEquivalentTo(itemName);
        }

        [Then(@"(.*) is the active menu item")]
        public void ThenXIsTheActiveMenuItem(string menuItemName)
        {
            var menu = context.ActiveWindow as Menu;

            menu.Layout.Focus.Name.Should().BeEquivalentTo(menuItemName);
        }

        [Then(@"(.*) is healed")]
        public void ThenPCIsHealed(string pcName)
        {
            var pc = context.Model.Party.Find(pcName);

            pc.HP.Should().Be(pc.MaxHP);
        }

    }
}