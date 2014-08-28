// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

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
			new PassiveModel(args).Run( () =>
			{
				frmSpriteTester form = new frmSpriteTester();

				form.Show();

				while (form.Visible)
				{
					form.UpdateDisplay();

					//System.Threading.Thread.Sleep(10);
					Core.KeepAlive();
				}
			});
		}
	}
}