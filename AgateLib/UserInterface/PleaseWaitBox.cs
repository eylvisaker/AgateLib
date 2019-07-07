using AgateLib.UserInterface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
    public static class PleaseWaitBoxExtensions
    {
        public static PleaseWaitBox ShowPleaseWaitBox(this IDisplaySystem displaySystem, PleaseWaitBoxProps props)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var workspace = new Workspace("wait-box", new App(new AppProps
            {
                Theme = props.Theme,
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Label(new LabelProps { Text = props.Text }),
                        },
                        OnCancel = e =>
                        {
                            if (props.AllowCancel)
                            {
                                e.System.PopWorkspace();
                                taskCompletionSource.SetResult("");
                            }
                        },
                    })
                }
            }));

            displaySystem.PushWorkspace(workspace);

            return new PleaseWaitBox(workspace);
        }
    }

    public class PleaseWaitBox : IDisposable
    {
        private Workspace workspace;

        public PleaseWaitBox(Workspace workspace)
        {
            this.workspace = workspace;
        }

        public void Dispose()
        {
            workspace.TransitionOut();
        }
    }

    public class PleaseWaitBoxProps
    {
        /// <summary>
        /// If true, allows the user to use the cancel action to close the dialog box.
        /// </summary>
        public bool AllowCancel { get; set; }

        /// <summary>
        /// The message prompt to display
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The theme to use for the workspace.
        /// </summary>
        public string Theme { get; set; }
    }
}
