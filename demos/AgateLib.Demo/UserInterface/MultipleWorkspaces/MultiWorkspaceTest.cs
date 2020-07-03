using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.MultipleWorkspaces
{
    public class MultiWorkspaceTest : UIDemo
    {
        public override string Name => "Multiple Workspaces";

        protected override IRenderable CreateUIRoot()
            => new MultiWorkspaceApp(new MultiWorkspaceAppProps());
    }
}
