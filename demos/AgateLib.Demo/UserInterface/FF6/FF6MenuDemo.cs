﻿using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Demo.UserInterface.FF6
{
    public class FF6MenuDemo : UIDemo
    {
        private FF6Menu menu;

        public override string Name => "FF6 Menu";

        public override string Category => "User Interface";

        protected override IRenderable CreateUIRoot()
        {
            menu = new FF6Menu(InitializeTestData());

            menu.ExitMenu += () => Scene.Exit();

            return menu.InitializeUIRoot();
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
