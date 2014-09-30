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

		Surface mSomeSurface;
		private double mLoadTime;

		protected override void OnSceneStart()
		{
			var watch = new Stopwatch();
			watch.Start();
			mSomeSurface = new Surface("largeimage.png");
			mSomeSurface.LoadComplete += (sender, e) =>
				{
					watch.Stop();
					mLoadTime = watch.ElapsedMilliseconds / 1000.0;
				};
		}
		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.White);

			var font = AgateLib.Assets.Fonts.AgateSans;
			font.Size = 24;
			mSomeSurface.Draw();
			font.DrawText(0, 0, "Load took {0} seconds.", mLoadTime);
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
