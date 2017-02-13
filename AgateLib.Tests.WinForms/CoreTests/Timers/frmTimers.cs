// The contents of this file are public domain.
// You may use them as you wish.
//

using System;
using System.Windows.Forms;
using AgateLib.Platform;

namespace AgateLib.Tests.CoreTests.Timers
{
	public partial class frmTimerTester : Form
	{
		IStopwatch timer = Timing.CreateStopWatch();
		int start = Environment.TickCount;
		double deltaTimeSum = 0;
		int count = 0;

		public frmTimerTester()
		{
			InitializeComponent();
		}

		internal void UpdateControls(double deltaTime)
		{
			txtTimer.Text = ((int)Timing.TotalMilliseconds).ToString();
			txtCustomTimer.Text = ((int)timer.TotalMilliseconds).ToString();

			txtEnv.Text = ((int)Timing.TotalMilliseconds - (Environment.TickCount - start)).ToString();

			deltaTimeSum += deltaTime;
			count++;

			txtDeltaTime.Text = ((int)deltaTimeSum).ToString();
			txtDeltaTimeAvg.Text = (Math.Round(deltaTimeSum / count, 2)).ToString();
		}

		private void btnPause_Click(object sender, EventArgs e)
		{
			Timing.Pause();
		}

		private void btnResume_Click(object sender, EventArgs e)
		{
			Timing.Resume();
		}

		private void btnForceResume_Click(object sender, EventArgs e)
		{
			Timing.ForceResume();
		}

		private void btnCustomReset_Click(object sender, EventArgs e)
		{
			timer.Reset();
		}

		private void btnCustomPause_Click(object sender, EventArgs e)
		{
			timer.Pause();
		}

		private void btnCustomResume_Click(object sender, EventArgs e)
		{
			timer.Resume();
		}

		private void btnCustomForceResume_Click(object sender, EventArgs e)
		{
			timer.ForceResume();
		}

		private void btnPauseAll_Click(object sender, EventArgs e)
		{
			Timing.PauseAllTimers();
		}

		private void btnResumeAll_Click(object sender, EventArgs e)
		{
			Timing.ResumeAllTimers();
		}

		private void btnForceAll_Click(object sender, EventArgs e)
		{
			Timing.ForceResumeAllTimers();
		}

	}
}