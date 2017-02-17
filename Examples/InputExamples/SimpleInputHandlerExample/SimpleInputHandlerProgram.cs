using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;

namespace Examples.InputExamples.SimpleInputHandlerExample
{
	class SimpleInputHandlerProgram
	{
		private SimpleInputHandler inputHandler;
		private Font font;

		private Point position = new Point(640, 360);
		private Size size = new Size(10, 10);
		private List<string> lines = new List<string>();

		[STAThread]
		static void Main(string[] args)
		{
			new SimpleInputHandlerProgram().Run(args);
		}

		private void Run(string[] args)
		{
			using (new AgateWinForms(args)
					.Initialize())
			using (new DisplayWindowBuilder(args)
					.BackbufferSize(1280, 720)
					.QuitOnClose()
					.Build())
			{
				inputHandler = new SimpleInputHandler();

				inputHandler.KeyDown += InputHandler_KeyDown;
				inputHandler.KeyUp += InputHandler_KeyUp;
				inputHandler.MouseDown += InputHandler_MouseDown;
				inputHandler.MouseMove += InputHandler_MouseMove;
				inputHandler.MouseUp += InputHandler_MouseUp;
				inputHandler.MouseWheel += InputHandler_MouseWheel;

				Input.Handlers.Add(inputHandler);

				font = new Font(Font.AgateSans, 12);

				while (AgateApp.IsAlive)
				{
					RenderScreen();

					AgateApp.KeepAlive();
				}

				// Dispose of the input handler 
				inputHandler.Dispose();
			}
		}

		private void RenderScreen()
		{
			Display.BeginFrame();
			Display.Clear(Color.Purple);

			int y = 0;
			foreach (var line in lines)
			{
				font.DrawText(0, y, line);
				y += font.FontHeight;
			}

			string keysPressed = string.Join(",", inputHandler.Keys.PressedKeys);

			font.DrawText(0, 720 - font.FontHeight,
				$"Keys pressed: {keysPressed}");

			Display.Primitives.FillRect(Color.LightGreen, new Rectangle(position, size));

			Display.EndFrame();
		}

		private void InputHandler_KeyDown(object sender, AgateInputEventArgs e)
		{
			switch (e.KeyCode)
			{
				case KeyCode.Plus:
				case KeyCode.NumPadPlus:
					size = new Size(size.Width + 5, size.Height + 5);
					break;

				case KeyCode.Minus:
				case KeyCode.NumPadMinus:
					size = new Size(size.Width - 5, size.Height - 5);
					break;

				case KeyCode.Up:
					position.Y -= 10;
					break;

				case KeyCode.Down:
					position.Y += 10;
					break;

				case KeyCode.Left:
					position.X -= 10;
					break;

				case KeyCode.Right:
					position.X += 10;
					break;
			}

			AddLine($"Key down: KeyCode={e.KeyCode}, KeyString={e.KeyString}");
		}

		private void InputHandler_KeyUp(object sender, AgateInputEventArgs e)
		{
			AddLine($"Key up:   KeyCode={e.KeyCode}, KeyString={e.KeyString}");
		}

		private void InputHandler_MouseDown(object sender, AgateInputEventArgs e)
		{
			TrackMouseWithSprite(e);

			AddLine($"Mouse button {e.MouseButton} down at {e.MousePosition}");
		}

		private void InputHandler_MouseMove(object sender, AgateInputEventArgs e)
		{
			if (inputHandler.IsMouseButtonDown(MouseButton.Primary))
			{
				TrackMouseWithSprite(e);

				AddLine($"Mouse move at {e.MousePosition}");
			}
		}

		private void InputHandler_MouseUp(object sender, AgateInputEventArgs e)
		{
			TrackMouseWithSprite(e);

			AddLine($"Mouse button {e.MouseButton} up at {e.MousePosition}");
		}

		private void InputHandler_MouseWheel(object sender, AgateInputEventArgs e)
		{
			TrackMouseWithSprite(e);

			AddLine($"Mouse wheel moved {e.MouseWheelDelta} at {e.MousePosition}");
		}

		private void TrackMouseWithSprite(AgateInputEventArgs e)
		{
			// Center the sprite on the mouse position
			position = new Point(
				e.MousePosition.X - size.Width / 2,
				e.MousePosition.Y - size.Height / 2);
		}

		private void AddLine(string text)
		{
			lines.Add(text);

			if (lines.Count > 5)
				lines.RemoveAt(0);
		}
	}
}
