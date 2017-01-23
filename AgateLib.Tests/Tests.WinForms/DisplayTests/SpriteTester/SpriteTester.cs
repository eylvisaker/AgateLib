// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.DisplayTests.SpriteTester
{
	class SpriteTester : IAgateTest
	{
		public string Name => "Sprite Tester";
		public string Category => "Display";

		public void Run(string[] args)
		{
			frmSpriteTester form = new frmSpriteTester();

			form.Show();

			while (form.Visible)
			{
				form.UpdateDisplay();

				AgateApp.KeepAlive();
			}
		}
	}
}