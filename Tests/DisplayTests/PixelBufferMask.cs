﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.DisplayTests
{
	class PixelBufferMask : IAgateTest
	{
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
			new PassiveModel(args).Run( () =>
			{
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

				while (Display.CurrentWindow.IsClosed == false)
				{
					Display.CurrentWindow.Title = Display.FramesPerSecond.ToString();
					Display.BeginFrame();

					if (Keyboard.Keys[KeyCode.Escape])
						return;

					if (Mouse.Buttons[Mouse.MouseButtons.Primary])
					{
						int mX = Mouse.X;
						int mY = Mouse.Y;

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
					Core.KeepAlive();
				}
			});
		}
	}
}