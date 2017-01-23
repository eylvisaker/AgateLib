using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests
{
	class ScreenCaptureTest : IAgateTest
	{
		public string Name => "Screen Capture";

		public string Category => "Display";

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				Surface someSurface = new Surface("Images/wallpaper.png");
				bool capturing = false;

				FrameBuffer capture = new FrameBuffer(1600, 1200);

				while (AgateApp.IsAlive)
				{
					if (Input.Unhandled.Keys[KeyCode.C])
					{
						capturing = true;
						Input.Unhandled.Keys.Release(KeyCode.C);
					}
					if (capturing)
					{
						Display.RenderTarget = capture;
						someSurface.SetScale(2, 2);
					}

					Display.BeginFrame();

					Display.Clear(Color.White);

					someSurface.Draw();
					Display.FillRect(10, 10, 10, 10, Color.Gray);

					Display.EndFrame();

					if (capturing)
					{
						capture.RenderTarget.SaveTo("CapturedImage.png", ImageFileFormat.Png);
						Display.RenderTarget = Display.CurrentWindow.FrameBuffer;
						someSurface.SetScale(1, 1);
						capturing = false;

						Debug.WriteLine("Captured image to CapturedImage.png");
					}

					AgateApp.KeepAlive();
				}
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}
	}
}
