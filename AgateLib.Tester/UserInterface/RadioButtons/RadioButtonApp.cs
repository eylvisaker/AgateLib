using AgateLib.UserInterface;
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

        public override IRenderable Render() => new App(new AppProps
        {
            Children =
            {
                new RadioMenu(new RadioMenuProps
                {
                    Style = new InlineElementStyle
                    {
                        Flex = new AgateLib.UserInterface.FlexStyle
                        {
                            Direction = FlexDirection.Row
                        }
                    },
                    Buttons = Props.Items.Select(x => new RadioButton(new RadioButtonProps
                    {
                        OnAccept = e => Props.OnValueSet?.Invoke(x),
                        OnSelect = e => Props.OnSelectionSet?.Invoke(x),
                        Text = x,
                    })).ToList(),
                    OnCancel = Props.OnCancel
                })
            }
        });
    }

    public class RadioButtonAppProps : WidgetProps
    {
        public Action<string> OnValueSet { get; set; }

        public Action<string> OnSelectionSet { get; set; }

        public List<string> Items { get; set; } = new List<string>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
