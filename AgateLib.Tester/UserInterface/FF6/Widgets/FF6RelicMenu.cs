using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6RelicMenu : Widget<FF6RelicMenuProps, FF6RelicMenuState>
    {
        private readonly ElementReference slotsMenuRef = new ElementReference();
        private readonly ElementReference actionMenuRef = new ElementReference();
        private readonly ElementReference itemsMenuRef = new ElementReference();

        private readonly UserInterfaceEvent<PlayerCharacter> charEvent = new UserInterfaceEvent<PlayerCharacter>();
        private readonly UserInterfaceEvent<PlayerCharacter, string> removeEvent = new UserInterfaceEvent<PlayerCharacter, string>();
        private readonly UserInterfaceEvent<PlayerCharacter, string, Item> equipEvent = new UserInterfaceEvent<PlayerCharacter, string, Item>();

        private Action<UserInterfaceEvent> slotsAction;

        public FF6RelicMenu(FF6RelicMenuProps props) : base(props)
        {
            SetState(new FF6RelicMenuState());
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Name = "RelicMenu",
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
                    new Window(new WindowProps
                    {
                        Style = new InlineElementStyle
                        {
                            Flex = new FlexStyle
                            {
                                Direction = FlexDirection.Row,
                            }
                        },
                        OnCancel = Props.OnCancel,
                        Name = "equipActionType",
                        Children  =
                        {
                            new Button(new ButtonProps{ Text = "Equip", OnAccept = e => SelectSlotThen(e, EquipRelic)}),
                            new Button(new ButtonProps{ Text = "Remove", OnAccept = e => SelectSlotThen(e, RemoveRelic)}),
                        },
                        Ref = actionMenuRef,
                        AllowNavigate = false,
                    }),
                    new Window(new WindowProps
                    {
                        Name = "slots",
                        Children = Props.EquipmentSlots.Select(eq =>
                            new Button(new ButtonProps
                            {
                                Name = eq.Name,
                                Text = $"{eq.Name}: {Props.PlayerCharacter.Equipment[eq.Name]?.Name}",
                                OnFocus = e => UpdateAvailableItems(e, eq.Name),
                                OnAccept = e => slotsAction(e),
                            })
                        ).ToList<IRenderable>(),
                        OnCancel = e => e.System.SetFocus(actionMenuRef),
                        Ref = slotsMenuRef,
                        AllowNavigate = false,
                    }),
                    new FlexBox(new FlexBoxProps
                    {
                        Name = "ItemArea",
                        AllowNavigate = false,
                        Children =
                        {
                            new Window(new WindowProps
                            {
                                Name = "AvailableItems",
                                OnCancel = e => e.System.SetFocus(slotsMenuRef),
                                Children = State.AvailableItems.Select(item =>
                                    new Button(new ButtonProps
                                    {
                                        Text = item.Name,
                                        OnFocus = e =>
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
                                )).ToList<IRenderable>(),
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

        private void EquipRelic(UserInterfaceEvent e)
        {
            e.System.SetFocus(itemsMenuRef);

        }

        private void RemoveRelic(UserInterfaceEvent e)
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

    public class FF6RelicMenuState : WidgetState
    {
        public List<Item> AvailableItems { get; set; } = new List<Item>();

        public Item SelectedItem { get; set; }
        public string SelectedSlot { get; set; }
    }

    public class FF6RelicMenuProps : WidgetProps
    {
        public PlayerCharacter PlayerCharacter { get; set; }
        public List<Item> Inventory { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string, Item> OnEquip { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string> OnEquipRemove { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipOptimum { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipEmpty { get; set; }
        public UserInterfaceEventHandler OnCancel { get; set; }

        public IEnumerable<EquipmentSlot> EquipmentSlots { get; set; }
    }
}
