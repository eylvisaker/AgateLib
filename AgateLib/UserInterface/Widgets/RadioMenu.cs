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
            AllowNavigate = Props.AllowNavigate,
            Style = Props.Style,
            Name = Props.Name,
            StyleClass = Props.StyleClass,
            StyleTypeId = "radiomenu",
            OnCancel = Props.OnCancel,
            Children = Props.Buttons.ToList<IRenderable>()
        });
    }

    public class RadioMenuProps : WidgetProps
    {
        public bool AllowNavigate { get; set; } = true;

        public List<RadioButton> Buttons { get; set; } = new List<RadioButton>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
