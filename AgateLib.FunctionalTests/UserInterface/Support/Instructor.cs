using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.FunctionalTests.UserInterface.Support
{
    public class Instructor
    {
        private readonly UIContext context;

        public Instructor(UIContext context)
        {
            this.context = context;
        }

        public void SendButtonPress(MenuInputButton btn)
        {
            context.Desktop.ButtonDown(btn);
            context.Desktop.ButtonUp(btn);

            context.WaitForAnimations();
        }
    }
}
