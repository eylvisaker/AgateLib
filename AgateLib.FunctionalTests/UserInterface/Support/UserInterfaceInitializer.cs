using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.FunctionalTests.UserInterface.Support.Initialization;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using FluentAssertions;
using TechTalk.SpecFlow;
using ManualTests.AgateLib.UserInterface.FF6;

namespace AgateLib.FunctionalTests.UserInterface.Support
{
    public class UserInterfaceInitializer
    {
        private readonly UIContext context;
        private readonly IReadOnlyDictionary<string, IMenuInitializer> initializers;

        private IMenuInitializer initializer;

        public UserInterfaceInitializer(UIContext context)
        {
            this.context = context;

            initializers = new Dictionary<string, IMenuInitializer>
            {
                { "title", new TitleMenuInitializer(context) },
                { "FF6", new FF6MenuInitializer(context) }
            };
        }

        public void WaitForAnimations()
        {
            context.WaitForAnimations();
        }

        public void SetParty(IEnumerable<IDictionary<string,string>> charAttributes)
        {
            initializer.SetParty(charAttributes);
        }

        public void SetInventory(IEnumerable<Item> items)
        {
            initializer.SetInventory(items);
        }

        public void Initialize(string menu)
        {
            initializers.Keys.Should().Contain(menu, "Could not find specified menu");

            initializer = initializers[menu];

            initializer.Initialize();

            context.Model = initializer.Model;
        }

        public void EquipPC(string pcName, string[] itemNames)
        {
            var pc = initializer.Model.Party.Find(pcName);
            var items = itemNames.Select(n => initializer.Model.FindInInventory(n));

            foreach (var item in items)
            {
                initializer.Model.EquipPC(pc, item);
            }
        }

        public void EmptyInventory()
        {
            initializer.Model.Inventory.Clear();
        }

        private void RecordEvent(string eventName)
        {
            context.RecordEvent(eventName);
        }
    }
}
