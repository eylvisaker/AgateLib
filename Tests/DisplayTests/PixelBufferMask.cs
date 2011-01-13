using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.DisplayTests
{
	class PixelBufferMask : IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Pixel Buffer Masking"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup())
			{
				bool isRunning = false;

				setup.InitializeAll();

				if (setup.WasCanceled)
					return;

				DisplayWindow window = DisplayWindow.CreateWindowed("Test", 800, 600);

				PixelBuffer pbMaskBg = PixelBuffer.FromFile("mask_bg-bricks.png");
				PixelBuffer pbBg = PixelBuffer.FromFile("bg-bricks.png");
				PixelBuffer pbMaskCircle = PixelBuffer.FromFile("mask_circle.png");

				Surface surfRealBg = new Surface(pbMaskBg.Size);


				for (int x = 0; x < pbMaskBg.Width; x++)
				{
					for (int y = 0; y < pbMaskBg.Height; y++)
					{
						if (pbMaskBg.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
							pbBg.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
					}
				}

				surfRealBg.WritePixels(pbBg);

				isRunning = true;

				while (isRunning)
				{
					Display.CurrentWindow.Title = Display.FramesPerSecond.ToString();
					Display.BeginFrame();

					if (Keyboard.Keys[KeyCode.Escape])
						isRunning = false;


					if (Mouse.Buttons[Mouse.MouseButtons.Primary])
					{
						int mX = Mouse.X;
						int mY = Mouse.Y;

						Rectangle rect = new Rectangle(mX, mY, pbMaskCircle.Width, pbMaskCircle.Height);
						Point p = new Point(mX, mY);

						for (int x = 0; x < pbMaskCircle.Width; x++)
						{
							for (int y = 0; y < pbMaskCircle.Height; y++)
							{
								if (pbMaskCircle.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
									pbBg.SetPixel(mX + x, mY + y, Color.FromArgb(0, 0, 0, 0));
							}
						}

						surfRealBg.WritePixels(pbBg, rect, p);
					}

					Display.Clear(Color.Blue);
					surfRealBg.Draw();

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		#endregion
	}
}
