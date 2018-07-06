using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.RadioButtons
{
    public class RadioButtonApp : Widget<RadioButtonAppProps>
    {
        public RadioButtonApp(RadioButtonAppProps props) : base(props)
        {
        }

        public override IRenderable Render() => new RadioMenu(new RadioMenuProps
        {
            Style = new InlineElementStyle
            {
                Flex = new AgateLib.UserInterface.Styling.FlexStyle
                {
                    Direction = FlexDirection.Row
                }
            },
            Buttons = Props.Items.Select(x => new RadioButton(new RadioButtonProps
            {
                OnAccept = () => Props.OnValueSet?.Invoke(x),
                OnSelect = () => Props.OnSelectionSet?.Invoke(x),
                Text = x,
            })).ToList(),
            Cancel = Props.Cancel
        });
    }

    public class RadioButtonAppProps : WidgetProps
    {
        public Action<string> OnValueSet { get; set; }

        public Action<string> OnSelectionSet { get; set; }

        public List<string> Items { get; set; } = new List<string>();

        public Action Cancel { get; set; }
    }
}
