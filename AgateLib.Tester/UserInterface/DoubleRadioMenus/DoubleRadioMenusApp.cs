using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Tests.UserInterface.DoubleRadioMenus
{
    public class DoubleRadioMenusApp : Widget<DoubleRadioMenusProps, DoubleRadioMenusState>
    {
        private ItemData selectedLeft, selectedRight;

        public DoubleRadioMenusApp(DoubleRadioMenusProps props) : base(props)
        {
            SetState(new DoubleRadioMenusState());
        }

        public ItemData SelectedLeft => selectedLeft;
        public ItemData SelectedRight => selectedRight;

        public override IRenderable Render() => new App(new AppProps
        {
            Children =
            {
                new FlexBox(new FlexBoxProps
                {
                    Children =
                        {
                            new Label(new LabelProps { Text = "Choose"}),
                            new FlexBox(new FlexBoxProps
                            {
                                Style = new InlineElementStyle { Flex = new FlexStyle { Direction = FlexDirection.Row } },
                                Children =
                                {
                                    new RadioMenu(new RadioMenuProps
                                    {
                                        Buttons = CreateLeftRadioButtons().ToList(),
                                        OnCancel = Props.OnCancel,
                                    }),
                                    new RadioMenu(new RadioMenuProps
                                    {
                                        Buttons = CreateRightRadioButtons().ToList(),
                                        OnCancel = Props.OnCancel,
                                    }),
                                }
                            }),
                            new Menu(new MenuProps{
                                OnCancel = Props.OnCancel,
                                MenuItems = {
                                    new MenuItem(new MenuItemProps {
                                        Text = "Accept",
                                        Enabled = State.AcceptEnabled,
                                        OnAccept = e => Props.OnAccept?.Invoke(selectedLeft, selectedRight)
                                    })
                                }
                            }),
                            new Label(new LabelProps {
                                Name = "description",
                                Text = State.DescriptionText
                            })
                        }
                })
            }
        });

        private void UpdateFlags()
        {
            if (selectedLeft == null || selectedRight == null)
                return;

            SetState(state => state.AcceptEnabled = true);
        }

        void SetLeft(ItemData item)
        {
            selectedLeft = item;
            UpdateFlags();
        }

        void SetRight(ItemData item)
        {
            selectedRight = item;
            UpdateFlags();
        }

        private IEnumerable<RadioButton> CreateLeftRadioButtons()
        {
            foreach (var item in Props.LeftItems)
            {
                var result = new RadioButton(new RadioButtonProps
                {
                    Text = item.Name,
                    OnSelect = () => SetState(state => state.DescriptionText = item.Description),
                    OnAccept = () => SetLeft(item),
                });

                yield return result;
            }
        }

        private IEnumerable<RadioButton> CreateRightRadioButtons()
        {
            foreach (var item in Props.RightItems)
            {
                var result = new RadioButton(new RadioButtonProps
                {
                    Text = item.Name,
                    OnSelect = () => SetState(state => state.DescriptionText = item.Description),
                    OnAccept = () => SetRight(item),
                });

                yield return result;
            }
        }
    }

    public class ItemData
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DoubleRadioMenusProps : WidgetProps
    {
        public IEnumerable<ItemData> LeftItems { get; set; }
        public IEnumerable<ItemData> RightItems { get; set; }
        public Action<ItemData, ItemData> OnAccept { get; set; }
        public UserInterfaceEventHandler OnCancel { get; set; }
    }

    public class DoubleRadioMenusState : WidgetState
    {
        public string DescriptionText { get; set; }
        public bool AcceptEnabled { get; set; }
    }
}
