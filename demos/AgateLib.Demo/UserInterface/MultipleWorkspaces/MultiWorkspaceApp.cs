﻿using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.MultipleWorkspaces
{
    public class MultiWorkspaceApp : Widget<MultiWorkspaceAppProps, MultiWorkspaceAppState>
    {
        public MultiWorkspaceApp(MultiWorkspaceAppProps props) : base(props)
        {
            SetState(new MultiWorkspaceAppState());
        }

        public override IRenderable Render()
        {
            return new App(new AppProps
            {
                Children =
                {
                    new MultiWorkspaceWindow(new MultiWorkspaceWindowProps
                    {
                        Status = (e, text) => SetState(state => state.Text = text),
                    }),
                    new StatusWindow(new StatusWindowProps
                    {
                        Text = State.Text,
                        StatusLabelRef = Props.StatusLabelRef,
                    }),
                }
            });
        }
    }

    public class MultiWorkspaceAppState
    {
        public string Text { get; set; }
    }

    public class MultiWorkspaceAppProps : WidgetProps
    {
        public ElementReference StatusLabelRef { get; set; }
    }
}
