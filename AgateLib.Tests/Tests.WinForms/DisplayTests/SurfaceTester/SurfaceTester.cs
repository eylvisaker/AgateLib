// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Testing.DisplayTests.SurfaceTester
{
	class SurfaceTester : IDiscreteAgateTest
	{
		public string Name { get { return "Surface Tester"; } }
		public string Category { get { return "Display"; } }

		public void Main(string[] args)
		{
			using (var model = new PassiveModel(args))
			{
				model.Run(() =>
				{
					frmSurfaceTester form = new frmSurfaceTester();

					form.Show();

					int frame = 0;

					while (form.Visible)
					{
						form.UpdateDisplay();

						frame++;

					}
				});
			}
		}
	}
}