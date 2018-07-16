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
        public RenderElementEventHandler OnCancel { get; set; }
        public RenderElementEventHandler Start { get; set; }
        public RenderElementEventHandler Load { get; set; }
        public RenderElementEventHandler Quit { get; set; }
    }

    public class TitleMenuAppState : WidgetState
    {
    }
}
