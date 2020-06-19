using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6SkillsMenu : Widget<FF6SkillsMenuProps>
    {
        public FF6SkillsMenu(FF6SkillsMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new FlexBox(new FlexBoxProps
            {
                Children =
                {
                    new Window(new WindowProps
                    {
                        Name = "skillType",
                        Children =
                        {
                            new Button(new ButtonProps{ Text = "Magic", OnAccept = Props.OnMagic }),
                            new Button(new ButtonProps{ Text = "Espers",  OnAccept = Props.OnEspers }),
                        },
                        OnCancel = Props.OnCancel
                    }),
                    new Window(new WindowProps
                    {
                        Name = "skillType2",
                        Children =
                        {
                            new Button(new ButtonProps{ Text = "Blitz",   OnAccept = Props.OnBlitz}),
                            new Button(new ButtonProps{ Text = "SwdTech", OnAccept = Props.OnSwdTech}),
                            new Button(new ButtonProps{ Text = "Blue",    OnAccept = Props.OnBlue}),
                            new Button(new ButtonProps{ Text = "Rage",    OnAccept = Props.OnRage}),
                        },
                        OnCancel = Props.OnCancel
                    })
                }
            });
        }
    }

    public class FF6SkillsMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
        public UserInterfaceEventHandler OnMagic { get; set; }
        public UserInterfaceEventHandler OnEspers { get; set; }
        public UserInterfaceEventHandler OnBlitz { get; set; }
        public UserInterfaceEventHandler OnSwdTech { get; set; }
        public UserInterfaceEventHandler OnBlue { get; set; }
        public UserInterfaceEventHandler OnRage { get; set; }
    }
}
