using AgateLib.UserInterface;
using System.Collections.Generic;

namespace AgateLib.Tests.UserInterface.Scrolling
{
    public class ScrollingApp : Widget<ScrollingAppProps, ScrollingAppState>
    {
        public ScrollingApp(ScrollingAppProps props) : base(props)
        {
            SetState(new ScrollingAppState());
        }

        public override IRenderable Render()
        {
            List<IRenderable> buttons = new List<IRenderable>();

            for (int i = 0; i < 50; i++)
            {
                buttons.Add(new Button(new ButtonProps { Text = $"Item {i + 1}" }));
            }

            return new App(new AppProps
            {
                Children =
                {
                    new ScrollWindow1(new ScrollWindow1Props { OnCancel = Props.OnCancel }),
                    new ScrollWindow2(new ScrollWindow2Props { OnCancel = Props.OnCancel }),
                }
            });
        }
    }

    public class ScrollingAppProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
    }

    public class ScrollingAppState
    {
        public TextAlign TextAlign { get; set; }
    }
}
