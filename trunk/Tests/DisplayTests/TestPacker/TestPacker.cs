// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tests.TestPacker
{
	class TestPacker : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Packing"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			new frmTestPacker().ShowDialog();
		}
	}
}