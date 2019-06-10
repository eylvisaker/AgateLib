using AgateLib.UserInterface;
using System.Linq;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
    public static class SimpleMessageBoxExtensions
    {
        public static Task<string> ShowMessageBox(this IDisplaySystem displaySystem, SimpleMessageBoxProps props)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            displaySystem.PushWorkspace(new Workspace("dialog-box", new App(new AppProps
            {
                Theme = props.Theme,
                Children =
                {
                    new Window(new WindowProps
                    {
                        Children =
                        {
                            new Label(new LabelProps { Text = props.Text }),
                            new Separator(),
                            new FlexBox(new FlexBoxProps
                            {
                                DefaultStyle =
                                {
                                    Flex =
                                    {
                                        Direction = FlexDirection.Row,
                                        JustifyContent = JustifyContent.End,
                                    },
                                },
                                InitialFocusIndex = props.DefaultIndex,
                                Children = props.Options.Select(option => new Button(new ButtonProps
                                {
                                    Text = option,
                                    OnAccept = e => 
                                    {
                                        e.System.PopWorkspace();
                                        taskCompletionSource.SetResult(option);
                                    },
                                })).ToList<IRenderable>(),
                            })
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
            })));

            return taskCompletionSource.Task;
        }
    }

    public class SimpleMessageBoxProps
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
        /// A list of string options.
        /// </summary>
        public string[] Options { get; set; }

        /// <summary>
        /// The theme to use for the workspace.
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// The index of the option which is selected by default.
        /// </summary>
        public int DefaultIndex { get; set; }
    }
}
