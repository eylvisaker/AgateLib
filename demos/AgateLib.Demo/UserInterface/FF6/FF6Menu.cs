﻿using AgateLib.Demo.UserInterface.FF6.Widgets;
using AgateLib.UserInterface;
using System;
using System.Linq;

namespace AgateLib.Demo.UserInterface.FF6
{
    public class FF6Menu
    {
        private FF6MainMenu mainMenu;

        public FF6Menu(FF6Model model)
        {
            Model = model;

            InitializeComponent();
        }

        public event Action ExitMenu;

        public IRenderable InitializeUIRoot()
        {
            return mainMenu;
        }

        public FF6Model Model { get; private set; }

        private void InitializeComponent()
        {
            mainMenu = InitializeMainMenu();
        }

        private Item BestItem(string slotName)
        {
            var slot = Model.EquipmentSlots.First(x => x.Name.Equals(slotName, StringComparison.OrdinalIgnoreCase));

            var items = Model.Inventory.Where(x => slot.AllowedItemTypes.Contains(x.ItemType, StringComparer.OrdinalIgnoreCase));

            return items.FirstOrDefault();
        }

        private FF6MainMenu InitializeMainMenu()
        {
            var mainMenu = new FF6MainMenu(new FF6MainMenuProps
            {
                Name = "main",
                Model = Model,
                OnExit = e => ExitMenu?.Invoke(),
                OnArrangeItems = e => ArrangeItems(),
                OnUseItem = UseItem,
                OnSwapItems = SwapItems,
                OnEquip = EquipItem,
                OnEquipRemove = UnequipItem,
                OnEquipEmpty = EmptyEquipment,
                OnEquipOptimum = EquipOptimum,
            });

            return mainMenu;
        }

        private void EquipOptimum(UserInterfaceEvent<PlayerCharacter> e)
        {
            Model.EquipPC(e.Arg1, BestItem("L-Hand"), "L-Hand");
            Model.EquipPC(e.Arg1, BestItem("R-Hand"), "R-Hand");
            Model.EquipPC(e.Arg1, BestItem("Head"), "Head");
            Model.EquipPC(e.Arg1, BestItem("Body"), "Body");
        }

        private void EmptyEquipment(UserInterfaceEvent<PlayerCharacter> e)
        {
            Model.EquipPC(e.Arg1, null, "L-Hand");
            Model.EquipPC(e.Arg1, null, "R-Hand");
            Model.EquipPC(e.Arg1, null, "Head");
            Model.EquipPC(e.Arg1, null, "Body");
        }

        private void UnequipItem(UserInterfaceEvent<PlayerCharacter, string> e)
        {
            var pc = e.Arg1;
            var slot = e.Arg2;

            Model.EquipPC(pc, null, slot);
        }

        private void EquipItem(UserInterfaceEvent<PlayerCharacter, string, Item> e)
        {
            var pc = e.Arg1;
            var slot = e.Arg2;
            var item = e.Arg3;

            Model.EquipPC(pc, item);
        }

        private void SwapItems(UserInterfaceEvent<Tuple<int, int>> e)
        {
            var a = Model.Inventory[e.Arg1.Item1];
            var b = Model.Inventory[e.Arg1.Item2];

            Model.Inventory[e.Arg1.Item2] = a;
            Model.Inventory[e.Arg1.Item1] = b;
        }

        private void ArrangeItems()
        {
            Model.Inventory.Sort((x, y) =>
            {
                int type = x.ItemType.CompareTo(y.ItemType);
                int name = x.Name.CompareTo(y.Name);

                if (type != 0)
                {
                    return type;
                }

                if (name != 0)
                {
                    return name;
                }

                return 0;
            });
        }

        private void UseItem(UserInterfaceEvent<Tuple<Item, PlayerCharacter>> e)
        {
            var item = e.Arg1.Item1;
            var targetPc = e.Arg1.Item2;

            switch (item.Effect)
            {
                case "heal":
                    targetPc.HP += item.EffectAmount;
                    targetPc.HP = Math.Min(targetPc.HP, targetPc.MaxHP);

                    break;
            }
        }
    }
}
