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
		public string Name { get { return "Packing"; } }
		public string Category { get { return "Display"; } }

		public void Main(string[] args)
		{
			new frmTestPacker().ShowDialog();
		}
	}
}