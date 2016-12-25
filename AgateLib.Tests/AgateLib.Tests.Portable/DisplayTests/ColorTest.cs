using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.ApplicationModels;

namespace AgateLib.Tests.DisplayTests
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

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
