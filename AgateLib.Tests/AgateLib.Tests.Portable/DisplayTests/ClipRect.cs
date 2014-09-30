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
	class ClipRect : Scene, ISceneModelTest
	{
		Surface surf;

		Color[] colors = new Color[] { 
					Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Green, Color.Turquoise, Color.Blue, Color.Violet, Color.Wheat, Color.White};

		double time;

		public string Name
		{
			get { return "Clip Rects"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		protected override void OnSceneStart()
		{
			surf = new Surface("wallpaper.png");
		}

		public void ModifyModelParameters(ApplicationModels.SceneModelParameters parameters)
		{
		}

		public ApplicationModels.Scene StartScene
		{
			get { return this; }
		}

		public override void Update(double deltaT)
		{
			time += deltaT / 1000.0;
		}

		public override void Draw()
		{
			Display.Clear(Color.DarkSlateGray);

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					Display.SetClipRect(new Rectangle(5 + i * 32, 5 + j * 32, 30, 30));

					surf.Draw();
				}
			}

			int index = colors.Length - ((int)time) % colors.Length - 1;
			for (int i = 10; i < 100; i += 10)
			{
				Display.SetClipRect(new Rectangle(320 + i, i, 310 - i * 2, 310 - i * 2));
				Display.FillRect(0, 0, 640, 480, colors[index]);
				index++;
				index %= colors.Length;
			}
		}
	}
}