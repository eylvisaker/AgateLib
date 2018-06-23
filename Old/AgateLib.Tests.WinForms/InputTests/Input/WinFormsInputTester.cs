// The contents of this file are public domain.
// You may use them as you wish.
//

namespace AgateLib.Tests.InputTests.Input
{
	class WinFormsInputTester : IAgateTest
	{
		public string Name => "WinForms Input Tester";
		public string Category => "Input";

		public void Run(string[] args)
		{
			new frmInputTester().ShowDialog();
		}
	}
}