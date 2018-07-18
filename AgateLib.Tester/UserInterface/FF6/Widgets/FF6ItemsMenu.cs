using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6ItemsMenu : Widget<FF6ItemsMenuProps, FF6ItemsMenuState>
    {
        UserInterfaceEvent<Item> itemEvent = new UserInterfaceEvent<Item>();
        UserInterfaceEvent<IReadOnlyList<Item>> inventoryUpdateEvent = new UserInterfaceEvent<IReadOnlyList<Item>>();

        public FF6ItemsMenu(FF6ItemsMenuProps props) : base(props)
        {
            SetState(new FF6ItemsMenuState { Inventory = Props.Inventory.ToList() });
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Children =
                {
                    new Menu(new MenuProps
                    {
                        Name="ArrangeItems",
                        Style = new InlineElementStyle
                        {
                            Flex = new AgateLib.UserInterface.Styling.FlexStyle
                            {
                                Direction =  FlexDirection.Row
                            }
                        },
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps{ Text = "Arrange" }),
                            new MenuItem(new MenuItemProps{ Text = "Rare" })
                        }
                    }),
                    new Menu(new MenuProps
                    {
                        Name = "Items",
                        OnCancel = null, // Active the arrange items window,
                        MenuItems = State.Inventory.Select(item =>
                            new MenuItem(new MenuItemProps
                            {
                                Text = item.Name,
                                OnAccept = e => SelectItem(e, item)
                            })).ToList()
                    })
                },
                InitialFocusIndex = 1,
            });

            //var arrangeWindow = new Menu("ArrangeItems");

            //arrangeWindow.LayoutType = LayoutType.SingleRow;
            //arrangeWindow.Add("Arrange", () => ArrangeItems());
            //arrangeWindow.Add("Rare", () => RareItems());

            //itemsList = new Menu("Items");
            //itemsList.Cancel += cancel =>
            //{
            //    workspace.ActivateWindow(arrangeWindow);
            //    cancel.Cancel = true;
            //};

            //arrangeWindow.Exit += () => desktop.PopWorkspace();

            //itemTarget = new Menu("ItemTarget");
            //itemTarget.Display.IsVisible = false;

            //var layout = new FixedGridLayout(1, 8);

            //layout.Add(arrangeWindow, new Rectangle(0, 0, 1, 1));
            //layout.Add(itemsList, new Rectangle(0, 1, 1, 7));
            //layout.Add(itemTarget, new Rectangle(0, 3, 1, 1));

            //workspace.Layout = layout;
        }

        private void SelectItem(UserInterfaceEvent e, Item item)
        {
            if (State.SelectedItem == item)
            {
                Props.OnUseItem?.Invoke(itemEvent.Reset(e, item));
                UpdateState(state => state.SelectedItem = null);
            }
            else if (State.SelectedItem == null)
            {
                UpdateState(state => state.SelectedItem = item);
            }
            else
            {
                UpdateState(state =>
                {
                    var first = state.Inventory.IndexOf(state.SelectedItem);
                    var second = state.Inventory.IndexOf(item);

                    state.Inventory[first] = item;
                    state.Inventory[second] = state.SelectedItem;

                    Props.OnInventoryUpdated?.Invoke(inventoryUpdateEvent.Reset(e, state.Inventory));

                    state.SelectedItem = null;
                });
            }
        }
    }

    public class FF6ItemsMenuProps : WidgetProps
    {
        public List<Item> Inventory { get; set; } = new List<Item>();

        public Action<UserInterfaceEvent<Item>> OnUseItem { get; set; }

        public Action<UserInterfaceEvent<IReadOnlyList<Item>>> OnInventoryUpdated { get; set; }
    }

    public class FF6ItemsMenuState : WidgetState
    {
        public Item SelectedItem { get; set; }

        public List<Item> Inventory { get; set; }
    }
}