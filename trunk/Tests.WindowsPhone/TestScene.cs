using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.WindowsPhone
{
	class TestScene : Scene
	{
		Color clr;
		double time;
		Surface image;

		protected override void OnSceneStart()
		{
			image = new Surface("ApplicationIcon.png");
		}

		public override void Update(double delta_t)
		{
			time += delta_t / 1000.0;
	
			clr = Color.FromHsv(time * 50, 1, 1);
		}

		public override void Draw()
		{
			Display.Clear(clr);
		}
	}
}
