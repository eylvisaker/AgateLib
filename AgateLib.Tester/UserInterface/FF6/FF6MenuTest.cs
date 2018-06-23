using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Scenes;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using ManualTests.AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ManualTests.AgateLib.UserInterface.FF6
{
    public class FF6MenuTest : ITest
    {
        private SceneStack stack;
        private UserInterfaceScene scene;
        private FF6Menu menu;

        public string Name => "FF6 Menu";

        public string Category => "User Interface";

        public void Initialize(ITestResources resources)
        {
            scene = new UserInterfaceScene(
                resources.GraphicsDevice,
                resources.UserInterfaceRenderer,
                resources.LocalizedContent,
                resources.StyleConfigurator)
            {
                DrawBelow = false,
                Theme = "FF",
            };

            menu = new FF6Menu(m => Debug.WriteLine(m));

            InitializeTestData(menu.Model = new FF6Model());

            stack = resources.SceneStack;
            stack.Add(scene);

            scene.Initialize();
            scene.Indicator = new PointerIndicator(
                resources.Content.Load<Texture2D>("UserInterface/Pointer"));

            menu.Begin(scene.Desktop);
        }

        private void InitializeTestData(FF6Model model)
        {
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
        }

        public void Update(GameTime gameTime)
        {
            stack.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            stack.Draw(gameTime);
        }
    }
}
