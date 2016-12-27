using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class ColorTest : Scene, IAgateTest
	{
		public string Name
		{
			get { return "Color Test"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public override void Update(double deltaT)
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

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}

		public void Run()
		{
			SceneStack.Begin(this);
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public AgateConfig Configuration { get; set; }
	}
}
