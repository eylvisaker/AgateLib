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

				handler.KeyMap.Add(KeyCode.Up, KeyMapItem.ToLeftStick(gamepad, StickAxis.Y, -1));
				handler.KeyMap.Add(KeyCode.Down, KeyMapItem.ToLeftStick(gamepad, StickAxis.Y, 1));
				handler.KeyMap.Add(KeyCode.Left, KeyMapItem.ToLeftStick(gamepad, StickAxis.X, -1));
				handler.KeyMap.Add(KeyCode.Right, KeyMapItem.ToLeftStick(gamepad, StickAxis.X, 1));

				handler.ProcessEvent(AgateInputEventArgs.KeyDown(KeyCode.Down, new KeyModifiers()));
				handler.ProcessEvent(AgateInputEventArgs.KeyDown(KeyCode.Right, new KeyModifiers()));
				handler.ProcessEvent(AgateInputEventArgs.KeyUp(KeyCode.Down, new KeyModifiers()));

				Assert.AreEqual(new Vector2(1, 0), gamepad.LeftStick);
			}
		}
	}
}