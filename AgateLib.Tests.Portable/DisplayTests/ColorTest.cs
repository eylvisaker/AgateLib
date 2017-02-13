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

		public override void Update(ClockTimeSpan elapsed)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.Gray);

			for (int i = 0; i < 360; i++)
			{
				Display.FillRect(new Rectangle(i * 2, 0, 2, 75), Color.FromHsv(i, 1, 1));
			}
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				SceneStack.Start(this);
			}
		}

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
