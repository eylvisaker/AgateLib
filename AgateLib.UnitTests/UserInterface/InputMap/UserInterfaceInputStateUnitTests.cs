using Microsoft.Xna.Framework.Input;
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
