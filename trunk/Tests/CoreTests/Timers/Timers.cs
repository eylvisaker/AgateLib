// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform;

namespace Tests.TimerTester
{
	class TimerTester : IAgateTest
	{
		public void Main(string[] args)
		{
			PassiveModel.Run(args, () =>
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
			});
		}

		#region IAgateTest Members

		public string Name { get { return "Timers"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}