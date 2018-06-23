using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using ManualTests.AgateLib.UserInterface.FF6;

namespace AgateLib.FunctionalTests.UserInterface.Support.Initialization
{
    public class FF6MenuInitializer : IMenuInitializer
    {
        private UIContext context;
        private FF6Menu menu;

        public FF6MenuInitializer(UIContext context)
        {
            this.context = context;
        }

        public FF6Model Model => menu.Model;

        public void Initialize()
        {
            this.menu = new FF6Menu(m => context.RecordEvent(m));

            menu.Begin(context.Desktop);
        }

        public void SetInventory(IEnumerable<Item> items)
        {
            menu.Model.Inventory.Clear();
            menu.Model.Inventory.AddRange(items);
        }

        public void SetParty(IEnumerable<IDictionary<string, string>> charAttributes)
        {
            var party = menu.Model.Party;

            party.Clear();

            foreach(var ch in charAttributes)
            {
                party.Add(new PlayerCharacter(ch["Name"], int.Parse(ch["MaxHP"]))
                {
                    HP = int.Parse(ch["HP"])
                });
            }

            menu.PartyUpdated();
        }
    }
}