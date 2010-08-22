﻿using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.DisplayTests
{
	class PrerenderedTest : IAgateTest
	{
		public string Name
		{
			get { return "Prerendering"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			AgateSetup AS = new AgateSetup();

			AS.AskUser = true;
			AS.Initialize(true, false, true);
			if (AS.WasCanceled)
				return;
			
			DisplayWindow MainWindow = DisplayWindow.CreateWindowed ("Test", 800, 600);
			FrameBuffer myBuffer = new FrameBuffer(200,35);
			FontSurface font = FontSurface.AgateSans10;

			Display.RenderTarget = myBuffer;
			Display.BeginFrame();

			Display.Clear(Color.Blue);
			font.Color = Color.Red;
			font.DrawText(3,3 , "HELLO WORLD");
		
			Display.EndFrame();
			Display.FlushDrawBuffer();
			Display.RenderTarget = MainWindow.FrameBuffer;

			
			while (MainWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear(Color.Black);
				
				myBuffer.RenderTarget.Draw(35,35);

				Display.EndFrame();
								
				Core.KeepAlive();
			}
		}

	}
}