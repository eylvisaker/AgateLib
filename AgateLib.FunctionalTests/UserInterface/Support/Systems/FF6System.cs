using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using FluentAssertions;

namespace AgateLib.Tests.UserInterface.Support.Systems
{
    class FF6System : ITestSystem
    {
        FF6Model model = new FF6Model();

        public void EmptyInventory()
        {
            model.Inventory.Clear();
        }

        public void EquipPC(string pcName, string[] itemNames)
        {
            var pc = model.Party.Find(pcName);
            var items = itemNames.Select(n => model.FindInInventory(n));

            foreach (var item in items)
            {
                model.EquipPC(pc, item);
            }
        }

        public Workspace OpenMenu(Action<string> log)
            => new FF6Menu(model).InitializeWorkspace();
        
        public void SetInventory(IEnumerable<Item> items)
        {
            model.Inventory.Clear();
            model.Inventory.AddRange(items);
        }

        public void SetParty(IEnumerable<IDictionary<string, string>> charAttributes)
        {
            var party = model.Party;

            party.Clear();

            foreach (var ch in charAttributes)
            {
                party.Add(new PlayerCharacter(ch["Name"], int.Parse(ch["MaxHP"]))
                {
                    HP = int.Parse(ch["HP"])
                });
            }
        }

        public void VerifyItemIsEquipped(string itemName, string pcName, string slot)
        {
            var pc = model.Party.Characters.FirstOrDefault(x =>
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

        public void VerifyItemIsInInventory(string itemName)
        {
            var item = model.FindInInventory(itemName);
            item.Should().NotBeNull();
        }

        public void VerifyItemIsInSlotXInTheInventory(string itemName, int slot)
        {
            var existing = model.Inventory[slot];

            existing.Name.Should().BeEquivalentTo(itemName);
        }

        public void VerifyItemsAreArranged()
        {
            var items = model.Inventory.ToList();

            items.Sort((x, y) =>
            {
                int type = x.ItemType.CompareTo(y.ItemType);
                int name = x.Name.CompareTo(y.Name);

                if (type != 0) return type;
                if (name != 0) return name;

                return 0;
            });

            model.Inventory.Should().BeEquivalentTo(items, config => config.WithStrictOrdering());
        }

        public void VerifyPCIsHealed(string pcName)
        {
            var pc = model.Party.Find(pcName);

            pc.HP.Should().Be(pc.MaxHP);
        }
    }
}
