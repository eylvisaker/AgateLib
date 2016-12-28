// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.DisplayTests.SpriteTester
{
	class SpriteTester : IAgateTest
	{
		public string Name => "Sprite Tester";
		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			frmSpriteTester form = new frmSpriteTester();

			form.Show();

			while (form.Visible)
			{
				form.UpdateDisplay();

				Core.KeepAlive();
			}
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}