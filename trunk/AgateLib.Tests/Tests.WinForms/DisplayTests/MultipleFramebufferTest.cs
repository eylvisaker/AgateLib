using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.DisplayTests
{
	class MultipleFramebufferTest : IAgateTest
	{
		List<Surface> mRegionColors = new List<Surface>();

		public string Name
		{
			get { return "Multiple Framebuffer Test"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		List<Surface> tests = new List<Surface>();
		bool done;
		Font font;
			
		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Multiple Framebuffer Test", 
					640, 480);

				font = Assets.Fonts.AgateSans;

				CreateTests();

				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

				while (wind.IsClosed == false && done == false)
				{
					Display.RenderTarget = wind.FrameBuffer;
					Display.BeginFrame();
					Display.Clear(Color.Gray);

					font.Color = Color.White;
					font.DisplayAlignment = OriginAlignment.TopLeft;
					font.DrawText("Press space to create another frame buffer.");

					int y = font.FontHeight;
					int x = 10;
					foreach (var surf in tests)
					{
						surf.Draw(x, y);
						y += surf.DisplayHeight + 10;

						if (y + 42 >= wind.Height)
						{
							y = font.FontHeight;
							x += 42;
						}
					}

					Display.EndFrame();
					Core.KeepAlive();
				}
			});
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Escape)
				done = true;

			if (e.KeyCode == KeyCode.Space)
				CreateTests();
		}

		int hueAngle = 0;
		private void CreateTests()
		{
			const int angleIncrement = 373 / 8;
			font.Color = Color.Black;
			font.DisplayAlignment = OriginAlignment.CenterRight;
			Surface mySurface = new Surface("jellybean.png");

			FrameBuffer frame = new FrameBuffer(32, 32);
			var clr = Color.FromHsv(hueAngle, 1, 1);

			Display.RenderTarget = frame;
			Display.BeginFrame();
			Display.Clear();
			mySurface.Draw();
			Display.FillRect(new Rectangle(0, 0, 8, 32), clr);

			var pt = new Point(31, 16);
			string text = tests.Count.ToString();

			font.Color = Color.Black;
			font.DrawText(pt.X - 1, pt.Y, text);
			font.DrawText(pt.X + 1, pt.Y, text);
			font.DrawText(pt.X, pt.Y - 1, text);
			font.DrawText(pt.X, pt.Y + 1, text);

			font.Color = Color.White;
			font.DrawText(pt.X, pt.Y, text);

			Display.EndFrame();
			tests.Add(frame.RenderTarget);
			frame.RenderTarget.SaveTo("test.png", ImageFileFormat.Png);

			hueAngle += angleIncrement;
		}
	}
}
