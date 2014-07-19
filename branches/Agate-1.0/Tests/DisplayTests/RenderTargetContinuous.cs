using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.DisplayTests
{
	class RenderTargetContinuous : IAgateTest
	{
		public string Name
		{
			get { return "Render Target Continuous"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled) return;

				DisplayWindow wind = DisplayWindow.CreateWindowed(Name, 300, 300);
				FrameBuffer buffer = new FrameBuffer(300, 300);

				FontSurface font = FontSurface.AgateSans24;
				font.Color = Color.White;
				
				while (wind.IsClosed == false)
				{
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
		}
	}
}
