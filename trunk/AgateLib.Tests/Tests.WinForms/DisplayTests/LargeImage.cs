using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests
{
	class LargeImageTest : Scene, ISceneModelTest
	{
		public string Name { get { return "Large Image"; } }
		public string Category { get { return "Display"; } }

		double loadTime;
		Surface someSurface;

		protected override void OnSceneStart()
		{
			System.Diagnostics.Stopwatch watch = new Stopwatch();
			watch.Start();
			someSurface = new Surface("largeimage.png");
			someSurface.LoadComplete += (sender, e) =>
				{
					watch.Stop();
					double loadTime = watch.ElapsedMilliseconds / 1000.0;
				};
		}
		public override void Update(double delta_t)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.White);

			var font = AgateLib.Assets.Fonts.AgateSans;
			font.Size = 24;
			someSurface.Draw();
			font.DrawText(0, 0, "Load took {0} seconds.", loadTime);
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
