// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.ApplicationModels;

namespace Tests.AudioTester
{
	class AudioTester : IAgateTest
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

		#region IAgateTest Members

		public string Name
		{
			get { return "Audio Player"; }
		}
		public string Category
		{
			get { return "Audio"; }
		}

		#endregion
	}
}