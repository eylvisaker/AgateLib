// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AgateLib;
using AgateLib.DisplayLib;
using Color = AgateLib.DisplayLib.Color;
using Rectangle = AgateLib.Mathematics.Geometry.Rectangle;

namespace AgateLib.Tests.DisplayTests.SurfaceTester
{
	public partial class frmSurfaceTester : Form
	{
		Surface mSurface;

		public frmSurfaceTester()
		{
			InitializeComponent();

			try
			{
				Icon = new Icon(@"../../../AgateLib.ico");
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not load icon.  Error: {0}", e);
			}
		}


		private void frmImageRotation_Load(object sender, EventArgs e)
		{
			// initialize the display
			InitDisplay();

			// fill the combo boxes
			foreach (OriginAlignment align in Enum.GetValues(typeof(OriginAlignment)))
			{
				cboAlignment.Items.Add(align);
				cboRotation.Items.Add(align);
			}

			cboAlignment.SelectedItem = OriginAlignment.TopLeft;
			cboRotation.SelectedItem = OriginAlignment.Center;

		}


		private void InitDisplay()
		{
			// This will create a display "window" that renders to the graphics
			// control on this form
			DisplayWindow wind = DisplayWindow.CreateFromControl(pctGraphics);

			// load an image
			string fileName = @"Images/jellybean.png";


			mSurface = new Surface(fileName);


		}

		internal void UpdateDisplay()
		{
			if (this.Visible == false)
				return;

			Display.BeginFrame();
			Display.Clear(Color.LightGray);

			// draw the grid
			Color clr = Color.Gray;

			for (int x = 0; x < pctGraphics.Width; x += 30)
				Display.Primitives.DrawRect(clr, new Rectangle(0, 0, x, pctGraphics.Height));

			for (int y = 0; y < pctGraphics.Height; y += 30)
				Display.Primitives.DrawRect(clr, new Rectangle(0, 0, pctGraphics.Width, y));

			if (mSurface != null)
			{
				// set all the state-drawing options on the surface
				UpdateSurface();

				mSurface.Draw((int)nudX.Value, (int)nudY.Value);


				// this image should be drawn at 200, 100 unrotated.
				// this is to test to make sure that RotationCenter does not have
				// any effect on a displayed, unrotated sprite.
				mSurface.RotationAngleDegrees = 0;
				mSurface.DisplayAlignment = OriginAlignment.TopLeft;
				mSurface.Alpha = 1.0;
				mSurface.SetScale(1.0, 1.0);

				mSurface.Draw(200, 100);

			}

			// box around sprite point to check alignment
			const int rectsize = 3;
			Display.Primitives.DrawRect(Color.Fuchsia,
				new Rectangle(
					(int)nudX.Value - rectsize,
					(int)nudY.Value - rectsize, 
					2 * rectsize, 
					2 * rectsize));


			Display.EndFrame();
			AgateApp.KeepAlive();

		}

		private void UpdateSurface()
		{
			mSurface.RotationAngleDegrees = (double)nudAngle.Value;
			mSurface.DisplayAlignment = (OriginAlignment)cboAlignment.SelectedItem;
			mSurface.RotationCenter = (OriginAlignment)cboRotation.SelectedItem;
			mSurface.SetScale((double)nudScaleWidth.Value / 100.0, (double)nudScaleHeight.Value / 100.0);
			mSurface.Color = Color.FromArgb(colorBox.BackColor.ToArgb());
			mSurface.Alpha = (double)nudAlpha.Value / 100.0;
		}

		private void nudAngle_ValueChanged(object sender, EventArgs e)
		{

		}

		private void cboAlignment_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void cboRotation_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void nudX_ValueChanged(object sender, EventArgs e)
		{
		}

		private void nudAlpha_ValueChanged(object sender, EventArgs e)
		{
		}

		private void colorBox_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = colorBox.BackColor;

			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				colorBox.BackColor = colorDialog1.Color;
			}
		}

	}
}