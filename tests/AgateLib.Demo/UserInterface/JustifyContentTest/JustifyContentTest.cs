using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.JustifyContentTest
{
    public class JustifyContentTester : UITest
    {
        public override string Name => "Justify Content";

        protected override IRenderable CreateUIRoot()
            => new JustifyContentApp(new JustifyContentAppProps
            {
                OnCancel = e => ExitTest(),
            });
    }
}
