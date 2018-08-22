using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6MainMenu : Widget<FF6MainMenuProps, FF6MainMenuState>
    {
        private UserInterfaceEventHandler<PlayerCharacter> afterSelectPC;
        private ElementReference mainRef = new ElementReference();
        private ElementReference selectPcRef = new ElementReference();

        public FF6MainMenu(FF6MainMenuProps props) : base(props)
        {
            SetState(new FF6MainMenuState
            {
                Characters = props.Model.Party.Characters,
                Inventory = props.Model.Inventory,
            });
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Style = new InlineElementStyle
                {
                    Flex = new FlexStyle
                    {
                        Direction = FlexDirection.Row,
                    }
                },
                Name = Props.Name,
                InitialFocusIndex = 1,
                Children =
                {
                    new PartyStatusWindow(new PartyStatusWindowProps
                    {
                        Characters = State.Characters.ToList(),
                        Name = "partystatus",
                        Enabled = false,
                        Ref = selectPcRef,
                        OnSelectPC = e => {
                            afterSelectPC(e);
                            e.System.SetFocus(mainRef.Current);
                        },
                        OnCancel = e => e.System.SetFocus(mainRef.Current),
                    }),
                    new Window(new WindowProps
                    {
                        AllowNavigate = false,
                        Name = "main",
                        OnCancel = Props.OnExit,
                        Ref = mainRef,
                        Children =
                        {
                            new Button(new ButtonProps { Text = "Items", OnAccept = RunItemsMenu  }),
                            new Button(new ButtonProps { Text = "Skills", OnAccept = e => SelectPCThen(e, RunSkillsMenu) }),
                            new Button(new ButtonProps { Text = "Equip",  OnAccept = e => SelectPCThen(e, RunEquipMenu) }),
                            new Button(new ButtonProps { Text = "Relic",  OnAccept = e => SelectPCThen(e, RunRelicMenu) }),
                            new Button(new ButtonProps { Text = "Status", OnAccept = RunStatusMenu }),
                            new Button(new ButtonProps { Text = "Config", OnAccept = RunConfigMenu }),
                            new Button(new ButtonProps { Text = "Save",   OnAccept = RunSaveMenu   }),
                        },
                    }),
                },
            });
        }

        private void SelectPCThen(UserInterfaceEvent e, Action<UserInterfaceEvent, PlayerCharacter> thenDo)
        {
            e.System.SetFocus(selectPcRef.Current);

            afterSelectPC = y => thenDo(y, y.Data);
        }

        private void RunEquipMenu(UserInterfaceEvent evt, PlayerCharacter pc)
        {
            evt.System.PushWorkspace(new Workspace("equip", new FF6EquipMenu(new FF6EquipMenuProps
            {
                PlayerCharacter = pc,
                Inventory = Props.Model.Inventory,
                EquipmentSlots = Props.Model.EquipmentSlots.Where(x => !x.Name.Contains("Relic")),
                OnEquip = Props.OnEquip,
                OnEquipRemove = Props.OnEquipRemove,
                OnEquipOptimum = Props.OnEquipOptimum,
                OnEquipEmpty = Props.OnEquipEmpty,
                OnCancel = e => e.System.PopWorkspace(),
            })));
        }

        private void RunRelicMenu(UserInterfaceEvent evt, PlayerCharacter pc)
        {
            evt.System.PushWorkspace(new Workspace("relic", new FF6RelicMenu(new FF6RelicMenuProps
            {
                PlayerCharacter = pc,
                Inventory = Props.Model.Inventory,
                EquipmentSlots = Props.Model.EquipmentSlots.Where(x => x.Name.Contains("Relic")),
                OnEquip = Props.OnEquip,
                OnEquipRemove = Props.OnEquipRemove,
                OnCancel = e => e.System.PopWorkspace(),
            })));
        }

        private void RunStatusMenu(UserInterfaceEvent e)
        {
        }

        private void RunConfigMenu(UserInterfaceEvent e)
        {
        }

        private void RunSaveMenu(UserInterfaceEvent e)
        {
        }

        private void RunItemsMenu(UserInterfaceEvent evt)
        {
            evt.System.PushWorkspace(new Workspace("items", new FF6ItemsMenu(new FF6ItemsMenuProps
            {
                Inventory = Props.Model.Inventory,
                OnUseItem = UseItem,
                OnSwapItems = e =>
                {
                    Props.OnSwapItems(e);
                    SetState(state => state.Inventory = Props.Model.Inventory);
                },
                OnCancel = e => e.System.PopWorkspace(),
                OnArrangeItems = e =>
                {
                    Props.OnArrangeItems(e);
                    SetState(state =>
                    {
                        state.Inventory = Props.Model.Inventory;
                    });
                },
            })));
        }

        private void UseItem(UserInterfaceEvent<Item> e)
        {
            switch (e.Data.Effect)
            {
                case "heal":
                    SelectItemTarget(e, targetPc
                        => Props.OnUseItem?.Invoke(new UserInterfaceEvent<Tuple<Item, PlayerCharacter>>().Reset(e,
                        new Tuple<Item, PlayerCharacter>(e.Data, targetPc))));

                    break;
            }
        }

        private void SelectItemTarget(UserInterfaceEvent<Item> evt, Action<PlayerCharacter> afterSelection)
        {
            Workspace workspace = null;

            workspace = new Workspace("itemTarget", new FF6ItemTarget(new FF6ItemTargetProps
            {
                Characters = Props.Model.Party.Characters.ToList(),
                OnAccept = e =>
                {
                    workspace.TransitionOut();
                    afterSelection(e.Data);
                }
            }));

            evt.System.PushWorkspace(workspace);
        }

        private void RunSkillsMenu(UserInterfaceEvent evt, PlayerCharacter pc)
        {
            evt.System.PushWorkspace(new Workspace("skills", new FF6SkillsMenu(new FF6SkillsMenuProps
            {
                OnCancel = e => e.System.PopWorkspace(),
                OnMagic = e => e.System.PushWorkspace(RunMagicMenu(pc)),
                OnEspers = e => e.System.PushWorkspace(RunEspersMenu(pc)),
            })));
        }

        private Workspace RunEspersMenu(PlayerCharacter pc)
        {
            return new Workspace("espers", new FF6EspersMenu(new FF6EspersMenuProps
            {
                OnCancel = e => e.System.PopWorkspace()
            }));
        }

        private Workspace RunMagicMenu(PlayerCharacter pc)
        {
            return new Workspace("magic", new FF6MagicMenu(new FF6MagicMenuProps
            {
                OnCancel = e => e.System.PopWorkspace()
            }));
        }
    }

    public class FF6MainMenuProps : WidgetProps
    {
        public FF6Model Model { get; set; }
        public UserInterfaceEventHandler OnArrangeItems { get; set; }
        public UserInterfaceEventHandler<Tuple<Item, PlayerCharacter>> OnUseItem { get; set; }
        public UserInterfaceEventHandler<Tuple<int, int>> OnSwapItems { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string, Item> OnEquip { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter, string> OnEquipRemove { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipOptimum { get; set; }
        public UserInterfaceEventHandler<PlayerCharacter> OnEquipEmpty { get; set; }
        public UserInterfaceEventHandler OnExit { get; set; }
    }

    public class FF6MainMenuState : WidgetState
    {
        public IEnumerable<PlayerCharacter> Characters { get; set; }
        public List<Item> Inventory { get; set; }
    }
}
