using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.MultipleWorkspaces
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
                        Text = State.Text
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
    }
}
