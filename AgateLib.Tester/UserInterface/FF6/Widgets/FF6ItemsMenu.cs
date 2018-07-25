using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6ItemsMenu : Widget<FF6ItemsMenuProps, FF6ItemsMenuState>
    {
        UserInterfaceEvent<Item> itemEvent = new UserInterfaceEvent<Item>();
        UserInterfaceEvent<Tuple<int, int>> swapItemsEvent = new UserInterfaceEvent<Tuple<int, int>>();

        public FF6ItemsMenu(FF6ItemsMenuProps props) : base(props)
        {
            SetState(new FF6ItemsMenuState { Inventory = Props.Inventory.ToList() });
        }

        public override IRenderable Render()
        {
            var arrangeItemsRef = new ElementReference();
            var itemsRef = new ElementReference();

            return new FlexBox(new FlexBoxProps
            {
                Children =
                {
                    new Menu(new MenuProps
                    {
                        Name = "ArrangeItems",
                        Style = new InlineElementStyle
                        {
                            Flex = new AgateLib.UserInterface.Styling.FlexStyle
                            {
                                Direction =  FlexDirection.Row
                            }
                        },
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps{
                                Text = "items",
                                OnAccept = e => e.System.SetFocus(itemsRef.Current)
                            }),
                            new MenuItem(new MenuItemProps{
                                Text = "Arrange",
                                OnAccept = Props.OnArrangeItems
                            }),
                            new MenuItem(new MenuItemProps{ Text = "Rare" })
                        },
                        Ref = arrangeItemsRef,
                        OnCancel = Props.OnCancel
                    }),
                    new Menu(new MenuProps
                    {
                        AllowNavigate = false,
                        Name = "Items",
                        OnCancel = e => e.System.SetFocus(arrangeItemsRef.Current),
                        MenuItems = State.Inventory.Select(item =>
                            new MenuItem(new MenuItemProps
                            {
                                Text = item.Name,
                                OnAccept = e => SelectItem(e, item)
                            })).ToList(),
                        Ref = itemsRef
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
                SetState(state => state.SelectedItem = null);
            }
            else if (State.SelectedItem == null)
            {
                SetState(state => state.SelectedItem = item);
            }
            else
            {
                var first = State.Inventory.IndexOf(State.SelectedItem);
                var second = State.Inventory.IndexOf(item);

                Props.OnSwapItems(swapItemsEvent.Reset(e, new Tuple<int, int>(first, second)));
                SetState(state => state.SelectedItem = null);
            }
        }
    }

    public class FF6ItemsMenuProps : WidgetProps
    {
        public List<Item> Inventory { get; set; } = new List<Item>();

        public Action<UserInterfaceEvent<Item>> OnUseItem { get; set; }

        public Action<UserInterfaceEvent<Tuple<int, int>>> OnSwapItems { get; set; }

        public UserInterfaceEventHandler OnCancel { get; set; }

        public UserInterfaceEventHandler OnArrangeItems { get; set; }
    }

    public class FF6ItemsMenuState : WidgetState
    {
        public Item SelectedItem { get; set; }

        public List<Item> Inventory { get; set; }
    }
}