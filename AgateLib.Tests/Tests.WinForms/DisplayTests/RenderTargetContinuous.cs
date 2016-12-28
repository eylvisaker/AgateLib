using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class RenderTargetContinuous : IAgateTest
	{
		public string Name => "Render Target Continuous";

		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			DisplayWindow wind = DisplayWindow.CreateWindowed(Name, 300, 300);
			FrameBuffer buffer = new FrameBuffer(300, 300);

			while (wind.IsClosed == false)
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
				Core.KeepAlive();
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}
