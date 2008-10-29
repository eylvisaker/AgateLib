using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AgateLib.BitmapFont;
using AgateLib.WinForms;

namespace FontCreator
{
    public partial class frmViewFont : Form
    {
        public frmViewFont()
        {
            InitializeComponent();
        }

        Image image;
        FontMetrics font;

        
        internal DialogResult ShowDialog(IWin32Window owner, string tempImage, string tempXml)
        {
            image = new Bitmap(tempImage);

            font = new FontMetrics();
            font.Load(tempXml);

            foreach (char key in font.Keys)
            {
                lstItems.Items.Add(key);
            }

            return ShowDialog(owner);
        }

        private void pctImage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DarkRed, new Rectangle(0,0,image.Width, image.Height));
            e.Graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));

            if (lstItems.SelectedIndex != -1)
            {
                char glyph = (char)lstItems.SelectedItem;
                GlyphMetrics metric = font[glyph];
                Color clr = Color.FromArgb(128, Color.Blue);

                using (Pen p = new Pen(clr))
                {
                    e.Graphics.DrawRectangle(p, Interop.Convert(metric.SourceRect));
                }
            }
        }

        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem == null)
            {
                properties.SelectedObject = null;
            }
            else
            {
                properties.SelectedObject = font[(char)lstItems.SelectedItem];
            }

            pctImage.Invalidate();
        }
    }
}