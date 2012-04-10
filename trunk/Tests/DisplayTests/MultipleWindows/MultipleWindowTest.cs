// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.MultipleWindows
{
	class MultipleWindowTest : IAgateTest 
	{
		Surface surf;
		Random rand = new Random();
		FrameBuffer frameBuffer;

		public string Name
		{
			get { return "Multiple Render Targets"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled) return;

				MultipleRenderTargetExample myForm = new MultipleRenderTargetExample();
				myForm.Show();

				// create three display windows
				DisplayWindow wnd_1 = DisplayWindow.CreateFromControl(myForm.pictureBox1);
				DisplayWindow wnd_2 = DisplayWindow.CreateFromControl(myForm.pictureBox2);
				DisplayWindow wnd_3 = DisplayWindow.CreateFromControl(myForm.pictureBox3);

				frameBuffer = new FrameBuffer(wnd_3.Width, wnd_3.Height);
				myForm.pictureBox3.Resize += new EventHandler(wnd_3_Resize);

				// this is the code that will be called when the button is pressed
				myForm.btnDraw.Click += new EventHandler(btnDraw_Click);
				myForm.btnClearSurface.Click += new EventHandler(btnClear_Click);
				myForm.btnDrawText.Click += new EventHandler(btnDrawText_Click);
				Surface image1 = new Surface("jellybean.png");
				Surface image2 = new Surface("9ball.png");
				image1.DisplayWidth = 40;
				image1.DisplayHeight = (int)(image1.DisplayWidth * image1.SurfaceHeight / (double)image1.SurfaceWidth);
				image2.DisplayWidth = 40;
				image2.DisplayHeight = (int)(image2.DisplayWidth * image2.SurfaceHeight / (double)image2.SurfaceWidth);

				double time = 0;

				while (myForm.Visible)
				{
					// Render targets must be set before the call to BeginFrame,
					// and may not be changed between BeginFrame and EndFrame.
					// Use the FrameBuffer property of each DisplayWindow object
					// to set the Display.RenderTarget value.
					Display.RenderTarget = wnd_1.FrameBuffer;

					Display.BeginFrame();
					Display.Clear(Color.Red);
					Display.FillRect(new Rectangle(20, 20, 40, 30), Color.Blue);
					image1.Draw(120 + (int)(30 * Math.Sin(time)), 20);

					Display.EndFrame();

					// now do the second window.
					Display.RenderTarget = wnd_2.FrameBuffer;

					Display.BeginFrame();
					Display.Clear(Color.Green);
					Display.FillRect(new Rectangle(20, 20, 40, 30), Color.Yellow);
					image2.Draw(120 + (int)(30 * Math.Cos(time)), 20);

					Display.EndFrame();

					// draw the third window from the surface
					Display.RenderTarget = wnd_3.FrameBuffer;

					surf = frameBuffer.RenderTarget;

					Display.BeginFrame();
					Display.Clear(Color.Gray);
					surf.Draw(0, 0);
					Display.EndFrame();

					Core.KeepAlive();
					time = Timing.TotalSeconds;
				}
			}
		}

		void btnDrawText_Click(object sender, EventArgs e)
		{
			Display.RenderTarget = frameBuffer;
			Display.BeginFrame();

			int x = rand.Next(20, 100);
			int y = rand.Next(20, 100);

			Color clr = Color.White;

			FontSurface.AgateSans14.DrawText(x, y, "Sample Text");

			Display.EndFrame();

			surf.SaveTo("test.png", ImageFileFormat.Png);

			System.Diagnostics.Debug.Print("Wrote text to {0},{1}.", x,y);
			System.Diagnostics.Debug.Flush();
		}

		void wnd_3_Resize(object sender, EventArgs e)
		{
			var ctrl = (System.Windows.Forms.Control )sender;

			FrameBuffer newBuffer = new FrameBuffer(ctrl.Width, ctrl.Height);

			Display.RenderTarget = newBuffer;
			
			Display.BeginFrame();
			Display.Clear(Color.FromArgb(0, 0, 0, 0));
			Display.RenderState.AlphaBlend = false;

			frameBuffer.RenderTarget.Draw();

			Display.EndFrame();
			Display.RenderState.AlphaBlend = true;

			frameBuffer.Dispose();
			frameBuffer = newBuffer;
		}

		void btnClear_Click(object sender, EventArgs e)
		{
			Display.RenderTarget = frameBuffer;
			Display.BeginFrame();
			Display.Clear(0, 0, 0, 0);
			Display.EndFrame();
		}

		void btnDraw_Click(object sender, EventArgs e)
		{
			Display.RenderTarget = frameBuffer;
			Display.BeginFrame();

			int width = rand.Next(20, 100);
			int height = rand.Next(20, 100);

			Rectangle rect = new Rectangle(
				rand.Next(0, frameBuffer.Width - width),
				rand.Next(0, frameBuffer.Height - height),
				width,
				height);

			Color clr = Color.FromArgb(255 /*rand.Next(200, 256)*/, rand.Next(0, 256),
					rand.Next(0, 256), rand.Next(0, 256));

			Display.FillRect(rect, clr);

			Display.EndFrame();

			surf.SaveTo("test.png", ImageFileFormat.Png);

			System.Diagnostics.Debug.Print("Wrote rectangle to {0} with color {1}.", rect, clr);
			System.Diagnostics.Debug.Flush();
		}


	}
}