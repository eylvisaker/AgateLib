using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests
{
	class Interpolation : Scene, ISceneModelTest
	{
		public string Name
		{
			get { return "Interpolation Mode"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		Surface surf, surf2;
		FontSurface font;

		protected override void OnSceneStart()
		{
			surf = new Surface("jellybean.png");
			surf2 = new Surface("jellybean.png");
			font = new FontSurface("Arial", 14);

			surf.SetScale(6.0, 6.0);
			font.SetScale(3.0, 3.0);
			surf2.SetScale(6.0, 6.0);
		}

		public override void Draw()
		{
			Display.Clear(Color.Blue);

			surf.InterpolationHint = InterpolationMode.Fastest;
			surf.Draw(10, 10);

			font.DrawText(10, 500, "Chonky chonk chonk");

			surf2.InterpolationHint = InterpolationMode.Fastest;
			surf2.Draw(500, 10);
		}

		public override void Update(double delta_t)
		{
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
