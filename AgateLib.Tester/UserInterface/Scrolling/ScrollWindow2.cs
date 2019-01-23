using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.Scrolling
{
    public class ScrollWindow2 : Widget<ScrollWindow2Props>
    {
        public ScrollWindow2(ScrollWindow2Props props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            List<IRenderable> buttons = new List<IRenderable>();

            for (int i = 0; i < 50; i++)
                buttons.Add(new Button(new ButtonProps { Text = $"Item {i + 1}" }));

            return new Window(new WindowProps
            {
                OnCancel = Props.OnCancel,
                Children =
                {
                    new Label(new LabelProps { Text = "There should be 50 items in the list below."}),
                    new FlexBox(new FlexBoxProps
                    {
                        Children = buttons
                    }),
                },
            });
        }
    }

    public class ScrollWindow2Props : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
