using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.Scrolling
{
    public class ScrollingTest : UITest
    {
        public override string Name => "Scrolling";

        protected override IRenderable CreateUIRoot()
        {
            return new ScrollingApp(new ScrollingAppProps
            {
                OnCancel = e => ExitTest(),
            });
        }
    }
}
