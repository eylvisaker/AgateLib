using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class PixelBufferMask : IAgateTest
	{
		Point mouse;

		public string Name => "Pixel Buffer Masking";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
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

				bool mouseDown = false;

				Input.Unhandled.MouseDown += (sender, e) =>
				{
					if (e.MouseButton == MouseButton.Primary)
						mouseDown = true;
				};
				Input.Unhandled.MouseUp += (sender, e) =>
				{
					if (e.MouseButton == MouseButton.Primary)
						mouseDown = false;
				};
				Input.Unhandled.MouseMove += (sender, e) => mouse = e.MousePosition;

				while (AgateApp.IsAlive)
				{
					Display.CurrentWindow.Title = Display.FramesPerSecond.ToString();
					Display.BeginFrame();

					if (Input.Unhandled.Keys[KeyCode.Escape])
						return;

					if (mouseDown)
					{
						int mX = mouse.X;
						int mY = mouse.Y;

						Rectangle rect = new Rectangle(mX, mY, pbMaskCircle.Width, pbMaskCircle.Height);
						Point p = new Point(mX, mY);

						for (int x = 0; x < pbMaskCircle.Width; x++)
						{
							if (mX + x >= pbBg.Width)
								break;

							for (int y = 0; y < pbMaskCircle.Height; y++)
							{
								if (mY + y >= pbBg.Height)
									break;

								if (pbMaskCircle.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
									pbBg.SetPixel(mX + x, mY + y, Color.FromArgb(0, 0, 0, 0));
							}
						}

						surfRealBg.WritePixels(pbBg, rect, p);
					}

					Display.Clear(Color.Blue);
					surfRealBg.Draw();

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}
	}
}
