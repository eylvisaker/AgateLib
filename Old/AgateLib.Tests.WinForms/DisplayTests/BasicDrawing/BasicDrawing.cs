// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests.BasicDrawing
{
	class BasicDrawing : IAgateTest
	{
		List<Shape> shapes = new List<Shape>();
		Random random = new Random();
		DrawingTester frm;

		private Color ShapeColor => Color.FromArgb(frm.SelectedColor.ToArgb());

		public string Name => "Basic Drawing";

		public string Category => "Display";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public void Run(string[] args)
		{
			// create a random number generation object 
			// so that we can make pretty colors.
			Random rand = new Random();

			frm = new DrawingTester();

			frm.btnClear.Click += btnClear_Click;
			frm.btnDrawLine.Click += btnDrawLine_Click;
			frm.btnDrawRect.Click += btnDrawRect_Click;
			frm.btnDrawCircle.Click += btnDrawCircle_Click;
			frm.btnDrawPolygon.Click += btnDrawPolygon_Click;
			frm.btnFillRect.Click += btnFillRect_Click;
			frm.btnFillCircle.Click += btnFillCircle_Click;
			frm.btnFillPolygon.Click += btnFillPolygon_Click;
			frm.Show();

			// This creates the window that we will be drawing in.
			// 640x480 are the dimensions of the screen area that we will write to
			using (var window = new DisplayWindowBuilder(args)
				.RenderToControl(frm.panel1)
				.AutoResizeBackBuffer()
				.Build())
			{
				while (window.IsClosed == false)
				{
					// Display.BeginFrame must be called before any rendering takes place.
					Display.BeginFrame();

					// Clear back buffer
					Display.Clear();

					// draw shapes
					foreach (Shape s in shapes)
						s.Draw();

					// Display.EndFrame must be called after rendering is done
					// in order to actually update the display.
					Display.EndFrame();

					// AgateApp.KeepAlive() is where we play nice window the OS, 
					// allowing events to be processed and such.
					// This is also required to process events that happen in our OWN 
					// code (ie. user input), so be sure to call this once a frame.
					AgateApp.KeepAlive();

					// This gives a nice 1 second delay between each frame.
					// Using the Sleep() call causes this application to
					// relinquish CPU time.
					System.Threading.Thread.Sleep(10);
				}
			}
		}

		private void btnDrawPolygon_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawPolygon, ShapeColor, RandomRect()));
			NextColor();
		}

		void btnDrawCircle_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawEllipse, ShapeColor, RandomRect()));
			NextColor();
		}

		void btnDrawRect_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawRect, ShapeColor, RandomRect()));
			NextColor();
		}

		void btnFillRect_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.FillRect, ShapeColor, RandomRect()));
			NextColor();
		}

		void btnFillCircle_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.FillEllipse, ShapeColor, RandomRect()));
			NextColor();
		}

		private void btnFillPolygon_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.FillPolygon, ShapeColor, RandomRect()));
			NextColor();
		}

		private void NextColor()
		{
			var rnd = new Random();

			frm.SelectedColor = System.Drawing.Color.FromArgb(
				frm.SelectedColor.A,
				rnd.Next(256),
				rnd.Next(256),
				rnd.Next(256));
		}

		void btnDrawLine_Click(object sender, EventArgs e)
		{
			int x = random.Next(0, frm.panel1.Width * 2 / 3);
			int y = random.Next(0, frm.panel1.Width * 2 / 3);

			switch (random.Next(10))
			{
				case 0:
				case 1:
				case 2:
					shapes.Add(new Shape(ShapeType.DrawLine, Color.FromArgb(frm.SelectedColor.ToArgb()), new Rectangle(x, y, 14, 0)));
					break;

				case 3:
				case 4:
				case 5:
					shapes.Add(new Shape(ShapeType.DrawLine, Color.FromArgb(frm.SelectedColor.ToArgb()), new Rectangle(x, y, 0, 14)));
					break;

				default:
					shapes.Add(new Shape(ShapeType.DrawLine, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
					break;
			}
		}

		void btnClear_Click(object sender, EventArgs e)
		{
			shapes.Clear();
		}

		Rectangle RandomRect()
		{
			return new Rectangle(
				random.Next(0, frm.panel1.Width * 2 / 3),
				random.Next(0, frm.panel1.Height * 2 / 3),
				random.Next(10, frm.panel1.Width / 2),
				random.Next(10, frm.panel1.Height / 2)
				);
		}
	}
	enum ShapeType
	{
		FillRect,
		FillEllipse,
		DrawRect,
		DrawEllipse,
		DrawLine,
		DrawPolygon,
		FillPolygon,
	}
}