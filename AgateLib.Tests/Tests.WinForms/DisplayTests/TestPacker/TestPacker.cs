// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgateLib.Tests.DisplayTests.TestPacker
{
	class TestPacker : IAgateTest
	{
		public string Name => "Packing";
		public string Category => "Display";

		public void Run(string[] args)
		{
			new frmTestPacker().ShowDialog();
		}
	}
}