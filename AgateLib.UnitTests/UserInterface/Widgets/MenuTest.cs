using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace AgateLib.Tests.UserInterface.Widgets
{
    public class MenuTest
    {
        [Fact]
        public void MenuInputCancel()
        {
            bool canceled = false;

            var menu = new Window(new WindowProps
            {
                OnCancel = e => canceled = true,
                Children = { new Button(new MenuItemProps { Text = "Hello" }) }
            });

            TestUIDriver driver = new TestUIDriver(menu);

            driver.Press(Buttons.B);

            canceled.Should().BeTrue("Menu did not exit when cancel was pressed.");
        }

        [Fact]
        public void SkipDisabledMenuItems()
        {
            var menu = new Window(new WindowProps
            {
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Button(new MenuItemProps { Text = "Hello" }),
                            new Button(new MenuItemProps { Text = "Some" }),
                            new Button(new MenuItemProps { Text = "Items", Enabled = false, }),
                            new Button(new MenuItemProps { Text = "Are" }),
                            new Button(new MenuItemProps { Text = "Disabled", Enabled = false }),
                        }
                    }),
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Button(new MenuItemProps { Text = "Testing if we" }),
                            new Button(new MenuItemProps { Text = "Can navigate out" }),
                            new Button(new MenuItemProps { Text = "of a menu who's last" }),
                            new Button(new MenuItemProps { Text = "item is disabled." }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(menu);

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Some");

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Are");

            driver.Press(Buttons.DPadDown);
            FocusItemHasText(driver, "Testing if we");
        }

        #region --- DisabledMenuItemsWontTriggerOnAccept ---

        private class MenuEnableToggleComponent : Widget<MenuEnableToggleComponentProps, MenuEnableToggleState>
        {
            public MenuEnableToggleComponent(MenuEnableToggleComponentProps props) : base(props)
            {
                SetState(new MenuEnableToggleState());
            }

            public override IRenderable Render()
            {
                return new Window(new WindowProps
                {
                    Children =
                    {
                        new Button(new MenuItemProps
                        {
                            Text = "DisableMe",
                            Enabled = State.MenuItemEnabled,
                            OnAccept = e =>
                            {
                                SetState(state =>
                                {
                                    state.MenuItemEnabled = false;
                                    Props.OnMenuItemDisabled?.Invoke(e);
                                });
                            },
                        }),
                        new Button(new MenuItemProps { Text = "I don't do anything."})
                    }
                });
            }
        }

        private class MenuEnableToggleComponentProps : WidgetProps
        {
            public UserInterfaceEventHandler OnMenuItemDisabled { get; set; }
        }

        private class MenuEnableToggleState
        {
            public bool MenuItemEnabled { get; internal set; } = true;
        }

        [Fact]
        public void DisabledMenuItemsWontTriggerOnAccept()
        {
            int disabledCount = 0;

            var menu = new Window(new WindowProps
            {
                Children =
                {
                    new MenuEnableToggleComponent(new MenuEnableToggleComponentProps
                    {
                        OnMenuItemDisabled = e =>
                        {
                            if (disabledCount > 0)
                                throw new InvalidOperationException("OnAccept triggered for disabled menu item.");

                            disabledCount++;
                        },
                    }),
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Button(new MenuItemProps { Text = "Testing if we" }),
                            new Button(new MenuItemProps { Text = "Can navigate out" }),
                            new Button(new MenuItemProps { Text = "of a menu who's last" }),
                            new Button(new MenuItemProps { Text = "item is disabled." }),
                        }
                    })
                }
            });

            TestUIDriver driver = new TestUIDriver(menu);

            driver.Press(Buttons.A);
            driver.Press(Buttons.A);

            disabledCount.Should().Be(1, "menu item's onaccept should only be called once.");
        }

        #endregion

        private static void FocusItemHasText(TestUIDriver driver, string text)
        {
            driver.Focus.Props.Should().BeOfType(typeof(MenuItemElementProps));
            (driver.Focus.Props as MenuItemElementProps).Text.Should().Be(text);
        }
    }

}
