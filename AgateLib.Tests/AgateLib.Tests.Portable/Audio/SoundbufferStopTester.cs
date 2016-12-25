using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.AudioTests
{
	class SoundbufferStopTester : ISerialModelTest
	{
		public string Name
		{
			get { return "SoundBuffer Stop"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		SoundBuffer snda, sndb;
		SoundBuffer last;

		public void EntryPoint()
		{
			snda = new SoundBuffer("snda.wav");
			sndb = new SoundBuffer("sndb.wav");

			IFont font = DefaultAssets.Fonts.AgateSans;

			Input.Unhandled.KeyDown += Keyboard_KeyDown;
			Input.Unhandled.MouseDown += Mouse_MouseDown;
			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear();

				font.Size = 14;
				font.Color = Color.White;
				font.DrawText("Press a for first sound, b for second sound.");

				if (snda.IsPlaying)
					font.DrawText(0, 30, "first sound is playing");
				if (sndb.IsPlaying)
					font.DrawText(0, 60, "second sound is playing");

				Display.EndFrame();
				Core.KeepAlive();
			}
		}

		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			if (last == snda)
				last = sndb;
			else
				last = snda;

			last.Play();
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.A)
				snda.Play();

			if (e.KeyCode == KeyCode.B)
				sndb.Play();
		}


		public void ModifyModelParameters(ApplicationModels.SerialModelParameters parameters)
		{
		}
	}
}
