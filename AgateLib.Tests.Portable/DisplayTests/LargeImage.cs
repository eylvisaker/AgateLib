using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class LargeImageTest : Scene, IAgateTest
	{
		Surface mSomeSurface;
		private double mLoadTime;

		public string Name => "Large Image";
		public string Category => "Display";

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			if (Input.Unhandled.Keys[KeyCode.Escape])
				AgateApp.IsAlive = false;
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.White);

			var font = Font.AgateSans;
			font.Size = 24;
			mSomeSurface.Draw();
			font.DrawText(0, 0, "Load took {0} seconds.", mLoadTime);
		}

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

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}
	}
}
