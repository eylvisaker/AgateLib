using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.FF6.Widgets
{
    public class FF6MagicMenu : Widget<FF6MagicMenuProps>
    {
        public FF6MagicMenu(FF6MagicMenuProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new Window(new WindowProps
            {
                Name = "Magic",
                OnCancel = Props.OnCancel,
                Children = {
                    new Button(new ButtonProps { Text = "Does nothing" })
                }
            });
        }
    }

    public class FF6MagicMenuProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
