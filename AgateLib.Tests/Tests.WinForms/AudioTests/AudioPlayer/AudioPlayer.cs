// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;

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

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.CreateDisplayWindow = false;
			setup.AssetLocations.Path = "";
		}

		public void Run()
		{
			new frmAudioTester().ShowDialog();
		}

	}
}