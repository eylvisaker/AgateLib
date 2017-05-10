using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.InputTests
{
	[TestClass]
	public class StateTrackingTests : AgateUnitTest
	{
		[TestMethod]
		public void Input_KeyboardStateTracking()
		{
			bool gotKeyUp = false;

			var handlerA = new SimpleInputHandler();
			Input.Handlers.Add(handlerA);

			handlerA.KeyUp += (sender, args) =>
			{
				if (args.KeyCode == KeyCode.A)
				{
					gotKeyUp = true;
				}
			};

			// Send key down, and verify that the first handler processed it.
			Input.QueueInputEvent(AgateInputEventArgs.KeyDown(KeyCode.A, new KeyModifiers()));
			Input.DispatchQueuedEvents();

			Assert.IsTrue(handlerA.Keys[KeyCode.A]);

			// Add a new handler and verify that it does not get a key down event.
			var handlerB = new SimpleInputHandler();
			Input.Handlers.Add(handlerB);
			Assert.IsFalse(handlerB.Keys[KeyCode.A]);

			// Send key up, and verify that only the second handler processed it.
			Input.QueueInputEvent(AgateInputEventArgs.KeyUp(KeyCode.A, new KeyModifiers()));
			Input.DispatchQueuedEvents();

			Assert.IsTrue(handlerA.Keys[KeyCode.A]);
			Assert.IsFalse(handlerB.Keys[KeyCode.A]);
			Assert.IsFalse(gotKeyUp);

			// Remove the second handler and verify that the first one got a key up event.
			Input.Handlers.Remove(handlerB);
			Input.DispatchQueuedEvents();

			Assert.IsFalse(handlerA.Keys[KeyCode.A]);
			Assert.IsTrue(gotKeyUp);
		}

		[TestMethod]
		public void Input_JoystickButtonStateTracking()
		{
			bool gotReleased = false;

			var handlerA = new GamepadInputHandler();
			var gamepadA = handlerA.Gamepads.First();
			Input.Handlers.Add(handlerA);

			gamepadA.ButtonReleased += (sender, args) => 
			{
				if (args.Button == GamepadButton.A)
				{
					gotReleased = true;
				}
			};

			// Send key down, and verify that the first handler processed it.
			Input.QueueInputEvent(AgateInputEventArgs.JoystickButtonPressed(Input.Joysticks.First(), 0));
			Input.DispatchQueuedEvents();

			Assert.IsTrue(gamepadA.A);

			// Add a new handler and verify that it does not get a key down event.
			var handlerB = new GamepadInputHandler();
			var gamepadB = handlerB.Gamepads.First();
			Input.Handlers.Add(handlerB);
			Assert.IsFalse(gamepadB.A);

			// Send key up, and verify that only the second handler processed it.
			Input.QueueInputEvent(AgateInputEventArgs.JoystickButtonReleased(Input.Joysticks.First(), 0));
			Input.DispatchQueuedEvents();

			Assert.IsTrue(gamepadA.A);
			Assert.IsFalse(gamepadB.A);
			Assert.IsFalse(gotReleased);

			// Remove the second handler and verify that the first one got a key up event.
			Input.Handlers.Remove(handlerB);
			Input.DispatchQueuedEvents();

			Assert.IsFalse(gamepadA.A, "First input handler still thinks joystick button is depressed!");
			Assert.IsTrue(gotReleased);
		}

		[TestMethod]
		public void Input_JoystickHatStateTracking()
		{
			bool gotReleased = false;

			var handlerA = new GamepadInputHandler();
			var gamepadA = handlerA.Gamepads.First();
			Input.Handlers.Add(handlerA);

			gamepadA.DirectionPadChanged += (sender, args) =>
			{
				if (gamepadA.DirectionPad == Point.Zero)
				{
					gotReleased = true;
				}
			};

			// Send key down, and verify that the first handler processed it.
			Platform.Input.Joysticks[0].SetHatState(0, HatState.DownRight);
			Input.PollJoysticks();
			Input.DispatchQueuedEvents();

			Assert.AreEqual(new Point(1, 1), gamepadA.DirectionPad);

			// Add a new handler and verify that it does not get a key down event.
			var handlerB = new GamepadInputHandler();
			var gamepadB = handlerB.Gamepads.First();
			Input.Handlers.Add(handlerB);

			Assert.AreEqual(Point.Zero, gamepadB.DirectionPad);

			// Send key up, and verify that only the second handler processed it.
			Platform.Input.Joysticks[0].SetHatState(0, HatState.None);
			Input.PollJoysticks();
			Input.DispatchQueuedEvents();

			Assert.AreEqual(new Point(1, 1), gamepadA.DirectionPad);
			Assert.AreEqual(Point.Zero, gamepadB.DirectionPad);
			Assert.IsFalse(gotReleased);

			// Remove the second handler and verify that the first one got a key up event.
			Input.Handlers.Remove(handlerB);
			Input.PollJoysticks();
			Input.DispatchQueuedEvents();

			Assert.AreEqual(Point.Zero, gamepadA.DirectionPad,
				"First input handler still thinks joystick button is depressed!");

			Assert.IsTrue(gotReleased);
		}
	}
}
