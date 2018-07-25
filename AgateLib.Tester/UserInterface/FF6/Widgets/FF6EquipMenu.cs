using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6EquipMenu : Widget<FF6EquipMenuProps, FF6EquipMenuState>
    {
        private readonly ElementReference slotsMenuRef = new ElementReference();
        private readonly ElementReference actionMenuRef = new ElementReference();
        private readonly ElementReference itemsMenuRef = new ElementReference();

        private readonly UserInterfaceEvent<PlayerCharacter> charEvent = new UserInterfaceEvent<PlayerCharacter>();
        private readonly UserInterfaceEvent<PlayerCharacter, string> removeEvent = new UserInterfaceEvent<PlayerCharacter, string>();
        private readonly UserInterfaceEvent<PlayerCharacter, string, Item> equipEvent = new UserInterfaceEvent<PlayerCharacter, string, Item>();

        private Action<UserInterfaceEvent> slotsAction;

        public FF6EquipMenu(FF6EquipMenuProps props) : base(props)
        {
            SetState(new FF6EquipMenuState());
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Name = "EquipMenu",
                DefaultStyle = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Column,
                        AlignItems = AlignItems.Stretch,
                    },
                },
                Children =
                {
                    new Menu(new MenuProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                            }
                        },
                        Name = "equipActionType",
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps{ Text = "Equip", OnAccept = e => SelectSlotThen(e, EquipItem)}),
                            new MenuItem(new MenuItemProps{ Text = "Remove", OnAccept = e => SelectSlotThen(e, RemoveItem)}),
                            new MenuItem(new MenuItemProps{ Text = "Optimum", OnAccept = e => Props.OnEquipOptimum?.Invoke(charEvent.Reset(e, Props.PlayerCharacter))}),
                            new MenuItem(new MenuItemProps{ Text = "Empty", OnAccept = e => Props.OnEquipEmpty?.Invoke(charEvent.Reset(e, Props.PlayerCharacter))}),
                        },
                        Ref = actionMenuRef,
                        AllowNavigate = false,
                    }),
                    new Menu(new MenuProps
                    {
                        Name = "slots",
                        MenuItems = Props.EquipmentSlots.Select(eq =>
                            new MenuItem(new MenuItemProps
                            {
                                Name = eq.Name,
                                Text = $"{eq.Name}: {Props.PlayerCharacter.Equipment[eq.Name]?.Name}",
                                OnSelect = e => UpdateAvailableItems(e, eq.Name),
                                OnAccept = e => slotsAction(e),
                            })
                        ).ToList(),
                        Ref = slotsMenuRef,
                        AllowNavigate = false,
                    }),
                    new FlexBox(new FlexBoxProps
                    {
                        Name = "ItemArea",
                        AllowNavigate = false,
                        DefaultStyle = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                            }
                        },
                        Children =
                        {
                            new Menu(new MenuProps
                            {
                                Name = "AvailableItems",
                                MenuItems = State.AvailableItems.Select(item =>
                                    new MenuItem(new MenuItemProps
                                    {
                                        Text = item.Name,
                                        OnSelect = e =>
                                        {
                                            SetState(state => {
                                                state.SelectedItem = item;
                                            });
                                        },
                                        OnAccept = e =>
                                        {
                                            EquipItem(e, item);
                                        },
                                    }
                                )).ToList(),
                                Ref = itemsMenuRef,
                            }),
                            new Window(new WindowProps
                            {
                                Name = "ItemDescription",
                                Children =
                                {
                                    new Label(new LabelProps
                                    {
                                        Text = State.SelectedItem?.Name,
                                    }),
                                }
                            })
                        },
                    }),
                }
            });
        }

        private void EquipItem(UserInterfaceEvent e, Item item)
        {
            Props.OnEquip?.Invoke(equipEvent.Reset(e, Props.PlayerCharacter, State.SelectedSlot, item));
            e.System.SetFocus(slotsMenuRef);
        }

        private void EquipItem(UserInterfaceEvent e)
        {
            e.System.SetFocus(itemsMenuRef);

        }

        private void RemoveItem(UserInterfaceEvent e)
        {
            Props.OnEquipRemove?.Invoke(removeEvent.Reset(e, Props.PlayerCharacter, State.SelectedSlot));
        }

        private void UpdateAvailableItems(UserInterfaceEvent e, string slot)
        {
            SetState(state =>
            {
                state.AvailableItems.Clear();

                state.SelectedSlot = slot;
                state.AvailableItems.AddRange(Props.Inventory.Where(
                    it => Props.EquipmentSlots.First(s => s.Name == slot).AllowedItemTypes.Contains(it.ItemType, StringComparer.OrdinalIgnoreCase)));
            });
        }

        private void SelectSlotThen(UserInterfaceEvent e, Action<UserInterfaceEvent> followAction)
        {
            e.System.SetFocus(slotsMenuRef);

            slotsAction = followAction;
        }
    }

    public class FF6EquipMenuState : WidgetState
    {
        public List<Item> AvailableItems { get; set; } = new List<Item>();

        public Item SelectedItem { get; set; }
        public string SelectedSlot { get; set; }
    }

    public class FF6EquipMenuProps : WidgetProps
    {
        public PlayerCharacter PlayerCharacter { get; set; }
        public List<Item> Inventory { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string, Item> OnEquip { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string> OnEquipRemove { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipOptimum { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipEmpty { get; set; }
        public IEnumerable<EquipmentSlot> EquipmentSlots { get; set; }
    }
}
