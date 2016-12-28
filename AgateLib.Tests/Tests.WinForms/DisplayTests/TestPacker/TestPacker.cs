// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib.Configuration;

namespace AgateLib.Tests.DisplayTests.TestPacker
{
	class TestPacker : IAgateTest
	{
		public string Name => "Packing";
		public string Category => "Display";

		public AgateConfig Configuration { get; set; }

		public void Run()
		{
			new frmTestPacker().ShowDialog();
		}

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
		}
	}
}