using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.UserInterface.InputMap
{
    public class UserInterfaceInputStateUnitTests
    {
        [Fact]
        public void PressingActionButtonIsRecorded()
        {
            var input = new UserInterfaceInputState();

            input.UpdateGamePadButtonState(Buttons.A, UserInterfaceAction.Accept, true);

            input.PressedActions.Contains(UserInterfaceAction.Accept);
        }
    }
}
