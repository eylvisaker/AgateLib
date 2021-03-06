﻿using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.TitleMenu
{
    public class TitleMenuApp : Widget<TitleMenuAppProps>
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
                    new Window(new WindowProps
                    {
                        OnCancel = Props.OnCancel,
                        Children =
                        {
                            new Button(new ButtonProps { Text = "Start", OnAccept = Props.OnStart }),
                            new Button(new ButtonProps { Text = "Load", OnAccept = Props.OnLoad }),
                            new Button(new ButtonProps { Text = "Quit", OnAccept = Props.OnQuit })
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
}
