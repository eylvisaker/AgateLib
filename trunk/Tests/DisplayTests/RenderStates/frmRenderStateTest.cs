using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.DisplayLib;

namespace Tests.DisplayTests.RenderStates
{
	public partial class frmRenderStateTest : Form
	{
		DisplayWindow wind;

		public frmRenderStateTest()
		{
			InitializeComponent();
			
			wind = DisplayWindow.CreateFromControl(agateRenderTarget1);

			propertyGrid1.SelectedObject = Display.RenderState;
		}

		internal void UpdateFrame()
		{
			Display.BeginFrame();
			Display.Clear(AgateLib.Geometry.Color.Green);
			Display.EndFrame();
		}
	}
}
