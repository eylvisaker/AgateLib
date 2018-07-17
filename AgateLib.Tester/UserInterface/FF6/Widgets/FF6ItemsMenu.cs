using AgateLib.UserInterface.Widgets;
using System;
using System.Linq;

namespace AgateLib.Tests.UserInterface.FF6
{
    public class FF6ItemsMenu : Widget<FF6ItemsMenuProps, FF6ItemsMenuState>
    {
        public FF6ItemsMenu(FF6ItemsMenuProps props) : base(props)
        {
            SetState(new FF6ItemsMenuState());
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
                        MenuItems = Props.Model.Inventory.Select(item =>
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

        ItemEvent itemEvent = new ItemEvent();

        private void SelectItem(UserInterfaceEvent e, Item item)
        {
            itemEvent.Reset(e, item);

            if (State.SelectedItem == item)
            {
                Props.OnUseItem?.Invoke(itemEvent);
                UpdateState(state => state.SelectedItem = null);
            }
            else
            {
                UpdateState(state => state.SelectedItem = item);
            }
        }
    }

    public class FF6ItemsMenuProps : WidgetProps
    {
        public FF6Model Model { get; set; }

        public Action<ItemEvent> OnUseItem { get; set; }
    }

    public class FF6ItemsMenuState : WidgetState
    {
        public Item SelectedItem { get; set; }
    }

    public class ItemEvent : UserInterfaceEvent<Item> { }
}