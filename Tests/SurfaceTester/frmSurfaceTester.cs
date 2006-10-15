// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib;

namespace ERY.SurfaceTester
{
    public partial class frmSurfaceTester : Form
    {
        Surface mSurface;
        
        public frmSurfaceTester()
        {
            InitializeComponent();

            try
            {
                Icon = new Icon(@"..\..\src\AgateLib.ico");
            }
            catch
            {
            }
        }
        
        
        private void frmImageRotation_Load(object sender, EventArgs e)
        {
            // initialize the display
            InitDisplay();
            
            // fill the combo boxes
            foreach (AgateLib.OriginAlignment align in Enum.GetValues(typeof(AgateLib.OriginAlignment)))
            {
                cboAlignment.Items.Add(align);
                cboRotation.Items.Add(align);
            }

            cboAlignment.SelectedItem = AgateLib.OriginAlignment.TopLeft;
            cboRotation.SelectedItem = AgateLib.OriginAlignment.Center;

        }


        private void InitDisplay()
        { 
            // This will create a display "window" that renders to the graphics
            // control on this form
            DisplayWindow wind = new DisplayWindow(pctGraphics);
            
            // load an image
            string fileName = @"test.jpg";

            
            try
            {
                mSurface = new Surface(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: Could not load surface " + fileName + "\n" + e.Message, 
                    "Error initializing display", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw e;
            }

        }

        internal void UpdateDisplay()
        {
            if (this.Visible == false)
                return;

            Display.BeginFrame();
            Display.Clear(AgateLib.Geometry.Color.LightGray);

            // draw the grid
            AgateLib.Geometry.Color clr = AgateLib.Geometry.Color.Gray;

            for (int x = 0; x < pctGraphics.Width; x += 30)
                Display.DrawRect(new AgateLib.Geometry.Rectangle(0, 0, x, pctGraphics.Height), clr);

            for (int y = 0; y < pctGraphics.Height; y += 30)
                Display.DrawRect(new AgateLib.Geometry.Rectangle(0, 0, pctGraphics.Width, y), clr);

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
            Display.DrawRect(new AgateLib.Geometry.Rectangle((int)nudX.Value - rectsize, (int)nudY.Value - rectsize,
                2 * rectsize, 2 * rectsize), AgateLib.Geometry.Color.Fuchsia);

            
            Display.EndFrame();
            Core.KeepAlive();

        }

        private void UpdateSurface()
        {
            mSurface.RotationAngleDegrees = (double)nudAngle.Value;
            mSurface.DisplayAlignment = (AgateLib.OriginAlignment)cboAlignment.SelectedItem;
            mSurface.RotationCenter = (AgateLib.OriginAlignment)cboRotation.SelectedItem;
            mSurface.SetScale((double)nudScaleWidth.Value / 100.0, (double)nudScaleHeight.Value / 100.0);
            mSurface.Color = AgateLib.Geometry.Color.FromArgb(colorBox.BackColor.ToArgb());
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