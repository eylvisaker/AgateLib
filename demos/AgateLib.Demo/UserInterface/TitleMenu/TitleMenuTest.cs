using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.TitleMenu
{
    public class TitleMenuTest : UITest
    {
        public override string Name => "Example Title Menu";

        protected override IRenderable CreateUIRoot()
        {
            return new TitleMenuApp(new TitleMenuAppProps
            {
                OnCancel = e => ExitTest(),
            });
        }
    }
}
