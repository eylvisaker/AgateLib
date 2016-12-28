using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class LargeImageTest : Scene, IAgateTest
	{
		Surface mSomeSurface;
		private double mLoadTime;

		public string Name { get { return "Large Image"; } }
		public string Category { get { return "Display"; } }

		public AgateConfig Configuration { get; set; }

		public override void Update(double deltaT)
		{
		}

		public override void Draw()
		{
			Display.Clear(Color.White);

			var font = Font.AgateSans;
			font.Size = 24;
			mSomeSurface.Draw();
			font.DrawText(0, 0, "Load took {0} seconds.", mLoadTime);
		}

		protected internal override void OnSceneStart()
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

		public void ModifySetup(IAgateSetup setup)
		{
		}

		public void Run()
		{
			SceneStack.Start(this);
		}
	}
}
