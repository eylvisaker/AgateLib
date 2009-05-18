// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace Tests.DisplayTests.BasicDrawing
{
	class BasicDrawing : IAgateTest
	{
		enum ShapeType
		{
			FillRect,
			FillEllipse,
			DrawRect,
			DrawEllipse,
			DrawLine,
		}

		class Shape
		{
			public Shape()
			{
			}
			public Shape(ShapeType shapeType, Color color, Rectangle rect)
			{
				FigureType = shapeType;
				Color = color;
				Rect = rect;
			}

			public ShapeType FigureType;
			public Color Color;
			public Rectangle Rect;

			public void Draw()
			{
				switch (FigureType)
				{
					case ShapeType.DrawLine:
						Display.DrawLine(Rect.Left, Rect.Top, Rect.Right, Rect.Bottom, Color);
						break;

					case ShapeType.DrawRect:
						Display.DrawRect(Rect, Color);
						break;

					case ShapeType.DrawEllipse:
						Display.DrawEllipse(Rect, Color);
						break;

					case ShapeType.FillRect:
						Display.FillRect(Rect, Color);
						break;

					case ShapeType.FillEllipse:
						Display.FillEllipse(Rect, Color);
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		static List<Shape> shapes = new List<Shape>();
		static Random random = new Random();
		static DrawingTester frm;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public void Main(string[] args)
		{

			using (AgateSetup setup = new AgateSetup(args))
			{
				// create a random number generation object 
				// so that we can make pretty colors.
				Random rand = new Random();

				// initialize the display, asking the user what display driver to use.
				setup.AskUser = true;
				setup.Initialize(true, false, false);

				// normally, the display should initialize fine, and goahead will be true.
				// However, here we are asking the user what display mode they want to pick,
				// and they may push the cancel button.  If they do, then exit the program.
				if (setup.WasCanceled)
					return;

				frm = new DrawingTester();

				frm.btnClear.Click += new EventHandler(btnClear_Click);
				frm.btnDrawLine.Click += new EventHandler(btnDrawLine_Click);
				frm.btnDrawRect.Click += new EventHandler(btnDrawRect_Click);
				frm.btnFillRect.Click += new EventHandler(btnFillRect_Click);
				frm.btnDrawCircle.Click += new EventHandler(btnDrawCircle_Click);
				frm.btnFillCircle.Click += new EventHandler(btnFillCircle_Click);
				frm.Show();

				// This creates the window that we will be drawing in.
				// 640x480 are the dimensions of the screen area that we will write to
				DisplayWindow wind = DisplayWindow.CreateFromControl(frm.panel1);

				while (wind.IsClosed == false)
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

					// Core.KeepAlive is where we play nice window the OS, 
					// allowing events to be processed and such.
					// This is also required to process events that happen in our OWN 
					// code (ie. user input), so be sure to call this once a frame.
					Core.KeepAlive();

					// This gives a nice 1 second delay between each frame.
					// Using the Sleep() call causes this application to
					// relinquish CPU time.
					System.Threading.Thread.Sleep(10);
				}

			}
		}

		void btnFillCircle_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.FillEllipse, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
		}

		static void btnDrawCircle_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawEllipse, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
		}

		static Rectangle RandomRect()
		{
			return new Rectangle(
				random.Next(0, frm.panel1.Width * 2 / 3),
				random.Next(0, frm.panel1.Height * 2 / 3),
				random.Next(10, frm.panel1.Width / 2),
				random.Next(10, frm.panel1.Height / 2)
				);
		}
		static void btnFillRect_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.FillRect, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
		}

		static void btnDrawRect_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawRect, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
		}

		static void btnDrawLine_Click(object sender, EventArgs e)
		{
			shapes.Add(new Shape(ShapeType.DrawLine, Color.FromArgb(frm.SelectedColor.ToArgb()), RandomRect()));
		}

		static void btnClear_Click(object sender, EventArgs e)
		{
			shapes.Clear();

		}

		#region IAgateTest Members

		public string Name
		{
			get { return "Basic Drawing"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		#endregion
	}
}