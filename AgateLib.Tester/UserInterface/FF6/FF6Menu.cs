using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;

namespace ManualTests.AgateLib.UserInterface.FF6
{
    public class FF6Menu
    {
        private Menu pcMenu;
        private PlayerCharacter selectedPC;
        private Action AfterSelectPC;
        private Menu itemsList;
        private Menu magicList;
        private Menu esperList;
        private Action<PlayerCharacter> AfterItemTarget;
        private Menu itemTarget;
        private Workspace mainWorkspace;
        private Workspace itemsWorkspace;
        private Workspace skillsWorkspace;
        private Workspace magicWorkspace;
        private Workspace espersWorkspace;
        private Workspace relicWorkspace;
        private Workspace equipWorkspace;
        private Desktop desktop;
        private readonly Action<string> log;

        public FF6Menu(Action<string> log)
        {
            this.log = log;

            InitializeComponent();
        }

        /// <summary>
        /// Begins the menu on the passed desktop.
        /// </summary>
        /// <param name="desktop"></param>
        public void Begin(Desktop desktop)
        {
            this.desktop = desktop;

            PartyUpdated();
            desktop.PushWorkspace(mainWorkspace);
        }

        public FF6Model Model { get; set; } = new FF6Model();

        private void InitializeComponent()
        {
            InitializeMainMenu(mainWorkspace = new Workspace("default"));
            InitializeItemsMenu(itemsWorkspace = new Workspace("items"));
            InitializeSkillsMenu(skillsWorkspace = new Workspace("skills"));
            InitializeMagicMenu(magicWorkspace = new Workspace("magic"));
            InitializeEspersMenu(espersWorkspace = new Workspace("espers"));
            InitializeEquipMenu(equipWorkspace = new Workspace("equip"));
            InitializeRelicMenu(relicWorkspace = new Workspace("relic"));
        }

        private void InitializeRelicMenu(Workspace workspace)
        {
            var relicMenu = new Menu("Relic");
            var slotsMenu = new Menu("Slots");
            var itemsMenu = new Menu("Items");

            Action<string> slotsFollowUp = null;

            void EquipMenuFor(string location)
            {
                workspace.ActivateWindow("Items");

                itemsMenu.Layout.Clear();

                foreach (var item in Model.Inventory)
                {
                    itemsMenu.Add(item.Name, () => Model.EquipPC(selectedPC, item, location));
                }
            }

            relicMenu.Add("Equip", () =>
            {
                slotsFollowUp = EquipMenuFor;
                workspace.ActivateWindow(slotsMenu);
            });

            relicMenu.Add("Remove", () =>
            {
                slotsFollowUp = location =>
                {
                    Model.EquipPC(selectedPC, null, location);
                };
                workspace.ActivateWindow(slotsMenu);
            });

            slotsMenu.Add("Relic 1", "Relic", () => slotsFollowUp("Relic 1"));
            slotsMenu.Add("Relic 2", "Relic", () => slotsFollowUp("Relic 2"));

            var layout = new FixedGridLayout(2, 6);

            layout.Add(relicMenu, new Rectangle(0, 0, 2, 1));
            layout.Add(slotsMenu, new Rectangle(0, 1, 2, 2));
            layout.Add(itemsMenu, new Rectangle(0, 3, 1, 3));

            workspace.Layout = layout;
        }

        private void InitializeEquipMenu(Workspace workspace)
        {
            var equipMenu = new Menu("Equip") { LayoutType = LayoutType.SingleRow };
            var slotsMenu = new Menu("Slots");
            var itemsMenu = new Menu("Items");

            Action<string> slotsFollowUp = null;

            void EquipMenuFor(string location)
            {
                workspace.ActivateWindow("Items");

                itemsMenu.Layout.Clear();

                foreach (var item in Model.ItemsForSlot(selectedPC, location))
                {
                    itemsMenu.Add(item.Name, () =>
                    {
                        Model.EquipPC(selectedPC, item, location);
                        workspace.ActivateWindow("Slots");

                        UpdateEquipmentSlots();

                        itemsMenu.Layout.Clear();
                    });
                }
            }

            equipMenu.Add("Equip", () =>
            {
                slotsFollowUp = EquipMenuFor;
                workspace.ActivateWindow("Slots");

                UpdateEquipmentSlots();
            });

            equipMenu.Add("Optimum", () =>
            {
                Model.EquipPC(selectedPC, BestItem("L-Hand"), "L-Hand");
                Model.EquipPC(selectedPC, BestItem("R-Hand"), "R-Hand");
                Model.EquipPC(selectedPC, BestItem("Head"), "Head");
                Model.EquipPC(selectedPC, BestItem("Body"), "Body");

                UpdateEquipmentSlots();
            });

            equipMenu.Add("Remove", () =>
            {
                slotsFollowUp = location =>
                {
                    Model.EquipPC(selectedPC, null, location);

                    UpdateEquipmentSlots();
                };

                workspace.ActivateWindow("Slots");
            });

            equipMenu.Add("Empty", () =>
            {
                Model.EquipPC(selectedPC, null, "L-Hand");
                Model.EquipPC(selectedPC, null, "R-Hand");
                Model.EquipPC(selectedPC, null, "Head");
                Model.EquipPC(selectedPC, null, "Body");

                UpdateEquipmentSlots();
            });

            equipMenu.Exit += () => desktop.PopWorkspace();

            slotsMenu.Add("L-Hand", () => slotsFollowUp("L-Hand"));
            slotsMenu.Add("R-Hand", () => slotsFollowUp("R-Hand"));
            slotsMenu.Add("Head", () => slotsFollowUp("Head"));
            slotsMenu.Add("Body", () => slotsFollowUp("Body"));

            slotsMenu.Exit += () => workspace.ActivateWindow(equipMenu);

            itemsMenu.Exit += () =>
            {
                itemsMenu.Layout.Clear();
                workspace.ActivateWindow(slotsMenu);
            };

            var layout = new FixedGridLayout(2, 6);

            layout.Add(equipMenu, new Rectangle(0, 0, 2, 1));
            layout.Add(slotsMenu, new Rectangle(0, 1, 2, 2));
            layout.Add(itemsMenu, new Rectangle(0, 3, 1, 3));

            workspace.Layout = layout;
        }

        private Item BestItem(string slotName)
        {
            var slot = Model.EquipmentSlots.First(x => x.Name.Equals(slotName, StringComparison.OrdinalIgnoreCase));

            var items = Model.Inventory.Where(x => slot.AllowedItemTypes.Contains(x.ItemType, StringComparer.OrdinalIgnoreCase));

            return items.FirstOrDefault();
        }

        private void InitializeEspersMenu(Workspace workspace)
        {
            esperList = new Menu("Espers");

            workspace.Add(esperList);
        }

        private void InitializeMagicMenu(Workspace workspace)
        {
            magicList = new Menu("Magic");

            magicList.Exit += () => desktop.PopWorkspace();

            workspace.Add(magicList);
        }

        private void InitializeMainMenu(Workspace workspace)
        {
            var menu = new Menu("Main");

            menu.Add("Items", StartItemsMenu);
            menu.Add("Skills", StartSkillsMenu);
            menu.Add("Equip", StartEquipMenu);
            menu.Add("Relic", StartRelicMenu);
            menu.Add("Status", () => RecordEvent("Status"));
            menu.Add("Config", () => RecordEvent("Config"));
            menu.Add("Save", () => RecordEvent("Save"));

            pcMenu = new Menu("SelectPC");

            pcMenu.Exit += () => workspace.ActivateWindow(menu);

            var layout = new FixedGridLayout(8, 8);
            workspace.Layout = layout;

            layout.Add(pcMenu, new Rectangle(0, 0, 8, 8));
            layout.Add(menu, new Rectangle(7, 0, 1, 5));
        }

        private void InitializeItemsMenu(Workspace workspace)
        {
            var arrangeWindow = new Menu("ArrangeItems");

            arrangeWindow.LayoutType = LayoutType.SingleRow;
            arrangeWindow.Add("Arrange", () => ArrangeItems());
            arrangeWindow.Add("Rare", () => RareItems());

            itemsList = new Menu("Items");
            itemsList.Cancel += cancel =>
            {
                workspace.ActivateWindow(arrangeWindow);
                cancel.Cancel = true;
            };

            arrangeWindow.Exit += () => desktop.PopWorkspace();

            itemTarget = new Menu("ItemTarget");
            itemTarget.Display.IsVisible = false;

            var layout = new FixedGridLayout(1, 8);

            layout.Add(arrangeWindow, new Rectangle(0, 0, 1, 1));
            layout.Add(itemsList, new Rectangle(0, 1, 1, 7));
            layout.Add(itemTarget, new Rectangle(0, 3, 1, 1));

            workspace.Layout = layout;
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

        private void RareItems()
        {
            throw new NotImplementedException();
        }

        private void InitializeSkillsMenu(Workspace workspace)
        {
            var layout = new FixedGridLayout(1, 1);
            workspace.Layout = layout;

            var menu = new Menu("SkillType");

            menu.Add("Magic", StartMagicMenu);
            menu.Add("Espers", StartEspersMenu);
            menu.Add("Blitz", () => RecordEvent("Blitz"));
            menu.Add("SwdTech", () => RecordEvent("SwdTech"));
            menu.Add("Blue", () => RecordEvent("Blue"));
            menu.Add("Rage", () => RecordEvent("Rage"));

            menu.Exit += ReturnToDesktop;

            layout.Add(menu, Point.Zero);
        }

        private void ReturnToDesktop()
        {
            desktop.PopWorkspace();
            desktop.ActivateWindowInWorkspace("default", "main");
        }

        private void StartEspersMenu()
        {
            UpdateEspers();

            desktop.PushWorkspace(espersWorkspace, "Espers");
        }

        private void StartMagicMenu()
        {
            UpdateMagic();

            desktop.PushWorkspace(magicWorkspace, "Magic");
        }

        private void StartItemsMenu()
        {
            UpdateItems();

            desktop.PushWorkspace(itemsWorkspace, "Items");
        }

        private void UpdateEspers()
        {
        }

        private void UpdateMagic()
        {
            magicList.Layout.Clear();

            foreach (var item in selectedPC.Magic)
            {
                magicList.Add(item.Name, () => RecordEvent("Used magic " + item.Name));
            }
        }


        private void UpdateItems()
        {
            itemsList.Layout.Clear();
            Item selectedItem = null;

            void ItemAccept(Item item)
            {
                if (selectedItem == null)
                {
                    selectedItem = item;
                }
                else
                {
                    if (selectedItem != item)
                    {
                        var spot1 = Model.Inventory.IndexOf(selectedItem);
                        var spot2 = Model.Inventory.IndexOf(item);

                        Model.Inventory[spot1] = item;
                        Model.Inventory[spot2] = selectedItem;
                    }
                    else
                    {
                        UseItem(item);
                    }

                    selectedItem = null;
                }
            }

            foreach (var item in Model.Inventory)
            {
                itemsList.Add(item.Name, () => ItemAccept(item));
            }
        }

        private void UseItem(Item item)
        {
            switch (item.Effect)
            {
                case "heal":
                    SelectItemTarget(targetPc =>
                    {
                        targetPc.HP += item.EffectAmount;
                        targetPc.HP = Math.Min(targetPc.HP, targetPc.MaxHP);
                    });

                    break;
            }
        }

        private void SelectItemTarget(Action<PlayerCharacter> afterSelection)
        {
            itemTarget.Layout.Clear();

            foreach (var pc in Model.Party.Characters)
            {
                itemTarget.Add(pc.Name, () =>
                {
                    itemTarget.Display.IsVisible = false;
                    AfterItemTarget(pc);
                });
            }

            desktop.ActiveWorkspace.ActivateWindow("ItemTarget");
            AfterItemTarget = afterSelection;
        }

        private void StartSkillsMenu()
        {
            RecordEvent("Skills");

            SelectPC(() =>
            {
                desktop.PushWorkspace(skillsWorkspace, "SkillType");
            });
        }

        private void StartEquipMenu()
        {
            RecordEvent("Equip");

            SelectPC(() =>
            {
                desktop.PushWorkspace(equipWorkspace, "Equip");
                UpdateEquipmentSlots();
            });
        }

        private void UpdateEquipmentSlots()
        {
            var slots = equipWorkspace.FindWindow<Menu>(x => x.Name == "Slots");

            foreach (var item in slots.Layout.OfType<ContentMenuItem>())
            {
                var type = item.Name;

                item.Text = type + ": " + selectedPC.Equipment[type]?.Name ?? "Empty";
            }
        }

        private void SelectPC(Action afterSelectPc)
        {
            mainWorkspace.ActivateWindow("SelectPC", WindowActivationBehaviors.None);

            AfterSelectPC = afterSelectPc;
        }

        private void StartRelicMenu()
        {
            SelectPC(() =>
            {
                desktop.PushWorkspace(relicWorkspace, "Relic");
            });
        }

        public void PartyUpdated()
        {
            pcMenu.Layout.Clear();

            foreach (var ch in Model.Party.Characters)
            {
                pcMenu.Add(ch.Name, () => OnPCSelected(ch));
            }
        }

        private void OnPCSelected(PlayerCharacter pc)
        {
            selectedPC = pc;
            AfterSelectPC();
        }

        private void RecordEvent(string v)
        {
        }
    }
}
