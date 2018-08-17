using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Tests.UserInterface.Support
{
    public class Instructor
    {
        private readonly UIContext context;

        public Instructor(UIContext context)
        {
            this.context = context;
        }

        public void SendButtonPress(UserInterfaceAction btn)
        {
            context.Desktop.OnUserInterfaceAction(new UserInterfaceActionEventArgs(btn));

            context.WaitForAnimations();
        }
    }
}
