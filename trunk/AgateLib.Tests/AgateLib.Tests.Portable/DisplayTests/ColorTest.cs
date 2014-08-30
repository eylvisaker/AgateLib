using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests
{
	class ColorTest : Scene, ISceneModelTest
	{
		public string Name
		{
			get { return "Color Test"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public override void Update(double delta_t)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.Gray);

			for (int i = 0; i < 360; i++)
			{
				Display.FillRect(new Rectangle(0, i, 75, 1), Color.FromHsv(i, 1, 1));
			}
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
