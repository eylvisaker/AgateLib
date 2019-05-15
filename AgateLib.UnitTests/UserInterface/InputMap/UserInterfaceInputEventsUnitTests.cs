using AgateLib.Input;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AgateLib.UserInterface.InputMap
{
    public class UserInterfaceInputEventsUnitTests
    {
        private int buttonDownTimes = 0;
        private List<UserInterfaceAction> pressedButtons = new List<UserInterfaceAction>();
        private UserInterfaceInputEvents input;
        private Mock<IInputState> inputState;

        public UserInterfaceInputEventsUnitTests()
        {
            input = new UserInterfaceInputEvents();
            inputState = new Mock<IInputState>();

            input.ButtonDown += e =>
            {
                pressedButtons.Add(e.ActionButton);
                buttonDownTimes++;
            };

            input.ButtonUp += e => pressedButtons.Remove(e.ActionButton);
        }

        [Fact]
        public void ButtonDownIsTriggered()
        {
            SetupGamePad(buttons: Buttons.A);

            input.UpdateState(inputState.Object);
            input.TriggerEvents(new GameTime());

            pressedButtons.Should().Contain(UserInterfaceAction.Accept);
        }

        [Fact]
        public void ButtonDownIsTriggeredOnlyOnce()
        {
            SetupGamePad(buttons: Buttons.A);

            input.UpdateState(inputState.Object);
            input.TriggerEvents(new GameTime());

            input.UpdateState(inputState.Object);
            input.TriggerEvents(new GameTime());

            input.UpdateState(inputState.Object);
            input.TriggerEvents(new GameTime());

            buttonDownTimes.Should().Be(1);
        }

        private void SetupGamePad(Vector2? leftThumbStick = null,
                                  Vector2? rightThumbStick = null,
                                  float leftTrigger = 0,
                                  float rightTrigger = 0,
                                  Buttons buttons = 0)
        {
            inputState.Setup(x => x.GamePadStateOf(PlayerIndex.One))
                .Returns(CreateGamePadState(leftThumbStick, rightThumbStick, leftTrigger, rightTrigger, buttons));
        }

        private static GamePadState CreateGamePadState(Vector2? leftThumbStick = null,
                                                       Vector2? rightThumbStick = null,
                                                       float leftTrigger = 0,
                                                       float rightTrigger = 0,
                                                       Buttons buttons = 0)
        {
            return new GamePadState(leftThumbStick ?? Vector2.Zero,
                                    rightThumbStick ?? Vector2.Zero,
                                    leftTrigger,
                                    rightTrigger,
                                    buttons);
        }
    }
}
