using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.JustifyContentTest
{
    public class JustifyContentTester : UIDemo
    {
        public override string Name => "Justify Content";

        protected override IRenderable CreateUIRoot()
            => new JustifyContentApp(new JustifyContentAppProps
            {
                OnCancel = e => ExitTest(),
            });
    }
}
