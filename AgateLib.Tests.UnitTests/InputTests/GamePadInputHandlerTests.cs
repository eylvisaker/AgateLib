using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test;
using AgateLib.Platform.Test.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.InputTests
{
	[TestClass]
	public class GamepadInputHandlerTests : AgateUnitTest
	{
		private GamepadInputHandler handler;
		private IGamepad gamepad;
		private IJoystick joystick;
		private FakeJoystickImpl joystickImpl;
		
		[TestInitialize]
		public void Initialize()
		{
			joystick = Input.Joysticks.First();
			joystickImpl = Platform.Input.Joysticks.First();

			handler = new GamepadInputHandler();
			gamepad = handler.Gamepads.First();
		}

		[TestMethod]
		public void Gamepad_JoystickToLeftStick()
		{
			JoystickAxis(0, 0.5);
			JoystickAxis(1, 0.25);

			Assert.IsTrue(new Vector2(0.5, 0.25).Equals(gamepad.LeftStick, 1e-6));
		}

		[TestMethod]
		public void Gamepad_JoystickToRightStick()
		{
			JoystickAxis(2, 0.5);
			JoystickAxis(3, 0.25);

			Assert.IsTrue(new Vector2(0.5, 0.25).Equals(gamepad.RightStick, 1e-6), $"Expected (0.5, 0.25), actual {gamepad.RightStick}");
		}

		[TestMethod]
		public void Gamepad_JoystickToDirectionPad()
		{
			JoystickHat(0, HatState.UpLeft);
			Assert.AreEqual(new Point(-1, -1), gamepad.DirectionPad, "Up-left should be (-1, -1)");

			JoystickHat(0, HatState.DownRight);
			Assert.AreEqual(new Point(1, 1), gamepad.DirectionPad, "Down-right should be (1, 1)");
		}

		[TestMethod]
		public void Gamepad_JoystickToButton()
		{
			JoystickButtonDown(0);
			Assert.IsTrue(gamepad.A);

			JoystickButtonDown(1);
			Assert.IsTrue(gamepad.B);
			
			JoystickButtonUp(0);
			Assert.IsFalse(gamepad.A);
			Assert.IsTrue(gamepad.B);
		}

		[TestMethod]
		public void Gamepad_KeyToRightStick()
		{
			KeyDown(KeyCode.K);
			KeyDown(KeyCode.L);
			KeyUp(KeyCode.K);

			Assert.AreEqual(new Vector2(1, 0), gamepad.RightStick);
		}

		[TestMethod]
		public void Gamepad_KeyToTriggers()
		{
			KeyDown(KeyCode.Q);
			KeyDown(KeyCode.E);

			Assert.AreEqual(1, gamepad.LeftTrigger);
			Assert.AreEqual(1, gamepad.RightTrigger);
		}

		[TestMethod]
		public void Gamepad_KeyChange()
		{
			handler.KeyMap[KeyCode.Up] = KeyMapItem.ToLeftStick(gamepad, Axis.Y, -1);
			handler.KeyMap[KeyCode.Down] = KeyMapItem.ToLeftStick(gamepad, Axis.Y, 1);
			handler.KeyMap[KeyCode.Left] = KeyMapItem.ToLeftStick(gamepad, Axis.X, -1);
			handler.KeyMap[KeyCode.Right] = KeyMapItem.ToLeftStick(gamepad, Axis.X, 1);

			KeyDown(KeyCode.Down);
			KeyDown(KeyCode.Right);
			KeyUp(KeyCode.Down);

			Assert.AreEqual(new Vector2(1, 0), gamepad.LeftStick);
		}

		[TestMethod]
		public void Gamepad_KeyboardToButton()
		{
			KeyDown(KeyCode.Space);
			Assert.IsTrue(gamepad.A);
			
			KeyUp(KeyCode.Space);
			Assert.IsFalse(gamepad.A);

			KeyDown(KeyCode.AltLeft);
			Assert.IsTrue(gamepad.B);

			KeyDown(KeyCode.ControlLeft);
			Assert.IsTrue(gamepad.X);

			KeyDown(KeyCode.ShiftLeft);
			Assert.IsTrue(gamepad.Y);

			KeyDown(KeyCode.F);
			Assert.IsTrue(gamepad.LeftStickButton);

			KeyDown(KeyCode.Semicolon);
			Assert.IsTrue(gamepad.RightStickButton);

			KeyDown(KeyCode.Enter);
			Assert.IsTrue(gamepad.Start);

			KeyDown(KeyCode.BackSpace);
			Assert.IsTrue(gamepad.Back);
		}

		[TestMethod]
		public void Gamepad_KeyToJoystick_LeftRightReleaseRight()
		{
			KeyDown(KeyCode.A);
			Assert.AreEqual(new Vector2(-1, 0), gamepad.LeftStick);

			KeyDown(KeyCode.D);
			Assert.AreEqual(new Vector2(1, 0), gamepad.LeftStick);

			KeyUp(KeyCode.D);
			Assert.AreEqual(new Vector2(-1, 0), gamepad.LeftStick);
		}

		[TestMethod]
		public void Gamepad_KeyToJoystick_UpDownReleaseDown()
		{
			KeyDown(KeyCode.W);
			Assert.AreEqual(new Vector2(0, -1), gamepad.LeftStick);

			KeyDown(KeyCode.S);
			Assert.AreEqual(new Vector2(0, 1), gamepad.LeftStick);

			KeyUp(KeyCode.S);
			Assert.AreEqual(new Vector2(0, -1), gamepad.LeftStick);
		}

		[TestMethod]
		public void Gamepad_KeyToDpad_LeftRightReleaseLeft()
		{
			KeyDown(KeyCode.Left);
			Assert.AreEqual(new Point(-1, 0), gamepad.DirectionPad);

			KeyDown(KeyCode.Right);
			Assert.AreEqual(new Point(1, 0), gamepad.DirectionPad);

			KeyUp(KeyCode.Left);
			Assert.AreEqual(new Point(1, 0), gamepad.DirectionPad);
		}

		[TestMethod]
		public void Gamepad_KeyToDpad_LeftRightReleaseRight()
		{
			KeyDown(KeyCode.Left);
			Assert.AreEqual(new Point(-1, 0), gamepad.DirectionPad);

			KeyDown(KeyCode.Right);
			Assert.AreEqual(new Point(1, 0), gamepad.DirectionPad);

			KeyUp(KeyCode.Right);
			Assert.AreEqual(new Point(-1, 0), gamepad.DirectionPad);
		}

		[TestMethod]
		public void Gamepad_KeyToDpad_UpDownReleaseDown()
		{
			KeyDown(KeyCode.Up);
			Assert.AreEqual(new Point(0, -1), gamepad.DirectionPad);

			KeyDown(KeyCode.Down);
			Assert.AreEqual(new Point(0, 1), gamepad.DirectionPad);

			KeyUp(KeyCode.Down);
			Assert.AreEqual(new Point(0, -1), gamepad.DirectionPad);
		}

		[TestMethod]
		public void Gamepad_KeymapDictionaryContract()
		{
			KeyCode key = KeyCode.A;

			CollectionUnitTest.DictionaryContract(new KeyboardGamepadMap(), () => key++, () => new KeyMapItem());
		}

		private void JoystickAxis(int axis, double value)
		{
			joystickImpl.SetAxisValue(axis, value);
			Input.PollJoysticks();
			handler.ProcessEvent(AgateInputEventArgs.JoystickAxisChanged(joystick, axis));
		}

		private void JoystickButtonDown(int buttonIndex)
		{
			joystickImpl.SetButtonState(buttonIndex, true);
			Input.PollJoysticks();
			handler.ProcessEvent(AgateInputEventArgs.JoystickButtonPressed(joystick, buttonIndex));
		}

		private void JoystickButtonUp(int buttonIndex)
		{
			joystickImpl.SetButtonState(buttonIndex, false);
			Input.PollJoysticks();
			handler.ProcessEvent(AgateInputEventArgs.JoystickButtonReleased(joystick, buttonIndex));
		}

		private void JoystickHat(int hatIndex, HatState value)
		{
			joystickImpl.SetHatState(hatIndex, value);
			Input.PollJoysticks();
			handler.ProcessEvent(AgateInputEventArgs.JoystickHatStateChanged(joystick, hatIndex));
		}

		private void KeyDown(KeyCode key)
		{
			handler.ProcessEvent(AgateInputEventArgs.KeyDown(key, new KeyModifiers()));
		}

		private void KeyUp(KeyCode key)
		{
			handler.ProcessEvent(AgateInputEventArgs.KeyUp(key, new KeyModifiers()));
		}
	}
}