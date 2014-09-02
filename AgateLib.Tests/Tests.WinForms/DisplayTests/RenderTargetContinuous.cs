using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform;

namespace AgateLib.Testing.DisplayTests
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
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed(Name, 300, 300);
				FrameBuffer buffer = new FrameBuffer(300, 300);

				while (wind.IsClosed == false)
				{
					Font font = AgateLib.Assets.Fonts.AgateSans;
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
			});
		}
	}
}
