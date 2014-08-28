using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.DisplayTests
{
	class Interpolation : AgateLib.AgateGame, IAgateTest
	{

		#region IAgateTest Members

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

		protected override void Initialize()
		{
			surf = new Surface("jellybean.png");
			surf2 = new Surface("jellybean.png");
			font = new FontSurface("Arial", 14);

			surf.SetScale(6.0, 6.0);
			font.SetScale(3.0, 3.0);
			surf2.SetScale(6.0, 6.0);
		}

		protected override void Render()
		{
			Display.Clear(Color.Blue);

			surf.InterpolationHint = InterpolationMode.Fastest;
			surf.Draw(10, 10);

			font.DrawText(10, 500, "Chonky chonk chonk");

			surf2.InterpolationHint = InterpolationMode.Fastest;
			surf2.Draw(500, 10);
		}

		#endregion

		#region IAgateTest Members


		public void Main(string[] args)
		{
			new PassiveModel(args).Run(() => Run(args));
		}

		#endregion
	}
}
