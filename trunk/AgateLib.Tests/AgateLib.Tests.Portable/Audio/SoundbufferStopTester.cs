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

		public void EntryPoint()
		{
			snda = new SoundBuffer("snda.wav");
			sndb = new SoundBuffer("sndb.wav");

			Font font = Assets.Fonts.AgateSans;

			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

			while (Display.CurrentWindow.IsClosed == false)
			{
				Display.BeginFrame();
				Display.Clear();

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

		void Keyboard_KeyDown(InputEventArgs e)
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
