// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Utility;
using AgateLib.WinForms;

namespace TestPacker
{

    public partial class Form1 : Form
    {
        SurfacePacker.RectPacker<object> packer;
        int maxSize = 40;
        int minSize = 30;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            packer = new SurfacePacker.RectPacker<object>(Interop.Convert(pictureBox1.ClientSize));

            Redraw();
        }
        private void btnLotsSorted_Click(object sender, EventArgs e)
        {
            packer = new SurfacePacker.RectPacker<object>(Interop.Convert(pictureBox1.ClientSize));
            Random rand = new Random();

            btnOne.Enabled = true;

            for (int i = 0; i < 200; i++)
            {
                Size sz = new Size(rand.Next(minSize, maxSize), rand.Next(minSize, maxSize));

                packer.QueueObject(sz, null);
            }

            packer.AddQueue();

            Redraw();
        }
        private void btnMany_Click(object sender, EventArgs e)
        {
            packer = new SurfacePacker.RectPacker<object>(Interop.Convert(pictureBox1.ClientSize));
            Random rand = new Random();
            bool done = false;

            btnOne.Enabled = true;

            while (!done)
            {
                Size sz = new Size(rand.Next(minSize, maxSize), rand.Next(minSize, maxSize));
                Rectangle rect;

                if (packer.FindEmptySpace(sz, out rect))
                {
                    packer.AddRect(rect, null);
                }
                else
                    done = true;
            }

            Redraw();
        }
        private void btnOne_Click(object sender, EventArgs e)
        {

            if (packer == null)
                packer = new SurfacePacker.RectPacker<object>(Interop.Convert(pictureBox1.ClientSize));

            Random rand = new Random();
            Size sz = new Size(rand.Next(minSize, maxSize), rand.Next(minSize, maxSize));
            Rectangle rect;

            if (packer.FindEmptySpace(sz, out rect))
            {
                packer.AddRect(rect, null);
                statusBar1.Panels[1].Text = "Added rect: " + rect.ToString();
            }
            else
                statusBar1.Panels[1].Text = "Failed to add rect of size " + sz.ToString();

            Redraw();
        }

        private void Redraw()
        {
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(
                pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Brush brush = new System.Drawing.Drawing2D.HatchBrush(
                System.Drawing.Drawing2D.HatchStyle.DiagonalCross, System.Drawing.Color.Red);

            foreach (SurfacePacker.RectHolder<object> r in packer)
            {
                g.FillRectangle(brush, Interop.Convert(r.Rect));
                g.DrawRectangle(pen, Interop.Convert(r.Rect));
            }

            g.Dispose();

            pictureBox1.Image = img;

            statusBar1.Panels[0].Text = "Percentage Used: " + (int)(packer.PixelsUsedPercentage * 100 + 0.5);

        }


    }
}
