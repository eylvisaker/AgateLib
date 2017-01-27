// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform;

namespace AgateLib.Tests.CoreTests.Timers
{
	class TimerTester : IAgateTest
	{
		public string Name => "Timers";

		public string Category => "App";

		public void Run(string[] args)
		{
			frmTimerTester frm = new frmTimerTester();
			frm.Show();

			Application.DoEvents();
			System.Threading.Thread.Sleep(0);

			double startTime = Timing.TotalMilliseconds;

			while (frm.Visible)
			{
				frm.UpdateControls(Timing.TotalMilliseconds - startTime);

				Application.DoEvents();
				System.Threading.Thread.Sleep(0);
			}
		}
	}
}