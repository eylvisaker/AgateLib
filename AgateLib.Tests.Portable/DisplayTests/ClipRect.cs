using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class ClipRect : Scene, IAgateTest
	{
		Surface surf;

		Color[] colors = new Color[] { 
					Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Green, Color.Turquoise, Color.Blue, Color.Violet, Color.Wheat, Color.White};

		double time;

		public string Name => "Clip Rects";

		public string Category => "Display";

		protected override void OnSceneStart()
		{
			surf = new Surface("Images/wallpaper.png");
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			time += gameClockElapsed.TotalSeconds;
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.DarkSlateGray);

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					Display.SetClipRect(new Rectangle(5 + i * 32, 5 + j * 32, 30, 30));

					surf.Draw();
				}
			}

			int index = colors.Length - ((int)time) % colors.Length - 1;
			for (int i = 10; i < 100; i += 10)
			{
				Display.SetClipRect(new Rectangle(320 + i, i, 310 - i * 2, 310 - i * 2));
				Display.Primitives.FillRect(colors[index], new Rectangle(0, 0, 640, 480));
				index++;
				index %= colors.Length;
			}
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				Input.Unhandled.KeyDown += (sender, e) =>
				{
					if (e.KeyCode == KeyCode.Escape)
						AgateApp.IsAlive = false;
				};

				new SceneStack().Start(this);
			}
		}
	}
}