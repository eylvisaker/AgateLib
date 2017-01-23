// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.DisplayTests.SurfaceTester
{
	class SurfaceTester : IAgateTest
	{
		public string Name => "Surface Tester";

		public string Category => "Display";

		public void Run(string[] args)
		{
			frmSurfaceTester form = new frmSurfaceTester();

			form.Show();

			int frame = 0;

			while (form.Visible)
			{
				form.UpdateDisplay();

				frame++;
			}
		}
	}
}