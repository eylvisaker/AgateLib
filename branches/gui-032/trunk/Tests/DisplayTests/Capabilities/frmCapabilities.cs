using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tests.DisplayTests.Capabilities
{
	public partial class frmCapabilities : Form
	{
		public frmCapabilities()
		{
			InitializeComponent();

			Text += AgateLib.Drivers.Registrar.DisplayDrivers[0].FriendlyName;
			propertyGrid1.SelectedObject = AgateLib.DisplayLib.Display.Caps;
		}
	}
}
