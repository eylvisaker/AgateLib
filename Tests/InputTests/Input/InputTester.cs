// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace Tests.InputTester
{
	class InputTester : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Input Tester"; } }
		public string Category { get { return "Input"; } }

		#endregion

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				new frmInputTester().ShowDialog();
			});
		}
	}
}