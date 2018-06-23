using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            //notebookPage1.Image = ResizeImage(notebookPage1.Image, new Size(32, 32));
            //notebookPage2.Image = ResizeImage(notebookPage2.Image, new Size(48,48));
        }

        Image ResizeImage(Image b, Size newSize)
        {
            Bitmap retval = new Bitmap(newSize.Width, newSize.Height);

            using (Graphics g = Graphics.FromImage(retval))
            {
                g.DrawImage(b, new Rectangle(Point.Empty, newSize));
            }

            return retval;
        }

        private void notebookPage9_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}