using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.MultipleWorkspaces
{
    public class StatusWindow : Widget<StatusWindowProps>
    {
        public StatusWindow(StatusWindowProps props) : base(props)
        {
        }

        public override IRenderable Render() =>
            new Window(new WindowProps
            {
                Children =
                {
                    new Label(new LabelProps { Text = Props.Text })
                }
            });
    }

    public class StatusWindowProps : WidgetProps
    {
        public string Text { get; set; }
    }
}
