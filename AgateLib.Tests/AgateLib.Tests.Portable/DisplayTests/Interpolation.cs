using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class Interpolation : Scene, IAgateTest
	{
		Surface surf, surf2;

		public string Name
		{
			get { return "Interpolation Mode"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			SceneStack.Start(this);
		}

		protected override void OnSceneStart()
		{
			surf = new Surface("jellybean.png");
			surf2 = new Surface("jellybean.png");

			surf.SetScale(5.0, 5.0);
			surf2.SetScale(5.0, 5.0);
		}

		public override void Draw()
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

		public override void Update(double deltaT)
		{
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}
	}
}
