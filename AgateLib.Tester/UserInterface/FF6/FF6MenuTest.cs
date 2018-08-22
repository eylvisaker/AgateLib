using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Scenes;
using AgateLib.UserInterface;
using AgateLib.Tests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6MenuTest : UITest
    {
        private FF6Menu menu;

        public override string Name => "FF6 Menu";

        public override string Category => "User Interface";

        protected override Workspace InitializeWorkspace()
        {
            Scene.Theme = "FF";

            menu = new FF6Menu(InitializeTestData());

            menu.ExitMenu += () => Scene.Exit();

            Scene.Indicator = new PointerIndicator(Content.Load<Texture2D>("UserInterface/Pointer"));

            return menu.InitializeWorkspace();
        }

        private FF6Model InitializeTestData()
        {
            FF6Model model = new FF6Model();

            model.Party.Add(new PlayerCharacter("Tora", 100));
            model.Party.Add(new PlayerCharacter("Unlocke", 120));
            model.Party.Add(new PlayerCharacter("Deadgar", 150));
            model.Party.Add(new PlayerCharacter("Sabo", 170));

            model.Inventory.Add(new Item { Name = "Short Sword", ItemType = "Weapon" });
            model.Inventory.Add(new Item { Name = "Long Sword", ItemType = "Weapon" });
            model.Inventory.Add(new Item { Name = "Leather Shield", ItemType = "Shield" });
            model.Inventory.Add(new Item { Name = "Leather Armor", ItemType = "Armor" });
            model.Inventory.Add(new Item { Name = "Leather Helm", ItemType = "Helm" });
            model.Inventory.Add(new Item { Name = "Sprint Shoes", ItemType = "Relic" });
            model.Inventory.Add(new Item { Name = "Running Shoes", ItemType = "Relic" });
            model.Inventory.Add(new Item { Name = "White Cape", ItemType = "Relic" });
            model.Inventory.Add(new Item { Name = "Dried Meat", ItemType = "Item", Effect = "heal", EffectAmount = 999 });

            return model;
        }
    }
}
