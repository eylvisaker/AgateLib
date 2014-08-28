using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace Tests.ScreenCaptureExample
{
	class ScreenCaptureTest : IAgateTest
	{

		#region IAgateTest Members

		public string Name { get { return "Screen Capture"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Hello", 800, 600);
				Surface someSurface = new Surface("wallpaper.png");
				bool capturing = false;

				FrameBuffer capture = new FrameBuffer(1600, 1200);

				while (wind.IsClosed == false)
				{
					if (Keyboard.Keys[KeyCode.C])
					{
						capturing = true;
						Keyboard.ReleaseKey(KeyCode.C);
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
						Display.RenderTarget = wind.FrameBuffer;
						someSurface.SetScale(1, 1);
						capturing = false;

						Debug.Print("Captured image to CapturedImage.png");
					}

					Core.KeepAlive();
					System.Threading.Thread.Sleep(10);
				}
			});
		}
	}
}
