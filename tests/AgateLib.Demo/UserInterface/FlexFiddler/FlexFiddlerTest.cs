using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.FlexFiddler
{
    public class FlexFiddlerTest : UITest
    {
        public override string Name => "Flex Fiddler";

        protected override IRenderable CreateUIRoot()
        {
            Scene.Theme = "FF";

            return new FlexFiddlerApp(new FlexFiddlerAppProps
            {
                OnCancel = e => ExitTest(),
            });
        }
    }
}
