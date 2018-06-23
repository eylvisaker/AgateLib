using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib;
using AgateLib.DisplayLib;

namespace Examples.Initialization.WindowsFormsInitialization
{
	public partial class AgateFormMixtureForm : Form
	{
		DisplayWindow window;

		public AgateFormMixtureForm()
		{
			InitializeComponent();

			window = new DisplayWindowBuilder()
				.RenderToControl(renderTarget)
				.AutoResizeBackBuffer()
				.Build();
		}

		public event EventHandler Draw;

		public DisplayWindow DisplayWindow => window;

		public string EntryText => txtEntry.Text;
		
		private void timer1_Tick(object sender, EventArgs e)
		{
			Draw?.Invoke(this, EventArgs.Empty);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			timer1.Enabled = false;
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
