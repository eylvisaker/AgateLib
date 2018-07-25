﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;
using AgateLib.Tests.UserInterface.FF6.Widgets;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6Menu
    {
        private Menu pcMenu;
        private PlayerCharacter selectedPC;
        private Menu itemsList;
        private Menu magicList;
        private Menu esperList;
        private Action<PlayerCharacter> AfterItemTarget;
        private Menu itemTarget;
        private Workspace mainWorkspace;
        private Workspace skillsWorkspace;
        private Workspace magicWorkspace;
        private Workspace espersWorkspace;
        private Workspace relicWorkspace;
        private Workspace equipWorkspace;

        public FF6Menu(FF6Model model)
        {
            Model = model;

            InitializeComponent();
        }

        public Workspace InitializeWorkspace()
        {
            return mainWorkspace;
        }

        public FF6Model Model { get; private set; }

        private void InitializeComponent()
        {
            mainWorkspace = InitializeMainMenu();
            //InitializeSkillsMenu(skillsWorkspace = new Workspace("skills"));
            //InitializeMagicMenu(magicWorkspace = new Workspace("magic"));
            //InitializeEspersMenu(espersWorkspace = new Workspace("espers"));
            //InitializeEquipMenu(equipWorkspace = new Workspace("equip"));
            //InitializeRelicMenu(relicWorkspace = new Workspace("relic"));
        }

        private void InitializeRelicMenu(Workspace workspace)
        {
            throw new NotImplementedException();

            //var relicMenu = new Menu("Relic");
            //var slotsMenu = new Menu("Slots");
            //var itemsMenu = new Menu("Items");

            //Action<string> slotsFollowUp = null;

            //void EquipMenuFor(string location)
            //{
            //    workspace.ActivateWindow("Items");

            //    itemsMenu.Layout.Clear();

            //    foreach (var item in Model.Inventory)
            //    {
            //        itemsMenu.Add(item.Name, () => Model.EquipPC(selectedPC, item, location));
            //    }
            //}

            //relicMenu.Add("Equip", () =>
            //{
            //    slotsFollowUp = EquipMenuFor;
            //    workspace.ActivateWindow(slotsMenu);
            //});

            //relicMenu.Add("Remove", () =>
            //{
            //    slotsFollowUp = location =>
            //    {
            //        Model.EquipPC(selectedPC, null, location);
            //    };
            //    workspace.ActivateWindow(slotsMenu);
            //});

            //slotsMenu.Add("Relic 1", "Relic", () => slotsFollowUp("Relic 1"));
            //slotsMenu.Add("Relic 2", "Relic", () => slotsFollowUp("Relic 2"));

            //var layout = new FixedGridLayout(2, 6);

            //layout.Add(relicMenu, new Rectangle(0, 0, 2, 1));
            //layout.Add(slotsMenu, new Rectangle(0, 1, 2, 2));
            //layout.Add(itemsMenu, new Rectangle(0, 3, 1, 3));

            //workspace.Layout = layout;
        }

        private void InitializeEquipMenu(Workspace workspace)
        {
            throw new NotImplementedException();

            //var equipMenu = new Menu("Equip") { LayoutType = LayoutType.SingleRow };
            //var slotsMenu = new Menu("Slots");
            //var itemsMenu = new Menu("Items");

            //Action<string> slotsFollowUp = null;

            //void EquipMenuFor(string location)
            //{
            //    workspace.ActivateWindow("Items");

            //    itemsMenu.Layout.Clear();

            //    foreach (var item in Model.ItemsForSlot(selectedPC, location))
            //    {
            //        itemsMenu.Add(item.Name, () =>
            //        {
            //            Model.EquipPC(selectedPC, item, location);
            //            workspace.ActivateWindow("Slots");

            //            UpdateEquipmentSlots();

            //            itemsMenu.Layout.Clear();
            //        });
            //    }
            //}

            //equipMenu.Add("Equip", () =>
            //{
            //    slotsFollowUp = EquipMenuFor;
            //    workspace.ActivateWindow("Slots");

            //    UpdateEquipmentSlots();
            //});

            //equipMenu.Add("Optimum", () =>
            //{
            //    Model.EquipPC(selectedPC, BestItem("L-Hand"), "L-Hand");
            //    Model.EquipPC(selectedPC, BestItem("R-Hand"), "R-Hand");
            //    Model.EquipPC(selectedPC, BestItem("Head"), "Head");
            //    Model.EquipPC(selectedPC, BestItem("Body"), "Body");

            //    UpdateEquipmentSlots();
            //});

            //equipMenu.Add("Remove", () =>
            //{
            //    slotsFollowUp = location =>
            //    {
            //        Model.EquipPC(selectedPC, null, location);

            //        UpdateEquipmentSlots();
            //    };

            //    workspace.ActivateWindow("Slots");
            //});

            //equipMenu.Add("Empty", () =>
            //{
            //    Model.EquipPC(selectedPC, null, "L-Hand");
            //    Model.EquipPC(selectedPC, null, "R-Hand");
            //    Model.EquipPC(selectedPC, null, "Head");
            //    Model.EquipPC(selectedPC, null, "Body");

            //    UpdateEquipmentSlots();
            //});

            //equipMenu.Exit += () => desktop.PopWorkspace();

            //slotsMenu.Add("L-Hand", () => slotsFollowUp("L-Hand"));
            //slotsMenu.Add("R-Hand", () => slotsFollowUp("R-Hand"));
            //slotsMenu.Add("Head", () => slotsFollowUp("Head"));
            //slotsMenu.Add("Body", () => slotsFollowUp("Body"));

            //slotsMenu.Exit += () => workspace.ActivateWindow(equipMenu);

            //itemsMenu.Exit += () =>
            //{
            //    itemsMenu.Layout.Clear();
            //    workspace.ActivateWindow(slotsMenu);
            //};

            //var layout = new FixedGridLayout(2, 6);

            //layout.Add(equipMenu, new Rectangle(0, 0, 2, 1));
            //layout.Add(slotsMenu, new Rectangle(0, 1, 2, 2));
            //layout.Add(itemsMenu, new Rectangle(0, 3, 1, 3));

            //workspace.Layout = layout;
        }

        private Item BestItem(string slotName)
        {
            var slot = Model.EquipmentSlots.First(x => x.Name.Equals(slotName, StringComparison.OrdinalIgnoreCase));

            var items = Model.Inventory.Where(x => slot.AllowedItemTypes.Contains(x.ItemType, StringComparer.OrdinalIgnoreCase));

            return items.FirstOrDefault();
        }


        private Workspace InitializeMainMenu()
        {
            var selectPCRef = new ElementReference();

            var mainMenu = new FF6MainMenu(new FF6MainMenuProps
            {
                Name = "main",
                Model = Model,
                OnArrangeItems = e => ArrangeItems(),
                OnUseItem = UseItem,
                OnSwapItems = SwapItems,
                OnEquip = EquipItem,
                OnEquipRemove = UnequipItem,
                OnEquipEmpty = EmptyEquipment,
                OnEquipOptimum = EquipOptimum,
            });

            return new Workspace("default", mainMenu);
        }

        private void EquipOptimum(UserInterfaceEvent<PlayerCharacter> e)
        {
            Model.EquipPC(e.Data, BestItem("L-Hand"), "L-Hand");
            Model.EquipPC(e.Data, BestItem("R-Hand"), "R-Hand");
            Model.EquipPC(e.Data, BestItem("Head"), "Head");
            Model.EquipPC(e.Data, BestItem("Body"), "Body");
        }

        private void EmptyEquipment(UserInterfaceEvent<PlayerCharacter> e)
        {
            Model.EquipPC(e.Data, null, "L-Hand");
            Model.EquipPC(e.Data, null, "R-Hand");
            Model.EquipPC(e.Data, null, "Head");
            Model.EquipPC(e.Data, null, "Body");
        }

        private void UnequipItem(UserInterfaceEvent<PlayerCharacter, string> e)
        {
            var pc = e.Data1;
            var slot = e.Data2;

            Model.EquipPC(pc, null, slot);
        }

        private void EquipItem(UserInterfaceEvent<PlayerCharacter, string, Item> e)
        {
            var pc = e.Data1;
            var slot = e.Data2;
            var item = e.Data3;

            Model.EquipPC(pc, item);
        }

        private void SwapItems(UserInterfaceEvent<Tuple<int, int>> e)
        {
            var a = Model.Inventory[e.Data.Item1];
            var b = Model.Inventory[e.Data.Item2];

            Model.Inventory[e.Data.Item2] = a;
            Model.Inventory[e.Data.Item1] = b;
        }

        private void UpdateEspers()
        {
        }

        private void UpdateMagic()
        {
            throw new NotImplementedException();
            //magicList.Layout.Clear();

            //foreach (var item in selectedPC.Magic)
            //{
            //    magicList.Add(item.Name, () => RecordEvent("Used magic " + item.Name));
            //}
        }


        private void UpdateItems()
        {
            throw new NotImplementedException();
            //itemsList.Layout.Clear();
            //Item selectedItem = null;

            //void ItemAccept(Item item)
            //{
            //    if (selectedItem == null)
            //    {
            //        selectedItem = item;
            //    }
            //    else
            //    {
            //        if (selectedItem != item)
            //        {
            //            var spot1 = Model.Inventory.IndexOf(selectedItem);
            //            var spot2 = Model.Inventory.IndexOf(item);

            //            Model.Inventory[spot1] = item;
            //            Model.Inventory[spot2] = selectedItem;
            //        }
            //        else
            //        {
            //            UseItem(item);
            //        }

            //        selectedItem = null;
            //    }
            //}

            //foreach (var item in Model.Inventory)
            //{
            //    itemsList.Add(item.Name, () => ItemAccept(item));
            //}
        }

        private void ArrangeItems()
        {
            Model.Inventory.Sort((x, y) =>
            {
                int type = x.ItemType.CompareTo(y.ItemType);
                int name = x.Name.CompareTo(y.Name);

                if (type != 0) return type;
                if (name != 0) return name;

                return 0;
            });
        }

        private void UseItem(UserInterfaceEvent<Tuple<Item, PlayerCharacter>> e)
        {
            var item = e.Data.Item1;
            var targetPc = e.Data.Item2;

            switch (item.Effect)
            {
                case "heal":
                    targetPc.HP += item.EffectAmount;
                    targetPc.HP = Math.Min(targetPc.HP, targetPc.MaxHP);

                    break;
            }
        }

        private void StartSkillsMenu()
        {
            RecordEvent("Skills");

            SelectPC(e =>
            {
                e.System.PushWorkspace(skillsWorkspace);
            });
        }

        private void StartEquipMenu()
        {
            RecordEvent("Equip");

            SelectPC(e =>
            {
                e.System.PushWorkspace(equipWorkspace);
                UpdateEquipmentSlots();
            });
        }

        private void UpdateEquipmentSlots()
        {
            throw new NotImplementedException();

            //var slots = equipWorkspace.FindWindow<Menu>(x => x.Name == "Slots");

            //foreach (var item in slots.Layout.OfType<ContentMenuItem>())
            //{
            //    var type = item.Name;

            //    item.Text = type + ": " + selectedPC.Equipment[type]?.Name ?? "Empty";
            //}
        }

        private void SelectPC(UserInterfaceEventHandler afterSelectPc)
        {
            throw new NotImplementedException();

            //mainWorkspace.ActivateWindow("SelectPC", WindowActivationBehaviors.None);

            //AfterSelectPC = afterSelectPc;
        }

        private void StartRelicMenu()
        {
            SelectPC(e =>
            {
                e.System.PushWorkspace(relicWorkspace);
            });
        }

        public void PartyUpdated()
        {
            Log.Warn("Not implemented yet.");

            //pcMenu.Layout.Clear();

            //foreach (var ch in Model.Party.Characters)
            //{
            //    pcMenu.Add(ch.Name, () => OnPCSelected(ch));
            //}
        }

        private void OnPCSelected(PlayerCharacter pc, UserInterfaceEvent e)
        {
            selectedPC = pc;
            //AfterSelectPC(e);
            throw new NotImplementedException();
        }

        private void RecordEvent(string v)
        {
        }
    }
}
