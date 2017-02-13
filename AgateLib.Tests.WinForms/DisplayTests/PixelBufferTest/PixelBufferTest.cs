using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests.PixelBufferTest
{
	class PixelBufferTest : IAgateTest
	{
		Surface image;
		Point imageLocation = new Point(50, 50);
		PixelBuffer buffer;
		PixelBufferForm frm;
		bool mouseDown;

		public string Name => "Pixel Buffer";
		public string Category => "Display";

		public void Run(string[] args)
		{
			using (frm = new PixelBufferForm())
			{
				frm.Show();

				DisplayWindow wind = DisplayWindow.CreateFromControl(frm.panel1);

				image = new Surface("9ball.png");
				buffer = image.ReadPixels(PixelFormat.Any);

				Input.Unhandled.MouseDown += Mouse_MouseDown;
				Input.Unhandled.MouseMove += Mouse_MouseMove;
				Input.Unhandled.MouseUp += (sender, e) => mouseDown = false;

				while (AgateApp.IsAlive && frm.IsDisposed == false)
				{
					Display.BeginFrame();
					Display.Clear();

					image.Draw(imageLocation);

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}

		void Mouse_MouseMove(object sender, AgateInputEventArgs e)
		{
			Color clr;
			Point pt = new Point(e.MousePosition.X - imageLocation.X,
								 e.MousePosition.Y - imageLocation.Y);

			if (buffer.IsPointValid(pt) == false)
			{
				frm.lblPixelColor.Text = "No Pixel";
				return;
			}

			if (mouseDown)
			{
				// do a circle of radius 3
				for (int y = -3; y <= 3; y++)
				{
					for (int x = -3; x <= 3; x++)
					{
						// if we're out of the circle radius, go to the next iteration.
						if (x * x + y * y > 9)
							continue;

						Point newpt = new Point(pt.X + x, pt.Y + y);

						if (newpt.X < 0 || newpt.X >= buffer.Width) continue;
						if (newpt.Y < 0 || newpt.Y >= buffer.Height) continue;

						buffer.SetPixel(newpt.X, newpt.Y, Color.FromArgb(frm.btnColor.BackColor.ToArgb()));
					}
				}
				image.WritePixels(buffer);
			}

			clr = buffer.GetPixel(e.MousePosition.X - imageLocation.X,
			e.MousePosition.Y - imageLocation.Y);

			frm.lblPixelColor.Text =
				string.Format("R: {0}  G: {1}\r\nB: {2}  A: {3}",
				FormatComponent(clr.R), FormatComponent(clr.G),
				FormatComponent(clr.B), FormatComponent(clr.A));
		}

		private string FormatComponent(byte p)
		{
			return (p / 255.0).ToString("0.00");
		}

		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			mouseDown = true;
			Mouse_MouseMove(sender, e);
		}

	}
}