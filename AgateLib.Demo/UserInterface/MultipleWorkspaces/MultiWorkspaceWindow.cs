using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.MultipleWorkspaces
{
    public class MultiWorkspaceWindow : Widget<MultiWorkspaceWindowProps>
    {
        public MultiWorkspaceWindow(MultiWorkspaceWindowProps props) : base(props)
        {

        }

        public override IRenderable Render()
        {
            return new Window(new WindowProps
            {
                Children =
                {
                    new Button(new ButtonProps
                    {
                        Text = "Start Workspace A",
                        OnFocus = e => Props.Status(e, "This will open the first workspace."),
                        OnAccept = e =>
                        {
                            e.System.PushWorkspace(new FirstWorkspaceApp(new FirstWorkspaceAppProps
                            {
                                Status = Props.Status
                            }));
                        },
                    }),

                    new Button(new ButtonProps
                    {
                        Text = "Start Workspace B",
                        OnFocus = e => Props.Status(e, "This will open the second workspace."),
                        OnAccept = e =>
                        {
                            e.System.PushWorkspace(new FirstWorkspaceApp(new FirstWorkspaceAppProps
                            {
                                Status = Props.Status
                            }));
                        },
                    }),
                },
                OnCancel = e => e.System.PopWorkspace(),
            });
        }
    }

    public class MultiWorkspaceWindowProps : WidgetProps
    {
        public UserInterfaceEventHandler2<string> Status { get; set; }
    }
}
