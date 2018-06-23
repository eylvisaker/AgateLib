using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;
using Examples.Configuration.ConsoleExample;

namespace Examples.Scenes.EventDrivenSceneExample
{
	class EventDrivenSceneProgram
	{
		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args)
				.Initialize())
			using (new DisplayWindowBuilder(args)
				.Title("Event Driven Scene Example")
				.BackbufferSize(1280, 720)
				.AutoResizeBackBuffer()
				.QuitOnClose()
				.Build())
			{
				// Create a Scene object for the title screen. When the enter key or 
				// the start button on the first gamepad is pressed, that will
				// begin a new scene.
				GamepadInputHandler titleInputHandler = NewInputHandler();

				Scene titleScene = new Scene { InputHandler = titleInputHandler };

				titleInputHandler.Gamepads.First().ButtonPressed += (sender, e) =>
				{
					if (e.Button == GamepadButton.Start)
					{
						CreateGameScene(titleScene.SceneStack);
					}
					else if (e.Button == GamepadButton.Back)
					{
						titleScene.IsFinished = true;
					}
				};

				var font = new Font(Font.AgateSans)
				{
					Size = 22,
					Style = FontStyles.Bold
				};

				titleScene.Redraw += (sender, eventArgs) =>
				{
					Display.Clear(Color.Maroon);
					font.DrawText(0, 0, "Press enter to begin game, escape to quit.");
				};

				var stack = new SceneStack();

				stack.Start(titleScene);
			}
		}

		private static void CreateGameScene(ISceneStack stack)
		{
			// Here we build a simple scene for a "game" where the
			// player can move a big white circle around the screen.
			var gameInputHandler = NewInputHandler();
			Scene gameScene = new Scene { InputHandler = gameInputHandler };

			Vector2 position = 0.5 * (Vector2)Display.CurrentWindow.Size;
			Vector2 velocity = Vector2.Zero;

			var gamepad = gameInputHandler.Gamepads.First();

			gamepad.ButtonPressed += (sender, e) =>
			{
				if (e.Button == GamepadButton.Back)
					gameScene.IsFinished = true;
			};

			gamepad.LeftStickChanged += (sender, e) =>
			{
				velocity = 500 * gamepad.LeftStick;
			};

			gameScene.Update += (sender, e) =>
			{
				position += velocity * e.TotalSeconds;
			};

			var font = new Font(Font.AgateSerif)
			{
				Size = 22,
				Style = FontStyles.Bold
			};

			gameScene.Redraw += (sender, e) =>
			{
				Display.Clear(Color.Blue);
				font.DrawText(0, 0, "Press escape to go back to title screen.");

				Display.Primitives.FillCircle(Color.White, position, 40);
			};

			stack.Add(gameScene);
		}

		private static GamepadInputHandler NewInputHandler()
		{
			var result = new GamepadInputHandler();

			result.KeyMap.Add(KeyCode.Left, KeyMapItem.ToLeftStick(result.Gamepads.First(), Axis.X, -1));
			result.KeyMap.Add(KeyCode.Right, KeyMapItem.ToLeftStick(result.Gamepads.First(), Axis.X, 1));
			result.KeyMap.Add(KeyCode.Up, KeyMapItem.ToLeftStick(result.Gamepads.First(), Axis.Y, -1));
			result.KeyMap.Add(KeyCode.Down, KeyMapItem.ToLeftStick(result.Gamepads.First(), Axis.Y, 1));

			result.KeyMap.Add(KeyCode.Enter, KeyMapItem.ToButton(result.Gamepads.First(), GamepadButton.Start));
			result.KeyMap.Add(KeyCode.Escape, KeyMapItem.ToButton(result.Gamepads.First(), GamepadButton.Back));

			return result;
		}
	}
}
