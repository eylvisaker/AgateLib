using AgateLib.Tests.UserInterface.FF6;
using AgateLib.Tests.UserInterface.Support.Systems;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace AgateLib.Tests.UserInterface.Support
{
    public class UserInterfaceInitializer
    {
        private readonly UIContext context;
        private readonly IReadOnlyDictionary<string, Func<ITestSystem>> initializers;

        public UserInterfaceInitializer(UIContext context)
        {
            this.context = context;

            initializers = new Dictionary<string, Func<ITestSystem>>
            {
                { "title", () => new TitleSystem() },
                { "FF6", () => new FF6System() }
            };
        }

        public void OpenMenu()
        {
            context.Desktop.PushWorkspace(TestSystem.OpenMenu(context.RecordEvent));
        }

        public ITestSystem TestSystem
        {
            get => context.TestSystem;
            set => context.TestSystem = value;
        }

        public void WaitForAnimations()
        {
            context.WaitForAnimations();
        }

        public void SetParty(IEnumerable<IDictionary<string, string>> charAttributes)
        {
            TestSystem.SetParty(charAttributes);
        }

        public void SetInventory(IEnumerable<Item> items)
        {
            TestSystem.SetInventory(items);
        }

        public void Initialize(string menu)
        {
            initializers.Keys.Should().Contain(menu, "Could not find specified menu");

            context.TestSystem = initializers[menu]();
        }

        public void EquipPC(string pcName, string[] itemNames)
        {
            TestSystem.EquipPC(pcName, itemNames);
        }

        public void EmptyInventory()
        {
            TestSystem.EmptyInventory();
        }

        private void RecordEvent(string eventName)
        {
            context.RecordEvent(eventName);
        }
    }
}
