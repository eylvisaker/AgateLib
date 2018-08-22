using AgateLib.UserInterface;
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
                    new Window(new WindowProps
                    {
                        Name = "ArrangeItems",
                        Style = new InlineElementStyle
                        {
                            Flex = new AgateLib.UserInterface.Styling.FlexStyle
                            {
                                Direction =  FlexDirection.Row
                            }
                        },
                        Children =
                        {
                            new Button(new ButtonProps{
                                Text = "items",
                                OnAccept = e => e.System.SetFocus(itemsRef.Current)
                            }),
                            new Button(new ButtonProps{
                                Text = "Arrange",
                                OnAccept = Props.OnArrangeItems
                            }),
                            new Button(new ButtonProps{ Text = "Rare" })
                        },
                        Ref = arrangeItemsRef,
                        OnCancel = Props.OnCancel
                    }),
                    new Window(new WindowProps
                    {
                        AllowNavigate = false,
                        Name = "Items",
                        OnCancel = e => e.System.SetFocus(arrangeItemsRef.Current),
                        Children = State.Inventory.Select(item =>
                            new Button(new ButtonProps
                            {
                                Text = item.Name,
                                OnAccept = e => SelectItem(e, item)
                            })).ToList<IRenderable>(),
                        Ref = itemsRef
                    })
                },
                InitialFocusIndex = 1,
            });
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