using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class ColorTest : Scene, IAgateTest
	{
		public string Name => "Color Test";

		public string Category => "Display";

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.Gray);

			for (int i = 0; i < 360; i++)
			{
				Display.Primitives.FillRect(Color.FromHsv(i, 1, 1),
					new Rectangle(i * 2, 0, 2, 75));
			}
		}

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

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
