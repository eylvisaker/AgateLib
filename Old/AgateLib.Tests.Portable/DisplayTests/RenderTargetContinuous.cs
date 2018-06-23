using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class RenderTargetContinuous : IAgateTest
	{
		public string Name => "Render Target Continuous";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (var wind = new DisplayWindowBuilder(args)
				.BackbufferSize(300, 300)
				.QuitOnClose()
				.Title(Name)
				.Build())
			{
				FrameBuffer buffer = new FrameBuffer(300, 300);

				while (AgateApp.IsAlive)
				{
					IFont font = Font.AgateSans;
					font.Size = 24;
					font.Color = Color.White;

					Display.RenderTarget = buffer;
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					font.DrawText(string.Format("Time: {0}", Timing.TotalSeconds.ToString("0.0")));

					Display.EndFrame();

					Display.RenderTarget = wind.FrameBuffer;
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					buffer.RenderTarget.Draw();

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}
	}
}
