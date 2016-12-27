using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.AudioLib;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace AgateLib.Tests.AudioTests
{
	class SoundbufferStopTester : IAgateTest
	{
		SoundBuffer snda, sndb;
		SoundBuffer last;

		public string Name
		{
			get { return "SoundBuffer Stop"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		public AgateConfig Configuration { get; set; }

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}

		public void Run()
		{
			snda = new SoundBuffer("snda.wav");
			sndb = new SoundBuffer("sndb.wav");

			IFont font = Font.AgateSans;

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
		
	}
}
