// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Testing.DisplayTests.SpriteTester
{
	class SpriteTester : IAgateTest
	{
		public string Name { get { return "Sprite Tester"; } }
		public string Category { get { return "Display"; } }

		public void Main(string[] args)
		{
			using (var model = new PassiveModel(args))
			{
				model.Run(() =>
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
}