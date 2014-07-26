// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.SurfaceTester
{
	class SurfaceTester : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Surface Tester"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			PassiveModel.Run(args, () =>
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