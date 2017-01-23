// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.InputTests.InputTester
{
	class InputTester : IAgateTest
	{
		public string Name => "Input Tester";
		public string Category => "Input";

		public void Run(string[] args)
		{
			new frmInputTester().ShowDialog();
		}
	}
}