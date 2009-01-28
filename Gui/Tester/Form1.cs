using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Gui.Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;
            var window = new AgateLib.DisplayLib.DisplayWindow(AgateLib.DisplayLib.CreateWindowParams.FromControl(agateRenderTarget1));

            renderer = new RenderGui();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        RenderGui renderer;
        private void agateRenderTarget1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        internal void Run()
        {
            renderer.Run();
        }
    }
}
