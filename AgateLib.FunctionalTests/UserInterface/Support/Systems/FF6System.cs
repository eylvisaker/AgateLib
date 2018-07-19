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
            throw new NotImplementedException();
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

        public void VerifyPCIsHealed(string pcName)
        {
            var pc = model.Party.Find(pcName);

            pc.HP.Should().Be(pc.MaxHP);
        }
    }
}
