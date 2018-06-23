using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class Interpolation : Scene, IAgateTest
	{
		Surface surf, surf2;

		public string Name => "Interpolation Mode";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}

		protected override void OnSceneStart()
		{
			surf = new Surface("Images/jellybean.png");
			surf2 = new Surface("Images/jellybean.png");

			surf.SetScale(5.0, 5.0);
			surf2.SetScale(5.0, 5.0);
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.Blue);

			surf.InterpolationHint = InterpolationMode.Fastest;
			surf.Draw(10, 10);

			IFont font = Font.AgateSans;
			font.Size = 30;
			font.DrawText(10, 500, "Chonky chonk chonk");

			surf2.InterpolationHint = InterpolationMode.Nicest;
			surf2.Draw(420, 10);
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
		}
	}
}
