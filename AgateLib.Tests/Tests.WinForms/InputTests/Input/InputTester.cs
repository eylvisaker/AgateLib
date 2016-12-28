// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Configuration;
using AgateLib.Platform.WinForms.ApplicationModels;

namespace AgateLib.Tests.InputTests.InputTester
{
	class InputTester : IAgateTest
	{
		public string Name => "Input Tester";
		public string Category => "Input";

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}

		public void Run()
		{
			new frmInputTester().ShowDialog();
		}
	}
}