using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.TitleMenu
{
    public class TitleMenuApp : Widget<TitleMenuAppProps, TitleMenuAppState>
    {
        public TitleMenuApp(TitleMenuAppProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new App(new AppProps
            {
                Children =
                {
                    new Menu(new MenuProps
                    {
                        OnCancel = Props.OnCancel,
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Start", OnAccept = Props.OnStart }),
                            new MenuItem(new MenuItemProps { Text = "Load", OnAccept = Props.OnLoad }),
                            new MenuItem(new MenuItemProps { Text = "Quit", OnAccept = Props.OnQuit })
                        }
                    })
                }
            });
        }
    }

    public class TitleMenuAppProps : WidgetProps
    {
        public UserInterfaceEventHandler OnCancel { get; set; }
        public UserInterfaceEventHandler OnStart { get; set; }
        public UserInterfaceEventHandler OnLoad { get; set; }
        public UserInterfaceEventHandler OnQuit { get; set; }
    }

    public class TitleMenuAppState : WidgetState
    {
    }
}
