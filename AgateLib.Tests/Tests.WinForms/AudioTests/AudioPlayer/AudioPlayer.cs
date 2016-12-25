// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;

namespace AgateLib.Tests.AudioTests.AudioPlayer
{
	class AudioPlayer : IDiscreteAgateTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				new frmAudioTester().ShowDialog();
			});
		}

		

		public string Name
		{
			get { return "Audio Player"; }
		}
		public string Category
		{
			get { return "Audio"; }
		}

	}
}