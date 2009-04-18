// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace Tests.SpriteTester
{
	class SpriteTester : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Sprite Tester"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			frmSpriteTester form = new frmSpriteTester();

			AgateSetup displaySetup = new AgateSetup(args);

			using (displaySetup)
			{
				displaySetup.Initialize(true, false, false);
				if (displaySetup.WasCanceled)
					return;

				form.Show();

				while (form.Visible)
				{
					form.UpdateDisplay();

					//System.Threading.Thread.Sleep(10);
					Core.KeepAlive();
				}
			}
		}
	}
}