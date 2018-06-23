// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.AudioTests.AudioPlayer
{
	class AudioPlayer : IAgateTest
	{
		public string Name
		{
			get { return "Audio Player"; }
		}
		public string Category
		{
			get { return "Audio"; }
		}

		public void Run(string[] args)
		{
			new frmAudioTester().ShowDialog();
		}

	}
}