using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.DoubleRadioMenus
{
    public class DoubleRadioMenusTest : UITest
    {
        public override string Name => "Double Radio Buttons";

        protected override IRenderable CreateUIRoot()
        {
            return new DoubleRadioMenusApp(new DoubleRadioMenusProps
            {
                LeftItems = new[]
                {
                    new ItemData { Name = "Foo", Description = "Food is good for you." },
                    new ItemData { Name = "Bar", Description = "Bars serve drinks." },
                    new ItemData { Name = "Gra", Description = "Gra is a nonsense word." },
                    new ItemData { Name = "Hoh", Description = "Hoh is one letter short." },
                },
                RightItems = new[]
                {
                    new ItemData { Name = "MegaFoo", Description = "MegaFood is good for you." },
                    new ItemData { Name = "MegaBar", Description = "MegaBars serve mega drinks." },
                    new ItemData { Name = "MegaGra", Description = "MegaGra is a nonsense word." },
                    new ItemData { Name = "MegaHoh", Description = "MegaHoh is nothing." },
                },
                OnCancel = e => ExitTest(),
            });
        }
    }
}
