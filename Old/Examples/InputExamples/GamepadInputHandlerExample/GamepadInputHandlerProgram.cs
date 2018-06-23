using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.GamepadModel;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;
using AgateLib.Platform.WinForms;

namespace Examples.InputExamples.GamepadInputHandlerExample
{
	class GamepadInputHandlerProgram
	{
		private const int polygonSize = 25;
		private GamepadInputHandler inputHandler;
		private Font font;

		private Vector2 position = new Vector2(640, 360);

		private Polygon poly = new Polygon
		{
			{-polygonSize, -polygonSize*2 },
			{ polygonSize, -polygonSize*2 },
			{ polygonSize*2, -polygonSize },
			{polygonSize*2, polygonSize },
			{polygonSize, polygonSize*2 },
			{-polygonSize, polygonSize*2 },
			{-polygonSize*2, polygonSize },
			{-polygonSize*2, -polygonSize }
		};

		private List<string> lines = new List<string>();

		private IGamepad playerOne;

		[STAThread]
		static void Main(string[] args)
		{
			new GamepadInputHandlerProgram().Run(args);
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
				inputHandler = new GamepadInputHandler();

				playerOne = inputHandler.Gamepads.First();

				Input.Handlers.Add(inputHandler);

				font = new Font(Font.AgateSans, 12);

				while (AgateApp.IsAlive)
				{
					Update(AgateApp.GameClock.Elapsed);
					RenderScreen();

					AgateApp.KeepAlive();
				}

				// Dispose of the input handler 
				inputHandler.Dispose();
			}
		}

		private void Update(ClockTimeSpan elapsed)
		{
			position += playerOne.LeftStick * 200 * elapsed.TotalSeconds;

			poly = poly.Rotate(elapsed.TotalSeconds * 5 * (playerOne.LeftTrigger - playerOne.RightTrigger));
		}

		private void RenderScreen()
		{
			Display.BeginFrame();
			Display.Clear(Color.Purple);

			font.DrawText(Vector2.Zero, BuildGamepadStateText());

			Display.Primitives.FillPolygon(Color.LightGreen, poly.Translate(position));

			Display.EndFrame();
		}

		private string BuildGamepadStateText()
		{
			StringBuilder text = new StringBuilder();

			text.AppendLine($"Left stick: {playerOne.LeftStick}");
			text.AppendLine($"Right stick: {playerOne.RightStick}");
			text.AppendLine($"Direction pad: {playerOne.DirectionPad}");
			text.AppendLine($"Left trigger: {playerOne.LeftTrigger}");
			text.AppendLine($"Right trigger: {playerOne.RightTrigger}");

			text.Append("Button: ");

			text.Append(string.Join(", ", Enum.GetValues(typeof(GamepadButton))
				.Cast<GamepadButton>()
				.Where(x => playerOne.GetButton(x))));

			return text.ToString();
		}
	}
}
