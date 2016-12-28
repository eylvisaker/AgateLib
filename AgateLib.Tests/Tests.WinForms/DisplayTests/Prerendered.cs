using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.DisplayTests
{
	class PrerenderedTest : IAgateTest
	{
		public string Name => "Prerendering";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			DisplayWindow MainWindow = DisplayWindow.CreateWindowed("Test", 800, 600);
			FrameBuffer myBuffer = new FrameBuffer(200, 35);

			IFont font = AgateLib.DefaultAssets.Fonts.AgateSans;
			RenderToFrameBuffer(myBuffer, font);

			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();

			while (MainWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.Black);

				myBuffer.RenderTarget.Draw(35, 35);
				font.DrawText(38, 73, "HELLO WORLD");

				Display.EndFrame();

				Core.KeepAlive();

				if (watch.ElapsedMilliseconds > 3000)
				{
					RenderToFrameBuffer(myBuffer, font);

					watch.Reset();
					watch.Start();
				}
			}
		}

		private static void RenderToFrameBuffer(FrameBuffer myBuffer, IFont font)
		{
			FrameBuffer save = Display.RenderTarget;

			Display.RenderTarget = myBuffer;
			Display.BeginFrame();

			Display.Clear(Color.Blue);
			Display.FillRect(new Rectangle(2, 2, 20, 20), Color.Black);

			font.Color = Color.Red;
			font.DrawText(3, 3, "HELLO WORLD");

			Display.EndFrame();

			Display.RenderTarget = save;
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}