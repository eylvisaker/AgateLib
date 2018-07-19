using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class RadioMenu : Widget<RadioMenuProps>
    {
        public RadioMenu(RadioMenuProps props) : base(props)
        {
        }

        public override IRenderable Render() => new MenuElement(new MenuElementProps
        {
            Style = Props.Style,
            StyleId = Props.Name,
            StyleClass = Props.StyleClass,
            StyleTypeId = "radiomenu",
            OnCancel = Props.OnCancel,
            Children = Props.Buttons.ToList<IRenderable>()
        });
    }

    public class RadioMenuProps : WidgetProps
    {
        public List<RadioButton> Buttons { get; set; } = new List<RadioButton>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
