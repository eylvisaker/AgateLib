using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class PrerenderedTest : IAgateTest
	{
		public string Name => "Prerendering";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				FrameBuffer myBuffer = new FrameBuffer(200, 35);

				IFont font = Font.AgateSans;
				RenderToFrameBuffer(myBuffer, font);

				var watch = System.Diagnostics.Stopwatch.StartNew();

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear(Color.Black);

					myBuffer.RenderTarget.Draw(35, 35);
					font.DrawText(38, 73, "HELLO WORLD");

					Display.EndFrame();

					AgateApp.KeepAlive();

					if (watch.ElapsedMilliseconds > 3000)
					{
						RenderToFrameBuffer(myBuffer, font);

						watch.Reset();
						watch.Start();
					}
				}
			}
		}

		private static void RenderToFrameBuffer(FrameBuffer myBuffer, IFont font)
		{
			FrameBuffer save = Display.RenderTarget;

			Display.RenderTarget = myBuffer;
			Display.BeginFrame();

			Display.Clear(Color.Blue);
			Display.Primitives.FillRect(Color.Black, new Rectangle(2, 2, 20, 20));

			font.Color = Color.Red;
			font.DrawText(3, 3, "HELLO WORLD");

			Display.EndFrame();

			Display.RenderTarget = save;
		}
	}
}