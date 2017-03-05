using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.InputTests
{
	[TestClass]
	public class GamepadInputHandlerTests
	{
		[TestMethod]
		public void KeyChange()
		{
			using (new AgateUnitTestPlatform()
				.Initialize())
			{
				GamepadInputHandler handler = new GamepadInputHandler();
				var gamepad = handler.Gamepads.First();

				handler.KeyMap[KeyCode.Up] = KeyMapItem.ToLeftStick(gamepad, Axis.Y, -1);
				handler.KeyMap[KeyCode.Down] = KeyMapItem.ToLeftStick(gamepad, Axis.Y, 1);
				handler.KeyMap[KeyCode.Left] = KeyMapItem.ToLeftStick(gamepad, Axis.X, -1);
				handler.KeyMap[KeyCode.Right] = KeyMapItem.ToLeftStick(gamepad, Axis.X, 1);

				handler.ProcessEvent(AgateInputEventArgs.KeyDown(KeyCode.Down, new KeyModifiers()));
				handler.ProcessEvent(AgateInputEventArgs.KeyDown(KeyCode.Right, new KeyModifiers()));
				handler.ProcessEvent(AgateInputEventArgs.KeyUp(KeyCode.Down, new KeyModifiers()));

				Assert.AreEqual(new Vector2(1, 0), gamepad.LeftStick);
			}
		}
	}
}