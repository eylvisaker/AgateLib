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
                        Cancel = Props.Cancel,
                        MenuItems =
                        {
                            new MenuItem(new MenuItemProps { Text = "Start", OnAccept = Props.Start }),
                            new MenuItem(new MenuItemProps { Text = "Load", OnAccept = Props.Load }),
                            new MenuItem(new MenuItemProps { Text = "Quit", OnAccept = Props.Quit })
                        }
                    })
                }
            });
        }
    }

    public class TitleMenuAppProps : WidgetProps
    {
        public Action Cancel { get; set; }
        public Action Start { get; set; }
        public Action Load { get; set; }
        public Action Quit { get; set; }
    }

    public class TitleMenuAppState : WidgetState
    {
    }
}
