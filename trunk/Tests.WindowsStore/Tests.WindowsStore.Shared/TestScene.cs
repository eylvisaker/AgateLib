using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.WindowsStore
{
	class TestScene : Scene
	{
		Color clr;
		double time;
		Surface image;

		protected override void OnSceneStart()
		{
			image = new Surface("agatelogo.png");
		}

		public override void Update(double delta_t)
		{
			time += delta_t / 1000.0;

			clr = Color.FromHsv(time * 50, 1, 1);
		}

		public override void Draw()
		{
			Display.Clear(clr);

			int pos = (int)(time * 100);

			image.Draw(pos % 100, pos % 200);

			image.Draw(200, 200);
		}
	}
}
